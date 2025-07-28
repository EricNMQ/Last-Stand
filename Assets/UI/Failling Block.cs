using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaillingBlock : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;
    [SerializeField] private float destroyDelay = 2f;

    private bool falling = false;

    [SerializeField] private Rigidbody2D rd;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (falling)
            return;
        if (collision.transform.tag == "Player")
        {
            StartCoroutine(StartFall());
        }
    }

    private IEnumerator StartFall()
    {
        yield return new WaitForSeconds(fallDelay);

        rd.bodyType = RigidbodyType2D.Dynamic;
        Destroy(gameObject, destroyDelay);
    }
}
