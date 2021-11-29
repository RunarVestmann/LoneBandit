using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    private float timeBtwImages;
    [SerializeField] float startTimeBtwImages = 1f;
    [SerializeField] GameObject dashEffect;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent <SpriteRenderer>();
    }

    public void Dash() => StartCoroutine(DashRoutine());

    IEnumerator DashRoutine()
    {
        if (timeBtwImages <= 0)
        {
            GameObject instance = (GameObject)Instantiate(dashEffect, transform.position, Quaternion.identity);
            Destroy(instance, 1f);
            timeBtwImages = startTimeBtwImages;
        }
        else
        {
            timeBtwImages -= Time.deltaTime;
        }
        yield return null;
    }
}