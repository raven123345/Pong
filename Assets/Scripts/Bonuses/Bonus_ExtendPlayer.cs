using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus_ExtendPlayer : MonoBehaviour
{
    WhoToGiveBonus _whoToGiveBonus;
    [SerializeField]
    float bonusTime = 20f;
    [SerializeField]
    float timeBeforDie = 1f;

    [SerializeField]
    SpriteRenderer sr;

    [SerializeField]
    BoxCollider2D col;

    [Header("Bonus data")]
    [SerializeField]
    Sprite bonusSprite;
    [SerializeField]
    BoxCollider2D bonusCollider;
    Sprite initSprite;
    BoxCollider2D initCollider;

    Color initColor; // to be sprite?
    Color hitColor; // to be sprite?



    void Start()
    {
        _whoToGiveBonus = GetComponent<WhoToGiveBonus>();
        initColor = sr.color;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Ball"))
        {
            sr.color = hitColor;
            StartCoroutine(HitCountDown());
            StartCoroutine(DisableCountDown(gameObject.GetComponent<WhoToGiveBonus>()._whotoGiveBonus));
        }
    }

    IEnumerator HitCountDown()
    {
        yield return new WaitForSeconds(0.1f);
        sr.color = initColor;
    }

    IEnumerator DisableCountDown(int whoToGiveBonus)
    {
        yield return new WaitForSeconds(timeBeforDie);

        sr.enabled = false;
        col.enabled = false;

        StartCoroutine(BonusTimeout(bonusTime));
    }

    IEnumerator BonusTimeout(float bonusTimeout)
    {
        yield return new WaitForSeconds(bonusTimeout);

        Destroy(gameObject);
    }
}
