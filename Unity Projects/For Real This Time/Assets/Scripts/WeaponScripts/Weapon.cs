using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collision
{
    //Used to pause things
    public Settings settingsPause;


    public float weaponDmg;
    public string damageType;
    public Damage dmgOutput;

    // Attack
    public float attackSpd;
    public float lastAtk;
    public float pushforce;
    public Animator swingAnim;

    protected override void Start(){
        base.Start(); 
        swingAnim = GetComponent<Animator>();
    }

    protected override void Update() {
        if (settingsPause.isOpen || DialogueManager.instance.isPlaying) {
            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            swing();
        }
        
        base.Update();
    }

    private void swing() {
        if (Time.time - lastAtk > attackSpd) {
            lastAtk = Time.time;
            swingAnim.SetTrigger("Swing");
            Debug.Log("Swing");
            AudioManager.instance.playSound("AttackSwing");
            
        }
    }

    protected override void OnCollision(Collider2D coll) {
        if (coll.tag == "Enemy") {
            if (coll.tag == "Player") {
                return;
            }

            if (coll.gameObject.GetComponent<EnemyClass>() != null) {
                Debug.Log("Enemy Detected");
                EnemyClass temp = coll.gameObject.GetComponent<EnemyClass>();
                temp.ReceiveDamage(weaponDmg, damageType, pushforce);
                return;
            }
            
            else if (coll.gameObject.GetComponent<BossClass>() != null) {
                BossClass temp = coll.gameObject.GetComponent<BossClass>();
                temp.ReceiveDamage(weaponDmg, damageType, pushforce);
                return;
            }
        }  
    }  
}
