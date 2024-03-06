//
// ゲームディレクタースクリプト
// Name:西浦晃太 Date:02/07
// Update:03/05
//
using System;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class GameDirector : MonoBehaviour
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
    /// 死亡人数加算変数
    /// </summary>
    int deadCnt;

    /// <summary>
    /// プレイヤータイプ
    /// </summary>
    int playerType = UnitController.TYPE_RED;

    /// <summary>
    /// ひとつ前のプレイヤータイプ
    /// </summary>
    int oldType;

    /// <summary>
    /// 死亡するプレイヤータイプ
    /// </summary>
    int[] type = { 2 };

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
    [SerializeField] GameObject[] BoomPotion;
    [SerializeField] GameObject[] BuffPotion;
    [SerializeField] GameObject[] DebuffPotion;
    [SerializeField] GameObject[] Potion;

    /// <summary>
    /// ゲーム終了テキスト
    /// </summary>
    [SerializeField] Text GameEndTXT;

    /// <summary>
    /// (仮)ポーションランダム生成変数
    /// </summary>
    System.Random r = new System.Random();

    /// <summary>
    /// ポーション爆発クラス
    /// </summary>
    PotionBoom potionBoom;

    /// <summary>
    /// 移動判定のプロパティ
    /// </summary>
    public bool IsMoved
    { 
        get { return isMoved; }
    }

    /// <summary>
    /// 現在プレイヤーのターン変数のプロパティ
    /// </summary>
    public int NowPlayerType
    {
        get { return nowPlayerType; }
    }

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
        deadCnt = 0;

        for (int i = 0; i < player.Length; i++)
        { //配列分のプレイヤーの構造体を生成
            player[i] = new Player();      
        }

        for (int i = 0; i < player.Length; i++)
        { //各変数を初期化
            player[i].IsThrowed= false;
            player[i].TurnClock = 0;
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

        nowTurn = 0;
        nextMode = MODE.MOVE_SELECT;
    }

    // Update is called once per frame
    void Update()
    {
        if (deadCnt >= 3)
        {
            GameEnd();
        }

        if (nowPlayerType >= 4)
        {
            nowPlayerType = 0;
        }

        oldType = nowPlayerType - 1;

        if (oldType == -1)
        {
            oldType = 3;
        }

        if (oldType >= 4)
        {
            oldType = 0;
        }

        if (player[oldType].TurnClock >= 5)
        {
            potionBoom.BoomPotion(type);
        }

        for (int i = 0; i < player.Length; i++)
        {
            if (player[i].TurnClock >= 6)
            {
                player[i].TurnClock = 0;
            }
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
        else if (MODE.FIELD_UPDATE == nowMode)
        {
            FieldUpdateMode();
        }
        else if (MODE.TURN_CHANGE == nowMode)
        {
            TurnChangeMode();
        }
        else if (MODE.POTION_THROW== nowMode)
        {
            ThrowPotion();
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
                        Vector3 pos = hit.collider.gameObject.transform.position;

                        int x = (int)(pos.x + (tileData.GetLength(1) / 2 - 0.5f));
                        int z = (int)(pos.z + (tileData.GetLength(0) / 2 - 0.5f));

                        if (0 < unitData[z, x].Count && player[nowTurn].PlayerNo == unitData[z, x][0].GetComponent<UnitController>().PlayerNo)
                        { //ユニット選択
                            if (null != selectUnit)
                            {
                                selectUnit.GetComponent<UnitController>().Select(false);
                            }
                            selectUnit = unitData[z, x][0];
                            oldX = x;
                            oldY = z;

                            selectUnit.GetComponent<UnitController>().Select();
                        }
                        else if (null != selectUnit)
                        { //移動先タイル選択
                            if (movableTile(oldX, oldY, x, z))
                            {
                                isMoved = true;
                                unitData[oldY, oldX].Clear();
                                pos.y += 0.1f;
                                selectUnit.transform.position = pos;
                                unitData[z, x].Add(selectUnit);
                                unitData[z, x][0].GetComponent<UnitController>().OffColliderEnable();
                                nextMode = MODE.FIELD_UPDATE;
                            }
                            Debug.Log("現在のプレイヤー:" + nowPlayerType);
                        }
                    }
                }
            }
        }
    }

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

        nextMode = MODE.MOVE_SELECT;

        if (player[nowPlayerType].IsThrowed)
        { //投げてからカウント開始
            for (int i = 0; i < player.Length; i++)
            {
                player[nowPlayerType].TurnClock++;
            }
        }

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
    public void TurnEnd()
    {
        nextMode = MODE.MOVE_SELECT;
    }

    /// <summary>
    /// ポーション生成ボタン
    /// </summary>
    public void Brewing()
    {
        int rndPotion = r.Next(4);
        if (player[nowPlayerType].PlayerState == PLAYERSTATE.PARALYSIS_STATE || player[nowPlayerType].PlayerState == PLAYERSTATE.FROZEN_STATE || player[nowPlayerType].IsDead == true)
        { //しびれていた場合または凍っていた場合
            Debug.Log((nowPlayerType + 1) + "Pはしびれている。ポーションが作れない！");
        }
        else
        {
            if (player[nowPlayerType].OwnedPotionList?.Count >= 4)
            { //枠が埋まっていた場合
                Debug.Log((nowPlayerType + 1) + "Pのポーション枠は満杯だ！！");
            }
            else
            {
                if (rndPotion == 0)
                { //枠1
                    if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.BOMB))
                    { //すでに同じポーションを所持していた場合
                        Debug.Log((nowPlayerType + 1) + "Pのボムすでにあるよ");
                    }
                    else
                    {
                        BoomPotion[nowPlayerType].SetActive(true);
                        player[nowPlayerType].OwnedPotionList.Add(TYPE.BOMB);
                    }
                }
                else if (rndPotion == 1)
                { //枠２
                    if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.REFRESH))
                    { //すでに同じポーションを所持していた場合
                        Debug.Log((nowPlayerType + 1) + "Pのバフすでにあるよ");
                    }
                    else
                    {
                        BuffPotion[nowPlayerType].SetActive(true);
                        player[nowPlayerType].OwnedPotionList.Add(TYPE.REFRESH);
                    }
                }
                else if (rndPotion == 2)
                { //枠３
                    if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.CURSE))
                    { //すでに同じポーションを所持していた場合
                        Debug.Log((nowPlayerType + 1) + "Pの呪すでにあるよ");
                    }
                    else
                    {
                        DebuffPotion[nowPlayerType].SetActive(true);
                        player[nowPlayerType].OwnedPotionList.Add(TYPE.CURSE);
                    }
                }
                else if (rndPotion == 3)
                { //枠４
                    if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.NORMAL))
                    { //すでに同じポーションを所持していた場合
                        Debug.Log((nowPlayerType + 1) + "Pのノーマルすでにあるよ");
                    }
                    else
                    {
                        Potion[nowPlayerType].SetActive(true);
                        player[nowPlayerType].OwnedPotionList.Add(TYPE.NORMAL);
                    }
                }
            }
        }
        //nextMode = MODE.FIELD_UPDATE;
    }

    /// <summary>
    /// ポーション使用ボタン
    /// </summary>
    public void UsePotion(int buttonNum)
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
                    ThrowPotion();
                    GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isThrow", true);     //そのポーションにあったアニメーションをする
                    BoomPotion[nowPlayerType].SetActive(false);                 //使用したポーションのアイコンを消す
                    player[nowPlayerType].OwnedPotionList.Remove(TYPE.BOMB);    //使用したポーションをリストから削除する
                }
                else
                {
                    Debug.Log((nowPlayerType + 1) + "Pはまだ1枠目のポーションを作ってない!!");
                }
            }
            else if (buttonNum == 2)
            { //２番目の場合
                if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.CURSE))
                {
                    ThrowPotion();
                    GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isThrow", true);     //そのポーションにあったアニメーションをする
                    DebuffPotion[nowPlayerType].SetActive(false);                //使用したポーションのアイコンを消す
                    player[nowPlayerType].OwnedPotionList.Remove(TYPE.CURSE);    //使用したポーションをリストから削除する
                }
                else
                {
                    Debug.Log((nowPlayerType + 1) + "Pはまだ2枠目のポーションを作ってない!!");
                }
            }
            else if (buttonNum == 3)
            { //３番目の場合
                if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.REFRESH))
                {
                    ThrowPotion();
                    GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isThrow", true);     //そのポーションにあったアニメーションをする
                    BuffPotion[nowPlayerType].SetActive(false);                    //使用したポーションのアイコンを消す
                    player[nowPlayerType].OwnedPotionList.Remove(TYPE.REFRESH);    //使用したポーションをリストから削除する
                }
                else
                {
                    Debug.Log((nowPlayerType + 1) + "Pはまだ3枠目のポーションを作ってない!!");
                }
            }
            else if (buttonNum == 4)
            { //４番目の場合
                if (player[nowPlayerType].OwnedPotionList.Contains(TYPE.NORMAL))
                {
                    ThrowPotion();
                    GameObject.Find("Unit" + (nowPlayerType + 1) + "(Clone)").GetComponent<UnitController>().animator.SetBool("isThrow", true);     //そのポーションにあったアニメーションをする
                    Potion[nowPlayerType].SetActive(false);                       //使用したポーションのアイコンを消す 
                    player[nowPlayerType].OwnedPotionList.Remove(TYPE.NORMAL);    //使用したポーションをリストから削除する
                }
                else
                {
                    Debug.Log((nowPlayerType + 1) + "Pはまだ４枠目のポーションを作ってない!!");
                }
            }
        }
    }

    /// <summary>
    /// 指定ユニット破壊処理
    /// </summary>
    /// <param name="unitType"></param>
    public void DestroyUnit(int unitType)
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
                    { //当該ユニットを殺す
                        Destroy(Unit);
                        player[unitType].IsDead = true;
                        deadCnt++;
                        return;
                    }
                }
            }
        }
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
    /// ポーション投擲処理
    /// </summary>
    void ThrowPotion()
    {
        nowMode = MODE.POTION_THROW;
        string resname = "BombPotion";
        Vector3 pos = SerchUnit((nowPlayerType + 1));

        int x = (int)(pos.x + (tileData.GetLength(1) / 2 - 0.5f));
        int z = (int)(pos.z + (tileData.GetLength(0) / 2 - 0.5f));

        unitData[z, x][0].GetComponent<UnitController>().ThrowSelect();
        unitData[z, x][0].GetComponent<UnitController>().OnThrowColliderEnable();

        if (Input.GetMouseButtonDown(0))
        { //クリック時、投擲位置を設定
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (null != hit.collider.gameObject)
                {
                    Vector3 selectPos = hit.collider.gameObject.transform.position;

                    int selectX = (int)(selectPos.x + (tileData.GetLength(1) / 2 - 0.5f));
                    int selectZ = (int)(selectPos.z + (tileData.GetLength(0) / 2 - 0.5f));

                    resourcesInstantiate(resname, selectPos, Quaternion.Euler(0,0,0));
                    unitData[z, x][0].GetComponent<UnitController>().OffThrowColliderEnable();
                    player[nowPlayerType].IsThrowed= true;
                    potionBoom = GameObject.FindWithTag("Potion").GetComponent<PotionBoom>();
                    nextMode = MODE.FIELD_UPDATE;
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
        if (player[nowPlayerType].IsDead)
        {
            return new Vector3(0, 0, 0);
        }
        else
        {
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
        }
        return new Vector3(0, 0, 0);
    }
    /// <summary>
    /// ゲーム終了関数
    /// </summary>
    void GameEnd()
    {
        int wonPlayer = 0;  //勝者のプレイヤー番号代入用変数
        for(int i = 0; i<4; i++)
        {　//勝者のプレイヤーナンバーを取得
            if (player[i].IsDead ==false)
            { //勝ち残ったプレイヤーナンバーを代入
                wonPlayer = (i + 1);
            }
            else
            { //全員死亡していた場合
                wonPlayer = 0;
            }
        }

        if (wonPlayer == 0)
        { //全員死亡していた場合
            GameEndTXT.GetComponent<Text>().text = "引き分け";
        }
        else
        { //勝者のプレイヤーナンバーを反映
            GameEndTXT.GetComponent<Text>().text = wonPlayer.ToString() + "Pの勝ち!!!";
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Initiate.Fade("Result", Color.white, 0.7f);
        }
    }
}
