using System.Collections;
using UnityEngine;

public class ContextSteering : MonoBehaviour
{
    [SerializeField] private Transform player;
    public Vector2 target;
    [SerializeField] private Rigidbody2D baseRB;
    [SerializeField] private float movSpeed;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float losDistance;
    [SerializeField] private float minDistance;
    [SerializeField] private float targetThreshold;
    [SerializeField] private float detectionRate = 0.2f;
    [SerializeField] private int numDirections;
    [Range(0f, 1f)]
    [SerializeField] private float moveThreshold;
    [SerializeField] private bool thresholdMet;

    [SerializeField] private LayerMask obstacleLayer, playerLayer, losLayer;

    [SerializeField] private bool showGizmos;
    
    private EnemyManager enemyManager;
    private Collider2D[] colliderArray;
    private Vector2[] directionsArray;
    private float[] interest;
    private float[] danger;
    private float[] movPoss;
    private Vector2 movDir;

    public Vector2 playerLastPos;
    public bool playerInLoS;
    public bool objectsInRange;
    public bool reachedTarget;

    public bool activateSteering;

    private void Start() {
        reachedTarget = true;
        playerInLoS = false;
        activateSteering = false;
        playerLastPos = Vector2.zero;
        baseRB = GetComponent<Rigidbody2D>();
        enemyManager = GetComponent<EnemyManager>();

        generateDirections();
        InvokeRepeating("detect", 0f, detectionRate);
    }

    private void Update() {
        if (activateSteering) {
            StartCoroutine(contextSteering());
        }
        else {
            StopCoroutine(contextSteering());
        }
    }

    private void detect() {
        colliderArray = Physics2D.OverlapCircleAll(transform.position, detectionRadius, obstacleLayer);
        checkLoS();
        //Debug.Log(colliderArray.Length);
        //Debug.Log(colliderArray[0].name);
        if (colliderArray.Length > 1) {
            objectsInRange = true;
        }
        else {
            objectsInRange = false;
        }
    }

    private IEnumerator contextSteering() {
        interest = new float[numDirections];
        danger = new float[numDirections];
        movPoss = new float[numDirections];

        dangerWeights();
        interestWeights();
        if (enemyManager.playerObject != null) {
            movDir = directionSolverIdle();
            baseRB.velocity = movDir * movSpeed;
            yield return new WaitForSeconds(detectionRate);
            contextSteering();
        }
        else {
            movDir = directionSolverChase();
            baseRB.velocity = movDir * movSpeed;
            yield return new WaitForSeconds(detectionRate);
            contextSteering();
        }
    }

    private void checkLoS() {
        target = player.position;
        Vector2 playerDir = (target - (Vector2)transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDir, losDistance, losLayer);

        if (hit.collider != null && (playerLayer & (1 << hit.collider.gameObject.layer)) != 0) {
            Debug.DrawRay(transform.position, playerDir * hit.distance, Color.green);
            playerLastPos = hit.transform.position;
            playerInLoS = true;
        }
        else {
            playerInLoS = false;
        }
    }

    private void dangerWeights() {
        foreach (Collider2D coll in colliderArray) {
            Vector2 obstacleDir = coll.ClosestPoint(transform.position) - (Vector2)transform.position;
            float obstacleDist = obstacleDir.magnitude;

            float weight = obstacleDist <= minDistance ? 1 : ((detectionRadius - minDistance)- (obstacleDist - minDistance)) / (detectionRadius - minDistance);

            for (int i = 0; i < numDirections; i++) {
                float diff = Vector2.Dot(obstacleDir.normalized, directionsArray[i]);

                diff *= weight;

                if (diff > danger[i]) {
                    danger[i] = diff;
                }
            }
        }
    }

    private void interestWeights() {
        if (reachedTarget) {
            if (!playerInLoS) {
                return;
            }
            else {
                reachedTarget = false;
            }
        }

        if (Vector2.Distance(transform.position, playerLastPos) <= targetThreshold) {
            reachedTarget = true;
            return;
        }

        Vector2 targetDir = (playerLastPos - (Vector2)transform.position);
        for (int i = 0; i < numDirections; i++) {
            float diff = Vector2.Dot(targetDir.normalized, directionsArray[i]);

            diff = 1.0f - Mathf.Abs(diff - (float)0.65);

            if (diff > 0) {
                if (diff > interest[i]) {
                    interest[i] = diff;
                }
            }
        }
    }

    private Vector2 directionSolverChase() {
        float max = 0f;
        for (int i = 0; i < numDirections; i++) {
            movPoss[i] = Mathf.Clamp01(interest[i] - danger[i]);
            if (max < movPoss[i]) {
                max = movPoss[i];
            }
        }

        //Debug.Log(transform.name + ": " + max);
        if (max >= moveThreshold) {
            thresholdMet = true;
        }
        else {
            thresholdMet = false;
        }

        Vector2 output = Vector2.zero;
        for (int j = 0; j < numDirections; j++) {
            output += directionsArray[j] * movPoss[j];
        }

        output = output.normalized;

        if (!thresholdMet) {
            output = Vector2.zero;
        }

        return output;
    }

    private Vector2 directionSolverIdle() {
        return Vector2.zero;
    }

    private void generateDirections() {
        directionsArray = new Vector2[numDirections];
        float rotateIncre = (2 * Mathf.PI) / numDirections;
        Vector2 baseVector = new Vector2(0, 1).normalized;

        for (int i = 0; i < numDirections; i++) {
            float rotateAmount = i * rotateIncre;

            directionsArray[i] = new Vector2(baseVector.x * Mathf.Cos(rotateAmount) - baseVector.y * Mathf.Sin(rotateAmount),
                                             baseVector.x * Mathf.Sin(rotateAmount) + baseVector.y * Mathf.Cos(rotateAmount)).normalized;

        }
    }

    private void OnDrawGizmos() {
        if (!showGizmos) {
            return;
        }

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, minDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(playerLastPos, 0.3f);

        float[] temp = danger;
        float[] tempInterest = interest;
        if (Application.isPlaying && temp != null) {
            Gizmos.color = Color.red;
            for (int i = 0; i < temp.Length; i++) {
                Gizmos.DrawRay(transform.position, directionsArray[i] * temp[i] * detectionRadius);
            }
        }

        if (Application.isPlaying && tempInterest != null) {
            Gizmos.color = Color.green;
            for (int i = 0; i < tempInterest.Length; i++) {
                Gizmos.DrawRay(transform.position, directionsArray[i] * tempInterest[i] * detectionRadius);
            }
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, movDir * detectionRadius);
    }
}
