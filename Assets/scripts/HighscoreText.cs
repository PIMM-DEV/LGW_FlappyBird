using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Text))]
public class HighscoreText : MonoBehaviour
{
    TMP_Text highscore;

    private void OnEnable()
    {
        highscore = GetComponent<TMP_Text>();
        highscore.text = "High Score: "+ PlayerPrefs.GetInt("HighScore").ToString();
    }
}
