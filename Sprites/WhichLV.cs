using UnityEngine;
using UnityEngine.SceneManagement; // 記得加這行，才能切換場景

public class WhichLV : MonoBehaviour
{
    public void Lv_start(int LeveNum)
    {
        string SceneName = "LV" + LeveNum;
        SceneManager.LoadScene(SceneName);

    }
    public void BackTitle()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
