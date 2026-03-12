using UnityEngine;

public class DeathLine: MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果撞到的是玩家
        if (other.GetComponent<MagnetSystem>() != null)
        {
            Debug.Log("玩家掉下去了！死亡！");
            
            // 呼叫 GameManager 執行死亡流程 (我們等一下寫這個)
            if (GameManagerLV1.Instance != null)
            {
                GameManagerLV1.Instance.GameOver(); 
            }            
            other.gameObject.SetActive(false);
        }
    }
}
