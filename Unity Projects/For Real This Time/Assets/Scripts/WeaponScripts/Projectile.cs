using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Damage
    public string dmgType;
    public float weaponDmg;
    public string target;

    // Movement
    public float velocity;
    public Vector2 direction;
    public float pushforce;
    public float angle;

    // Customization
    public SpriteRenderer sprite;
    
    void Start() {
        GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        Debug.Log(direction);
        GetComponent<Rigidbody2D>().AddForce(direction * velocity, ForceMode2D.Impulse);
        sprite = GetComponent<SpriteRenderer>();
        
        
    }

    void OnTriggerEnter2D(Collider2D coll) {
        Debug.Log(coll.gameObject);
        if (coll.gameObject.tag == target) {
            GameObject target = coll.gameObject;

            if (this.target == "Enemy") {
                Debug.Log(coll);
                EnemyClass temp = target.GetComponent<EnemyClass>();
                temp.ReceiveDamage(weaponDmg, dmgType, pushforce);
                Destroy(gameObject);
            }
            else if (this.target == "Player") {
                Debug.Log(coll);
                Player temp = target.GetComponent<Player>();
                temp.ReceiveDamage(weaponDmg, dmgType, pushforce);
                Destroy(gameObject);
            }
        }

        if (coll.gameObject.tag == "Boundary"){
                Destroy(gameObject);
            
        }
    }
}
