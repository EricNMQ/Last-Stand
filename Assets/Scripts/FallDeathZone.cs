using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if (damageable != null && damageable.IsAlive)
        {
            damageable.Hit(9999, Vector2.zero); 
        }
    }
}
