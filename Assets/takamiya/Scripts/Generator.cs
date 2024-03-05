//======================================================
//ポーションを上から降らす
//Author：高宮祐翔
//Date/2/20
//
//=======================================================
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Generator : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // フィールド ----------------------------------------

    /// <summary>
    /// 落下させるポーションのプレハブ
    /// </summary>
    public GameObject[] falls;

    /// <summary>
    /// モード選択UI
    /// </summary>
    [SerializeField] GameObject selectUI;

    /// <summary>
    /// 選択待機UI
    /// </summary>
    [SerializeField] GameObject waitUI;

    //------------------------------------------------------------------------------
    // メソッド ------------------------------------------

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        if(NetworkManager.MyNo == 1)
        {   // 1Pの時は選択画面を表示
            selectUI.SetActive(true);
            waitUI.SetActive(false);
        }
        else
        {   // 1P以外は待機画面を表示
            selectUI.SetActive(false);
            waitUI.SetActive(true);
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (Time.frameCount % 30 == 0)
        {   // 指定した間隔でポーションの生成
            GameObject potion = Instantiate(
                falls[Random.Range(0, falls.Length)],
                new Vector3(Random.Range(-14f, 14f), transform.position.y, transform.position.z),
                Quaternion.identity
                );

            // 一定時間後にポーションの破棄
            Destroy(potion, 8f);
        }
    }

    /// <summary>
    /// ボタンを押した時
    /// </summary>
    public async void ButtonPushSE()
    {
        // 送信処理
        string json = "1";
        byte[] buffer = Encoding.UTF8.GetBytes(json);                      // JSONをbyteに変換
        buffer = buffer.Prepend((byte)EventID.InGameFlag).ToArray();       // 送信データの先頭にイベントIDを付与
        NetworkStream stream = NetworkManager.MyTcpClient.GetStream();
        await stream.WriteAsync(buffer, 0, buffer.Length);                 // JSON送信処理
#if DEBUG
        Debug.Log("インゲーム送信");
#endif
    }
}
