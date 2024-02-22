//---------------------------------------------------------------
//
//  とらっしゅぽーしょん！クライアント [ Client.cs ]
// Author:Kenta Nakamoto
// Data 2024/02/08
//
//---------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System;
using UnityEngine.UI;
using System.Linq;

public class Client : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // フィールド ----------------------------------------

    /// <summary>
    /// クライアント作成
    /// </summary>
    private TcpClient tcpClient;

    /// <summary>
    /// 接続先IPアドレス
    /// </summary>
    private const string ipAddress = "127.0.0.1";

    /// <summary>
    /// 送受信サイズ
    /// </summary>
    private const int dataSize = 1024;

    /// <summary>
    /// 接続情報格納用
    /// </summary>
    private NetworkStream stream;

    /// <summary>
    /// 自分のプレイヤー番号
    /// </summary>
    private int myNo;

    /// <summary>
    /// 全プレイヤーのデータリスト
    /// </summary>
    private UserDataList userDataList;

    /// <summary>
    /// 接続時の表示テキスト
    /// </summary>
    [SerializeField] Text connectText;

    /// <summary>
    /// プレイヤー番号表示テキスト
    /// </summary>
    [SerializeField] Text playerText;

    /// <summary>
    /// 名前入力用UI
    /// </summary>
    [SerializeField] GameObject nameInput;

    /// <summary>
    /// プレイヤーネーム表示用
    /// </summary>
    [SerializeField] GameObject playerName;

    /// <summary>
    /// 表示用プレイヤーオブジェ
    /// </summary>
    [SerializeField] GameObject playerObj;

    /// <summary>
    /// 準備完了ボタン格納用
    /// </summary>
    [SerializeField] GameObject completeButton;

    //------------------------------------------------------------------------------
    // メソッド ------------------------------------------

    // Start is called before the first frame update
    async void Start()
    {
        // クライアント処理
        await StartClient(ipAddress, 20001);
    }

    /// <summary>
    /// クライアント接続処理
    /// </summary>
    /// <param name="ipaddress"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    private async Task StartClient(string ipaddress, int port)
    {
        // サーバーへ接続
        try
        {
            //クライアント作成
            tcpClient = new TcpClient();

            // 送受信タイムアウト設定 (msec)
            tcpClient.SendTimeout = 1000;
            tcpClient.ReceiveTimeout = 1000;

            // サーバーへ接続要求
            await tcpClient.ConnectAsync(ipaddress, port);
            connectText.text = "接続完了";

            // サーバーからPL番号を受信待機
            byte[] recvBuffer = new byte[dataSize];                                    // 送受信データ格納用
            stream = tcpClient.GetStream();                                            // クライアントのデータ送受信に使うNetworkStreamを取得
            int length = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);     // 受信データのバイト数を取得

            // 受信データからイベントIDを取り出す
            int eventID = recvBuffer[0];

            // 受信データから文字列を取り出す
            byte[] bufferJson = recvBuffer.Skip(1).ToArray();                            // 1バイト目をスキップ
            string recevieString = Encoding.UTF8.GetString(bufferJson, 0, length-1);     // 受信データを文字列に変換

            // 何Pか表示
            playerText.text = "あなたは" + recevieString[0] + "Pです";

            // 自分のPL Noを保存
            myNo = int.Parse(recevieString[0].ToString());

            // 入力フィールドの有効化
            nameInput.SetActive(true);
        }
        catch (Exception ex)
        {
            // エラー発生時
            Debug.Log(ex);
            connectText.text = "接続失敗";
        }
    }

    /// <summary>
    /// ユーザーデータ送信処理
    /// </summary>
    public async void sendUserData()
    {
        // 送信用ユーザーデータの作成 ---------------------------------------------------------------------------

        UserData userData = new UserData();
        userData.UserName = nameInput.GetComponent<InputField>().text;   // 入力された名前を格納
        userData.PlayerNo = myNo;                                        // 自分のプレイヤー番号を格納

        // 送信データをJSONシリアライズ
        string json = JsonConvert.SerializeObject(userData);

        // 送信処理
        byte[] buffer = Encoding.UTF8.GetBytes(json);                // JSONをbyteに変換
        buffer = buffer.Prepend((byte)EventID.UserData).ToArray();   // 送信データの先頭にイベントIDを付与
        await stream.WriteAsync(buffer, 0, buffer.Length);           // JSON送信処理

        // 入力フィールドの無効化
        nameInput.SetActive(false);

        // 送信待機文字表示
        connectText.text = "送信中...";

        // サーバーからPLデータリストの受信 ----------------------------------------------------------------------------------------------

        byte[] recvBuffer = new byte[dataSize];                                    // 送受信データ格納用
        stream = tcpClient.GetStream();                                            // クライアントのデータ送受信に使うNetworkStreamを取得
        int length = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);     // 受信データのバイト数を取得

        // 受信データからイベントIDを取り出す
        int eventID = recvBuffer[0];

        // 受信データから文字列を取り出す
        byte[] bufferJson = recvBuffer.Skip(1).ToArray();                              // 1バイト目をスキップ
        string recevieString = Encoding.UTF8.GetString(bufferJson, 0, length - 1);     // 受信データを文字列に変換

        // Jsonデシリアライズ
        userDataList = JsonConvert.DeserializeObject<UserDataList>(recevieString);

        // データ受信表示
        connectText.text = "データ送受信完了";

        // プレイヤー一覧の表示
        OutputPlayer();
    }

    /// <summary>
    /// プレイヤー一覧の表示処理
    /// </summary>
    private void OutputPlayer()
    {
        // プレイヤーオブジェ・名前の表示
        playerObj.SetActive(true);
        playerName.SetActive(true);

        // プレイヤー名の反映
        for (int i = 0; i < userDataList.userList.Length; i++)
        {
            // 各Noプレイヤー名をテキストに適用
            string nameObj = (i + 1).ToString() + "PName";
            GameObject.Find(nameObj).GetComponent<Text>().text = userDataList.userList[i].UserName;
        }

        // 準備完了ボタンの表示
        completeButton.SetActive(true);
    }

    /// <summary>
    /// 準備完了フラグ送信処理
    /// </summary>
    public async void SendComplete()
    {   // 準備完了フラグ送信処理 -------------------------------------------------------------------------------------------------------------

        // 準備完了ボタンの非表示
        completeButton.SetActive(false);

        UserData userData = new UserData();
        userData.UserName = nameInput.GetComponent<InputField>().text;   // 入力された名前を格納
        userData.PlayerNo = myNo;                                        // 自分のプレイヤー番号を格納

        // 送信データをJSONシリアライズ
        string json = JsonConvert.SerializeObject(userData);

        // 送信処理
        byte[] buffer = Encoding.UTF8.GetBytes(json);                  // JSONをbyteに変換
        buffer = buffer.Prepend((byte)EventID.InGameFlag).ToArray();   // 送信データの先頭にイベントIDを付与
        await stream.WriteAsync(buffer, 0, buffer.Length);             // JSON送信処理

        // 送信待機文字表示
        connectText.text = "完了待機中...";

        // 完了フラグ受信処理 -------------------------------------------------------------------------------------------------------------

        byte[] recvBuffer = new byte[dataSize];                                    // 送受信データ格納用
        stream = tcpClient.GetStream();                                            // クライアントのデータ送受信に使うNetworkStreamを取得
        int length = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);     // 受信データのバイト数を取得

        // 受信データからイベントIDを取り出す
        int eventID = recvBuffer[0];

        // 受信データから文字列を取り出す
        byte[] bufferJson = recvBuffer.Skip(1).ToArray();                              // 1バイト目をスキップ
        string recevieString = Encoding.UTF8.GetString(bufferJson, 0, length - 1);     // 受信データを文字列に変換

        // データ受信表示
        connectText.text = recevieString;

#if DEBUG
        // 状況表示
        Debug.Log("シーン移動");
#endif

        //シーン移動処理
        Invoke("NextScene", 1.5f);
    }

    /// <summary>
    /// シーン移動処理
    /// </summary>
    private void NextScene()
    {
        /* フェード処理 (黒)  
            ( "シーン名",フェードの色, 速さ);  */
        Initiate.DoneFading();
        Initiate.Fade("ModeSelection", Color.black, 1.5f);
    }
}
