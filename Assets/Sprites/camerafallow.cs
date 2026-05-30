using UnityEngine;

public class camerafallow : MonoBehaviour
{
    public Transform target; // 玩家
    
    [Header("邊界設定 (推動框大小)")]
    public float xZone = 3f; // 左右容許範圍
    public float yZone = 3f; // 上下容許範圍
    public float y_up_Zone = 2f; // 上下容許範圍
    
    public float smoothSpeed = 0.125f; // 平滑度
    public Vector3 offset = new Vector3(0, 0, -10); // 記得 Z 要是 -10

    private Vector3 targetPos;

    void Start()
    {
        // 遊戲開始時，先對準玩家
        targetPos = transform.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // --- 核心邏輯：檢查有沒有頂到邊框 ---
        
        // 1. 檢查 X 軸 (左右)
        // 計算 "玩家" 減 "目前攝影機目標" 的距離
        float distX = target.position.x - targetPos.x;
        
        // 如果玩家跑到右邊邊界外 (距離 > 3)
        if (distX > xZone)
        {
            targetPos.x += distX - xZone; // 攝影機往右移一點，剛好跟上
        }
        // 如果玩家跑到左邊邊界外 (距離 < -3)
        else if (distX < -xZone)
        {
            targetPos.x += distX + xZone; // 攝影機往左移
        }

        // 2. 檢查 Y 軸 (上下) - 邏輯一樣
        float distY = target.position.y - targetPos.y;
        if (distY > yZone)
        {
            targetPos.y += distY - yZone;
        }
        else if (distY < -y_up_Zone)
        {
            targetPos.y += distY + y_up_Zone;
        }

        // 3. 最終移動 (加上 Lerp 讓推動時稍微平滑一點，不會太生硬)
        // 保持 Z 軸固定
        Vector3 finalPos = new Vector3(targetPos.x + offset.x, targetPos.y + offset.y, offset.z);
        transform.position = Vector3.Lerp(transform.position, finalPos, smoothSpeed);
    }
    
    // 畫出輔助線 (只有在 Scene 視窗看得到，讓你知道框框多大)
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        // 畫出中間那個死區框框
        Gizmos.DrawWireCube(transform.position - offset, new Vector3(xZone * 2, yZone * 2, 0));
    }
}