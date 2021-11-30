using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    Animator animator;
    Transform transform;

    [SerializeField] UnityEvent<Vector3> spawnLocationEvent;

    bool isActive;

    void Start()
    {
        animator = GetComponent<Animator>();
        transform = GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isActive) return;
        animator.Play("Active");
        isActive = true;
        spawnLocationEvent.Invoke(transform.position);
    }


}
