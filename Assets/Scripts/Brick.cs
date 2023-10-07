using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField]
    int hitNumbers = 1;
    [SerializeField]
    int points = 1;
    [SerializeField]
    float timeBeforDie = 1f;
    [SerializeField]
    Color hitColor;
    [SerializeField]
    GameObject[] bonusObjects;


    Color initColor;

    SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        initColor = sr.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ball"))
        {
            hitNumbers--;
            sr.color = hitColor;
            StartCoroutine(HitCountDown());

            if (hitNumbers <= 0)
            {
                int whoToGive = 0;

                switch (collision.gameObject.GetComponent<Ball>().owner)
                {
                    case Ball.OwnerPlayer.PlayerOne:
                        whoToGive = 1;
                        break;
                    case Ball.OwnerPlayer.PlayerTwo:
                        whoToGive = 2;
                        break;
                }
                StartCoroutine(DieCountDown(whoToGive));
            }
        }
    }

    IEnumerator HitCountDown()
    {
        yield return new WaitForSeconds(0.1f);
        sr.color = initColor;
    }
    IEnumerator DieCountDown(int whoToGiveBonus)
    {
        yield return new WaitForSeconds(timeBeforDie);
        GameManager.instance.AddScorePoints(1, points);

        var bonus = Instantiate(bonusObjects[Random.Range(0, bonusObjects.Length)]);
        bonus.GetComponent<WhoToGiveBonus>()._whotoGiveBonus = whoToGiveBonus;

        Destroy(gameObject);
    }
}
