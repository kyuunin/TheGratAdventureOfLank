using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// idea for this script from http://answers.unity.com/answers/553638/view.html

public class CheatCodeManager : MonoBehaviour
{
    private static readonly string[] cheats =
    {
        "eyeofgod",
        "holywater",
        "pileofcash",
        "goodbyebluesky"
    };
    private static readonly float inputTimeout = 1.0f;
    
    // stores index of next char in cheat to check
    private int[] cheatIndex = new int[cheats.Length];
    private float lastTypedTime = 0;
    
    private void DoCheat(int i)
    {
        if (i == 0)
            GameObject.FindObjectOfType<LevelGen>().ActiveMap();
        if (i == 1)
            GameObject.FindObjectOfType<MainCharMovementController>().RecoverFullHealth();
        if (i == 2)
            CoinScript.CheatAllCoins();
        if (i == 3)
            GameObject.FindObjectOfType<MainCharMovementController>().Die();
    }

    void Start()
    {
        for (int i = 0; i < cheats.Length; i++)
            cheatIndex[i] = 0;
    }
    
    void Update()
    {
        if(Input.anyKeyDown)
        {
            if(Time.realtimeSinceStartup - lastTypedTime > inputTimeout)
                for (int i = 0; i < cheats.Length; i++)
                    cheatIndex[i] = 0;
            lastTypedTime = Time.realtimeSinceStartup;

            for(int i = 0; i < cheats.Length; i++)
            {
                if (Input.GetKeyDown(cheats[i][cheatIndex[i]].ToString()))
                {
                    cheatIndex[i]++;
                    if (cheatIndex[i] == cheats[i].Length)
                    {
                        DoCheat(i);
                        cheatIndex[i] = 0;
                    }
                }
                else cheatIndex[i] = 0;
            }
        }
    }
}
