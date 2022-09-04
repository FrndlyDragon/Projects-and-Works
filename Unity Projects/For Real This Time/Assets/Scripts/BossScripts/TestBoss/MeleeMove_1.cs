using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMove_1 : AttackMoves
{
    public override void Set() {
        ranged = false;
        moveName = "MeleeMove_1";
        stage = 1;
        dmgBase = 1;
        dmgType = "Normal";
        this.dmgCalc = boss.DmgCalc;
    }
    public override void ExecuteMove(Player player, int StageID)
    {
        Set();
        dmgOut = dmgCalc.dmgOutput(dmgBase, "Normal", dmgType);
        Debug.Log("Execute " + moveName);
    }
}
