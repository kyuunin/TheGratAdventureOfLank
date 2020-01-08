using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateScript : MonoBehaviour
{
    public TMPro.TextMeshPro text;

    void Update()
    {
        int diff = CoinScript.GetRequiredCoinCount() - CoinScript.CoinsCollected;
        if (diff < 0) diff = 0;
        if (text == null)
        {
            Debug.Log("text is null");
            return;
        }
        text.SetText("You Need\n" + diff + " more\nCoins!");

        if(diff == 0)
        {
            Destroy(gameObject, 1.0f);
        }
    }
}
