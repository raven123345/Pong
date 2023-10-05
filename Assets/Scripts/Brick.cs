using System.Collections;
using System.Collections.Generic;
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
                StartCoroutine(DieCountDown());
        }
    }

    IEnumerator HitCountDown()
    {
        yield return new WaitForSeconds(0.1f);
        sr.color = initColor;
    }
    IEnumerator DieCountDown()
    {
        yield return new WaitForSeconds(timeBeforDie);
        GameManager.instance.AddScorePoints(1, points);
        Destroy(gameObject);
    }
}
