using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackMoves : MonoBehaviour
{
    public BossClass boss;
    public Damage dmgCalc;
    public string moveName;
    public bool ranged;
    // Stage will determine whether move should execute based on current parent state. 
    public int stage;

    protected float dmgBase;
    protected string dmgType;
    protected float dmgOut;

    public abstract void Set();
    public abstract void ExecuteMove(Player player, int StageID);
}
