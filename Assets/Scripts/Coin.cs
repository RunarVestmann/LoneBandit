using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class Coin : MonoBehaviour
{
    [SerializeField] Collider2D coinCollider;
    [SerializeField] Animator animator;
    [SerializeField] UnityEvent onCoinCollect;
    [SerializeField] Light2D pointLight;
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
        pointLight.intensity = 0f;
        Destroy(gameObject, 3.0f);
    }
}