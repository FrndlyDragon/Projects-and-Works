using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : MonoBehaviour
{

    public float damage;
    public string dmgType;
    public Projectile bullet;
    public float projectileSpeed;

    //Player Reference
    public Player player;

    public void Fire() {
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        
        Projectile temp = Instantiate(bullet, transform.position, Quaternion.Euler(0f, 0f, angle));
        
        temp.dmgType = this.dmgType;
        temp.weaponDmg = this.damage;
        temp.velocity = this.projectileSpeed;
        temp.target = "Player";

        temp.direction = direction.normalized;
    }

   
}
