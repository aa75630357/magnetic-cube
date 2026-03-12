using UnityEngine;
using UnityEngine.SceneManagement; // 記得加這行，才能切換場景
public class ChangeUI : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Choose_LV");
    }
    public void QuitGame()
    {
        Debug.Log("遊戲已關閉！"); // 在編輯器裡看不到關閉，所以印這行字給你看
        Application.Quit(); // 這是真的關閉程式 (打包後才有效)
    }
}
