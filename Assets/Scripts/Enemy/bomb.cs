using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public float speed = 5f;
    public GameObject explosionEffect;
    public float lifetime = 5f;
    public int damage = 1;

    Animator animator;
    Rigidbody2D rb;
    bool hasExploded = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right * speed * transform.localScale.x;

        Destroy(gameObject, lifetime); // tự hủy nếu không đụng gì
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return;

        // Nếu chạm Player hoặc Ground thì nổ
        bool isPlayer = collision.CompareTag("Player");
        bool isGround = collision.CompareTag("Ground");

        Damageable dmg = collision.GetComponent<Damageable>();
        if (dmg != null)
        {
            dmg.Hit(damage, Vector2.zero);
            isPlayer = true; // đảm bảo đánh dấu trúng Player
        }

        if (isPlayer || isGround)
        {
            Explode();
        }
    }

    void Explode()
    {
        hasExploded = true;

        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        animator.Play("bomb_explore");

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 0.4f);
    }
}

