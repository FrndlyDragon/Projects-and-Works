using UnityEngine;

public class BossHealthyState : BossBaseState
{
    public BossHealthyState(BossController controller) : base(controller){
        stageID = 1;
    }
    public override void EnterState() {
        Debug.Log("Entering Healthy State");

        InitializeSubState();

        Debug.Log("Current SubStates " + subState);
        Debug.Log("Current SuperState " + superState);
    }
    public override void UpdateState() {
        CheckSwitchState();
    }
    public override void CheckSwitchState() {
        if (controller.bossEntity.BossHealth <= controller.Threshold) {
            SwitchState(controller.WeakState);
        }
    }
    public override void ExitState() {
        Debug.Log("Exiting Healthy State");
    }

    public override void InitializeSubState() {
        if (!controller.PlayerInRange) {
            SetSubState(controller.ChaseState);
        }
        else {
            SetSubState(controller.AttackState);
        }
    }

}
