using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsibleStone : MonoBehaviour
{
    [SerializeField] float shakeTime;
    [SerializeField] GameObject parent;
    [SerializeField] float resetTime = 3f;

    bool isActive;

    float elapsedTime = 0f;

    Animator animator;
    Vector3 originalPosition;

    void Awake()
    {
        animator = GetComponent<Animator>();
        originalPosition = transform.position;


    }

    void FixedUpdate()
    {
        if (isActive)
        {
            if (elapsedTime > shakeTime)
            {
                elapsedTime = 0f;
                isActive = false;
                animator.enabled = false;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                parent.GetComponent<BoxCollider2D>().enabled = false;
                Invoke("Reset", resetTime);
            }
            else
            {
                elapsedTime += 0.01f;
                Debug.Log(elapsedTime);
            }
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

    void Reset()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        parent.GetComponent<BoxCollider2D>().enabled = true;
        transform.position = originalPosition;
        transform.rotation = Quaternion.identity;
    }
}
