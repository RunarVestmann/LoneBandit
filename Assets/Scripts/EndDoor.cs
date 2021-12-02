using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndDoor : MonoBehaviour
{
    [SerializeField] Collider2D endDoorCollider;
    [SerializeField] Animator animator;
    [SerializeField] UnityEvent onEndLevelEvent;

    void Awake() => animator.Play("EndDoorClosed");

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("ahaha");
        var player = collider.GetComponent<Player>();
        if (!player) return;
        animator.Play("EndDoorOpen");
    }
}