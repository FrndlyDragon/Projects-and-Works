using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    // Damage Information
    private string dmgType;
    private float dmgAmount;

    // References
    public Hashtable elementChart = new Hashtable(){
        {"Fire", 1},
        {"Grass", 2},
        {"Water", 3}
    };


    // Final Output
    public float dmgOut;

    // Will eventually run calculations given various variables for full output
    // Complete calculations will be done by individual ReceiveDamage functions
    public float dmgOutput(float dmgAmount, string enemyType, string thisType) {
        if (enemyType.Equals("Normal") || thisType.Equals("Normal")) {
            dmgOut = dmgAmount * 1f;
        }
        else if (typeAdvantage(enemyType, thisType) == 1) {
            dmgOut = dmgAmount * 2f;
        }
        else if (typeAdvantage(enemyType, thisType) == -1) {
            dmgOut = dmgAmount * 0.5f;
        }

        return dmgOut;
    }

    public int typeAdvantage(string enemyType, string thisType) {
        int baseType = (int)elementChart[thisType];
        int compareType = (int)elementChart[enemyType];
        int elementChartSize = elementChart.Count;

        if (baseType == elementChartSize) {
            if (compareType == 1) {
                return 1;
            }
        }
        
        if (compareType < baseType) {
            return -1;
        }
        else if (compareType == baseType) {
            return 0;
        }
        else if (compareType > baseType) {
            return 1;
        }

        // Dummy return/never gets called
        return -1;
    }
}
