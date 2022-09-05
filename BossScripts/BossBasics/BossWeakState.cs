using UnityEngine;

public class BossWeakState : BossBaseState
{
    public BossWeakState(BossController controller) : base(controller){
        stageID = 2;
    }
    public override void EnterState() {
        Debug.Log("Entering Weak State");

        InitializeSubState();

        Debug.Log("Current SubStates" + subState);
        Debug.Log("Current SuperState" + superState);
    }
    public override void UpdateState() {
        CheckSwitchState();
    }
    public override void CheckSwitchState() {
        if (controller.bossEntity.BossHealth > controller.Threshold) {
            SwitchState(controller.HealthyState);
        }
    }
    public override void ExitState() {
        
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
