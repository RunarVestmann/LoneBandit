using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    [SerializeField] float jumpForce;

    [Header("Wall Jump")]
    [SerializeField] float horizontalWallJumpForce;
    [SerializeField] float verticalWallJumpForce;
    [SerializeField] float wallJumpTime;
    float elapsedWallJumpTime;

    [Space]
    
    [Header("Wall Slide")] [SerializeField]
    float wallSlideVerticalVelocity;

    [Space] 
    
    [Header("Dash")] 
    [SerializeField] float dashForce;
    [SerializeField] float dashTime;
    float elapsedDashTime;

    [Space] 
    [Header("Colliders")] 
    [SerializeField] Collider2D belowCollider;

    [SerializeField] Collider2D frontCollider;

    [Space] 
    
    [Header("Layers")] 
    [SerializeField] LayerMask groundLayer;

    [SerializeField] LayerMask wallLayer;

    BetterJump betterJump;
    
    Rigidbody2D body;
    Vector2 moveDirection;

    Animator animator;

    bool facingRight = true;

    // bool isAttacking;
    bool isWallJumping ;
    bool isDashing;

    int currentAnimationState;

    // Animations
    int IDLE;
    int RUN;
    int JUMP;

    int FALL;
    // int ATTACK;

    float defaultGravityScale;

    void Awake()
    {
        betterJump = GetComponent<BetterJump>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        IDLE = Animator.StringToHash("Idle");
        RUN = Animator.StringToHash("Run");
        JUMP = Animator.StringToHash("Jump");
        FALL = Animator.StringToHash("Fall");
        // ATTACK = Animator.StringToHash("Attack");
        currentAnimationState = IDLE;
        defaultGravityScale = body.gravityScale;
    }

    void Update()
    {
        ApplyAnimations();
        ApplySpriteRotation();
    }

    void FixedUpdate()
    {
        if (isDashing) return;
        
        if (isWallJumping)
        {
            elapsedWallJumpTime += Time.deltaTime;
            if (!(elapsedWallJumpTime >= wallJumpTime)) return;
            isWallJumping = false;
            elapsedWallJumpTime = 0f;
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

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.started || isDashing) return;
        var dashDirection = moveDirection;
        
        // TODO: clamp direction into 45 degree angle intervals
        
        // Dash in the direction we are facing if there is no input
        if (dashDirection == Vector2.zero)
            dashDirection = transform.localScale.x * Vector2.right;
        else
        {
            dashDirection.x = clampDirectionalInput(dashDirection.x);
            dashDirection.y = clampDirectionalInput(dashDirection.y);
            dashDirection.Normalize();
        }

        StartCoroutine(Dash(dashDirection));
    }

    float clampDirectionalInput(float input)
    {
        if (input < -0.35f) return -1f;
        if (input > 0.35f) return 1f;
        return 0;
    }

    IEnumerator Dash(Vector2 direction)
    {
        betterJump.enabled = false;
        isDashing = true;
        body.velocity = direction * dashForce;
        body.gravityScale = 0f;

        yield return new WaitForSeconds(dashTime);
        
        isDashing = false;
        body.velocity = Vector2.zero;
        body.gravityScale = defaultGravityScale;
        betterJump.enabled = true;
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