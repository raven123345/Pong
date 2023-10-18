using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bonus_GivePoints : MonoBehaviour
{
    [SerializeField]
    WhoToGiveBonus whoToGiveBonus;
    
    public int pointsToGive = 2;
    [SerializeField]
    TMP_Text text;
    void Start()
    {
        text.text = pointsToGive.ToString();
    }
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void GivePoints()
    {
        switch (whoToGiveBonus._whotoGiveBonus)
        {
            case 1:
                GameManager.instance.AddScorePoints(1, pointsToGive);

                break;
            case 2:
                GameManager.instance.AddScorePoints(2, pointsToGive);
                break;
        }

        DestroyObject();
    }
}