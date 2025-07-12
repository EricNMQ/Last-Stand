using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public GameObject victoryUI;
    public GameObject bossObject; // GameObject của Boss
    public KeyCode interactKey = KeyCode.E;

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            if (bossObject == null) // Boss đã bị Destroy()
            {
                if (victoryUI != null)
                {
                    victoryUI.SetActive(true);
                    Debug.Log("🎉 Victory UI hiển thị!");
                }
            }
            else
            {
                Debug.Log("👹 Boss vẫn chưa chết!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("🟢 Player vào vùng tương tác Victory.");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("🔴 Player rời vùng Victory.");
        }
    }
}
