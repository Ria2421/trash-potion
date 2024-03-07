//-----------------------------------------------------------
//
//  送受信用データリスト [ DataList.cs ]
// Author:Kenta Nakamoto
// Data 2024/02/08
// Update 2024/03/06
//
//-----------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GameDirector;

/// <summary>
/// タイルデータ構造体
/// </summary>
class TileData
{
    public TileData(int no) { tNo = no; }
    public int tNo;                          // タイルの種類No
    public int pNo;                          // プレイヤーの種類No
}

/// <summary>
/// プレイヤー状態
/// </summary>
public enum PLAYERSTATE
{
    NONE = 0,
    NORMAL_STATE,       //通常状態
    PARALYSIS_STATE,    //マヒ状態
    FROZEN_STATE,       //氷結状態
    CURSED_STATE,       //呪い状態
    FLAME_STATE,        //炎上状態
    MUSCLE_STATE,       //筋力上昇状態
    INVICIBLE_STATE,    //無敵状態
}

/// <summary>
/// プレイヤー構造体
/// </summary>
class Player
{
    //プレイヤーの状態
    public PLAYERSTATE PlayerState
    { get; set; }

    //プレイヤーの人数
    public int PlayerNo
    { get; set; }

    //プレイヤーの名前
    public string PlayerName
    { get; set; }

    //死亡判定変数
    public bool IsDead
    { get; set; }

    //ポーション所持変数
    public List<TYPE> OwnedPotionList = new List<TYPE>(2);
}

/// <summary>
/// ポーションのタイプ
/// </summary>
public enum TYPE
{
    NONE = 0,
    NORMAL,     //ただのポーション
    BOMB,       //ボムポーション
    CRUSTER,    //クラスターポーション
    FLAME,      //火炎ポーション
    REFRESH,    //リフレッシュポーション
    INVISIBLE,  //無敵ポーション
    MUSCLE,     //筋力ポーション
    SOUR,       //超スッパイポーション
    CURSE,      //瓶詰めの呪い
    ICE,        //アイスポーション
}

class PotionType
{
    //ポーションのタイプ
   public TYPE PotionTypes
   { get; set; }
}

// ----------------------------------------------------------------------
// 送信用データリスト ----------------------------

/// <summary>
/// イベントID定義 (送受信データの1バイト目に設定)
/// </summary>
public enum EventID
{
    PlayerNo = 1,     // プレイヤーNo
    UserData,         // 名前・No
    UserDataList,     // 全PLのUserDataのリスト
    CompleteFlag,     // 準備完了フラグ
    InSelectFlag,     // モード選択画面遷移フラグ
    InGameFlag,       // インゲームフラグ
    MapData,          // マップデータ
    SelectUnit,       // 自ユニット選択
    MoveUnit,         // 自ユニット移動
    GeneratePotion,   // ポーション生成
}

/// <summary>
/// 入力ユーザーデータ
/// </summary>
class UserData
{
    /// <summary>
    /// ユーザー名
    /// </summary>
    public string UserName
    { get; set; }

    /// <summary>
    /// プレイヤーNo
    /// </summary>
    public int PlayerNo
    { get; set; }

    /// <summary>
    /// 完了フラグ
    /// </summary>
    public bool IsReady
    { get; set; }
}

/// <summary>
/// 全ユーザーのデータリスト
/// </summary>
class UserDataList
{
    /// <summary>
    /// ユーザーデータリスト
    /// </summary>
    public UserData[] userList
    { get; set; }

    //////////////////////
    // 戦績変数追加予定 //
    //////////////////////
}

/// <summary>
/// 送信マップデータ
/// </summary>
class MapData
{
    /// <summary>
    /// 初期タイルデータ格納用
    /// </summary>
    public int[,] tileData;

    /// <summary>
    /// 初期ユニットデータ
    /// </summary>
    public int[,] unitData;
}

/// <summary>
/// 選択ユニットデータ
/// </summary>
class SelectData
{
    /// <summary>
    /// PL番号
    /// </summary>
    public int plNo;

    /// <summary>
    /// 選択ユニットの座標[z]
    /// </summary>
    public int z;

    /// <summary>
    /// 選択ユニットの座標[x]
    /// </summary>
    public int x;
}

/// <summary>
/// 移動先送信データ
/// </summary>
class MoveData
{
    /// <summary>
    /// PL番号
    /// </summary>
    public int plNo;

    /// <summary>
    /// 選択ユニットの座標[z]
    /// </summary>
    public int z;

    /// <summary>
    /// 選択ユニットの座標[x]
    /// </summary>
    public int x;

    /// <summary>
    /// 移動先のX座標
    /// </summary>
    public float posX;

    /// <summary>
    /// 移動先のZ座標
    /// </summary>
    public float posZ;
}