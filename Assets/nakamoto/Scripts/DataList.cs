﻿//-----------------------------------------------------------
//
//  送受信用データリスト [ DataList.cs ]
// Author:Kenta Nakamoto
// Data 2024/02/08
//
//-----------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// イベントID定義 (送受信データの1バイト目に設定)
/// </summary>
public enum EventID
{
    PlayerNo = 1,     // プレイヤーNo
    UserData,         // 名前・No
    UserDataList,     // 全PLのUserDataのリスト
    CompleteFlag,     // 準備完了フラグ
    InGameFlag,       // インゲームフラグ
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
    {  get; set; }
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
/// タイルデータ構造体
/// </summary>
class TileData
{
    public TileData(int no) { tNo = no; }
    public int tNo;                          // タイルの種類No
    public int pNo;                          // プレイヤーの種類No
}