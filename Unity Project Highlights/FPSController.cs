using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : MonoBehaviour
{
    [SerializeField] private Transform characterCamera;
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private Transform characterCapsule;
    [SerializeField] private Transform playerGlobal;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    private Transform characterObject;
    private Vector3 cameraStartPos;

    [Header("Rotation")]
    [SerializeField] private float mouseSens = 100f;
    [SerializeField, Range(0f,1f)] private float mouseVertMult;
    [SerializeField] private float maxUpAngle = 90f;
    [SerializeField] private float minUpAngle = 65f;

    public static bool isGrounded = true;

    [Header("Movement")]
    [SerializeField] private float groundDrag;
    [SerializeField] private float maxVertVel;

    [Header("Walk")]
    [SerializeField] private bool isWalking;
    [SerializeField] private float walkVel;

    [Header("Sprint")]
    [SerializeField] private bool isSprinting;
    [SerializeField] private float sprintVel;

    public static bool isCrouching;
    [Header("Crouch")]
    [SerializeField] private float crouchVel;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float crouchTime;
    [SerializeField] private Vector3 crouchCamPos;
    [SerializeField] private Vector3 crouchCapsuleScale;
    [SerializeField] private Transform crouchHeadCheck;
    [SerializeField] private LayerMask ignorePlayer;
    private Vector3 standingCapsuleScale;
    private float standingHeight;

    [Header("Slide")]
    [SerializeField] private bool isSliding;
    [SerializeField] private float slideVel;
    [SerializeField] private float slideLength;
    [SerializeField] private float slideCounter;

    [Header("Jump")]
    [SerializeField] private bool isJumping;
    [SerializeField] private float jumpForce;

    [Header("Wall Run")]
    [SerializeField] private bool isWallRunning;
    [SerializeField] private float wallRunVel;
    [SerializeField] private float wallDist;
    [SerializeField] private bool wallDetected;
    private RaycastHit wallRay;

    [Header("Mantle")]
    [SerializeField] private Transform edgeDetection;
    [SerializeField] private float mantleHeight;
    [SerializeField] private float mantleTime;
    private RaycastHit obstacleHit;

    private float headRotation = 0f;
    private float bodyRotation = 0f;
    float velocity;

    #region Input Buffers
    private float jumpBuffer = 0.2f; //Small period of time where jump input is read. When player lands, jump is immediately executed.
    private float sprintBuffer = 0.2f; //Small period of time where player can still be considered sprinting.
    private float coyoteBuffer = 0.2f; //Gives players a small period of time where they can still jump despite not being grounded.

    private float jumpBufferCounter;
    private float sprintBufferCounter;
    private float coyoteBufferCounter;
    #endregion

    [Header("Animations")]
    [SerializeField] private Animator topAnimator;

    private void Start() {
        characterObject = this.gameObject.transform;
        Cursor.lockState = CursorLockMode.Locked;
        velocity = walkVel;
        standingHeight = playerCollider.height;
        cameraStartPos = characterCamera.localPosition;
        standingCapsuleScale = characterCapsule.localScale;
    }

    private void FixedUpdate() {
        Move();
    }

    private void Update()
    {
        SpeedClamp();
        GroundCheck();
        MouseRotation();
        DetectWall();
        Crouch();
        Jump();
        Slide();
    }

    //Move script that handles 2D movement. Handles velocities for wall running as well.
    private void Move()
    {
        Vector3 horzMovement = characterObject.right * GameInputs.instance.HorizontalInput().x + characterObject.forward * GameInputs.instance.HorizontalInput().y;
        horzMovement.Normalize();

        if (isGrounded) {
            velocity = walkVel;
            if (GameInputs.instance.sprint && !isSliding) {
                isSprinting = true;
                sprintBufferCounter = sprintBuffer;
                velocity = sprintVel;
            }
            else {
                isSprinting = false;
                sprintBufferCounter -= Time.deltaTime;
            }

            if (isCrouching) {
                velocity = crouchVel;
            }

            if (isSliding) {
                velocity = slideVel;
            }
        }

        if (isWallRunning) {
            velocity = wallRunVel;
            Vector3 wallDir = Vector3.Cross(wallRay.normal, playerGlobal.up);
            if (Mathf.Abs(playerRB.velocity.y) > 0) {
                playerRB.velocity = new Vector3(playerRB.velocity.x, 0f, playerRB.velocity.z);
            }

            //Check Orientation
            if (Vector3.Dot(wallDir, playerGlobal.forward) < 0) {
                wallDir = -wallDir;
            }

            //Move player along wall
            playerRB.AddForce(wallDir * velocity * 15f * GameInputs.instance.HorizontalInput().y, ForceMode.Force);

            //Move player down wall
            playerRB.AddForce(-playerGlobal.up * 50f, ForceMode.Force);

            //Stick player to wall for curves
            playerRB.AddForce(-wallRay.normal * 100f, ForceMode.Force);
            return;
        }

        playerRB.AddForce(horzMovement * velocity * 15f, ForceMode.Force);
    }

    private void MouseRotation()
    {
        float lookUp = GameInputs.instance.MouseUp();
        float lookSide = GameInputs.instance.MouseSide();
        bodyRotation += lookSide * mouseSens;
        headRotation -= lookUp * mouseSens * mouseVertMult;
        headRotation = Mathf.Clamp(headRotation, -maxUpAngle, minUpAngle);

        characterObject.rotation = Quaternion.Euler(0f, bodyRotation, 0f);
        characterCamera.localRotation = Quaternion.Euler(headRotation, 0f, 0f);

        playerGlobal.position = transform.position;
        //If player is not wall running, we rotate the body as well.
        if (!isWallRunning) {
            playerGlobal.rotation = characterObject.rotation;
        }
    }

    private void GroundCheck() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !isWallRunning) {
            coyoteBufferCounter = coyoteBuffer;
            playerRB.drag = groundDrag;
            playerRB.useGravity = false;
        }
        else if (!isGrounded && !isWallRunning) {
            playerRB.drag = 0;
            coyoteBufferCounter -= Time.deltaTime;
            playerRB.useGravity = true;
        }
        else if (isWallRunning) {
            playerRB.drag = 0;
            coyoteBufferCounter -= Time.deltaTime;
            playerRB.useGravity = false;
        }
    }

    private void Crouch() {
        if (GameInputs.instance.crouch && isGrounded) {
            isCrouching = true;
        }
        else {
            isCrouching = false;
        }

        if (isCrouching) {
            if (characterCamera.localPosition == crouchCamPos) { //If player is already crouching, don't do anything.
                return;
            }
            characterCamera.localPosition = crouchCamPos;
            playerCollider.height = crouchHeight;
            characterCapsule.localScale = crouchCapsuleScale;
            playerRB.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        else {
            if (Physics.CheckSphere(crouchHeadCheck.position, groundCheckRadius, ignorePlayer)) {
                isCrouching = true;
                return;
            }

            characterCamera.localPosition = cameraStartPos;
            playerCollider.height = standingHeight;
            characterCapsule.localScale = standingCapsuleScale;
        }
    }

    private void Slide() {
        if (isSliding) {
            slideCounter -= Time.deltaTime;
            if (slideCounter <= 0) {
                isSliding = false;
            }

            return;
        }

        if (sprintBufferCounter > 0 && GameInputs.instance.Crouch() && isGrounded) {
            if (slideCounter <= 0) {
                isSliding = true;
                isSprinting = false;
                slideCounter = slideLength;
            }
        }

    }

    private void Jump() {
        if (GameInputs.instance.Jump()) {
            jumpBufferCounter = jumpBuffer;
        }
        else {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (isGrounded) {
            isJumping = false;
        }

        if (jumpBufferCounter > 0 && coyoteBufferCounter > 0 && !isJumping) {
            if (isCrouching) {
                jumpBufferCounter = 0;
                coyoteBufferCounter = 0;
                return;
            }
            else {
                playerRB.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }

            isJumping = true;
            jumpBufferCounter = 0;
            coyoteBufferCounter = 0;
        }
    }

    private void DetectWall() {
        wallDetected = Physics.Raycast(transform.position, playerGlobal.right * GameInputs.instance.HorizontalInput().x, out wallRay, wallDist);

        if (wallDetected && !isGrounded && Mathf.Abs(GameInputs.instance.HorizontalInput().y) > 0) {
            isWallRunning = true;
        }
        else {
            isWallRunning = false;
        }
    }

    private void SpeedClamp() {
        if (isSliding) {
            return;
        }

        Vector3 currVel = new Vector3(playerRB.velocity.x, 0, playerRB.velocity.z);

        if (currVel.magnitude > velocity) {
            Vector3 velDir = currVel.normalized * velocity;
            playerRB.velocity = new Vector3(velDir.x, playerRB.velocity.y, velDir.z);
        }
    }

    #region Gizmos
    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(crouchHeadCheck.position, groundCheckRadius);
    }
    #endregion
}
