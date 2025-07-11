using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    //Pixel per second
    public Vector3 moveSpeed = new Vector3(0,75,0);
    public float timetoFade = 1f;

    RectTransform textTranform;
    TextMeshProUGUI textMeshPro;

    private float timeElapsed = 0;
    private Color startColor;

    private void Awake()
    {
        textTranform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        startColor = textMeshPro.color;
    }
    public void Update()
    {
        textTranform.position += moveSpeed * Time.deltaTime;

        timeElapsed += Time.deltaTime;

        if (timeElapsed < timetoFade)
        {
            float fadeAlpha = startColor.a * (1 - (timeElapsed / timetoFade));
            textMeshPro.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);
        }

    }
}
