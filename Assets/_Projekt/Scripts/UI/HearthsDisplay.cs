using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HearthsDisplay : MonoBehaviour
{
    public Sprite heartFullSprite;
    public Sprite heartHalfSprite;
    public Sprite heartEmptySprite;
    public GameObject DeathText;
    public Image[] hearthsImages;

    public void Die() {
        DeathText.SetActive(true);
    }

    public void SetValue(int value)
    {
        for(int i = 0; i < hearthsImages.Length; i++)
        {
            if      (i < value / 2)       hearthsImages[i].sprite = heartFullSprite;
            else if (i < (value + 1) / 2) hearthsImages[i].sprite = heartHalfSprite;
            else                          hearthsImages[i].sprite = heartEmptySprite;
        }
    }
}
