//
// ゲームディレクターコピースクリプト
// Name:西浦晃太 Date:02/07
// Update:03/07
//
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameDirectorCopy : MonoBehaviour
{
    /// <summary>
    /// タイル配置設定
    /// 0:Wall 1:NormalTile 2:SpawnPoint 3:Object1 4: -
    /// </summary>
    int[,] initTileData = new int[,]
    {
        {0,0,0,0,0,0,0,0,0,0,0},//手前
        {0,2,1,1,1,1,1,1,1,2,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,3,3,1,3,3,1,1,0},
        {0,1,1,3,1,1,1,3,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,3,1,1,1,3,1,1,0},
        {0,1,1,3,3,1,3,3,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,2,1,1,1,1,1,1,1,2,0},
        {0,0,0,0,0,0,0,0,0,0,0},
    };

    /// <summary>
    /// プレイヤー初期配置
    /// </summary>
    int[,] initUnitData = new int[,]
    {
        {0,0,0,0,0,0,0,0,0,0,0},//手前
        {0,3,0,0,0,0,0,0,0,1,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,4,0,0,0,0,0,0,0,2,0},
        {0,0,0,0,0,0,0,0,0,0,0},
    };

    /// <summary>
    /// ゲームモード
    /// </summary>
    public enum MODE
    {
        NONE = -1,
        WAIT_TURN_START,
        MOVE_SELECT,
        POTION_THROW,
        FIELD_UPDATE,
        WAIT_TURN_END,
        TURN_CHANGE,
    }

    ///========================================
    ///
    /// フィールド
    /// 
    ///========================================

    /// <summary>
    /// プレイヤー 配列
    /// </summary>
    Player[] player = new Player[playerNum];

    /// <summary>
    /// ポーションの種類
    /// </summary>
    PotionType potionType = new PotionType();

    /// <summary>
    /// プレイヤー人数
    /// </summary>
    const int playerNum = 4;

    /// <summary>
    /// フィールド上のプレイヤーリスト
    /// </summary>
    List<GameObject>[,] unitData;

    /// <summary>
    /// 選択ユニット
    /// </summary>
    GameObject selectUnit;
    int oldX, oldY;

    /// <summary>
    /// プレイヤーの移動判定変数
    /// </summary>
    bool isMoved = false;

    /// <summary>
    /// プレイヤータイプ
    /// </summary>
    int playerType = UnitController.TYPE_RED;

    /// <summary>
    /// 現在のターンのプレイヤータイプ
    /// </summary>
    int nowPlayerType = 0;

    /// <summary>
    /// タイルデータ構造体の宣言
    /// </summary>
    TileData[,] tileData;

    /// <summary>
    /// 現在のターン
    /// </summary>
    int nowTurn;

    /// <summary>
    /// カメラ移動スクリプト
    /// </summary>
    MoveCameraManager cameraManager;

    /// <summary>
    /// 現在のモード
    /// </summary>
    MODE nowMode;

    /// <summary>
    /// 次のモード
    /// </summary>
    public MODE nextMode;

    /// <summary>
    /// 各ポーションのアイコン
    /// </summary>
    [SerializeField] GameObject[] BoomPotion1;
    [SerializeField] GameObject[] BoomPotion2;

    /// <summary>
    /// (仮)ポーションランダム生成変数
    /// </summary>
    System.Random r = new System.Random();

    public bool IsMoved
    { //移動判定のプロパティ
        get { return isMoved; }
    }

    //+++++++++++++++++++++++++++++++++++++++++
    // 生成ボタンオブジェクト
    //+++++++++++++++++++++++++++++++++++++++++
    [SerializeField] GameObject[] brewingButton; 

    //+++++++++++++++++++++++++++++++++++++++++
    // ターン表示テキスト
    //+++++++++++++++++++++++++++++++++++++++++
    [SerializeField] GameObject turnText;

    //+++++++++++++++++++++++++++++++++++++++++
    // 累計ターン表示テキスト
    //+++++++++++++++++++++++++++++++++++++++++
    [SerializeField] Text allTurnText;

    //+++++++++++++++++++++++++++++++++++++++++
    // PLNo決め打ち用
    //+++++++++++++++++++++++++++++++++++++++++
    [SerializeField] int plNo;

    //+++++++++++++++++++++++++++++++++++++++++
    // ミニゲームプレハブ
    //+++++++++++++++++++++++++++++++++++++++++
    [SerializeField] GameObject minigamePrefab;

    //+++++++++++++++++++++++++++++++++++++++++
    // 各PLのポーションステータス (4種類)
    // [0.完了  1.失敗  2.生成  3.待機]
    //+++++++++++++++++++++++++++++++++++++++++
    [SerializeField] GameObject[] potionStatus1;
    [SerializeField] GameObject[] potionStatus2;
    [SerializeField] GameObject[] potionStatus3;
    [SerializeField] GameObject[] potionStatus4;

    //+++++++++++++++++++++++++++++++++++++++++
    // 全PLのポーションステータス格納用
    //+++++++++++++++++++++++++++++++++++++++++
    List<GameObject[]> allPotionStatus;

    //+++++++++++++++++++++++++++++++++++++++++
    // 累計ターン格納用
    //+++++++++++++++++++++++++++++++++++++++++
    public static int AllTurnNum
    { get; set; }

    //+++++++++++++++++++++++++++++++++++++++++
    // NetworkManager格納用変数
    //+++++++++++++++++++++++++++++++++++++++++
    NetworkManager networkManager;

    //+++++++++++++++++++++++++++++++++++++++++
    // 指定タイルのpos格納用
    //+++++++++++++++++++++++++++++++++++++++++
    Vector3 tilePos;

    //+++++++++++++++++++++++++++++++++++++++++
    // ポーション生成フラグ
    //+++++++++++++++++++++++++++++++++++++++++
    bool generateFlag;

    //+++++++++++++++++++++++++++++++++++++++++
    // キャラ移動アイコン格納用
    //+++++++++++++++++++++++++++++++++++++++++
    GameObject[] moveImgs = new GameObject[playerNum];

    //+++++++++++++++++++++++++++++++++++++++++
    // キャラ生成アイコン格納用
    //+++++++++++++++++++++++++++++++++++++++++
    GameObject[] generateImgs = new GameObject[playerNum];

    ///========================================
    ///
    /// メソッド
    /// 
    ///========================================

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        //++++++++++++++++++++++++++++++++++++++
        // 累計ターンの初期化
        //++++++++++++++++++++++++++++++++++++++
        AllTurnNum = 1;

        //++++++++++++++++++++++++++++++++++++++
        // 仮PLNoの代入
        //++++++++++++++++++++++++++++++++++++++
#if DEBUG
        //NetworkManager.MyNo = plNo;
#endif
        //++++++++++++++++++++++++++++++++++++++
        // 自分の生成ボタンを表示
        //++++++++++++++++++++++++++++++++++++++
        brewingButton[NetworkManager.MyNo - 1].SetActive(true);

        //++++++++++++++++++++++++++++++++++++++
        // ポーションステータスリストの作成
        //++++++++++++++++++++++++++++++++++++++
        allPotionStatus = new List<GameObject[]>()
        {
            potionStatus1,    // プレイヤー1
            potionStatus2,    // プレイヤー2
            potionStatus3,    // プレイヤー3
            potionStatus4,    // プレイヤー4
        };

        //++++++++++++++++++++++++++++++++++++++
        // 自分以外のポーションステータスを表示
        //++++++++++++++++++++++++++++++++++++++
        for (int i = 0; i < playerNum; i++)
        {
            if(i != NetworkManager.MyNo - 1)
            {   // 自分以外のPLのポーションステータスを表示
                allPotionStatus[i][3].SetActive(true);
            }
        }

        //++++++++++++++++++++++++++++++++++++++
        // NetworkManagerを取得
        //++++++++++++++++++++++++++++++++++++++
        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();

        //++++++++++++++++++++++++++++++++++++++
        // ポーション生成フラグ
        //++++++++++++++++++++++++++++++++++++++
        generateFlag = false;

        for (int i = 0; i < player.Length; i++)
        { //配列分のプレイヤーの構造体を生成
            player[i] = new Player();
        }

        for (int i = 0; i < player.Length; i++)
        { //プレイヤー初期値設定
            player[i].PlayerState = PLAYERSTATE.NORMAL_STATE;
            player[i].PlayerNo = i + 1;
        }

        // タイル配置情報分の配列を生成
        tileData = new TileData[initTileData.GetLength(0), initTileData.GetLength(1)];

        // タイルデータに配置位置を代入
        for (int i = 0; i < initTileData.GetLength(0); i++)
        {
            for (int j = 0; j < initTileData.GetLength(1); j++)
            {
                tileData[i, j] = new TileData(initTileData[i, j]);
            }
        }

        // タイルデータにプレイヤー情報を代入
        for (int i = 0; i < initTileData.GetLength(0); i++)
        {
            for (int j = 0; j < initTileData.GetLength(1); j++)
            {
                tileData[i, j].pNo = initUnitData[i, j];
            }
        }
        unitData = new List<GameObject>[tileData.GetLength(0), tileData.GetLength(1)];

        //タイル初期化
        for (int i = 0; i < tileData.GetLength(0); i++)
        {
            for (int j = 0; j < tileData.GetLength(1); j++)
            {
                float x = j - (tileData.GetLength(1) / 2 - 0.5f);
                float z = i - (tileData.GetLength(0) / 2 - 0.5f);

                //タイル配置
                string resname = "";

                int no = tileData[i, j].tNo;
                if (4 == no || 8 == no) no = 5;

                resname = "Cube (" + no + ")";

                resourcesInstantiate(resname, new Vector3(x, 0, z), Quaternion.identity);

                //プレイヤー配置
                unitData[i, j] = new List<GameObject>();

                //プレイヤー毎設定
                Vector3 angle = new Vector3(0, 0, 0);

                if (1 == tileData[i, j].pNo)
                { //1Pユニット配置
                    resname = "Unit1";
                    playerType = UnitController.TYPE_RED;
                }
                else if (2 == tileData[i, j].pNo)
                { //2Pユニット配置
                    resname = "Unit2";
                    playerType = UnitController.TYPE_BLUE;
                    angle.y = 180;        // オブジェクトの向き
                }
                else if (3 == tileData[i, j].pNo)
                { //3Pユニット配置
                    resname = "Unit3";
                    playerType = UnitController.TYPE_YELLOW;
                }
                else if (4 == tileData[i, j].pNo)
                { //4Pユニット配置
                    resname = "Unit4";
                    playerType = UnitController.TYPE_GREEN;
                    angle.y = 180;
                }
                else
                {
                    resname = "";
                }

                GameObject unit = resourcesInstantiate(resname, new Vector3(x, 0.1f, z), Quaternion.Euler(angle));

                if (null != unit)
                {
                    unit.GetComponent<UnitController>().PlayerNo = initUnitData[i, j];
                    unit.GetComponent<UnitController>().Type = playerType;
                    unitData[i, j].Add(unit);
                }
            }
        }

        for (int i = 0; i < playerNum; i++)
        {   // キャラアイコン情報取得
            moveImgs[i] = GameObject.Find((i + 1).ToString() + "MoveImg");
            generateImgs[i] = GameObject.Find((i + 1).ToString() + "GenerateImg");
        }

        for (int i = 0; i < playerNum; i++)
        {   // キャラアイコンを非表示に
            moveImgs[i].SetActive(false);
            generateImgs[i].SetActive(false);
        }

        // 1Pの移動アイコンを表示
        SetMoveIcon(1, true);

        nowTurn = 0;
        nextMode = MODE.MOVE_SELECT;
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if (nowPlayerType >= 4)
        {
            nowPlayerType = 0;
        }

        Mode();

        if (MODE.NONE != nextMode) InitMode(nextMode);
    }

    /// <summary>
    /// メインモード
    /// </summary>
    /// <param name="next"></param>
    void Mode()
    {
        if (MODE.MOVE_SELECT == nowMode)
        {
            SelectMode();
        }
        else if(MODE.POTION_THROW == nowMode)
        {
            ThrowPotionMode();
        }
        else if (MODE.FIELD_UPDATE == nowMode)
        {
            FieldUpdateMode();
        }
        else if (MODE.TURN_CHANGE == nowMode)
        {
            TurnChangeMode();
        }
    }

    /// <summary>
    /// 次のモード準備
    /// </summary>
    /// <param name="next"></param>
    void InitMode(MODE next)
    {
        if (MODE.WAIT_TURN_START == next)
        {
        }
        else if (MODE.MOVE_SELECT == next)
        {
            selectUnit = null;
        }
        else if (MODE.WAIT_TURN_END == next)
        {
        }
        else if (MODE.FIELD_UPDATE == next)
        {
        }
        nowMode = next;
        nextMode = MODE.NONE;
    }

    /// <summary>
    /// ユニット移動モード
    /// </summary>
    void SelectMode()
    {
        if (player[nowPlayerType].IsDead == true)
        {   // 現在ターンのプレイヤーが死んでいたらスキップ

            // 次のターンへ
            nextMode = MODE.FIELD_UPDATE;
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 現在のPLターン表示
        turnText.GetComponent<Text>().text = (nowPlayerType + 1).ToString() + "Pのターン";
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 自分のターンのみ行動可能に
        if (NetworkManager.MyNo == nowPlayerType + 1)
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        {
            if (player[nowPlayerType].PlayerState == PLAYERSTATE.FROZEN_STATE || player[nowPlayerType].PlayerState == PLAYERSTATE.CURSED_STATE || player[nowPlayerType].IsDead == true)
            { //凍っていた場合
                Debug.Log((nowPlayerType + 1) + "Pはうごけない！");
                nextMode = MODE.FIELD_UPDATE;
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                { //クリック時、ユニット選択
                    isMoved = false;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        if (null != hit.collider.gameObject)
                        {
                            // マウスでクリックしたタイルのposを取得
                            tilePos = hit.collider.gameObject.transform.position;

                            // クリックしたタイルの配列番号を計算
                            int x = (int)(tilePos.x + (tileData.GetLength(1) / 2 - 0.5f));
                            int z = (int)(tilePos.z + (tileData.GetLength(0) / 2 - 0.5f));

                            if (0 < unitData[z, x].Count && player[nowTurn].PlayerNo == unitData[z, x][0].GetComponent<UnitController>().PlayerNo)
                            {   // ユニット選択(選択したマスのユニット数が0以上・現在のPLターンとクリックしたタイルのユニットのPLNoが一致してたら)

                                if (null != selectUnit)
                                {   // 既にユニットを選択していた場合
                                    selectUnit.GetComponent<UnitController>().Select(false);
                                }

                                //++++++++++++++++++++++++++++++++++++++++++++++++++++//
                                // 現PLターンの「選択した」という情報をサーバーに送る //
                                //++++++++++++++++++++++++++++++++++++++++++++++++++++//
                                networkManager.SendSelectUnit(z, x);
#if DEBUG
                                Debug.Log("選択情報送信完了");
#endif
                            }
                            else if (null != selectUnit)
                            {   //移動先タイル選択(ユニットが選択されていた場合)

                                if (movableTile(oldX, oldY, x, z))
                                {   // 移動判定が通った場合

                                    // 現PLターンの「移動した」という情報(移動先のタイル)をサーバーに送る //
                                    // 全クライアントに移動情報を渡した後に画面に反映させる //
                                    networkManager.SendMoveUnit(x, z, tilePos.x, tilePos.z);

                                    // 移動情報送信後に生成フラグをfalseに戻す
                                    generateFlag = false;
#if DEBUG
                                    Debug.Log("移動情報送信完了");
#endif
                                }

                                Debug.Log("現在のプレイヤー:" + (nowPlayerType + 1));
                            }
                        }
                    }
                }
            }
        }
    }

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 引数座標のタイルに居るユニットを選択状態にする処理
    /// </summary>
    /// <param name="z"> タイルのz座標 </param>
    /// <param name="x"> タイルのx座標 </param>
    public void SelectUnit(int z, int x)
    {
        // 選択したユニットのGameObject情報を取得
        selectUnit = unitData[z, x][0];

        // 選択時の座標を保存
        oldX = x;
        oldY = z;

        // 選択ユニットを選択状態(持ち上がった状態)に変更
        selectUnit.GetComponent<UnitController>().Select();
    }
    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 引数座標のタイルに選択されたユニットを移動する処理
    /// </summary>
    /// <param name="z"></param>
    /// <param name="x"></param>
    public void MoveUnit(int z,int x, Vector3 pos)
    {
        isMoved = true;

        // 前回いたタイルの位置のユニットデータを削除
        unitData[oldY, oldX].Clear();

        // ユニットを選択したタイルのposに移動
        pos.y += 0.1f;
        selectUnit.transform.position = pos;

        // 現在位置のunitDataに移動させたユニットを追加
        unitData[z, x].Add(selectUnit);

        // 移動判定コライダーをオフに
        unitData[z, x][0].GetComponent<UnitController>().OffColliderEnable();

        // 次のターンへ
        nextMode = MODE.FIELD_UPDATE;
    }
    // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    void FieldUpdateMode()
    {
        nextMode = MODE.TURN_CHANGE;
    }

    /// <summary>
    /// ターンチェンジ
    /// </summary>
    void TurnChangeMode()
    {
        nextMode = MODE.NONE;
        nextMode = MODE.MOVE_SELECT;

        int oldTurn = nowTurn;
        nowTurn = getNextTurn();

        //+++++++++++++++++
        // 爆発判定
        //+++++++++++++++++

        //objectというタグ名のゲームオブジェクトを複数取得したい時
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Bomb");

        // 配列の要素一つ一つに対して処理を行う
        foreach (GameObject obj in objects)
        {
            // Bombのカウントダウンを下げる
            obj.GetComponent<PotionBoom>().bombCntDown();
        }
        //+++++++++++++++++

        //+++++++++++++++++
        // 終了判定
        //+++++++++++++++++

        nextMode = MODE.MOVE_SELECT;

        // 累計ターン表示
        AllTurnNum++;
        allTurnText.text = AllTurnNum.ToString() + "ターン目"; 

        nowPlayerType++;        
    }

    int getNextTurn()
    { //次ターンを取得
        int ret = nowTurn;

        ret++;
        if (3 < ret) ret = 0;

        return ret;
    }

    /// <summary>
    /// リソース内オブジェクト配置関数
    /// </summary>
    /// <param name="name">Object's Name</param>
    /// <param name="pos">Object's Position</param>
    /// <param name="angle">Object's Angle</param>
    /// <returns></returns>
    GameObject resourcesInstantiate(string name, Vector3 pos, Quaternion angle)
    {
        GameObject prefab = (GameObject)Resources.Load(name);

        if (null == prefab)
        {
            return null;
        }

        GameObject ret = Instantiate(prefab, pos, angle);
        return ret;
    }

    public bool movableTile(int oldx, int oldz, int x, int z)
    {
        bool ret = false;

        // 差分を取得
        int dx = Mathf.Abs(oldx - x);
        int dz = Mathf.Abs(oldz - z);

        Debug.Log("x:" + x);
        Debug.Log("z:" + z);

        // 斜め進行不可
        if (dx + dz > 2 || dx > 1 || dz > 1)
        {
            Debug.Log("進行不可");
            Debug.Log("Z:" + z + " " + "X:" + x);

            return ret = false;
        }

        // 壁以外
        if (1 == tileData[z, x].tNo
           || 2 == tileData[z, x].tNo
           || player[nowTurn].PlayerNo * 4 == tileData[z, x].tNo)
        {
            if (0 == unitData[z, x].Count)
            { //誰もいないマス
                ret = true;
            }
            else
            { //誰かいるマス
                if (unitData[z, x][0].GetComponent<UnitController>().PlayerNo != player[nowTurn].PlayerNo)
                { //敵だった場合
                    ret = true;
                }
            }
        }
        return ret;
    }

    /// <summary>
    /// ターン終了ボタン
    /// </summary>
    public void TurnEnda()
    {
        nextMode = MODE.MOVE_SELECT;
    }

    /// <summary>
    /// ポーション生成情報送信ボタン
    /// </summary>
    public void Brewing()
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 自分のターン以外生成可能に
        if (NetworkManager.MyNo != nowPlayerType + 1 && !generateFlag)
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        {
            // int rndPotion = r.Next(4);
            if (player[nowPlayerType].PlayerState == PLAYERSTATE.PARALYSIS_STATE || player[nowPlayerType].PlayerState == PLAYERSTATE.FROZEN_STATE || player[nowPlayerType].IsDead == true)
            { //しびれていた場合または凍っていた場合
                Debug.Log((NetworkManager.MyNo) + "Pはしびれている。ポーションが作れない！");
            }
            else
            {
                if (player[nowPlayerType].OwnedPotionList?.Count >= 2)
                { //枠が埋まっていた場合
                    Debug.Log((NetworkManager.MyNo) + "Pのポーション枠は満杯だ！！");
                }
                else
                {
                    // ポーション生成フラグをtrueに
                    generateFlag = true;

                    // ミニゲームの再生(仮)
                    Instantiate(minigamePrefab, minigamePrefab.GetComponent<Transform>().position, Quaternion.identity);

                    // ポーション生成情報をサーバーに送信
                    networkManager.SendPotionStatus((int)EventID.PotionGenerate);
                }
            }
        }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// ポーション生成処理
    /// </summary>
    /// <param name="plNo">生成したPLNo</param>
    public void GeneratePotion(int plNo)
    {
        if (player[plNo - 1].OwnedPotionList.Count == 0)
        {   // ポーションの持ち数が０個の時

            // 爆破ポーション1の生成
            BoomPotion1[plNo - 1].SetActive(true);
            player[plNo - 1].OwnedPotionList.Add(TYPE.BOMB);
        }
        else if (player[plNo - 1].OwnedPotionList.Count == 1)
        {
            // 爆破ポーション2の生成
            BoomPotion2[plNo - 1].SetActive(true);
            player[plNo - 1].OwnedPotionList.Add(TYPE.BOMB);
        }
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 指定PLNoのポーションステータスを待機に変更処理
    /// </summary>
    /// <param name="plNo">プレイヤーNo</param>
    /// <param name="staNum">ステータスNo</param>
    IEnumerator ChangeStatusWait(int plNo, float delay) // [0.完了  1.失敗  2.生成  3.待機]
    {
        // 指定秒数待機後
        yield return new WaitForSeconds(delay);

        // ステータス表示を全非表示に
        for (int i = 0; i < allPotionStatus[plNo - 1].GetLength(0); i++)
        {
            allPotionStatus[plNo - 1][i].SetActive(false);
        }

        if (NetworkManager.MyNo != plNo)
        {
            // 指定PLNoを指定ステに変更
            allPotionStatus[plNo - 1][3].SetActive(true);
        }
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 指定PLNoのポーションステータス変更処理
    /// </summary>
    /// <param name="plNo">プレイヤーNo</param>
    /// <param name="staNum">ステータスNo</param>
    public void ChangePotionStatus(int plNo,int staNum) // [0.完了  1.失敗  2.生成  3.待機]
    {
        // ステータス表示を全非表示に
        for (int i = 0; i < allPotionStatus[plNo - 1].GetLength(0); i++)
        {
            allPotionStatus[plNo - 1][i].SetActive(false);
        }

        if(NetworkManager.MyNo != plNo)
        {   // 自分のPLNo以外の時

            // 指定PLNoを指定ステに変更
            allPotionStatus[plNo - 1][staNum].SetActive(true);
        }

        if(staNum == 0 ||  staNum == 1) 
        {   // 成功・失敗表示の場合

            // 指定秒数後、指定ステから待機に戻す
            StartCoroutine(ChangeStatusWait(plNo, 2.0f));
        }
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    /// <summary>
    /// ポーション投擲処理
    /// </summary>
    void ThrowPotionMode()
    {
        if (Input.GetMouseButtonDown(0))
        { //クリック時、投擲位置を設定
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (null != hit.collider.gameObject)
                {
                    Vector3 selectPos = hit.collider.gameObject.transform.position;

                    // 設置情報をサーバーに送信
                    networkManager.SendThrowPos(selectPos.x, selectPos.z);
                }
            }
        }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// ポーション投擲処理
    /// </summary>
    public void PlayerThrow()
    {
        // 投擲モードに変更
        nowMode = MODE.POTION_THROW;

        //そのポーションにあったアニメーションをする
        GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isThrow", true);

        Vector3 pos = SerchUnit((nowPlayerType + 1));

        int x = (int)(pos.x + (tileData.GetLength(1) / 2 - 0.5f));
        int z = (int)(pos.z + (tileData.GetLength(0) / 2 - 0.5f));

        unitData[z, x][0].GetComponent<UnitController>().ThrowSelect();
        unitData[z, x][0].GetComponent<UnitController>().OnThrowColliderEnable();
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// ポーション設置処理
    /// </summary>
    public void SetPotion(Vector3 pos)
    {
        BoomPotion1[nowPlayerType].SetActive(false);                //使用したポーションのアイコンを消す
        player[nowPlayerType].OwnedPotionList.Remove(TYPE.BOMB);    //使用したポーションをリストから削除する

        Vector3 PlPos = SerchUnit((nowPlayerType + 1));

        int x = (int)(PlPos.x + (tileData.GetLength(1) / 2 - 0.5f));
        int z = (int)(PlPos.z + (tileData.GetLength(0) / 2 - 0.5f));

        unitData[z, x][0].GetComponent<UnitController>().OffThrowColliderEnable();

        string resname = "BombPotion";
        resourcesInstantiate(resname, pos, Quaternion.Euler(0, 0, 0));
        nextMode = MODE.FIELD_UPDATE;
    }
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    /// <summary>
    /// ポーション使用ボタン
    /// </summary>
    public void UsePotion(int buttonNum)
    {
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // 自分のターンのみ行動可能に
        if (NetworkManager.MyNo == nowPlayerType + 1)
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        {
            if (player[nowPlayerType].PlayerState == PLAYERSTATE.FROZEN_STATE || player[nowPlayerType].IsDead == true)
            { //凍っていた場合
                Debug.Log((nowPlayerType + 1) + "Pは凍っている。ポーションは使えない！");
            }
            else
            {
                //枠別判定
                if (buttonNum == 1)
                { //1番目の場合
                    if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.BOMB))
                    {
                        // サーバーにボム使用フラグを送信
                        networkManager.SendPotionStatus((int)EventID.PotionThrow);
                    }
                    else
                    {
                        Debug.Log((nowPlayerType + 1) + "Pはまだ1枠目のポーションを作ってない!!");
                    }
                }
                else if (buttonNum == 2)
                { //２番目の場合
                    if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.BOMB))
                    {
                        // サーバーにボム使用フラグを送信
                        networkManager.SendPotionStatus((int)EventID.PotionThrow);
                    }
                    else
                    {
                        Debug.Log((nowPlayerType + 1) + "Pはまだ2枠目のポーションを作ってない!!");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 指定ユニット破壊処理
    /// </summary>
    /// <param name="unitType"></param>
    public void DestroyUnit(List<int> deadList)
    {
        GameObject Unit;
        for (int i = 0; i < unitData.GetLength(0); i++)
        {
            for (int j = 0; j < unitData.GetLength(1); j++)
            {
                if (unitData[i, j].Count > 0)
                {
                    Unit = unitData[i, j][0];

                    for (int k = 0;k <deadList.Count;k++)
                    {
                        if (Unit.GetComponent<UnitController>().Type == deadList[k])
                        { //当該ユニットを殺す
                            Destroy(Unit);
                            player[deadList[k]-1].IsDead = true;
                        }
                    }
                }
            }
        }

        Debug.Log("死亡");
    }

    /// <summary>
    /// バフ処理
    /// </summary>
    /// <param name="unitType"></param>
    public void BuffUnit(int unitType, TYPE buffType)
    {
        GameObject Unit;
        for (int i = 0; i < unitData.GetLength(0); i++)
        {
            for (int j = 0; j < unitData.GetLength(1); j++)
            {
                if (unitData[i, j].Count > 0)
                {
                    Unit = unitData[i, j][0];

                    if (Unit.GetComponent<UnitController>().Type == unitType)
                    {
                        switch (buffType)
                        { //バフポーション別処理
                            case TYPE.REFRESH: //リフレッシュポーションの処理
                                player[unitType].PlayerState = PLAYERSTATE.NORMAL_STATE;
                                break;
                            case TYPE.INVISIBLE: //無敵ポーションの処理
                                player[unitType].PlayerState = PLAYERSTATE.INVICIBLE_STATE;
                                break;
                            case TYPE.MUSCLE: //筋力ポーションの処理
                                player[unitType].PlayerState = PLAYERSTATE.MUSCLE_STATE;
                                break;
                        }
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// デバフ処理
    /// </summary>
    /// <param name="unitType"></param>
    public void DebuffUnit(int unitType, TYPE debuffType)
    {
        GameObject Unit;
        for (int i = 0; i < unitData.GetLength(0); i++)
        {
            for (int j = 0; j < unitData.GetLength(1); j++)
            {
                if (unitData[i, j].Count > 0)
                {
                    Unit = unitData[i, j][0];

                    if (Unit.GetComponent<UnitController>().Type == unitType)
                    {
                        switch (debuffType)
                        { //バフポーション別処理
                            case TYPE.SOUR: //超スッパイポーションの処理
                                GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isParalysis", true);
                                player[unitType].PlayerState = PLAYERSTATE.PARALYSIS_STATE;
                                break;

                            case TYPE.CURSE: //瓶詰めの呪いの処理
                                GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isCurse", true);
                                player[unitType].PlayerState = PLAYERSTATE.CURSED_STATE;
                                break;

                            case TYPE.ICE: //アイスポーションの処理
                                GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isFrost", true);
                                player[unitType].PlayerState = PLAYERSTATE.FROZEN_STATE;
                                break;
                        }
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// ユニット指定処理
    /// </summary>
    /// <param name="unitType"></param>
    public Vector3 SerchUnit(int unitType)
    {
        GameObject Unit;
        for (int i = 0; i < unitData.GetLength(0); i++)
        {
            for (int j = 0; j < unitData.GetLength(1); j++)
            {
                if (unitData[i, j].Count > 0)
                {
                    Unit = unitData[i, j][0];

                    if (Unit.GetComponent<UnitController>().Type == unitType)
                    {
                        Vector3 pos = Unit.transform.position;
                        return pos;
                    }
                }
            }
        }
        return new Vector3(0, 0, 0);
    }

    /// <summary>
    /// 移動アイコン表示
    /// </summary>
    /// <param name="plNo">プレイヤーNo</param>
    /// <param name="flag">表示・非表示フラグ</param>
    public void SetMoveIcon(int plNo, bool flag)
    {
        if (moveImgs[plNo-1] != null) 
        {
            moveImgs[plNo - 1].SetActive(flag);
        }
    }

    /// <summary>
    /// 生成アイコン表示
    /// </summary>
    /// <param name="plNo">プレイヤーNo</param>
    /// <param name="flag">表示・非表示フラグ</param>
    public void SetGenerateIcon(int plNo, bool flag)
    {
        if(generateImgs[plNo - 1] != null)
        {
            generateImgs[plNo - 1].SetActive(flag);
        }
    }
}