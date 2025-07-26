using System.Collections;
using UnityEngine;

public class BombExplode : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private float delay = 2f;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float explosionRadius = 1.5f;
    [SerializeField] private int damage = 20;
    [SerializeField] private LayerMask damageableLayers = -1;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip explosionSound;

    [Header("Visual Effects")]
    [SerializeField] private bool enableScreenShake = true;
    [SerializeField] private float shakeIntensity = 0.5f;
    [SerializeField] private float shakeDuration = 0.3f;

    private bool hasExploded = false;
    private Coroutine explosionCoroutine;

    // Cached components
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        // Cache components
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Auto-assign AudioSource if not set
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        // Start countdown coroutine instead of Invoke for better control
        explosionCoroutine = StartCoroutine(ExplosionCountdown());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collision is with ground or solid objects
        if (!hasExploded && ShouldExplodeOnContact(collision))
        {
            Explode();
        }
    }

    private bool ShouldExplodeOnContact(Collision2D collision)
    {
        // Only explode on contact with ground or solid objects
        // Don't explode when hitting Player, Enemy, or other projectiles
        string tag = collision.collider.tag;
        return tag != "Player" &&
               tag != "Enemy" &&
               tag != "Bomb" &&
               collision.collider.gameObject.layer != LayerMask.NameToLayer("Projectile");
    }

    private IEnumerator ExplosionCountdown()
    {
        // Visual countdown effect
        float timeLeft = delay;
        Color originalColor = spriteRenderer != null ? spriteRenderer.color : Color.white;

        while (timeLeft > 0 && !hasExploded)
        {
            // Blinking effect as countdown approaches
            if (timeLeft <= 1f && spriteRenderer != null)
            {
                float blinkSpeed = Mathf.Lerp(2f, 10f, 1f - timeLeft);
                spriteRenderer.color = Color.Lerp(originalColor, Color.red,
                    (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f);
            }

            timeLeft -= Time.deltaTime;
            yield return null;
        }

        if (!hasExploded)
            Explode();
    }

    void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;

        // Stop countdown coroutine
        if (explosionCoroutine != null)
        {
            StopCoroutine(explosionCoroutine);
        }

        // Play explosion sound
        PlayExplosionSound();

        // Create explosion effect
        CreateExplosionEffect();

        // Apply damage and effects
        ApplyExplosionDamage();

        // Screen shake effect
        if (enableScreenShake)
        {
            CameraShake.Instance?.Shake(shakeIntensity, shakeDuration);
        }

        // Destroy bomb object
        DestroyBomb();
    }

    private void PlayExplosionSound()
    {
        if (explosionSound != null)
        {
            if (audioSource != null)
            {
                audioSource.PlayOneShot(explosionSound);
            }
            else
            {
                // Play at bomb position if no AudioSource component
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            }
        }
    }

    private void CreateExplosionEffect()
    {
        if (explosionEffect != null)
        {
            GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);

            // Auto-destroy effect after 5 seconds to prevent memory leaks
            Destroy(effect, 5f);
        }
    }

    private void ApplyExplosionDamage()
    {
        // Use LayerMask for better performance and control
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageableLayers);

        foreach (var hit in hits)
        {
            if (hit == null) continue;

            // Calculate distance-based damage
            float distance = Vector2.Distance(transform.position, hit.transform.position);
            float damageMultiplier = 1f - (distance / explosionRadius);
            int actualDamage = Mathf.RoundToInt(damage * damageMultiplier);

            // Apply damage to different entity types
            ApplyDamageToEntity(hit, actualDamage);

            // Apply knockback force
            ApplyKnockback(hit, distance);
        }
    }

    private void ApplyDamageToEntity(Collider2D hit, int actualDamage)
    {
        // Try IDamageable interface first (most common in your project)
        var damageable = hit.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(actualDamage);
            return;
        }

        // Try to find any component with TakeDamage method
        var components = hit.GetComponents<MonoBehaviour>();
        foreach (var component in components)
        {
            var takeDamageMethod = component.GetType().GetMethod("TakeDamage", new System.Type[] { typeof(int) });
            if (takeDamageMethod != null)
            {
                takeDamageMethod.Invoke(component, new object[] { actualDamage });
                return;
            }
        }

        // Debug log if no damage component found
        Debug.LogWarning($"No damage component found on {hit.name}");
    }

    private void ApplyKnockback(Collider2D hit, float distance)
    {
        var hitRb = hit.GetComponent<Rigidbody2D>();
        if (hitRb != null)
        {
            Vector2 knockbackDirection = (hit.transform.position - transform.position).normalized;
            float knockbackForce = Mathf.Lerp(10f, 5f, distance / explosionRadius);
            hitRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void DestroyBomb()
    {
        // Disable collider and renderer immediately
        var collider = GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
        if (spriteRenderer != null) spriteRenderer.enabled = false;

        // Destroy after a short delay to allow sound to play
        Destroy(gameObject, audioSource != null && explosionSound != null ? explosionSound.length : 0.1f);
    }

    // Public method to manually detonate
    public void ManualDetonate()
    {
        if (!hasExploded)
        {
            Explode();
        }
    }

    // Gizmos for debugging
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    void OnDestroy()
    {
        // Clean up coroutines
        if (explosionCoroutine != null)
        {
            StopCoroutine(explosionCoroutine);
        }
    }
}

// Interface for damageable entities (đảm bảo tương thích với DamageZone)
public interface IDamageable
{
    void TakeDamage(int damage);
    bool IsAlive { get; }
}

// Simple CameraShake singleton (optional)
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Shake(float intensity, float duration)
    {
        StartCoroutine(ShakeCoroutine(intensity, duration));
    }

    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * intensity;
            float y = Random.Range(-1f, 1f) * intensity;

            transform.position = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPos;
    }
}