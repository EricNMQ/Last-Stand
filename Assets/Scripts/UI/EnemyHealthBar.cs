using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyHealthBar : MonoBehaviour
{
    [Header("Thanh máu thực tế (xanh lá)")]
    public Image fillBar;

    
    public void SetHealth(int current, int max)
    {
        if (fillBar != null && max > 0)
        {
            fillBar.fillAmount = (float)current / max;
        }
    }
    public void UpdateHealth(int current, int max)
    {
        if (fillBar != null)
        {
            fillBar.fillAmount = (float)current / max;
        }
    }

}
