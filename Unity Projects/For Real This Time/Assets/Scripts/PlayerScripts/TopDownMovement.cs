using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDistance;
    private float speed;
    private RaycastHit2D collision;

    // Start is called before the first frame update
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (DialogueManager.instance.isPlaying) {
            return;
        }

        speed = 10;
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //Sprint
        if (Input.GetKey(KeyCode.LeftShift)) {
            speed = speed * 1.5f;
        }

        moveDistance = new Vector3(x * speed, y * speed, 0);

        //Change direction sprite is facing
        if (Input.GetAxisRaw("Horizontal") > 0) {
            transform.localScale = Vector3.one;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        // Collision detection with NPC, Boundaries
        collision = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0,moveDistance.y), 
                                      Mathf.Abs(moveDistance.y * Time.deltaTime), LayerMask.GetMask("BOUNDARY"));

        // Movement in y-axis. Checks collision on y-axis
        if (collision.collider == null) {
            transform.Translate(0, moveDistance.y * Time.deltaTime, 0);    
        }

        collision = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDistance.x,0), 
                                      Mathf.Abs(moveDistance.x * Time.deltaTime), LayerMask.GetMask("BOUNDARY"));
        // Movement in x-axis. Checks collision on y-axis
        if (collision.collider == null) {
            transform.Translate(moveDistance.x * Time.deltaTime, 0, 0);    
        }
        
    }
}
