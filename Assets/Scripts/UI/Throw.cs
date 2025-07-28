using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Throw : MonoBehaviour
{
    [SerializeField] float throwSpeed;
    [SerializeField] GameObject burst;
    Rigidbody2D myrb;
    private AudioSource audioSource;

    private void Start()
    {
        myrb = GetComponent<Rigidbody2D>();
        myrb.velocity = new Vector2(transform.localScale.x * throwSpeed, 0f);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject clone= Instantiate(burst, transform.position, transform.rotation);
        clone.SetActive(true);
        audioSource.Play(); // phát âm thanh nổ
        Destroy(gameObject, 0.2f);

        if (collision.transform.CompareTag("Enemy"))
        {

            Damageable target = collision.transform.GetComponent<Damageable>();
            if (target != null)
            {
                target.Hit(10, transform.position); // Gây sát thương
            }

            Debug.Log("Trúng enemy!");
        }

        Destroy(this.gameObject);
    }
}
