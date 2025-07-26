using UnityEngine;

public class GoblinBombThrower : MonoBehaviour
{
    public GameObject bombPrefab;
    public Transform throwPoint;
    public float throwForce = 5f;
    public float throwInterval = 3f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= throwInterval)
        {
            ThrowBomb();
            timer = 0f;
        }
    }

    void ThrowBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(transform.localScale.x * throwForce, throwForce), ForceMode2D.Impulse);
    }
}
