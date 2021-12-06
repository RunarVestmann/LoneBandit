using UnityEngine;

public class Sign : MonoBehaviour
{
    [SerializeField] SpriteRenderer imageSpriteRenderer;
    bool showingDialogue;

    void Awake() => imageSpriteRenderer.enabled = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (!player || showingDialogue) return;
        showingDialogue = true;
        imageSpriteRenderer.enabled = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (!player || !showingDialogue) return;
        showingDialogue = false;
        imageSpriteRenderer.enabled = false;
    }
}