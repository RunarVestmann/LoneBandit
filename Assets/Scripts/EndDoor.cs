using UnityEngine;
using UnityEngine.Events;

public class EndDoor : MonoBehaviour
{
    [SerializeField] Collider2D endDoorCollider;
    [SerializeField] Animator animator;
    [SerializeField] UnityEvent onEndLevelEvent;
    bool hasOpened;

    void Awake() => animator.Play("EndDoorClosed");

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasOpened) return;
        var player = other.GetComponent<Player>();
        if (!player) return;
        hasOpened = true;
        animator.Play("EndDoorOpen");
        onEndLevelEvent.Invoke();
    }
}