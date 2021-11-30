using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSpikes : MonoBehaviour
{
    [SerializeField] float timeToLive;
    Rigidbody2D rb2d;
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Destroy(gameObject, timeToLive);
    }

    public void Shoot(Vector2 direction, float speed)
    {
        transform.up = direction;
        rb2d.velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        var player = collider.GetComponent<Player>();
        if (!player) return;
        Destroy(gameObject);
    }

}
