using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class BossClass: MonoBehaviour
{
    [SerializeField] protected int health;
    public Rigidbody2D bossRB;
    public BoxCollider2D bossHitBox;
    public Player player;
    public Damage dmgCalc;
    public bool initializing;
 
    public int BossHealth { get {return health;}} 
    public Damage DmgCalc { get {return dmgCalc;}}

    public abstract void StartEncounter();
    public abstract void ReceiveDamage(float dmgAmount, string enemyType, float pushforce);
    public abstract void Death();
    public abstract AttackMoves SelectRangeMove();
    public abstract AttackMoves SelectMeleeMove();
    public abstract void ExecuteMeleeMove(Player player, int stageID);
    public abstract void ExecuteRangeMove(Player player, int stageID);
}
