using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Player : MonoBehaviour
{
    [Header("Events")] [SerializeField] UnityEvent dashEvent;
    [SerializeField] UnityEvent jumpEvent;
    [SerializeField] UnityEvent walljumpEvent;
    [SerializeField] UnityEvent landEvent;
    [SerializeField] UnityEvent deathEvent;
    [SerializeField] UnityEvent coinEvent;
    [SerializeField] UnityEvent jumpReplenishEvent;

    [Space] [Header("Movement")] [SerializeField]
    float movementSpeed;

    [SerializeField] float jumpForce;
    [SerializeField] float airborneJumpForce;

    [Header("Wall Jump")] [SerializeField] float horizontalWallJumpForce;
    [SerializeField] float verticalWallJumpForce;
    [SerializeField] float wallJumpTime;
    float elapsedWallJumpTime;

    [Space] [Header("Wall Slide")] [SerializeField]
    float wallSlideVerticalVelocity;

    [Space] [Header("Dash")] [SerializeField]
    float dashForce;

    [SerializeField] float dashTime;

    [Space] [Header("Colliders")] [SerializeField]
    Collider2D belowCollider;

    [SerializeField] Collider2D leftCollider;
    [SerializeField] Collider2D rightCollider;

    [Space] [Header("Layers")] [SerializeField]
    LayerMask groundLayer;

    [SerializeField] LayerMask wallLayer;

    BetterJump betterJump;
    SpriteRenderer spriteRenderer;
    Rigidbody2D body;
    Vector2 moveDirection;

    Animator animator;

    //Particles 
    [Space] [Header("Particles")] public GameObject dashParticles;
    public GameObject dashTrail;
    public GameObject landingParticles;
    public GameObject jumpingParticles;
    public GameObject wallSlidingParticles;


    bool facingRight = true;
    bool isWallSliding;

    bool isWallJumping;
    bool isDashing;
    bool hasDashed;
    bool hasJumped;
    bool groundStatusLastFrame = true;
    bool hasJumpedStatusLastFrame;
    bool isDead;

    int currentAnimationState;

    // Animations
    int IDLE;
    int RUN;
    int JUMP;
    int FALL;
    int WALLSLIDE;
    int DEATH;
    int RESPAWN;

    //After dash
    int REDFALL;
    int REDJUMP;
    int REDWALLSLIDE;

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
        WALLSLIDE = Animator.StringToHash("WallSlide");
        DEATH = Animator.StringToHash("Death");
        RESPAWN = Animator.StringToHash("Respawn");
        REDJUMP = Animator.StringToHash("RedJump");
        REDFALL = Animator.StringToHash("RedFall");
        REDWALLSLIDE = Animator.StringToHash("RedWallSlide");
        currentAnimationState = IDLE;
        defaultGravityScale = body.gravityScale;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (isDead) return;
        ApplyAnimations();
        ApplySpriteRotation();
    }

    void FixedUpdate()
    {
        if (isDashing || isDead) return;

        var direction = moveDirection;
        direction.y = 0f;
        direction.Normalize();

        if (isWallJumping)
        {
            elapsedWallJumpTime += Time.deltaTime;
            if (!(elapsedWallJumpTime >= wallJumpTime)) return;
            isWallJumping = false;
            elapsedWallJumpTime = 0f;
        }
        else
            body.velocity = new Vector2(direction.x * movementSpeed, body.velocity.y);

        var wallInFront = (facingRight && IsWallToRight()) || (!facingRight && IsWallToLeft());
        
        if (wallInFront && Mathf.Abs(direction.x) > 0f && !IsGrounded())
        {
            var particleDir = direction.x * 0.2f;
            var position = body.transform.position;
            var wallSlidingParticle = (GameObject) Instantiate(wallSlidingParticles,
                new Vector3(position.x + particleDir, position.y, position.z), Quaternion.identity);
            Destroy(wallSlidingParticle, 0.3f);
            isWallSliding = true;
            if (!hasDashed)
                SetAnimationState(WALLSLIDE);
            else
                SetAnimationState(REDWALLSLIDE);
            var velocity = body.velocity;
            body.velocity = new Vector2(velocity.x, Mathf.Max(velocity.y, wallSlideVerticalVelocity));
        }
        else
            isWallSliding = false;

        if (IsGrounded())
            hasDashed = false;

        if (!groundStatusLastFrame && IsGrounded())
        {
            var landingParticle = (GameObject) Instantiate(landingParticles, transform.position, Quaternion.identity);
            Destroy(landingParticle, 0.3f);
            hasJumped = false;
            landEvent.Invoke();
        }

        if (hasJumpedStatusLastFrame && !hasJumped)
            jumpReplenishEvent.Invoke();

        groundStatusLastFrame = IsGrounded();
        hasJumpedStatusLastFrame = hasJumped;
    }

    public void OnMovement(InputAction.CallbackContext context) => moveDirection = context.ReadValue<Vector2>();

    public void OnJump(InputAction.CallbackContext context)
    {
        var direction = moveDirection;
        direction.y = 0f;
        direction.Normalize();

        if (!context.started || isDead || !enabled) return;

        if (IsGrounded())
        {
            jumpEvent.Invoke();
            hasJumped = true;
            body.velocity = new Vector2(body.velocity.x, 0f);
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            var jumpEffect = (GameObject) Instantiate(jumpingParticles, transform.position, Quaternion.identity);
            Destroy(jumpEffect, 0.5f);
        }
        else if (IsWallToRight() || IsWallToLeft())
        {
            walljumpEvent.Invoke();
            var particleDirection = direction.x * 0.2f;
            isWallJumping = true;
            body.velocity = Vector2.zero;
            body.AddForce(Vector2.up * verticalWallJumpForce, ForceMode2D.Impulse);
            var horizontalDirection = IsWallToRight() ? Vector2.left : Vector2.right;
            body.AddForce(horizontalDirection * horizontalWallJumpForce, ForceMode2D.Impulse);
            
            if (horizontalDirection == Vector2.left && facingRight)
                FlipSprite();
            else if (horizontalDirection == Vector2.right && !facingRight)
                FlipSprite();
            
            var position = transform.position;
            var jumpEffect = (GameObject) Instantiate(jumpingParticles,
                new Vector3(position.x + particleDirection, position.y, position.z), Quaternion.identity);
            Destroy(jumpEffect, 0.5f);
        }
        else if (!hasJumped)
        {
            jumpEvent.Invoke();
            hasJumped = true;
            body.velocity = new Vector2(body.velocity.x, 0f);
            body.AddForce(Vector2.up * airborneJumpForce, ForceMode2D.Impulse);
            var jumpEffect = (GameObject)Instantiate(jumpingParticles, transform.position, Quaternion.identity);
            Destroy(jumpEffect, 0.5f);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.started || isDashing || hasDashed || !enabled || isDead) return;
        var dashDirection = moveDirection;

        // Dash in the direction we are facing if there is no input
        if (dashDirection == Vector2.zero)
            dashDirection = facingRight ? Vector2.right : Vector2.left;
        else
        {
            // Clamping the directional input to be a multiple of 45 degrees
            dashDirection.x = ClampDirectionalInput(dashDirection.x);
            dashDirection.y = ClampDirectionalInput(dashDirection.y);
            dashDirection.Normalize();
        }

        dashEvent.Invoke();
        StartCoroutine(Dash(dashDirection));
    }

    public void ReplenishMovement()
    {
        hasDashed = false;
        hasJumped = false;
    }

    float ClampDirectionalInput(float input)
    {
        if (input < -0.35f) return -1f;
        if (input > 0.35f) return 1f;
        return 0;
    }

    public void ForceIdle()
    {
        SetAnimationState(IDLE);
        body.velocity = Vector2.zero;
    }

    IEnumerator Dash(Vector2 direction)
    {
        var dashEffect = (GameObject) Instantiate(dashParticles, transform.position, Quaternion.identity);
        var trailEffect = Instantiate(dashTrail, transform.position, Quaternion.identity);
        trailEffect.transform.SetParent(body.transform); //setting the trail as the child of the player 
        var localScale = trailEffect.transform.localScale;
        localScale.x = facingRight ? 1f : -1f;
        trailEffect.transform.localScale = localScale;

        betterJump.enabled = false;
        isDashing = true;
        hasDashed = true;
        body.gravityScale = 0f;
        body.velocity = Vector2.zero;
        body.velocity = direction * dashForce;
        
        yield return new WaitForSeconds(dashTime);


        isDashing = false;

        body.velocity = Vector2.zero;
        betterJump.enabled = true;
        Destroy(dashEffect, 0.5f);
        Destroy(trailEffect, dashTime);
        if (isDead) yield break;
        body.gravityScale = defaultGravityScale;
    }

    void FlipSprite()
    {
        facingRight = !facingRight;
        spriteRenderer.flipX = !facingRight;
    }

    bool IsGrounded() => belowCollider.IsTouchingLayers(groundLayer);
    bool IsWallToLeft() => leftCollider.IsTouchingLayers(wallLayer);
    bool IsWallToRight() => rightCollider.IsTouchingLayers(wallLayer);

    void ApplyAnimations()
    {
        if (isWallSliding || isDead) return;
        if (!hasDashed)
            if (IsGrounded())
                SetAnimationState(moveDirection.x == 0f ? IDLE : RUN);
            else
                SetAnimationState(body.velocity.y > 0f ? JUMP : FALL);
        else
            SetAnimationState(body.velocity.y > 0f ? REDJUMP : REDFALL);
    }

    void ApplySpriteRotation()
    {
        if (isWallJumping) return;
        if (moveDirection.x < 0f && facingRight)
        {
            facingRight = false;
            spriteRenderer.flipX = !facingRight;
        }
        else if (moveDirection.x > 0f && !facingRight)
        {
            facingRight = true;
            spriteRenderer.flipX = !facingRight;
        }
    }

    void OnDeath()
    {
        deathEvent.Invoke();
        body.gravityScale = 0;
        body.velocity = Vector2.zero;
        isDead = true;
        SetAnimationState(DEATH);
    }

    public void OnRespawn()
    {
        body.gravityScale = 1;
        SetAnimationState(RESPAWN);
        Invoke(nameof(Respawned), 0.4f);
    }

    void Respawned() => isDead = false;

    void SetAnimationState(int state)
    {
        if (currentAnimationState == state) return;
        animator.Play(state);
        currentAnimationState = state;
    }
    
    Collider2D lastCoinTouched;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap") && !isDead)
            OnDeath();
        if (other.CompareTag("Coin") && other != lastCoinTouched)
        {
            lastCoinTouched = other;
            coinEvent.Invoke();
        }
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Trap") && !isDead)
            OnDeath();
    }
}