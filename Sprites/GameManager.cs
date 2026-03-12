using UnityEngine;
using UnityEngine.SceneManagement; // 切換場景需要這個
public class GameManager: MonoBehaviour
{
    // 單例模式 (Singleton)：讓別的腳本可以用 GameManager.Instance 找到我
    public static GameManager Instance;
    [Header("UI設定")]
    public GameObject deathPanel; // 把做好的死亡畫面(UI Panel)拉進來
    private void Awake()
    {
        // 確保只有一個總管
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
// 遊戲開始時，確保死亡畫面是關閉的
    void Start()
    {
        if(deathPanel != null) 
            deathPanel.SetActive(false);
            
        // 確保時間是流動的 (因為死掉時我們會把時間暫停)
        Time.timeScale = 1f;
    }

    // --- 給 DeathZone 呼叫的函式 ---
    public void GameOver()
    {
        Debug.Log("遊戲結束流程啟動");
        
        // 1. 顯示死亡畫面
        if(deathPanel != null) 
            deathPanel.SetActive(true);

        // 2. (選填) 暫停遊戲物理運算，讓方塊定格
        // Time.timeScale = 0f; 
    }

    // --- 給 UI 按鈕呼叫的函式 ---
    
    // 重試本關
    public void RestartLevel()
    {
        // 讀取現在這個場景的名字，重新載入一次
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 回主選單 (之後會用到)
    public void BackToMenu()
    {
        SceneManager.LoadScene("TitleScreen"); // 記得之後要建立這個場景
    }
}
