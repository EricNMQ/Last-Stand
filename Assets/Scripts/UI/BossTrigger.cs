using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public Dialogmanager dialog;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialog.ShowMessage("(Player:)Vậy là… ngươi chính là kẻ đã khiến cả ngôi làng chìm trong bóng tối? , (Boss:)Kẻ phàm tục như ngươi… lại dám bước vào lãnh địa của ta? , (Player:)Tôi không đến đây để nói chuyện. Tôi đến để chấm dứt mọi thứ. , (Boss:)Được thôi! Hãy để máu của ngươi tô đậm thêm truyền thuyết về ta!");
        }
    }
}
