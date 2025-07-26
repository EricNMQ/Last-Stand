using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explosionDelay = 2f;
    public float explosionRadius = 2f;
    public int damage = 20;
    public GameObject explosionEffect;

    void Start()
    {
        Invoke(nameof(Explode), explosionDelay);
    }

    void Explode()
    {
        // Hiệu ứng nổ
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Gây damage vùng
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            Damageable dmg = hit.GetComponent<Damageable>();
            if (dmg != null)
                dmg.Hit(damage, Vector2.zero);
        }

        Destroy(gameObject);
    }
}
