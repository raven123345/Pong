using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour
{
    public enum OwnerPlayer
    { PlayerOne, PlayerTwo };

    public OwnerPlayer owner;

    [SerializeField]
    float speed = 200f;
    [SerializeField]
    float levelTime = 60f;
    [SerializeField]
    float levelStep = 100f;
    [SerializeField]
    float ballResetTime = 2f;
    [SerializeField]
    AudioClip[] sounds;

    private Rigidbody2D rb;
    private Vector2 flyDirection;
    private bool gameStop;
    private bool goal = false;
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

        // if (levelCounter >= levelTime)
        // {
        //     speed += levelStep;
        //     levelCounter = 0f;
        // }
    }

    public void levelUp()
    {
        speed += levelStep;
    }

    public void ReleaseBall()
    {
        float i = Random.Range(-1f, 1f);
        gameStop = false;
        goal = false;

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
        if (!gameStop && !goal)
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
            _audioSource.PlayOneShot(sounds[1]);
            goal = true;

            if (collision.transform.tag == "LeftCollider") // левият колайдер е зад играч 1
            {
                switch (owner)
                {
                    case OwnerPlayer.PlayerOne:
                        GameManager.firstPlayerScore--;
                        GameManager.instance.changeScore.Invoke();
                        StartCoroutine(resetBallPosition(1, ballResetTime));
                        break;
                    case OwnerPlayer.PlayerTwo:
                        GameManager.secondPlayerScore++;
                        GameManager.instance.changeScore.Invoke();
                        StartCoroutine(resetBallPosition(2, ballResetTime));
                        break;
                }
            }
            else if (collision.transform.tag == "RightCollider") // десният колайдер е зад играч 2
            {
                switch (owner)
                {
                    case OwnerPlayer.PlayerOne:
                        GameManager.firstPlayerScore++;
                        GameManager.instance.changeScore.Invoke();
                        StartCoroutine(resetBallPosition(1, ballResetTime));
                        break;
                    case OwnerPlayer.PlayerTwo:
                        GameManager.secondPlayerScore--;
                        GameManager.instance.changeScore.Invoke();
                        StartCoroutine(resetBallPosition(2, ballResetTime));
                        break;
                }
            }
        }
        //Change the color and player label
        if (collision.transform.tag == "PlayerOne" || collision.transform.tag == "PlayerTwo" || collision.transform.tag == "PlayerComputer")
        {
            var pc = collision.gameObject.GetComponent<PlayerController>();
            spriteRend.color = pc.playerColor;

            switch (pc._player)
            {
                case PlayerController.Player.PlayerOne:
                    {
                        owner = OwnerPlayer.PlayerOne;
                        break;
                    }
                case PlayerController.Player.PlayerTwo:
                    {
                        owner = OwnerPlayer.PlayerTwo;
                        break;
                    }
                case PlayerController.Player.Computer:
                    {
                        owner = OwnerPlayer.PlayerTwo;
                        break;
                    }
            }
        }
    }

    IEnumerator reloadLevelCountdown()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //pos = 1 - first player; pos = 2 - second player
    IEnumerator resetBallPosition(int pos, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        switch (pos)
        {
            case 1:
                transform.position = GameManager.instance.firstPlayerBallPosition.position;
                spriteRend.enabled = true;
                ReleaseBall();
                break;
            case 2:
                transform.position = GameManager.instance.secondPlayerBallPosition.position;
                spriteRend.enabled = true;
                ReleaseBall();
                break;
        }

    }
}
