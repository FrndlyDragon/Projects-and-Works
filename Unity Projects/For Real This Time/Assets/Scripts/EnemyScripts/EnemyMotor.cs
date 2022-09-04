using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotor : MonoBehaviour
{
    // Find Dimensions of Detection Box
    private Vector2 origin;
    private Vector2 areaDimensions;


    // Locate Relevant Objects
    public Transform player;
    private Rigidbody2D enemyRB;
    public GameObject enemyArea;
    public OnCollideTrigger detectionTrigger;
    public OnCollideTrigger attackTrigger;
    private BoxCollider2D detectionArea;

    // Determine movement
    private Vector3 startPos;
    private Vector2 movement;
    private Vector3 scale;
    public float speed;
    public float range;
    private bool returning;

    //Gizmo for Detection Range
    public Color gizmoIdle;
    public Color gizmoActive;
    public bool gizmoShow;
    public bool inArea;

    void Start() {
        enemyRB = GetComponent<Rigidbody2D>();
        scale = transform.localScale;
        startPos = transform.parent.position;
        returning = false;

        detectionArea = enemyArea.GetComponent<BoxCollider2D>();

        origin = detectionArea.transform.position;
        areaDimensions = detectionArea.size;
        
    }

    void FixedUpdate() {
        if (detectionTrigger.playerInside && !returning) {
            faceDirection(player.position, transform.position);
            movement = player.position - transform.position;
            movement.Normalize();
            if (attackTrigger.playerInside) {
                return;
            }

            moveDirection(movement);
        }
        else {
            Reset();
        }
        
    }

    void Reset() {
        transform.localScale = scale;
        if (Vector3Int.RoundToInt((startPos - transform.position) + new Vector3(0.4f,0.4f,0)) != Vector3.zero) {
            returning = true;
            enemyRB.velocity = (startPos - transform.position).normalized * 5f;
        }
        else {
            transform.position = startPos;
            enemyRB.velocity = Vector2.zero;
            returning = false;
        }
    }

    void faceDirection(Vector3 player, Vector3 curr) {
        if (player.x - curr.x >= 0) {
            transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
        }
        else {
            transform.localScale = scale;
        }
    }

    void moveDirection(Vector2 movement) {
        enemyRB.MovePosition((Vector2)transform.position + (movement * speed * Time.deltaTime));
    }

    private void OnDrawGizmos() {
        if (gizmoShow) {
            Gizmos.color = gizmoIdle;
            if (inArea) {
                Gizmos.color = gizmoActive;
            }
            Gizmos.DrawCube(origin, areaDimensions);
        }
    }

}
