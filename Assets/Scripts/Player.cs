using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float horizontalWallJumpForce;
    [SerializeField] float verticalWallJumpForce;
    [SerializeField] float wallJumpTime;
    [SerializeField] float wallSlideVerticalVelocity;
    float wallJumpElapsedTime = 0f;

    [SerializeField] Collider2D belowCollider;
    [SerializeField] Collider2D frontCollider;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask wallLayer;

    Rigidbody2D body;
    Vector2 moveDirection;

    Animator animator;

    bool facingRight = true;

    // bool isAttacking = false;
    bool isWallJumping = false;

    int currentAnimationState;

    // Animations
    int IDLE;
    int RUN;
    int JUMP;

    int FALL;
    // int ATTACK;

    void Awake()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        IDLE = Animator.StringToHash("Idle");
        RUN = Animator.StringToHash("Run");
        JUMP = Animator.StringToHash("Jump");
        FALL = Animator.StringToHash("Fall");
        // ATTACK = Animator.StringToHash("Attack");
        currentAnimationState = IDLE;
    }

    void Update()
    {
        ApplyAnimations();
        ApplySpriteRotation();
    }

    void FixedUpdate()
    {
        if (isWallJumping)
        {
            wallJumpElapsedTime += Time.deltaTime;
            if (!(wallJumpElapsedTime >= wallJumpTime)) return;
            isWallJumping = false;
            wallJumpElapsedTime = 0f;
        }
        else
            body.velocity = new Vector2(moveDirection.x * movementSpeed, body.velocity.y);

        if (IsWallInFront() && Mathf.Abs(moveDirection.x) > 0f)
        {
            // TODO: add wall slide animation
            body.velocity = new Vector2(body.velocity.x, Mathf.Max(body.velocity.y, wallSlideVerticalVelocity));
        }
    }

    public void OnMovement(InputAction.CallbackContext context) => moveDirection = context.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (IsGrounded())
            body.AddForce(Vector2.up * jumpForce);
        else if (IsWallInFront())
        {
            isWallJumping = true;
            body.velocity = Vector2.zero;
            body.AddForce(Vector2.up * verticalWallJumpForce);
            body.AddForce(-transform.localScale * horizontalWallJumpForce);
            FlipScale();
        }
    }

    void FlipScale()
    {
        var localScale = transform.localScale;
        localScale.x = -localScale.x;
        facingRight = !facingRight;
        transform.localScale = localScale;
    }

    bool IsGrounded() => belowCollider.IsTouchingLayers(groundLayer);

    bool IsWallInFront() => frontCollider.IsTouchingLayers(wallLayer);

    void ApplyAnimations()
    {
        // if (isAttacking)
        // {
        //     SetAnimationState(ATTACK);
        //     return;
        // }

        if (IsGrounded())
            SetAnimationState(moveDirection.x == 0f ? IDLE : RUN);
        else
            SetAnimationState(body.velocity.y > 0f ? JUMP : FALL);
    }

    void ApplySpriteRotation()
    {
        if (isWallJumping) return;
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