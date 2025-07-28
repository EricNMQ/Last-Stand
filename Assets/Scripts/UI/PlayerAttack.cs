using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AudioClip slashSound;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;

    [Header("Fireball Settings")]
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float fireballSpeed = 7f;
    public AudioClip fireballSound;

    private AudioSource audioSource;
    private Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        if (slashSound == null)
            slashSound = Resources.Load<AudioClip>("SlashSound");

        if (fireballSound == null)
            fireballSound = Resources.Load<AudioClip>("FireballSound");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.F)) // Phím F để bắn
        {
            ShootFireball();
        }
    }

    public void Attack()
    {
        animator.SetTrigger("Attack");
        audioSource.PlayOneShot(slashSound);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Damageable target = enemy.GetComponent<Damageable>();
            if (target != null && target.IsAlive)
            {
                target.Hit(attackDamage, transform.position);
                Debug.Log("Chém trúng: " + enemy.name);
            }
        }
    }

    void ShootFireball()
    {
        if (fireballPrefab == null || firePoint == null) return;

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        float direction = transform.localScale.x;
        rb.velocity = new Vector2(direction * fireballSpeed, 0f);

        if (audioSource != null && fireballSound != null)
        {
            audioSource.PlayOneShot(fireballSound);
        }

        animator.SetTrigger("Attack"); // hoặc tạo thêm animation Fire
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
