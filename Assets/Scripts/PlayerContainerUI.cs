using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerContainerUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Image healthBarFill;

    public void Initialize (Color color)
    {
        scoreText.color = color;
        healthBarFill.color = color;

        scoreText.text = "0";
        healthBarFill.fillAmount = 1.0f;
    }

    public void UpdateScoreText (int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateHealthBar (int curHp, int maxHp)
    {
        healthBarFill.fillAmount = (float)curHp / (float)maxHp;
    }
}