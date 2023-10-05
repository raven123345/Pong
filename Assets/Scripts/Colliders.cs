using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    public enum Position
    { Top, Bottom, Left, Right }

    public Position _Position;
    public Camera cam;
    private Vector2 screenPosition;
    private BoxCollider2D _collider;
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        screenPosition = cam.WorldToScreenPoint(transform.position);

        switch (_Position)
        {
            case Position.Top:
                transform.position = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * 0.5f, cam.pixelHeight, 0f));
                transform.position = new Vector3(transform.position.x, transform.position.y + (_collider.size.y * 0.5f), 0);
                break;
            case Position.Bottom:
                transform.position = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * 0.5f, 0, 0f));
                transform.position = new Vector3(transform.position.x, transform.position.y - (_collider.size.y * 0.5f), 0);
                break;
            case Position.Left:
                transform.position = cam.ScreenToWorldPoint(new Vector3(0, cam.pixelHeight * 0.5f, 0f));
                transform.position = new Vector3(transform.position.x - (_collider.size.x * 0.5f), transform.position.y, 0);
                break;
            case Position.Right:
                transform.position = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth, cam.pixelHeight * 0.5f, 0f));
                transform.position = new Vector3(transform.position.x + (_collider.size.x * 0.5f), transform.position.y, 0);
                break;
        }
    }
}
