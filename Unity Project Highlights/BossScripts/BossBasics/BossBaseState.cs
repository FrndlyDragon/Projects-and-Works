using UnityEngine;

public abstract class BossBaseState
{
    protected BossController controller;
    protected BossBaseState subState;
    protected BossBaseState superState;
    protected int stageID;

    public int StageID {get {return stageID;}}
    
    public BossBaseState(BossController controller){
        this.controller = controller;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public void UpdateStates() {
        UpdateState();
        if (subState != null) {
            subState.UpdateStates();
        }
    }
    public abstract void CheckSwitchState();
    public abstract void ExitState();
    public abstract void InitializeSubState();
    protected void SetSubState(BossBaseState newSubState) {
        subState = newSubState;
        newSubState.SetSuperState(this);
    }
    protected void SetSuperState(BossBaseState newSuperState) {
        superState = newSuperState;
    }
    protected void SwitchState(BossBaseState newState) {
        ExitState();

        newState.EnterState();
        
        if (superState != null) {
            Debug.Log("Transfering superState");
            superState.SetSubState(newState);
        }
        else {
            controller.CurrentState = newState;
        }
    }
}
