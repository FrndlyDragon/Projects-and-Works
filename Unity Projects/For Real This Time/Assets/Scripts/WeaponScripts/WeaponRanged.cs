using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRanged : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Projectile projectile;
    public Player player;
    public Camera cam;
    public Transform firepoint;

    public string dmgType;
    public float weaponDmg;

    public float attackSpd;
    public float lastAtk;
    public float bulletVel;
    public Vector3 mousePos;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.instance.isPlaying) {
            return;
        }

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - firepoint.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        firepoint.rotation = Quaternion.Euler(0f, 0f, angle);

        if (Input.GetMouseButton(0)) {
            if (Time.time - lastAtk > attackSpd) {
                lastAtk = Time.time;
                Shoot();
            }
        }
    }

    void Shoot() {
        Projectile bullet = Instantiate(projectile, player.transform.position, firepoint.rotation);

        bullet.dmgType = this.dmgType;
        bullet.weaponDmg = this.weaponDmg;
        bullet.velocity = this.bulletVel;
        bullet.target = "Enemy";

        bullet.direction = firepoint.up;
    }

}
