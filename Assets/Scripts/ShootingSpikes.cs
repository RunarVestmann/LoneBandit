using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSpikes : MonoBehaviour
{
    Rigidbody2D rb2d;
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector2 direction, float speed, float timeToLive)
    {
        transform.up = direction;
        rb2d.velocity = direction * speed;
        Destroy(gameObject, timeToLive);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (!player) return;
        Destroy(gameObject);
    }

}
