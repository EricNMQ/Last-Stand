using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionDamage : MonoBehaviour
{
    public int damage = 10;
    public float duration = 0.3f; // thời gian tồn tại của vùng nổ

    void Start()
    {
        Destroy(gameObject, duration); // Xoá vùng nổ sau thời gian ngắn
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmg = collision.GetComponent<Damageable>();
        if (dmg != null && dmg.IsAlive)
        {
            dmg.Hit(damage, Vector2.zero);
        }
    }
}
