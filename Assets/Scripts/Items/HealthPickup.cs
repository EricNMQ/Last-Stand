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
    public HealType healType;

    private void Awake()
    {
        // Gán tự động dựa trên tên object hoặc prefab
        if (name.Contains("Small"))
            healType = HealType.Small;
        else if (name.Contains("Medium"))
            healType = HealType.Medium;
        else if (name.Contains("Large"))
            healType = HealType.Large;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable dmg = collision.GetComponent<Damageable>();
        if (dmg != null && dmg.IsAlive)
        {
            int healAmount = 0;

            switch (healType)
            {
                case HealType.Small: healAmount = 20; break;
                case HealType.Medium: healAmount = 40; break;
                case HealType.Large: healAmount = 70; break;
            }

            dmg.Health += healAmount;
            Destroy(gameObject);
        }
    }
}

