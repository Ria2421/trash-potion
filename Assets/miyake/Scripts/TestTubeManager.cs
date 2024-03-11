//==============================================
//Autor:三宅歩人
//Day:3/1
//寸止めゲーム処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestTubeManager : MonoBehaviour
{
    [SerializeField] float speed;
    public GameObject good;
    public GameObject veryGood;
    public GameObject Bad;
    public Slider slider;
    public Text timerText;
    private bool maxValue;
    bool endCountDown;
    private NetworkManager networkManager;

    void Start()
    {
        slider.value = 0;
        endCountDown = false;

        // ネットワークマネージャーの取得
        //networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    void Update()
    {
        if (timerText.text == "GO!!")
        {
            endCountDown = true;
        }

        if (endCountDown)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (slider.value >= 94)
                {   // 失敗
                    Bad.SetActive(true);
                    // 失敗情報の送信
                    //networkManager.SendPotionStatus((int)EventID.PotionFailure);
                }
                else if (slider.value >= 68 && slider.value < 84)
                {   // 成功
                    good.SetActive(true);
                    // 生成情報の送信
                    //networkManager.SendPotionStatus((int)EventID.PotionComplete);
                }
                else if (slider.value >= 84 && slider.value < 94)
                {   // 大成功
                    veryGood.SetActive(true);
                    // 生成情報の送信
                    //networkManager.SendPotionStatus((int)EventID.PotionComplete);
                }
                else if(slider.value < 68)
                {   // 失敗
                    Bad.SetActive(true);
                    // 失敗情報の送信
                    //networkManager.SendPotionStatus((int)EventID.PotionFailure);
                }

                // ミニゲームの終了
                Invoke("MiniGameDestroy", 1.5f);
            }

            //クリックされていなければ実行
            if (Input.GetMouseButton(0))
            {
                slider.value += speed;

                if (slider.value >= 94)
                {
                    Bad.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// ミニゲームの破棄
    /// </summary>
    private void MiniGameDestroy()
    {
        // ミニゲームを終了
        Destroy(GameObject.Find("MiniGames(Clone)"));
    }
}
