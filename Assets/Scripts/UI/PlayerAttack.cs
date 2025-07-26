using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public AudioClip slashSound;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;

    private AudioSource audioSource;
    private Animator animator;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        if (slashSound == null)
        {
            slashSound = Resources.Load<AudioClip>("TênFileTrongFolderResources");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
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
                target.Hit(attackDamage, transform.position); // Gọi hàm trong Damageable
                Debug.Log("Chém trúng: " + enemy.name);
            }
        }

        Debug.Log("Chém xong");
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
