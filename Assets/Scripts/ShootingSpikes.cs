using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSpikes : MonoBehaviour
{
    Rigidbody2D rb2d;
    Transform transform;
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();


    }

    public void Shoot(Vector2 direction, float speed, float rotateX)
    {
        transform.Rotate(rotateX, 0.0f, 0.0f, Space.Self);
        rb2d.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Hit");
    }
}
