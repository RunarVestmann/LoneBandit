using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    Animator animator;

    [SerializeField] UnityEvent<Vector3> spawnLocationEvent;

    bool isActive;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isActive) return;
        var player = collider.GetComponent<Player>();
        if (!player) return;
        animator.Play("Active");
        isActive = true;
        spawnLocationEvent.Invoke(transform.position);
        Debug.Log(transform.position);
    }


}
