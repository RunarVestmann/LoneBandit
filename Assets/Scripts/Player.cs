using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;

    [SerializeField] Collider2D belowCollider;
    [SerializeField] Collider2D leftCollider;
    [SerializeField] Collider2D rightCollider;

    Rigidbody2D body;
    Vector2 moveDirection;

    Animator animator;
    bool facingRight = true;
    bool isAttacking = false;

    int currentAnimationState;

    // Animations
    int IDLE;
    int RUN;
    int JUMP;
    int FALL;
    int ATTACK;

    void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        IDLE = Animator.StringToHash("Idle");
        // RUN = Animator.StringToHash("Run");
        // JUMP = Animator.StringToHash("Jump");
        // FALL = Animator.StringToHash("Fall");
        // ATTACK = Animator.StringToHash("Attack");
        currentAnimationState = IDLE;
    }

    void Update()
    {
        ApplyAnimations();
        ApplySpriteRotation();
    }

    void FixedUpdate() => body.AddForce(Vector2.right * (moveDirection.x * movementSpeed));

    public void OnMovement(InputAction.CallbackContext context) => moveDirection = context.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    bool IsGrounded()
    {
        return belowCollider.IsTouchingLayers(1 << LayerMask.NameToLayer("Default"));
    }

    void ApplyAnimations()
    {
        if (isAttacking)
        {
            SetAnimationState(ATTACK);
            return;
        }

        if (IsGrounded())
            SetAnimationState(moveDirection.x == 0f ? IDLE : RUN);
        else
            SetAnimationState(body.velocity.y > 0f ? JUMP : FALL);
    }

    void ApplySpriteRotation()
    {
        var localScale = transform.localScale;
        if (moveDirection.x < 0f && facingRight)
        {
            localScale.x = -1f;
            facingRight = false;
            transform.localScale = localScale;
        }
        else if (moveDirection.x > 0f && !facingRight)
        {
            localScale.x = 1f;
            facingRight = true;
            transform.localScale = localScale;
        }
    }

    void SetAnimationState(int state)
    {
        if (currentAnimationState == state) return;
        animator.Play(state);
        currentAnimationState = state;
    }
}