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
    /// 接続情報格納用
    /// </summary>
    private NetworkStream stream;

    /// <summary>
    /// 接続時の表示テキスト
    /// </summary>
    [SerializeField] Text connectText;

    /// <summary>
    /// プレイヤー番号表示テキスト
    /// </summary>
    [SerializeField] Text playerText;

    //------------------------------------------------------------------------------
    // メソッド ------------------------------------------

    // Start is called before the first frame update
    async void Start()
    {
        // クライアント処理
        await StartClient(ipAddress, 20000);
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

            // サーバーからPL番号を受信
            byte[] buffer = new byte[2048];                                        // 送受信データ格納用
            stream = tcpClient.GetStream();                                        // クライアントのデータ送受信に使うNetworkStreamを取得
            int length = await stream.ReadAsync(buffer, 0, buffer.Length);         // 受信データのバイト数を取得
            string recevieString = Encoding.UTF8.GetString(buffer, 0, length);     // 受信データを文字列に変換

            // 何Pか表示
            playerText.text = "あなたは" + recevieString + "Pです";

            // 接続完了の受信待ち
            length = await stream.ReadAsync(buffer, 0, buffer.Length);      // 受信データのバイト数を取得
            recevieString = Encoding.UTF8.GetString(buffer, 0, length);     // 受信データを文字列に変換

            if(recevieString == "完了")
            {
                /* フェード処理 (黒)  
                    ( "シーン名",フェードの色, 速さ);  */
                Initiate.DoneFading();
                Initiate.Fade("NextScene", Color.black, 1.5f);
            }
        }
        catch (Exception ex)
        {
            // エラー発生時
            Debug.Log(ex);
            connectText.text = "接続失敗";
        }
    }
}
