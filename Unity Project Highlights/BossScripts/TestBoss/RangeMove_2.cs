using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeMove_2 : AttackMoves
{
    public override void Set() {
        ranged = true;
        moveName = "RangeMove_2";
        stage = 2;
        dmgBase = 1;
        dmgType = "Normal";
        this.dmgCalc = boss.DmgCalc;
    }
    public override void ExecuteMove(Player player, int StageID)
    {
        dmgOut = dmgCalc.dmgOutput(dmgBase, "Normal", dmgType);
        Debug.Log("Execute " + moveName);
    }
}
