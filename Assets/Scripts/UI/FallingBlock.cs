using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    public float delayBeforeFall = 0.5f;
    public GameObject breakEffect; // Hiệu ứng vỡ
    private bool isTriggered = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isTriggered && collision.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
            Invoke("BreakAndFall", delayBeforeFall);
        }
    }

    void BreakAndFall()
    {
        if (breakEffect != null)
            Instantiate(breakEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
