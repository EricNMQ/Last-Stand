using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject damageTextFrefab;
    public GameObject healthTextFrefab;
    public Canvas gameCanvas;


    private void Awake()
    {
        

    }


    private void OnEnable()
    {
        CharacterEvents.characterdamaged +=CharacterTookDamage;
        CharacterEvents.characterhealed += CharacterHealed;
    }

    private void OnDisable()
    {
        CharacterEvents.characterdamaged -= CharacterTookDamage;
        CharacterEvents.characterhealed -= CharacterHealed;
    }

    public void CharacterTookDamage(GameObject character, int damageReceived)
    {
        Vector3 spawnPosistion = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextFrefab, spawnPosistion, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>(); 
        
        tmpText.text = damageReceived.ToString();
    }

    public void CharacterHealed(GameObject character, int healthRestored)
    {
        Vector3 spawnPosistion = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextFrefab, spawnPosistion, Quaternion.identity, gameCanvas.transform)
            .GetComponent<TMP_Text>();

        tmpText.text = healthRestored.ToString();
    }
}
