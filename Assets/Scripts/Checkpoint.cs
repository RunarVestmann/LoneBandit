using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    Animator animator;
    Light2D checkpointLight;

    [SerializeField] UnityEvent<Vector3> spawnLocationEvent;
    [SerializeField] AudioSource audioSource;

    bool isActive;

    void Start()
    {
        animator = GetComponent<Animator>();
        checkpointLight = GetComponentInChildren<Light2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive) return;
        var player = other.GetComponent<Player>();
        if (!player) return;
        animator.Play("Active");
        checkpointLight.enabled = true;
        isActive = true;
        audioSource.Play();
        spawnLocationEvent.Invoke(transform.position);
    }

}
