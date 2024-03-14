//---------------------------------------------------------------
//
// ネットワークマネージャー [NetworkManager.cs ]
// Author:Kenta Nakamoto
// Data:2024/02/08
// Update:2024/03/06
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
    const string ipAddress = "20.194.122.141";

    // ↑ローカル時は"127.0.0.1"

    /// <summary>
    /// プレイヤー人数
    /// </summary>
    const int playerNum = 4;

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
    {  get; set; }          // 本稼働時は{ get; private set; }

    /// <summary>
    /// マップデータ
    /// </summary>
    public static int[,] InitTileData
    {  get; private set; }

    /// <summary>
    /// ユニット配置データ
    /// </summary>
    public static int[,] InitUnitData
    { get; private set; }

    /// <summary>
    /// マッチング中テキスト
    /// </summary>
    [SerializeField] GameObject matchingText;

    /// <summary>
    /// 待機中UI
    /// </summary>
    [SerializeField] GameObject readyUI;

    /// <summary>
    /// 待機中オブジェ
    /// </summary>
    [SerializeField] GameObject readyObj;

    /// <summary>
    /// 自機を指す矢印のオブジェクト
    /// </summary>
    [SerializeField] GameObject[] arrowYouObjs;

    /// <summary>
    /// 自機を示すテキスト
    /// </summary>
    [SerializeField] GameObject[] arrowYouTexts;

    /// <summary>
    /// 全プレイヤー情報格納 
    /// </summary>
    [SerializeField] GameObject[] playerObjects;

    /// <summary>
    /// 全プレイヤーの名前格納
    /// </summary>
    public string[] PlayerNames
    { get; private set; }

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

    /// <summary>
    /// ゲームディレクター格納用
    /// </summary>
    GameDirectorCopy directorCopy;

    //------------------------------------------------------------------------------
    // メソッド ------------------------------------------

    /// <summary>
    /// 初期関数
    /// </summary>
    async void Start()
    {
        PlayerNames = new string[playerNum];

        context = SynchronizationContext.Current;

        // 接続処理の実行
        await StartClient(ipAddress, portNum);

        // シーン遷移時にNetworkManagerを破棄しないように設定
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {
        //終了機能
        if (Input.GetKeyDown(KeyCode.Escape))
        {//ESCキーを押した場合
#if UNITY_EDITOR    //Unityエディタの場合
            UnityEditor.EditorApplication.isPlaying = false;
#else   //ビルドの場合
            Application.Quit();
#endif
        }
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
            stream = MyTcpClient.GetStream();                                          // クライアントのデータ送受信に使うNetworkStreamを取得
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
            readyObj.SetActive(true);

            // 自分のPL番号の上に矢印表示
            arrowYouObjs[MyNo - 1].SetActive(true);
            arrowYouTexts[MyNo - 1].SetActive(true);

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
                    case (int)EventID.UserData: 
                        // 各PLの名前表示処理

                        // Jsonデシリアライズ
                        UserData userData = JsonConvert.DeserializeObject<UserData>(jsonString);

                        // 送信されてきたPLNoの名前をクライアントのテキストに反映させる
                        playerObjects[userData.PlayerNo - 1].GetComponent<Text>().text = userData.UserName;

                        PlayerNames[userData.PlayerNo - 1] = userData.UserName;

                        break;

                    case (int)EventID.CompleteFlag: 
                        // 各PLの準備完了表示処理

                        // Jsonデシリアライズ
                        userData = JsonConvert.DeserializeObject<UserData>(jsonString);

                        // 完了表示
                        playerStatus[userData.PlayerNo - 1].GetComponent<Text>().text = "準備完了";

                        break;

                    case (int)EventID.InSelectFlag: 
                        // 選択画面遷移処理

                        NextScene("ModeSelection");
                        break;

                    case (int)EventID.InGameFlag: 
                        // ゲーム画面遷移処理

                        // Jsonデシリアライズ
                        List<int[,]> mapDatas = JsonConvert.DeserializeObject<List<int[,]>>(jsonString);

                        // 受信マップデータを格納
                        InitTileData = mapDatas[0];
                        InitUnitData = mapDatas[1];

                        // ゲームシーンに遷移
                        Invoke("InGameScene", 1.5f);
                        break;

                    case (int)EventID.SelectUnit:
                        // 現在ターンのユニットを選択状態へ

                        // 受信データをJsonデシリアライズ
                        SelectData selectData = JsonConvert.DeserializeObject<SelectData>(jsonString);

                        //- 指定タイルに居るユニットを選択状態へ -//

                        // ゲームディレクターの取得
                        GetGameDirector();

                        // 指定タイルのユニット選択処理
                        directorCopy.SelectUnit(selectData.z, selectData.x);

                        break;

                    case (int)EventID.MoveUnit:
                        // 現在ターンのユニットの移動処理

                        // 受信データをJsonデシリアライズ
                        MoveData moveData = JsonConvert.DeserializeObject<MoveData>(jsonString);

                        Vector3 pos = new Vector3(moveData.posX,0,moveData.posZ);

                        // 指定タイルへ現在ターンのPLオブジェクトを移動
                        directorCopy.MoveUnit(moveData.z, moveData.x, pos);

                        break;

                    case (int)EventID.PotionGenerate:
                        // PLのポーション生成開始情報を取得

                        // ゲームディレクターの取得
                        GetGameDirector();

                        // 受信PLNoをJsonデシリアライズ
                        int plNo = JsonConvert.DeserializeObject<int>(jsonString);

                        // 生成アイコン表示
                        directorCopy.SetGenerateIcon(plNo, true);

                        // ポーションステータスを生成中に変更
                        directorCopy.ChangePotionStatus(plNo, 2);

                        break;

                    case (int)EventID.PotionComplete:
                        // PLのポーション生成成功情報を取得

                        // 受信PLNoをJsonデシリアライズ
                        plNo = JsonConvert.DeserializeObject<int>(jsonString);

                        // 指定したPLNoのポーションを生成
                        directorCopy.GeneratePotion(plNo);

                        // 生成アイコン表示
                        directorCopy.SetGenerateIcon(plNo, false);

                        // ポーションステータスを生成中に変更
                        directorCopy.ChangePotionStatus(plNo, 0);

                        break;

                    case (int)EventID.PotionFailure:
                        // PLのポーション生成失敗情報を取得

                        // 受信PLNoをJsonデシリアライズ
                        plNo = JsonConvert.DeserializeObject<int>(jsonString);

                        // 生成アイコン表示
                        directorCopy.SetGenerateIcon(plNo, false);

                        // ポーションステータスを生成中に変更
                        directorCopy.ChangePotionStatus(plNo, 1);

                        break;

                    case (int)EventID.PotionThrow:
                        // 投擲フラグを受信

                        // 投擲処理を実行
                        directorCopy.PlayerThrow();

                        break;

                    case (int)EventID.PotionSetPos:

                        // 受信した設置位置をJsonデシリアライズ
                        SetPotionData setPotionData = JsonConvert.DeserializeObject<SetPotionData>(jsonString);

                        Vector3 SetPos = new Vector3(setPotionData.posX, 0, setPotionData.posZ);

                        // ポーションを設置
                        directorCopy.SetPotion(SetPos);

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

    /// <summary>
    /// ゲームシーン遷移処理
    /// </summary>
    private void InGameScene()
    {
        /* フェード処理 (黒)  
            ( "シーン名",フェードの色, 速さ);  */
        Initiate.DoneFading();
        Initiate.Fade("IGCcopy", Color.black, 1.5f);
    }

    /// <summary>
    /// ゲームディレクター取得関数
    /// </summary>
    private void GetGameDirector()
    {
        if (directorCopy != null)
        {   // 取得済みの場合はreturn
            return;
        }
        else
        {   // 取得していない時のみ実行

            // 関数呼び出しの為、コンポーネントの取得
            directorCopy = GameObject.Find("GameDirector").GetComponent<GameDirectorCopy>();
        }
    }

    //========================//
    // 接続画面データ送信処理 //
    //========================//

    /// <summary>
    /// ユーザー名送信処理
    /// </summary>
    public async void sendUserData()
    {
        if(nameInput.GetComponent<InputField>().text == "")
        {   // 空文字の時は何もせずに返す
            return;
        }

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

    //========================//
    // ゲーム内データ送信処理 //
    //========================//

    //----------------------------------------------------------------------------
    // 移動関係のデータ送信処理 ------------------------------

    /// <summary>
    /// 選択したタイル座標送信処理
    /// </summary>
    /// <param name="z"> z座標 </param>
    /// <param name="x"> x座標 </param>
    public async void SendSelectUnit(int z,int x)
    {
        // 送信用ユーザーデータの作成
        SelectData selectData = new SelectData();
        // 座標の代入
        selectData.plNo = MyNo; // 自分のPLNo
        selectData.z = z;       // 現在のタイル座標(z軸)
        selectData.x = x;       // 現在のタイル座標(x軸)

        // 送信データをJSONシリアライズ
        string json = JsonConvert.SerializeObject(selectData);

        // 送信処理
        byte[] buffer = Encoding.UTF8.GetBytes(json);                  // JSONをbyteに変換
        buffer = buffer.Prepend((byte)EventID.SelectUnit).ToArray();   // 送信データの先頭にイベントIDを付与
        await stream.WriteAsync(buffer, 0, buffer.Length);             // JSON送信処理
    }

    /// <summary>
    /// 選択した移動タイル座標送信処理
    /// </summary>
    /// <param name="z">タイル配列番号1</param>
    /// <param name="x">タイル配列番号2</param>
    /// <param name="posX">移動先タイルX座標</param>
    /// <param name="posZ">移動先タイルZ座標</param>
    /// <param name="eventID">送信データの種類</param>
    public async void SendMoveUnit(int x, int z, float posX, float posZ)
    {
        // 送信用ユーザーデータの作成
        MoveData moveData = new MoveData();
        // 座標の代入
        moveData.plNo = MyNo; // 自分のPLNo
        moveData.z = z;       // 現在のタイル座標(z軸)
        moveData.x = x;       // 現在のタイル座標(x軸)
        moveData.posX = posX; // 選択したタイル座標(x軸)
        moveData.posZ = posZ; // 選択したタイル座標(z軸)

        // 送信データをJSONシリアライズ
        string json = JsonConvert.SerializeObject(moveData);

        // 送信処理
        byte[] buffer = Encoding.UTF8.GetBytes(json);               // JSONをbyteに変換
        buffer = buffer.Prepend((byte)EventID.MoveUnit).ToArray();  // 送信データの先頭にイベントIDを付与
        await stream.WriteAsync(buffer, 0, buffer.Length);          // JSON送信処理
    }

    //----------------------------------------------------------------------------
    // ポーション関係のデータ送信処理 ----------------------

    /// <summary>
    /// ポーション生成情報の送信
    /// </summary>
    /// <param name="status">生成情報</param>
    public async void SendPotionStatus(int status)
    {
        // 送信データをJSONシリアライズ
        string json = JsonConvert.SerializeObject(MyNo);

        // 送信処理
        byte[] buffer = Encoding.UTF8.GetBytes(json);         // JSONをbyteに変換
        buffer = buffer.Prepend((byte)status).ToArray();      // 送信データの先頭にイベントIDを付与
        await stream.WriteAsync(buffer, 0, buffer.Length);    // JSON送信処理
    }

    /// <summary>
    /// 投擲位置を送信
    /// </summary>
    public async void SendThrowPos(float x,float z)
    {
        SetPotionData setPotionData = new SetPotionData();

        setPotionData.plNo = MyNo;
        setPotionData.posX = x;
        setPotionData.posZ = z;

        // 送信データをJSONシリアライズ
        string json = JsonConvert.SerializeObject(setPotionData);

        // 送信処理
        byte[] buffer = Encoding.UTF8.GetBytes(json);                   // JSONをbyteに変換
        buffer = buffer.Prepend((byte)EventID.PotionSetPos).ToArray();  // 送信データの先頭にイベントIDを付与
        await stream.WriteAsync(buffer, 0, buffer.Length);              // JSON送信処理
    }

    //----------------------------------------------------------------------------
    // ゲーム終了フラグ送信処理 ----------------------
    /// <summary>
    /// 投擲位置を送信
    /// </summary>
    public async void SendEndFlag()
    {
        string data = "";

        // 送信処理
        byte[] buffer = Encoding.UTF8.GetBytes(data);              // JSONをbyteに変換
        buffer = buffer.Prepend((byte)EventID.GameEnd).ToArray();  // 送信データの先頭にイベントIDを付与
        await stream.WriteAsync(buffer, 0, buffer.Length);         // JSON送信処理
    }
}
