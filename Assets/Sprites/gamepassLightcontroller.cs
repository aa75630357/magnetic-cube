using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class gamepassLightcontroller : MonoBehaviour
{
    public ParticleSystem LightBeamFX;  //引入特效
    public GameObject disappearFX;   //玩家消失特效
    public float lightTime = 2.0f;
    private bool isTregger = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTregger) { return; }
        if (other.GetComponent<PlayerMovement>() != null)
        {
            Debug.Log("過關~");
            isTregger = true;
            if (LightBeamFX != null)
            {
                LightBeamFX.Play();
            }
            // 如果你有做一個 "砰" 的特效 prefab，把它拉進來，這裡會自動生成
            if (disappearFX != null)
            {
                Instantiate(disappearFX, other.transform.position, Quaternion.identity);
            }
            // 簡單暴力，直接關掉
            other.gameObject.SetActive(false);
            StartCoroutine(WaitAndShowUI());
        }
    }
    IEnumerator WaitAndShowUI()
    {
        yield return new WaitForSeconds(lightTime);

        Debug.Log("呼叫過關 UI...");
        
        // 呼叫 GameManager，if是確認只有一個管理員

        if(GameManagerLV1.Instance != null)
        {
        GameManagerLV1.Instance.LevelComplete();         
        }
    }

}

