using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManagerLV1 : MonoBehaviour
{
    public static GameManagerLV1 Instance;
    public GameObject endGame;
    public GameObject GamePass;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        if(endGame != null)endGame.SetActive(false);
        if (GamePass != null) GamePass.SetActive(false);
        
        Time.timeScale = 1f;
    }
    public void LoadNextLevel()
    {
        // 取得目前場景的編號，取得編號然後加一就是下一關
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {SceneManager.LoadScene(currentSceneIndex + 1);}
        else{
            SceneManager.LoadScene("Choose_LV");
            Debug.Log("最後一關");
        }
 
    }
    public void Back()
    {
        SceneManager.LoadScene("Choose_LV");
    }
    public void LevelComplete()
    {
        Debug.Log("恭喜過關！顯示選單");
        if (GamePass != null)
        GamePass.SetActive(true);
    }
    public void GameOver()
    {
        Debug.Log("死亡~");
        if (endGame != null)
            endGame.SetActive(true);
    }
    public void TryAgain()
    {
        Debug.Log("再試試看");
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
