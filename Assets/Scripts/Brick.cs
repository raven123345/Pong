using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField]
    int hitNumbers = 1;
    [SerializeField]
    int brickPoints = 1;
    [SerializeField]
    float timeBeforDie = 1f;
    [SerializeField]
    Color hitColor;
    [SerializeField]
    int chanseToGiveBonus = 20;
    [SerializeField]
    GameObject pointsObject;
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
                StartCoroutine(DieCountDown(gameObject.GetComponent<WhoToGiveBonus>()._whotoGiveBonus));
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
        // GameManager.instance.AddScorePoints(whoToGiveBonus, points);

        if (UnityEngine.Random.Range(0, 101) <= chanseToGiveBonus)
        {
            var bonus = Instantiate(bonusObjects[UnityEngine.Random.Range(0, bonusObjects.Length)], transform.position, quaternion.identity);
            bonus.GetComponent<WhoToGiveBonus>()._whotoGiveBonus = whoToGiveBonus;
        }
        else//no bonus, give points
        {
            var points = Instantiate(pointsObject, transform.position, quaternion.identity);
            points.GetComponent<WhoToGiveBonus>()._whotoGiveBonus = whoToGiveBonus;
            points.GetComponent<Bonus_GivePoints>().pointsToGive = brickPoints;
        }

        Destroy(gameObject);
    }
}
