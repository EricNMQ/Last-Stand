using UnityEngine;
using System.Collections;

public class PlayerHealthManager : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    public HealthBar healthBar;
    public GameObject Panel;

    public Animator animator; // Gắn animator của nhân vật
    public float deathDelay = 2f; // Delay trước khi hiện panel

    private bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // Nếu đã chết thì bỏ qua

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            isDead = true;
            Debug.Log("💀 Người chơi đã chết!");

            // Gọi animation chết nếu có
            if (animator != null)
                animator.SetTrigger("Die");

            // Bắt đầu coroutine để chờ animation chết xong rồi hiện panel
            StartCoroutine(HandleDeath());
        }
    }

    IEnumerator HandleDeath()
    {
        yield return new WaitForSeconds(deathDelay);

        if (Panel != null)
            Panel.SetActive(true);

        Time.timeScale = 0f; // Dừng game
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth, maxHealth);
    }
}
