using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bonus_GivePoints : MonoBehaviour
{
    [SerializeField]
    WhoToGiveBonus whoToGiveBonus;
    [SerializeField]
    int pointsToGive = 2;
    [SerializeField]
    TMP_Text text;
    void Start()
    {
        text.text = pointsToGive.ToString();

        switch (whoToGiveBonus._whotoGiveBonus)
        {
            case 1:
                GameManager.firstPlayerScore += pointsToGive;
                break;
            case 2:
                GameManager.secondPlayerScore += pointsToGive;
                break;
        }
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}