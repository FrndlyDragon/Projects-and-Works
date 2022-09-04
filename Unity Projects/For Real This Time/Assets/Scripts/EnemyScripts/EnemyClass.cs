using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    public bool isRanged;
    public EnemyRange rangeAtk;
    // Base stats
    public int health;

    public float damage;
    public Damage dmgTaken;
    public string dmgType;

    public int expGive;

    public float atkSpeed;
    private float lastAttack;
    public float pushforce;

    // Immunity
    public float immuneTime;
    public float lastImmune;
    private Vector3 temp;

    // Collision
    public ContactFilter2D filter;
    private Collider2D attackArea;
    private Collider2D[] collisions = new Collider2D[10];

    private SpriteRenderer enemySprite;

    //Player Reference
    public Player player;
    
    private void Start()
    {
        attackArea = GetComponentInChildren<CircleCollider2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        Vector3 temp = transform.localScale;
    }

    private void Update()
    {
        attackArea.OverlapCollider(filter, collisions);
        for (int i = 0; i < collisions.Length; i++) {
            if (collisions[i] == null) {
                continue;
            }

            else {
                OnCollision(collisions[i]);
                collisions[i] = null;
            }
        }
    }

    private void OnCollision(Collider2D coll) {
        if (coll.gameObject.tag == "Player") {
            if (!isRanged) {
                if (Time.time - lastAttack > atkSpeed) {
                    Player temp = coll.gameObject.GetComponent<Player>();
                    temp.ReceiveDamage(damage, dmgType, pushforce);
                    lastAttack = Time.time;
                }
            }
            else if (isRanged) {
                if (Time.time - lastAttack > atkSpeed) {
                    rangeAtk.Fire();
                    lastAttack = Time.time;
                }
            }
        }
    }

    protected void Death() {
        Debug.Log("Death");
        player.exp += expGive;
        Destroy(gameObject);

    }

    public void ReceiveDamage(float dmgAmount, string enemyType, float pushforce) {
        if (Time.time - lastImmune > immuneTime) {
            
            AudioManager.instance.playSound("AttackHit");
            AudioManager.instance.playSound("EnemyTakeDamage");

            if (health - 1 <= 0) {
                health = 0;
                Death();
            }
            else {
                lastImmune = Time.time;
                float dmg = dmgTaken.dmgOutput(dmgAmount, enemyType, dmgType);
                health -= (int)dmg;

                StartCoroutine(TakeDamage());
            }
            
        }
    }

    IEnumerator TakeDamage() {
        enemySprite.color = Color.red;

        yield return new WaitForSeconds(immuneTime);

        enemySprite.color = Color.white;    
    }

}
