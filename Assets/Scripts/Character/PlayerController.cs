using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 4f;
    public float jumpImpulse = 3f;
    public float dashForce = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool canDash = true;
    private bool isDashing = false;

    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;
    Rigidbody2D rb;
    Animator animator;

    [SerializeField]
    public bool _isRuning = false;
    public bool isRuning
    {
        get => _isRuning;
        private set
        {
            _isRuning = value;
            animator.SetBool(AnimationStrings.isRuning, value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get => _isFacingRight;
        private set
        {
            if (_isFacingRight != value)
                transform.localScale *= new Vector2(-1, 1);
            _isFacingRight = value;
        }
    }

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);
    public bool IsAlive => animator.GetBool(AnimationStrings.isAlive);

    public float CurrentMoveSpeed => (CanMove && isRuning && !touchingDirections.IsOnWall) ? runSpeed : 0;

    [SerializeField] private LayerMask groundLayer;


    [SerializeField] private float attackCooldown = 3f;
    private float currentCooldown;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    void Update()
    {
        // Attack cooldown
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
            animator.SetFloat("cooldown", currentCooldown);
        }

        // Attack input
        if (Input.GetButtonDown("Fire1") && currentCooldown <= 0f)
        {
            animator.SetTrigger("attack");
            currentCooldown = attackCooldown;
            animator.SetFloat("cooldown", attackCooldown);
        }

        // Dash input
        if (Input.GetKeyDown(KeyCode.Q) && canDash && CanMove)
        {
            StartCoroutine(DoDash());
        }
    }

    private void FixedUpdate()
    {
        if (!damageable.LockVelocity && !isDashing)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            isRuning = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            isRuning = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y * knockback.y);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
            IsFacingRight = true;
        else if (moveInput.x < 0 && IsFacingRight)
            IsFacingRight = false;
    }

    private IEnumerator DoDash()
    {
        canDash = false;
        isDashing = true;

        animator.SetTrigger("Dash");

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        float dashDirection = IsFacingRight ? 1f : -1f;
        rb.velocity = new Vector2(dashDirection * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;

        // Nếu đang ở trên không, ép rơi xuống
        if (!touchingDirections.IsGrounded)
        {
            rb.velocity = new Vector2(0f, -5f); // Điều chỉnh rơi nhẹ/nhanh tuỳ ý
        }

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    public bool IsGrounded()
    {
        // Dùng raycast để kiểm tra xem có đứng trên mặt đất không
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

}
