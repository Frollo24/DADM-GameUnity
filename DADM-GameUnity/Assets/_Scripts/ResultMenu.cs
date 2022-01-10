using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultMenu : MonoBehaviour
{
    public static bool HasWon = false;

    public GameObject resultPanel;
    public GameObject resultMenu;
    public Text resultText;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameManager.IsPlayerDead || HasWon)
        {
            if (HasWon)
            {
                resultText.text = "YOU WON";
                Time.timeScale = 0f;
            }

            resultPanel.SetActive(true);
            resultMenu.SetActive(true);
        }
    }
}
