using UnityEngine;

public class MagnetSystem : MonoBehaviour
{
    public Color colorN = Color.red;
    public Color colorS = Color.blue;
    public Color colorNone = Color.gray;
    public SpriteRenderer[] faces = new SpriteRenderer[4];
    //是否有按右鍵來變換SN
    private bool isSwitching = false;
    private int activeFaceIndex = -1; //當前啟用的面索引是0~3這邊給-1是怕預設錯誤位子
    [Header("磁力狀態 (唯讀)")]
    // 【修改】改成記錄這一幀 "目前受到的最大力道"
    public float maxForceReceivedThisFrame = 0f;
    void Update()
    {
        //這是判斷是否有點滑鼠左鍵(1是左2是右)
        if (Input.GetMouseButtonUp(1)) { isSwitching = !isSwitching; }
        ActFace();
        ChangeColor();
    }


    //這邊是判斷哪個面靠近來去做SN極
    void ActFace()
    #region 
    {   //取得滑鼠位置
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;    //因為是2D所以Z軸歸0
        Vector2 dirToMouse = (mousePos - transform.position).normalized;
        // 我們要找出主角的 "上、右、下、左" 哪一個軸跟滑鼠方向最接近
        // 使用 Vector2.Dot (內積) 來計算相似度，越接近 1 代表方向越一致
        float dotTop = Vector2.Dot(transform.up, dirToMouse);
        float dotRight = Vector2.Dot(transform.right, dirToMouse);
        float dotBottom = Vector2.Dot(-transform.up, dirToMouse);
        float dotLeft = Vector2.Dot(-transform.right, dirToMouse);
        //這邊是單純比大小而已
        float maxDot = dotTop;
        activeFaceIndex = 0;    //先設定上方為最近
        if (dotRight > maxDot) { maxDot = dotRight; activeFaceIndex = 1; }
        if (dotBottom > maxDot) { maxDot = dotBottom; activeFaceIndex = 2; }
        if (dotLeft > maxDot) { maxDot = dotLeft; activeFaceIndex = 3; }

    }
    #endregion

    //這邊是改變顏色
    void ChangeColor()
    #region
    {
        // 先把所有面重置為灰色
        for (int i = 0; i < faces.Length; i++)
        {
            if (faces[i] != null) faces[i].color = colorNone;
        }

        // 決定 N 和 S
        // 如果沒反轉：面向滑鼠的是 N (紅)，對面是 S (藍)
        // 如果有反轉：面向滑鼠的是 S (藍)，對面是 N (紅)
        Color faceColor = isSwitching ? colorS : colorN;
        Color oppositeColor = isSwitching ? colorN : colorS;

        // 設定面向滑鼠的那一面
        if (faces[activeFaceIndex] != null)
            faces[activeFaceIndex].color = faceColor;

        // 設定對面 (索引值 +2 就是對面，例如 0的對面是2)
        int oppositeIndex = (activeFaceIndex + 2) % 4;
        if (faces[oppositeIndex] != null)
            faces[oppositeIndex].color = oppositeColor;
    }
    #endregion

    //這是牆壁會呼叫玩家靠近牆壁的那面是哪個極的
    public int GetFacePolarity(SpriteRenderer faceToCheck)
    #region 
    {
        if (faceToCheck.color == colorN) return 1;
        if (faceToCheck.color == colorS) return -1;
        return 0;
    }
    #endregion
    //每一幀歸零
    void FixedUpdate()
    {
        maxForceReceivedThisFrame = 0f;
    }
    
}
