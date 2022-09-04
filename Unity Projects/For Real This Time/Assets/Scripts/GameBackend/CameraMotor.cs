using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform lookat;
    public float boundX = 0.15f;
    public float boundY = 0.05f;

    private void LateUpdate() {
        Vector3 delta = Vector3.zero;

        float deltax = lookat.position.x - transform.position.x;
        if (deltax > boundX || deltax < -boundX) {
            if (transform.position.x < lookat.position.x) {
                delta.x = deltax  - boundX;
            }
            else {
                delta.x = deltax + boundX;
            }
        }

        float deltay = lookat.position.y - transform.position.y;
        if (deltay > boundY || deltay < -boundY) {
            if (transform.position.y < lookat.position.y) {
                delta.y = deltay  - boundY;
            }
            else {
                delta.y = deltay + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
