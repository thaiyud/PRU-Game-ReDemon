using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoardScene : MonoBehaviour
{
    private string scoreboardSceneName = "HighScoreMenu"; // Tên Scene bảng điểm

    void Update()
    {
        // Kiểm tra khi nhấn phím Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            string currentSceneName = SceneManager.GetActiveScene().name; // Lấy tên Scene hiện tại
            
            // Nếu đang ở Scene bảng điểm, chuyển về Scene trước đó
            if (currentSceneName == scoreboardSceneName)
            {
                SceneManager.LoadScene(PlayerPrefs.GetString("LastScene", "DefaultScene"));
            }
            // Nếu đang ở bất kỳ Scene nào khác, chuyển qua Scene bảng điểm và lưu lại tên Scene
            else
            {
                PlayerPrefs.SetString("LastScene", currentSceneName); // Lưu tên Scene hiện tại vào PlayerPrefs
                SceneManager.LoadScene(scoreboardSceneName);
            }
        }
    }
}
