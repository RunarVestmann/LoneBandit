using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Orb : MonoBehaviour
{
    [SerializeField] Collider2D orbCollider;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    [SerializeField] float timeUntilPlayerCanCollectAgain;
    [SerializeField] float fadeInTime;
    [SerializeField] UnityEvent onCollect;
    [SerializeField] ParticleSystemRenderer particles;

    // Start playing the hover animation at a random offset so orbs don't hover at the exact same rate
    void Start() => animator.PlayInFixedTime("Hover", 0, Random.Range(0f, 0.3f));

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
        particles.enabled = false;
        orbCollider.enabled = false;
        animator.Play("Collect");
        yield return new WaitForSeconds(timeUntilPlayerCanCollectAgain);
        orbCollider.enabled = true;
        particles.enabled = true;
        yield return FadeInRoutine();
    }

    IEnumerator FadeInRoutine()
    {
        animator.Play("Hover");
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