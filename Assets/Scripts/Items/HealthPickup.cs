using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
public enum HealType
{
    Small,
    Medium,
    Large
}

public class HealthPickup : MonoBehaviour
{
    public HealType healType = HealType.Small;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmg = collision.GetComponent<Damageable>();
        if (dmg != null && dmg.IsAlive)
        {
            int healAmount = 0;

            switch (healType)
            {
                case HealType.Small: healAmount = 10; break;
                case HealType.Medium: healAmount = 25; break;
                case HealType.Large: healAmount = 50; break;
            }

            dmg.Health += healAmount;
            Destroy(gameObject);
        }
    }
}

