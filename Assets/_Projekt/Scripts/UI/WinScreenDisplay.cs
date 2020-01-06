using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreenDisplay : MonoBehaviour
{
    public Text winText;

    private void Start()
    {
        winText.gameObject.SetActive(false);
    }

    private bool isWon = false;
    public void Win()
    {
        isWon = true;
        winText.gameObject.SetActive(true);
        GameObject.FindObjectOfType<MusicManager>().PlayWinMusic();
    }

    private void Update()
    {
        if (isWon)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                CoinScript.Reset();
            }
        }
    }
}
