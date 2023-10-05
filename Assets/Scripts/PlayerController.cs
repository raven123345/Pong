using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum Player
    {
        PlayerOne, PlayerTwo, Computer
    };

    public Player _player;
    public Color playerColor;
    private RG_Inputs inputActions;
    private InputAction _UpDown;

    private float offset;

    private Vector3 initialPosition;
    private Vector2 boxColliderSize;

    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    Camera cam;
    [SerializeField]
    int _XOffset = 20;
    [SerializeField]
    Transform otherPlayer;
    [SerializeField]
    Transform ball;

    private void Awake()
    {
        inputActions = new RG_Inputs();
    }
    private void OnEnable()
    {
        switch (_player)
        {
            case Player.PlayerOne:
                _UpDown = inputActions.PlayerOne.Move;
                _UpDown.Enable();
                break;
            case Player.PlayerTwo:
                _UpDown = inputActions.PlayerTwo.Move;
                _UpDown.Enable();
                break;
            case Player.Computer:
                break;
        }
    }

    private void OnDisable()
    {
        switch (_player)
        {
            case Player.PlayerOne:
                _UpDown.Disable();
                break;
            case Player.PlayerTwo:
                _UpDown.Disable();
                break;
            case Player.Computer:
                break;
        }
    }
    void Start()
    {
        boxColliderSize = GetComponent<BoxCollider2D>().size;

        if (gameObject.transform.tag == "Second" && GameManager.numPlayers == 2)
        {
            _player = Player.PlayerTwo;
        }
        else if (gameObject.transform.tag == "Second")
        {
            _player = Player.Computer;
        }

        switch (_player)
        {
            case Player.PlayerOne:
                transform.position = cam.ScreenToWorldPoint(new Vector3(_XOffset, cam.pixelHeight * 0.5f, 0f));
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                initialPosition = transform.position;
                break;

            case Player.PlayerTwo:
                transform.position = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth - _XOffset, cam.pixelHeight * 0.5f, 0f));
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                initialPosition = transform.position;
                break;

            case Player.Computer:
                transform.position = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth - _XOffset, cam.pixelHeight * 0.5f, 0f));
                transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
                initialPosition = transform.position;
                break;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (_player == Player.Computer)
        {
            MoveComputer();
        }
        else
        {
            Move();
        }

    }

    void Move()
    {
        Vector3 worldToViewportMax = cam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + (boxColliderSize.y * 0.5f), transform.position.z));
        Vector3 worldToViewportMin = cam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - (boxColliderSize.y * 0.5f), transform.position.z));

        if (worldToViewportMax.y < cam.pixelHeight && worldToViewportMin.y > 0)
        {
            offset += _UpDown.ReadValue<float>() * speed * Time.deltaTime;

            transform.position = new Vector3(initialPosition.x, initialPosition.y + offset, initialPosition.z);
        }
        else if (worldToViewportMax.y >= cam.pixelHeight)
        {
            if (_UpDown.ReadValue<float>() < 0)
            {
                offset += _UpDown.ReadValue<float>() * speed * Time.deltaTime;
                transform.position = new Vector3(initialPosition.x, initialPosition.y + offset, initialPosition.z);
            }
        }
        else if (worldToViewportMin.y <= 0)
        {
            if (_UpDown.ReadValue<float>() > 0)
            {
                offset += _UpDown.ReadValue<float>() * speed * Time.deltaTime;
                transform.position = new Vector3(initialPosition.x, initialPosition.y + offset, initialPosition.z);
            }
        }
    }

    void MoveComputer()
    {
        Vector3 worldToViewportMax = cam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + (boxColliderSize.y * 0.5f), transform.position.z));
        Vector3 worldToViewportMin = cam.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - (boxColliderSize.y * 0.5f), transform.position.z));
        float moveDir = 0;
        float otherPlayerDist = (otherPlayer.position - transform.position).sqrMagnitude;
        float ballDist = (ball.position - transform.position).sqrMagnitude;

        if (ballDist <= (otherPlayerDist * 0.10f))
        {
            moveDir = Mathf.Clamp(ball.position.y - transform.position.y, -1f, 1f);
            //Debug.Log("y Dist : " + (ball.position.y - transform.position.y));

            if (worldToViewportMax.y < cam.pixelHeight && worldToViewportMin.y > 0)
            {

                offset += moveDir * speed * Time.deltaTime;

                transform.position = new Vector3(initialPosition.x, initialPosition.y + offset, initialPosition.z);
            }
            else if (worldToViewportMax.y >= cam.pixelHeight)
            {
                if (moveDir < 0)
                {
                    offset += moveDir * speed * Time.deltaTime;
                    transform.position = new Vector3(initialPosition.x, initialPosition.y + offset, initialPosition.z);
                }
            }
            else if (worldToViewportMin.y <= 0)
            {
                if (moveDir > 0)
                {
                    offset += moveDir * speed * Time.deltaTime;
                    transform.position = new Vector3(initialPosition.x, initialPosition.y + offset, initialPosition.z);
                }
            }
        }
    }
}
