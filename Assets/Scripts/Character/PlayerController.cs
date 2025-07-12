
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof (Damageable))]
public class PlayerController : MonoBehaviour
{
    public float runSpeed = 4f;
    public float jumpImpulse= 3f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;
    

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (isRuning && !touchingDirections.IsOnWall)
                {
                    return runSpeed;
                }
                else
                {
                    return 0;
                }
            }
            //movement locked
            return 0;
        }
    } 
    
        

    [SerializeField]
    public bool _isRuning = false;

    public bool isRuning
    {
        get
        {
            return _isRuning;
        }
        private set
        {
            _isRuning = value;
            animator.SetBool(AnimationStrings.isRuning, value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight { get { return _isFacingRight;  } private set {
            if (_isFacingRight != value) 
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            
           _isFacingRight = value;
        }}

    public bool CanMove { get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
}

    

    Rigidbody2D rb;
    Animator animator;


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
        // Đếm ngược cooldown
        if (currentCooldown > 0f)
        {
            currentCooldown -= Time.deltaTime;
            animator.SetFloat("cooldown", currentCooldown);
        }

        if (Input.GetButtonDown("Fire1") && currentCooldown <= 0f)
        {
            animator.SetTrigger("attack");
            currentCooldown = attackCooldown;
            animator.SetFloat("cooldown", attackCooldown); // reset animator param
        }
    }
    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    private void FixedUpdate()
    {
        if(!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue < Vector2>();

        if (IsAlive)
        {
            isRuning = moveInput != Vector2.zero;

            SetFacingDirection(moveInput);
        }else
        {
            isRuning = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0  && !IsFacingRight)
        {
            // Face the right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            // Face the left
            IsFacingRight = false;
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

}
