using UnityEngine;
using UnityEngine.UI;  // nếu dùng Text hoặc Button
using UnityEngine.SceneManagement; // nếu muốn chuyển scene
using System.Collections;

public class GameWinManager : MonoBehaviour
{
    public GameObject victory; // Gán UI hiện "You Win"
    public AudioClip winSound; // Âm thanh khi thắng
    private AudioSource audioSource;

    private bool gameWon = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (victory != null)
            victory.SetActive(false); // Ẩn panel lúc đầu
    }

    public void TriggerWin()
    {
        if (gameWon) return; // tránh gọi nhiều lần
        gameWon = true;

        Debug.Log("You Win!");
        if (victory != null)
            victory.SetActive(true); // Hiện UI

        if (audioSource != null && winSound != null)
            audioSource.PlayOneShot(winSound); // Phát nhạc thắng

        // Option: Dừng mọi hoạt động game
        Time.timeScale = 0f;

        // Hoặc chuyển scene sau vài giây
        // StartCoroutine(LoadNextScene());
    }

    // Nếu muốn chuyển cảnh sau 3 giây:
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene("MainMenu"); // hoặc scene tiếp theo
    }


}
