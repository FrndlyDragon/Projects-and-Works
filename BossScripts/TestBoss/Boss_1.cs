using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_1 : BossClass
{
    [SerializeField] private float immuneTime;
    private float lastImmune;

    public List<AttackMoves> moveSet;

    //Parse range moves
    private List<string> rangeMoveName = new List<string>();
    private Dictionary<string, AttackMoves> rangeMove = new Dictionary<string, AttackMoves>();

    //Parse melee moves
    private List<string> meleeMoveName = new List<string>();
    private Dictionary<string, AttackMoves> meleeMove = new Dictionary<string, AttackMoves>();

    private void IntializingBoss() {
        initializing = true;

        Debug.Log("Sorting Moves");

        int index = 0;
        foreach (AttackMoves move in moveSet) {
            moveSet[index].boss = this;
            moveSet[index].Set();

            if (moveSet[index].ranged) {
                rangeMove.Add(moveSet[index].moveName, moveSet[index]);
                rangeMoveName.Add(moveSet[index].moveName);
            }
            else {
                meleeMove.Add(moveSet[index].moveName, moveSet[index]);
                meleeMoveName.Add(moveSet[index].moveName);
            }

            index++;
        }

        initializing = false;

    }

    public override void StartEncounter()
    {
        Debug.Log("Encounter Started");

        if (moveSet.Count == 0) {
            return;
        }

        IntializingBoss();
    }

    public override void ReceiveDamage(float dmgAmount, string enemyType, float pushforce)
    {
        if (Time.time - lastImmune > immuneTime) {
            Debug.Log("Receiving Damage");
            float dmgTaken = dmgCalc.dmgOutput(dmgAmount, enemyType, "Normal");

            health -= (int)dmgTaken;
            lastImmune = Time.time;
        }
    }


    public override void Death()
    {
        Debug.Log("Death");
    }

    // Returns a random range move
    public override AttackMoves SelectRangeMove() {
        if (rangeMoveName.Count == 0) {
            return null;
        }
        Debug.Log(rangeMoveName.Count);
        int i = Random.Range(0, rangeMoveName.Count);
        return rangeMove[rangeMoveName[i]];
    }

    // Returns a random melee move
    public override AttackMoves SelectMeleeMove() {
        if (meleeMoveName.Count == 0) {
            return null;
        }

        int i = Random.Range(0, meleeMoveName.Count);
        return meleeMove[meleeMoveName[i]];
    }

    public override void ExecuteMeleeMove(Player player, int stageID) {
        AttackMoves move = SelectMeleeMove();
        
        while (stageID != move.stage) {
            move = SelectMeleeMove();
        }

        Debug.Log("Selected move: " + move.moveName);
        Debug.Log("Using move " + move.moveName);
        move.ExecuteMove(player, stageID);
    }

    public override void ExecuteRangeMove(Player player, int stageID) {
        AttackMoves move = SelectRangeMove();

        while (stageID != move.stage) {
            move = SelectRangeMove();
        }

        Debug.Log("Selected move: " + move.moveName);
        Debug.Log("Using move " + move.moveName);
        move.ExecuteMove(player, stageID);
    }
}
