using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] Collider2D coinCollider;
    [SerializeField] Animator animator;
    [SerializeField] UnityEvent onCoinCollect;
    bool hasBeenPickedUp;
    
    void Awake() => animator.Play("CoinSpin");

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (!player || hasBeenPickedUp) return;
        CoinCollection();
    }

    void CoinCollection()
    {
        onCoinCollect.Invoke();
        coinCollider.enabled = false;
        hasBeenPickedUp = true;
        animator.Play("CoinPickup");
        Destroy(gameObject, 3.0f);
    }
}