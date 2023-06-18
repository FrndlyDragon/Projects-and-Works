using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*2D Player Platformer movement. Requires 2 layers for the ground and the walls. Supports the standard platformer movement features.
*Script is intended to be attached to a player object.
*Included: Double Jumping
*          Dynamic Jump Height
*          Wall Sliding and Jumping
*          Coyote Time
*/
public class PlayerMovement : MonoBehaviour
{
    //Objects: Wall check and ground check objects are required for detecting collision with environment.
    //Utilizes Unity 2D Physics so rigidbodies and colliders are needed on Player.
    private Rigidbody2D playerRB;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    //Values
    private float horizontalMov;
    private float jumpBufferCounter;
    private float wallBufferCounter;

    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float wallSpeed = 0.2f;
    [SerializeField] private float wallJumpHorzPower = 1f;
    [SerializeField] private float sprintMult = 1.5f;
    [SerializeField] private float jumpPower = 0f;
    [SerializeField] private float jumpDamp = 0.5f;
    [SerializeField] private float jumpBuffer = 0.1f;
    [SerializeField] private float wallJumpBuffer = 0.1f;

    //Booleans
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isWall;
    [SerializeField] private bool canDoubleJump;

    //Initialization of variables
    private void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        isGrounded = false;
        isJumping = false;
        isWall = false;
        canDoubleJump = false;
    }

    private void Update()
    {
        horizontalMov = Input.GetAxisRaw("Horizontal");
        isWall = wallDetection();
        isGrounded = groundDetection();

        if (isGrounded && !isJumping) {
            jumpBufferCounter = jumpBuffer;
            canDoubleJump = false;
        }
        else {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isWall) { //Normal jump script given player is not on a wall
            if (jumpBufferCounter > 0f || canDoubleJump) {
                isJumping = true;
                canDoubleJump = !canDoubleJump; //Boolean toggle for double jumping
                playerRB.velocity = new Vector2(playerRB.velocity.x, jumpPower);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && playerRB.velocity.y > 0f) { //Done so that player falls faster than they rise.
            isJumping = false;
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y * jumpDamp);
        }

        if (isJumping && playerRB.velocity.y < 0f) {
            isJumping = false;
            playerRB.velocity = new Vector2(playerRB.velocity.x, playerRB.velocity.y/jumpDamp);
        }

        wallJump(); //Runs constantly to determine if wall jumping can be done
    }

    //Fixed update is used for constantly movement due to fixed update times. Otherwise, Time would be needed to keep speed consistent.
    private void FixedUpdate() {
        playerFlip();
        if (Input.GetKey(KeyCode.LeftShift)) {
            playerRB.velocity = new Vector2(horizontalMov * movementSpeed * sprintMult, playerRB.velocity.y);
        }
        else if (wallBufferCounter > 0f) { //If player detaches from wall, this will slow their speed for a moment to make wall jumping easier
            playerRB.velocity = new Vector2(horizontalMov * movementSpeed, -wallSpeed);
        }
        else {
            playerRB.velocity = new Vector2(horizontalMov * movementSpeed, playerRB.velocity.y);
        }
    }

    //Flips the player according to input direction
    private void playerFlip() {
        if (horizontalMov * transform.localScale.x < 0) {
            Vector3 local = transform.localScale;
            local.x *= -1f;
            transform.localScale = local;
        }
    }

    //Wall jump function. Gives players small time to jump after detaching from wall.
    private void wallJump() {
        if (isWall && horizontalMov * transform.localScale.x > 0) {
            playerRB.velocity = new Vector2(playerRB.velocity.x, -wallSpeed);
            wallBufferCounter = wallJumpBuffer;
        }
        else {
            wallBufferCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallBufferCounter > 0f) {
            wallBufferCounter = 0f;
            playerRB.velocity = new Vector2(horizontalMov * wallJumpHorzPower, jumpPower);
        }
    }
 
    //Wall detection through creating a circle around the wall check object attached to the player. 
    private bool wallDetection() {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    //Ground detection through creating a circle around the ground check object attacjed to the player.
    private bool groundDetection() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    //Visualization for ground and wall check cirles.
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(wallCheck.position, 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(groundCheck.position, 0.2f);
    }
}
