using UnityEngine;

public class BossAttackState : BossBaseState
{
    private int maxRangeAttacks;
    private int countRangeAttacks;
    private bool meleeMoveExecuted;
    private float attackSpeed = 3f;
    private float lastAttack;
    private bool canAttack;
    public BossAttackState(BossController controller) : base(controller){
        stageID = 0;
        countRangeAttacks = 0;
    }
    public override void EnterState() {
        Debug.Log("Entering Attack State");
        Debug.Log("Current SubStates " + subState);
        Debug.Log("Current SuperState " + superState);
        
        meleeMoveExecuted = false;
        canAttack = true;
        maxRangeAttacks = Random.Range(1, 4);
    }
    public override void UpdateState() {
        stageID = superState.StageID;

        if (Time.time - lastAttack >= attackSpeed) {
            canAttack = true;
        }

        if (!controller.PlayerInRange && canAttack) {
            controller.bossEntity.ExecuteRangeMove(controller.player, stageID);
            countRangeAttacks++;
            canAttack = false;
            lastAttack = Time.time;
        }
        else if (controller.PlayerInRange && canAttack) {
            controller.bossEntity.ExecuteMeleeMove(controller.player, stageID);
            meleeMoveExecuted = true;
            canAttack = false;
            lastAttack = Time.time;
        }
        
        CheckSwitchState();
    }
    public override void CheckSwitchState() {
        if (countRangeAttacks == maxRangeAttacks) {
            SwitchState(controller.ChaseState);
        }
        else if (meleeMoveExecuted && !controller.PlayerInRange) {
            SwitchState(controller.ChaseState);
        }
    }
    public override void ExitState() {
        countRangeAttacks = 0;
        meleeMoveExecuted = false;
        Debug.Log("Exiting Attack State");
    }

    public override void InitializeSubState() {}
}
