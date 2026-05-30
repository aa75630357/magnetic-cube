using System;
using UnityEngine;

public class MagneticWall : MonoBehaviour
{
    [Header("牆壁屬性")]    //顯示在引擎的標題
    //牆壁的屬性吸收和排斥和NS
    public int SorN = 1;    //1是N(紅),-1是S(藍)
    public float magnetForce = 80f;   //吸力斥力最大
    public float magneDistenceReduce = 5f;  //距離越遠越小
    [Header("視覺呈現(磁力顏色)")]
    public SpriteRenderer myRenderer;

    //初始化顏色主要是確認顏色與SorN沒有不同色
    void Start()
    #region 
    {
        if (myRenderer != null)
        {
            myRenderer.color = (SorN == 1) ? Color.red : Color.blue;
        }
    }
    #endregion

    //曾測範圍內是否有角色來去最判斷是否吸斥
    void OnTriggerStay2D(Collider2D other)  //這是特別函式每一幀都會執行
    {
        
        //檢查進來的是不是玩家 (有沒有掛 MagnetSystem)
        //這邊主要是抓腳本如果有角色的腳本就代表這是玩家
        MagnetSystem playerMagnet = other.GetComponent<MagnetSystem>();
        if (playerMagnet == null) return;

        // 【關鍵邏輯】找出玩家身上 "離我最近" 的那張貼紙
        SpriteRenderer[] playerFaces = playerMagnet.faces; // 讀取玩家的 faces 陣列
        SpriteRenderer closestFace = null;  //這是先給最近的那面一個宣告
        float minDistance = float.MaxValue;//初質要抓最短距離所以這邊用最大

        //每個點和牆壁的距離
        foreach (var face in playerFaces)
        {
            if (face == null) continue;

            // 計算牆壁中心 到 這張貼紙 的距離
            float dist = Vector2.Distance(transform.position, face.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;//如果比較短就換
                closestFace = face;//這是紀錄更短的面
            }
        }
        //如果距離太遠，直接不吸了 (return)
        if (minDistance > magneDistenceReduce) return;
        if (closestFace == null) return; // 防呆

        //問玩家：這張最近的貼紙是什麼極性？
        int facePolarity = playerMagnet.GetFacePolarity(closestFace);
        

        // 如果那是灰色(無磁力)，牆壁就不理它
        if (facePolarity == 0) return;

        // 計算力道 (線性衰減：越近越強)
        // 這種算法會讓你在靠近牆壁時感受到強大的吸力/阻力
        float forceMultiplier = 1f - ( minDistance/magneDistenceReduce);
        forceMultiplier = Mathf.Clamp(forceMultiplier, 0f, 1f); // 保持至少 10% 力道
        //最後算出的這個劇裡所需要的力
        float myCalculatedForce = magnetForce * forceMultiplier;
        if (myCalculatedForce > playerMagnet.maxForceReceivedThisFrame)
        {
            float forceDifference = myCalculatedForce - playerMagnet.maxForceReceivedThisFrame;
            // D. 更新玩家身上的 "最大力紀錄"，告訴後面的磁鐵：「現在最強的是我，力道是 myCalculatedForce」
            playerMagnet.maxForceReceivedThisFrame = myCalculatedForce;
            // 要推人，必須先抓到對方身上的 "Rigidbody2D" (物理引擎核心)。
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();

            // 計算方向向量：從 "貼紙" 指向 "牆壁中心".normalized 意思是把箭頭長度變成 1 (只保留方向，不影響力道大小)。
            Vector2 directionToWall = (transform.position - closestFace.transform.position).normalized;


            // 同性相斥 (N碰N 或 S碰S)
            if (facePolarity == SorN)
            {
                // 往 "牆壁的反方向" 推那張貼紙
                // AddForceAtPosition 會推動邊緣，讓方塊一邊飛一邊旋轉
                playerRb.AddForceAtPosition(-directionToWall * forceDifference, closestFace.transform.position);
            }
            // 異性相吸 (N碰S)
            else
            {
                // 往 "牆壁的方向" 拉那張貼紙
                // 這會自動把方塊 "轉正" 並吸上去
                playerRb.AddForceAtPosition(directionToWall * forceDifference, closestFace.transform.position);
            }
            //測試程式碼
            Debug.DrawLine(closestFace.transform.position, transform.position, Color.yellow);
        }
    }
}
