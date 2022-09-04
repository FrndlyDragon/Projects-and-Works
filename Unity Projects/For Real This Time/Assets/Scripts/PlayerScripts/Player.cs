using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Collision
{
    // Health 
    public int health;
    public Text hpText;

    public Damage dmgTaken;

    // Currency
    public int currCoins;

    // Experience
    public int exp;
    public int level;

    // Player immunity
    protected float dmgCD = 0.5f;
    protected float lastHit;

    // Player Sprite
    SpriteRenderer playerSprite;

    protected override void Start() {
        base.Start();
        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>(); 
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnCollision(Collider2D coll) {
        
    }

    protected void Death() {
        Debug.Log("Death");
    }

    public void ReceiveDamage(float dmgAmount, string dmgType, float pushforce) {
        if (Time.time - lastHit > dmgCD) {
            lastHit = Time.time;

            float dmg = dmgTaken.dmgOutput(dmgAmount, dmgType, "Normal");
            health -= (int)dmg;

            if (health <= 0) {
                health = 0;
                Death();
                return;
            }
            else {
                hpText.SendMessage("updateHealth");
            }
        }
    }

    protected void addCoin(int coinAmount) {
        currCoins += coinAmount;
    }

    protected void minusCoin(int coinAmount) {
        currCoins -= coinAmount;
    }
}
