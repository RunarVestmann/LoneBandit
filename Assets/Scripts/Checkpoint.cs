using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    Animator animator;

    [SerializeField] UnityEvent<Vector3> spawnLocationEvent;
    [SerializeField] AudioSource audioSource;

    bool isActive;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive) return;
        var player = other.GetComponent<Player>();
        if (!player) return;
        animator.Play("Active");
        isActive = true;
        audioSource.Play();
        spawnLocationEvent.Invoke(transform.position);
    }


}
