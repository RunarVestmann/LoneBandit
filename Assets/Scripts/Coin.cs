using UnityEngine;
using UnityEngine.Events;

public class Coin : MonoBehaviour
{
    [SerializeField] Collider2D coinCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] UnityEvent onCoinCollect;

    // Start playing the hover animation at a random offset so orbs don't hover at the exact same rate
    void Awake()
    {
        animator.Play("CoinSpin");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (!player) return;
        CoinCollection();
    }

    void CoinCollection()
    {
        onCoinCollect.Invoke();
        coinCollider.enabled = false;
        animator.Play("CoinPickup");
        Destroy(gameObject, 3.0f);
    }
}