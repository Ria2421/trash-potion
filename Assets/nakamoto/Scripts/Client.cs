//---------------------------------------------------------------
//
//  とらっしゅぽーしょん！クライアント [ Trash_Portion_Server ]
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
    [SerializeField] InputField nameInput;

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
            byte[] bufferJson = recvBuffer.Skip(1).ToArray();                          // 1バイト目をスキップ
            string recevieString = Encoding.UTF8.GetString(recvBuffer, 0, length);     // 受信データを文字列に変換

            // 何Pか表示
            playerText.text = "あなたは" + recevieString + "Pです";

            //// 接続完了の受信待ち
            //length = await stream.ReadAsync(buffer, 0, buffer.Length);      // 受信データのバイト数を取得
            //recevieString = Encoding.UTF8.GetString(buffer, 0, length);     // 受信データを文字列に変換

            //if(recevieString == "完了")
            //{
            //    /* フェード処理 (黒)  
            //        ( "シーン名",フェードの色, 速さ);  */
            //    Initiate.DoneFading();
            //    Initiate.Fade("NextScene", Color.black, 1.5f);
            //}
        }
        catch (Exception ex)
        {
            // エラー発生時
            Debug.Log(ex);
            connectText.text = "接続失敗";
        }
    }

    public async void sendUserData()
    {
        // 送信用データの作成
        UserData userData = new UserData();
        userData.UserName = nameInput.text;   // 入力された名前を格納
        userData.PlayerNo = myNo;             // 自分のプレイヤー番号を格納

        // 送信データをJSONシリアライズ
        string json = JsonConvert.SerializeObject(userData);

        // 送信処理
        byte[] buffer = Encoding.UTF8.GetBytes(json);         // JSONをbyteに変換
        await stream.WriteAsync(buffer, 0, buffer.Length);    // JSON送信処理

        // 送信待機文字表示
        connectText.text = "送信中...";


    }
}
