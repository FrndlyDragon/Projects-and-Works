using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public ContactFilter2D filter;
    private Collider2D boxCollider;
    private Collider2D[] collisions = new Collider2D[10];
    public bool isCollisions;

    protected virtual void Start()
    {
        boxCollider = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        boxCollider.OverlapCollider(filter, collisions);
        for (int i = 0; i < collisions.Length; i++) {
            if (collisions[i] == null) {
                isCollisions = false;
                continue;
            }

            else {
                OnCollision(collisions[i]);
                collisions[i] = null;
                isCollisions = true;
            }
        }
    }

    protected virtual void OnCollision(Collider2D coll) {
        Debug.Log(coll + " + " + boxCollider);
    }

    protected virtual void revertToPrevious(bool revertToPrev) {
        Debug.Log("Revert");
    }
}
