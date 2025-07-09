using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;



    public class HealthBar: MonoBehaviour 
    {
        [Header("Thanh máu thực tế (xanh lá)")]
        public Image fillBar;

        /// <summary>
        /// Cập nhật thanh máu theo giá trị hiện tại và tối đa
        /// </summary>
        public void SetHealth(int current, int max)
        {
            if (fillBar != null && max > 0)
            {
                fillBar.fillAmount = (float)current / max;
            }
        }
    }

