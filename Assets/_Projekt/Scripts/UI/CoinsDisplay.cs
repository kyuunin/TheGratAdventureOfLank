using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsDisplay : MonoBehaviour
{
    public Text text;
    
    void Update()
    {
        text.text = "" + CoinScript.CoinsCollected + " / " + CoinScript.GetRequiredCoinCount();
    }
}
