using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Orb : MonoBehaviour
{
    [SerializeField] Collider2D orbCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float timeUntilPlayerCanCollectAgain;
    [SerializeField] float fadeInTime;
    [SerializeField] UnityEvent onCollect;

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (!player) return;
        player.ReplenishMovement();
        StartCoroutine(CollectRoutine());
    }

    IEnumerator CollectRoutine()
    {
        onCollect.Invoke();
        orbCollider.enabled = false;
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(timeUntilPlayerCanCollectAgain);
        orbCollider.enabled = true;
        spriteRenderer.enabled = true;
        yield return FadeInRoutine();
    }

    IEnumerator FadeInRoutine()
    {
        var elapsedTime = 0f;
        var color = spriteRenderer.color;

        while (elapsedTime <= fadeInTime)
        {
            color.a = elapsedTime / fadeInTime;
            spriteRenderer.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        color.a = 1f;
        spriteRenderer.color = color;
    }
}