using UnityEngine;
using UnityEngine.InputSystem;

public class BetterJump : MonoBehaviour
{
    [SerializeField] Rigidbody2D body;
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float lowJumpMultiplier = 2f;
    bool isJumping = false;
    
    public void OnJump(InputAction.CallbackContext context) => isJumping = !context.canceled;

    void Update()
    {
        if (body.velocity.y < 0)
            body.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (body.velocity.y > 0 && !isJumping)
            body.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }
}