using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsibleStone : MonoBehaviour
{
    [SerializeField] float shakeTime;
    [SerializeField] GameObject parent;

    bool isActive;

    float elapsedTime = 0;

    Animator animator;


    void Awake()
    {
        animator = GetComponent<Animator>();
        
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            if (elapsedTime > shakeTime)
            {
                animator.enabled = false;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                parent.GetComponent<BoxCollider2D>().enabled = false;
                Destroy(gameObject, 3f);
            }
            else
                elapsedTime += 0.01f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (!player) return;
        Shake();
    }

    void Shake()
    {
        animator.enabled = true;
        animator.Play("Shake");
        isActive = true;
    }
}
