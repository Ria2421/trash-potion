//---------------------------------------------------------------
//
// ネットワークマネージャー [NetworkManager.cs ]
// Author:Kenta Nakamoto
// Data:2024/02/08
// Update:2024/02/28
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
using System.Threading;

public class NetworkManager : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // フィールド ----------------------------------------

    /// <summary>
    /// クライアント作成
    /// </summary>
    public static TcpClient MyTcpClient
    {  get; private set; }

    /// <summary>
    /// 接続先IPアドレス
    /// </summary>
    const string ipAddress = "127.0.0.1";

    /// <summary>
    /// ポート番号
    /// </summary>
    const int portNum = 20001;

    /// <summary>
    /// 送受信サイズ
    /// </summary>
    const int dataSize = 1024;

    /// <summary>
    /// 接続情報格納用
    /// </summary>
    NetworkStream stream;

    /// <summary>
    /// 自分のプレイヤー番号
    /// </summary>
    public static int MyNo
    {  get; private set; }

    /// <summary>
    /// マッチング中テキスト
    /// </summary>
    [SerializeField] GameObject matchingText;

    /// <summary>
    /// 待機中UI
    /// </summary>
    [SerializeField] GameObject readyUI;

    /// <summary>
    /// 自機を指す矢印のオブジェクト
    /// </summary>
    [SerializeField] GameObject[] arrowYouObjs;

    /// <summary>
    /// 全プレイヤーの名前格納 
    /// </summary>
    [SerializeField] GameObject[] playerNames;

    /// <summary>
    /// 全プレイヤーの待機状態格納用
    /// </summary>
    [SerializeField] GameObject[] playerStatus;

    /// <summary>
    /// 名前入力用UI
    /// </summary>
    [SerializeField] GameObject nameInput;

    /// <summary>
    /// 準備完了ボタン
    /// </summary>
    [SerializeField] GameObject completeButton;

    /// <summary>
    /// メインスレッドに処理実行を依頼するもの
    /// </summary>
    SynchronizationContext context;

    //------------------------------------------------------------------------------
    // メソッド ------------------------------------------

    /// <summary>
    /// 初期関数
    /// </summary>
    async void Start()
    {
        context = SynchronizationContext.Current;

        // 接続処理の実行
        await StartClient(ipAddress, portNum);

        // シーン遷移時にNetworkManagerを破棄しないように設定
        DontDestroyOnLoad(gameObject);
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
            MyTcpClient = new TcpClient();

            // 送受信タイムアウト設定 (msec)
            MyTcpClient.SendTimeout = 1000;
            MyTcpClient.ReceiveTimeout = 1000;

            // サーバーへ接続要求
            await MyTcpClient.ConnectAsync(ipAddress, port);

            // サーバーからPL番号を受信待機
            byte[] recvBuffer = new byte[dataSize];                                    // 送受信データ格納用
            stream = MyTcpClient.GetStream();                                            // クライアントのデータ送受信に使うNetworkStreamを取得
            int length = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);     // 受信データのバイト数を取得

            // 受信データからイベントIDを取り出す
            int eventID = recvBuffer[0];

            // 受信データから文字列を取り出す
            byte[] bufferJson = recvBuffer.Skip(1).ToArray();                            // 1バイト目をスキップ
            string recevieString = Encoding.UTF8.GetString(bufferJson, 0, length - 1);   // 受信データを文字列に変換

            // 自分のPL Noを保存
            MyNo = int.Parse(recevieString[0].ToString());

            // マッチングテキストを非表示
            matchingText.SetActive(false);

            // 待機中UIを表示
            readyUI.SetActive(true);

            // 自分のPL番号の上に矢印表示
            arrowYouObjs[MyNo - 1].SetActive(true);

            // 受信用スレッドの起動
            Thread thread = new Thread(new ParameterizedThreadStart(RecvProc));
            thread.Start(MyTcpClient);
#if DEBUG
            // 準備ボタン等の有効化
            Debug.Log("待機画面表示");
#endif
        }
        catch (Exception ex)
        {
            // エラー発生時
            Debug.Log(ex);
        }
    }

    /// <summary>
    /// 受信用スレッド
    /// </summary>
    /// <param name="arg"></param>
    async void RecvProc(object arg)
    {
        TcpClient tcpClient = (TcpClient)arg;

        NetworkStream stream = tcpClient.GetStream();

        while (true)
        {
            // 受信待機する
            byte[] recvBuffer = new byte[dataSize];
            int length = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);

            // 接続切断チェック
            if(length <= 0)
            {
                
            }

            // 受信データからイベントIDを取り出す
            int eventID = recvBuffer[0];

            // 受信データからJSON文字列を取り出す
            byte[] bufferJson = recvBuffer.Skip(1).ToArray();     // 1バイト目をスキップ
            string jsonString = System.Text.Encoding.UTF8.GetString(bufferJson, 0, length - 1);

            context.Post(_ =>
            {
                switch (eventID)
                {
                    case (int)EventID.UserData: // 各PLの名前表示処理

                        // Jsonデシリアライズ
                        UserData userData = JsonConvert.DeserializeObject<UserData>(jsonString);

                        // 送信されてきたPLNoの名前をクライアントのテキストに反映させる
                        playerNames[userData.PlayerNo - 1].GetComponent<Text>().text = userData.UserName;

                        break;

                    case (int)EventID.CompleteFlag: // 各PLの準備完了表示処理

                        // Jsonデシリアライズ
                        userData = JsonConvert.DeserializeObject<UserData>(jsonString);

                        // 完了表示
                        playerStatus[userData.PlayerNo - 1].GetComponent<Text>().text = "準備完了";

                        break;

                    case (int)EventID.InSelectFlag: // 選択画面遷移処理
                        NextScene("ModeSelection");
                        break;

                    case (int)EventID.InGameFlag: // ゲーム画面遷移処理
                        NextScene("IGC");
                        break;

                    default: 
                        break;
                }
            }, null);
        }

        // 通信を切断
        tcpClient.Close();
    }

    /// <summary>
    /// ユーザー名送信処理
    /// </summary>
    public async void sendUserData()
    {
        //送信用ユーザーデータの作成
        UserData userData = new UserData();
        userData.UserName = nameInput.GetComponent<InputField>().text;   // 入力された名前を格納
        userData.PlayerNo = MyNo;                                        // 自分のプレイヤー番号を格納

        // 送信データをJSONシリアライズ
        string json = JsonConvert.SerializeObject(userData);

        // 送信処理
        byte[] buffer = Encoding.UTF8.GetBytes(json);                // JSONをbyteに変換
        buffer = buffer.Prepend((byte)EventID.UserData).ToArray();   // 送信データの先頭にイベントIDを付与
        await stream.WriteAsync(buffer, 0, buffer.Length);           // JSON送信処理

        // 入力フィールドの無効化
        nameInput.SetActive(false);

        // 準備完了ボタンの有効化
        completeButton.SetActive(true);
    }

    /// <summary>
    /// 準備完了フラグ送信処理
    /// </summary>
    public async void SendComplete()
    {
        // 準備完了ボタンの非表示
        completeButton.SetActive(false);

        UserData userData = new UserData();
        userData.IsReady = true;                 // 準備完了フラグをtrueに
        userData.PlayerNo = MyNo;                // 自分のプレイヤー番号を格納

        // 送信データをJSONシリアライズ
        string json = JsonConvert.SerializeObject(userData);

        // 送信処理
        byte[] buffer = Encoding.UTF8.GetBytes(json);                    // JSONをbyteに変換
        buffer = buffer.Prepend((byte)EventID.CompleteFlag).ToArray();   // 送信データの先頭にイベントIDを付与
        await stream.WriteAsync(buffer, 0, buffer.Length);               // JSON送信処理

#if DEBUG
        // 送信待機文字表示
        Debug.Log("準備完了送信");
#endif
    }

    /// <summary>
    /// シーン遷移処理
    /// </summary>
    /// <param name="sceneName"> 遷移したいシーン名 </param>
    private void NextScene(string sceneName)
    {
        /* フェード処理 (黒)  
            ( "シーン名",フェードの色, 速さ);  */
        Initiate.DoneFading();
        Initiate.Fade(sceneName, Color.black, 1.5f);
    }
}
