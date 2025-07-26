using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameObject startPanel; // Kéo Panel vào đây trong Inspector

    void Start()
    {
        // Dừng thời gian ngay khi bắt đầu game (game chưa chạy)
        Time.timeScale = 0f;
    }

    public void OnStartGame()
    {
        Debug.Log("Play button clicked"); // Kiểm tra trong Console
        startPanel.SetActive(false);      // Ẩn panel intro
        Time.timeScale = 1f;              // Bắt đầu game trở lại
    }
}
