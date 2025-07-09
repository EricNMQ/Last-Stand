using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    Animator animator;

    public HealthBar healthBar;

    [SerializeField]
    private int _maxHealth = 100;
    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;

            if (healthBar != null)
            {
                healthBar.SetHealth(_health, _maxHealth);
            }


            //if drop below 0 player will die
            if (_health <= 0)
            {
                IsAlive = false;
                DropLoot();
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

    [SerializeField] private GameObject[] dropItems;
    [SerializeField]
    private float[] dropWeights = new float[] { 0.6f, 0.3f, 0.1f };

    void DropLoot()
    {
        if (dropItems.Length == 0 || dropWeights.Length != dropItems.Length) return;

        float dropChance = 0.5f; 
        if (Random.value > dropChance) return;

        
        float totalWeight = 0f;
        foreach (float w in dropWeights)
            totalWeight += w;

        float randomValue = Random.value * totalWeight;
        float accumulated = 0f;

        for (int i = 0; i < dropItems.Length; i++)
        {
            accumulated += dropWeights[i];
            if (randomValue <= accumulated)
            {
                Instantiate(dropItems[i], transform.position, Quaternion.identity);
                break;
            }
        }
    }






    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive { 
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("isAlive set " + value);
        }
}
    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false ;
                timeSinceHit = 0 ;
            }

            timeSinceHit += Time.deltaTime;
        }
        
    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit.Invoke(damage, knockback);
            CharacterEvents.characterdamaged.Invoke(gameObject, damage);
            return true;
        }
        //unable to be hit
        return false;
    }
}
