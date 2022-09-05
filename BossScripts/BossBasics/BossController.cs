using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Boss")]
    public BossClass bossEntity;
    public Player player;
    [SerializeField] private OnCollideTrigger trigger;

    [SerializeField] private int threshold;
    [SerializeField] private bool playerInRange;

    BossBaseState currentState;
    BossHealthyState healthyState;
    BossWeakState weakState;
    BossAttackState attackState;
    BossChaseState chaseState;

    //Getters and Setters
    public BossBaseState CurrentState {get {return currentState;} set {currentState = value;}}
    public BossHealthyState HealthyState {get {return healthyState;}}
    public BossWeakState WeakState {get {return weakState;}}
    public BossAttackState AttackState {get {return attackState;}}
    public BossChaseState ChaseState {get {return chaseState;}}
    public int Threshold {get {return threshold;}}
    public bool PlayerInRange {get {return playerInRange;}}

    void Awake() {
        playerInRange = false;

        healthyState = new BossHealthyState(this);
        weakState = new BossWeakState(this);
        attackState = new BossAttackState(this);
        chaseState = new BossChaseState(this);

        currentState = healthyState;

        bossEntity.StartEncounter();
        currentState.EnterState();
    }

    void FixedUpdate() {
        if (bossEntity.initializing == true) {
            return;
        }

        if (trigger.playerInside) {
            playerInRange = true;
        }
        else if (!trigger.playerInside) {
            playerInRange = false;
        }

        Debug.Log(currentState);
        currentState.UpdateStates();
    }
}
