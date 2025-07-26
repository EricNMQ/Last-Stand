using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class goblinboss : MonoBehaviour
{
    public float walkspeed = 3f;
    public float walkStopRate = 0.6f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;
    public DetectionZone playerDetectionZone;
    public GameObject bombPrefab;
    public Transform throwPoint;


    public DetectionZone bombDetectionZone;


    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                //Direction plip
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }

            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }


    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float cooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.cooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.cooldown, Mathf.Max(value, 0f));
        }
    }

    public bool throwbomb
    {
        get
        {
            return animator.GetBool(AnimationStrings.throwbomb);
        }
    }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    public bool isInDialogue = false;
    void Update()
    {
        if (isInDialogue) return;
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
        }

        // ----------- BOMB THROWING ZONE -------------
        if (bombDetectionZone.DetectedColliders.Count > 0 && cooldown <= 0f)
        {
            Collider2D targetCollider = bombDetectionZone.DetectedColliders[0];
            GameObject targetObject = targetCollider.gameObject;
            Damageable target = targetObject.GetComponent<Damageable>();

            if (target != null && target.IsAlive)
            {
                Debug.Log("Đủ điều kiện ném bomb");

                HasTarget = true;



            }
        }



        if (playerDetectionZone.DetectedColliders.Count > 0)
        {
            Collider2D targetCollider = playerDetectionZone.DetectedColliders[0];
            GameObject targetObject = targetCollider.gameObject;
            Damageable target = targetObject.GetComponent<Damageable>();

            if (target != null && target.IsAlive)
            {
                float directionToPlayer = target.transform.position.x - transform.position.x;

                if (directionToPlayer > 0 && WalkDirection == WalkableDirection.Left)
                {
                    FlipDirection();
                }
                else if (directionToPlayer < 0 && WalkDirection == WalkableDirection.Right)
                {
                    FlipDirection();
                }
            }
        }


        if (attackZone.DetectedColliders.Count > 0)
        {
            Collider2D targetCollider = attackZone.DetectedColliders[0];
            GameObject targetObject = targetCollider.gameObject;
            Damageable target = targetObject.GetComponent<Damageable>();

            if (target != null && target.IsAlive)
            {
                HasTarget = true;
            }
            else
            {
                HasTarget = false;
                attackZone.DetectedColliders.RemoveAt(0);
            }
        }
        else
        {
            HasTarget = false;
        }
    }




    public void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall || cliffDetectionZone.DetectedColliders.Count == 9)
        {
            FlipDirection();
        }
        if (!damageable.LockVelocity)
        {
            if (CanMove)
                rb.velocity = new Vector2(walkspeed * walkDirectionVector.x, rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }

    }


    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("khong phu hop de di chuyen trai va phai");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {

        rb.velocity = new Vector2(knockback.x, rb.velocity.y * knockback.y);
    }

    public void OnCliffDetection()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }

    public bool IsDead()
    {
        return !damageable.IsAlive;
    }





    public void ThrowBomb()
    {
        if (bombPrefab != null && throwPoint != null)
        {
            Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);
            Debug.Log("Boss ném bomb!");
        }
    }

}