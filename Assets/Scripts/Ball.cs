using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    [SerializeField]
    enum OwnerPlayer
    { PlayerOne, PlayerTwo };
    [SerializeField]
    OwnerPlayer owner;

    [SerializeField]
    float speed = 200f;
    [SerializeField]
    float levelTime = 60f;
    [SerializeField]
    float levelStep = 100f;
    [SerializeField]
    AudioClip[] sounds;

    private Rigidbody2D rb;
    private Vector2 flyDirection;
    private bool gameStop;
    private float levelCounter = 0f;
    private AudioSource _audioSource;
    private SpriteRenderer spriteRend;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        flyDirection.Normalize();

        levelCounter += Time.deltaTime;
        if (levelCounter >= levelTime)
        {
            speed += levelStep;
            levelCounter = 0f;
        }
    }

    public void ReleaseBall()
    {
        float i = Random.Range(-1f, 1f);
        gameStop = false;
        switch (owner)
        {
            case OwnerPlayer.PlayerOne:
                {
                    if (i <= 0f) { flyDirection = new Vector2(1f, -1f); }
                    else { flyDirection = new Vector2(1f, 1f); }

                    break;
                }
            case OwnerPlayer.PlayerTwo:
                {
                    if (i <= 0f) { flyDirection = new Vector2(-1f, -1f); }
                    else { flyDirection = new Vector2(-1f, 1f); }

                    break;
                }
        };


        gameObject.transform.SetParent(null);

        _audioSource.PlayOneShot(sounds[0]);
    }

    private void FixedUpdate()
    {
        if (!gameStop)
            rb.velocity = flyDirection * speed * Time.deltaTime;
        else
            rb.velocity = Vector2.zero;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag != "LeftCollider" && collision.transform.tag != "RightCollider")
        {
            flyDirection = Vector2.Reflect(flyDirection, collision.GetContact(0).normal);
            _audioSource.PlayOneShot(sounds[0]);
        }
        else
        {
            gameStop = true;
            _audioSource.PlayOneShot(sounds[1]);
            StartCoroutine(reloadLevelCountdown());
            if (collision.transform.tag == "LeftCollider")
            {
                GameManager.secondPlayerScore++;
                GameManager.instance.changeScore.Invoke();
            }
            else if (collision.transform.tag == "RightCollider")
            {
                GameManager.firstPlayerScore++;
                GameManager.instance.changeScore.Invoke();
            }
        }
        if (collision.transform.tag == "PlayerOne" || collision.transform.tag == "PlayerTwo" || collision.transform.tag == "PlayerComputer")
        {
            spriteRend.color = collision.gameObject.GetComponent<PlayerController>().playerColor;
        }
    }

    IEnumerator reloadLevelCountdown()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
