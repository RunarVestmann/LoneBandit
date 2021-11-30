using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTrap : MonoBehaviour
{
    [SerializeField] GameObject spikesPrefab;
    [SerializeField] Transform spawnPosition;
    [SerializeField] Vector2 direction;
    [SerializeField] float speed;
    [SerializeField] float timeBetweenShots;
    [SerializeField] float rotationX;

    Animator animator;

    float counter = 0f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle");
    }

    void FixedUpdate()
    {
        if (counter > timeBetweenShots)
        {
            animator.Play("Shoot");
            Invoke("Shoot", 0.30f);
            counter = 0f;
            Invoke("PlayIdle", 0.36f);
        }
        else
            counter += Time.deltaTime;
    }

    void PlayIdle()
    {
        animator.Play("Idle");
    }

    public void Shoot()
    {
        var spikesObject = Instantiate(spikesPrefab, spawnPosition.position, Quaternion.identity);
        var spikes = spikesObject.GetComponent<ShootingSpikes>();
        spikes.Shoot(direction, speed, rotationX);
    }
}
