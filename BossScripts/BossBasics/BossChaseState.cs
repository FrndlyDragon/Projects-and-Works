using UnityEngine;

public class BossChaseState : BossBaseState
{
    private float timeSpent;
    private float timeLimit;
    public BossChaseState(BossController controller) : base(controller){
        stageID = 0;
        timeLimit = 5f;
    }
    public override void EnterState() {
        Debug.Log("Entering Chase State");
        Debug.Log("Current SubStates " + subState);
        Debug.Log("Current SuperState " + superState);

        timeSpent = 0f;
    }
    public override void UpdateState() {
        CheckSwitchState();
        ChasePlayer(controller.player.transform);
    }
    public override void CheckSwitchState() {
        if (controller.PlayerInRange) {
            SwitchState(controller.AttackState);
        }
        else if (timeSpent >= timeLimit) {
            SwitchState(controller.AttackState);
        }
        else {
            timeSpent += Time.deltaTime;
        }
    }
    public override void ExitState() {
        Debug.Log("Exiting Chase State");
    }

    public override void InitializeSubState() {}

    void ChasePlayer(Transform player) {
        Debug.Log("Chasing Player");
        Vector2 distance = player.position - controller.bossEntity.transform.position;
        distance.Normalize();
        controller.bossEntity.bossRB.MovePosition((Vector2)controller.bossEntity.transform.position + (distance * Time.deltaTime * 10));
    }
}
