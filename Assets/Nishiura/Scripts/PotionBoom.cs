//
// ポーション爆発スクリプト
// Name:西浦晃太 Date:02/26
// Update:02/29
//
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.Examples.ObjectSpin;

public class PotionBoom : MonoBehaviour
{
    /// <summary>
    /// 爆発エフェクトのプレハブ
    /// </summary>
    public GameObject explosionPrefab;

    /// <summary>
    /// ゲームディレクター
    /// </summary>
    GameDirectorCopy gameDirector;

    /// <summary>
    /// 死亡するプレイヤータイプ
    /// </summary>
    List<int> deadList = new List<int>();

    /// <summary>
    /// ポーションの種類
    /// </summary>
    PotionType potionType;

    /// <summary>
    /// 爆破カウント
    /// </summary>
    int bombCnt;

    /// <summary>
    /// カウントテキスト
    /// </summary>
    [SerializeField] Text countText;

    void Start()
    {
        bombCnt = 2;
        potionType = new PotionType();
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirectorCopy>();
    }

    void Update()
    {
        if (bombCnt == 0)
        { //指定番号のプレイヤーを殺害(番号はサーバから取得)
            BoomPotion(deadList);
        }
    }

    /// <summary>
    /// 爆発処理
    /// </summary>
    /// <param name="unitType"></param>
    void BoomPotion(List<int> deadList)
    { 
        GameObject explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        explosion.transform.position += new Vector3(0f, 0.5f, 0f);

        this.gameObject.transform.position += new Vector3(0f, -50f, 0f);

        gameDirector.DestroyUnit(deadList);

        Invoke("PotionKill",0.2f);

        //for(int i = 0; i < unitType.Length; i++)
        //{
        //    switch(potionType.PotionTypes)
        //    { //ポーション別処理
        //        case TYPE.BOMB:     //ボムの場合
        //            gameDirector.DestroyUnit(unitType[i]);
        //            break;

        //        case TYPE.CRUSTER:  //クラスターの場合
        //            gameDirector.DestroyUnit(unitType[i]);
        //            break;

        //        case TYPE.REFRESH:   //リフレッシュの場合
        //            gameDirector.BuffUnit(unitType[i],TYPE.REFRESH);
        //            break;

        //        case TYPE.INVISIBLE: //無敵の場合
        //            gameDirector.BuffUnit(unitType[i],TYPE.INVISIBLE);
        //            break;

        //        case TYPE.MUSCLE:   //筋力の場合
        //            gameDirector.BuffUnit(unitType[i],TYPE.MUSCLE);
        //            break;

        //        case TYPE.ICE:      //アイスの場合
        //            gameDirector.DebuffUnit(unitType[i],TYPE.ICE);
        //            break;

        //        case TYPE.CURSE:    //呪いの場合
        //            gameDirector.DebuffUnit(unitType[i],TYPE.CURSE);
        //            break;

        //        case TYPE.SOUR:     //スッパイ場合
        //            gameDirector.DebuffUnit(unitType[i],TYPE.SOUR);
        //            break;

        //        default:
        //            break;
        //    }
        //}
    }

    /// <summary>
    /// ポーションオブジェクト破壊処理
    /// </summary>
    void PotionKill()
    {
        //ポーションを破壊
        Destroy(this.gameObject);
    }

    /// <summary>
    /// ボムのカウントダウン
    /// </summary>
    public void bombCntDown()
    {
        bombCnt--;
        countText.text = bombCnt.ToString();
    }

    /// <summary>
    /// コライダーにオブジェクトが入った時
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {   // "player"タグにぶつかった時

            if (!deadList.Contains(other.gameObject.GetComponent<UnitController>().Type))
            {   // ぶつかったプレイヤーのNoを取得
                deadList.Add(other.gameObject.GetComponent<UnitController>().Type);
            }
        }

    }

    /// <summary>
    /// コライダーから出たオブジェクトがあった時
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {   // "player"タグが出た時

            if (deadList.Contains(other.gameObject.GetComponent<UnitController>().Type))
            {   // ぶつかったプレイヤーのNoを消す
                deadList.Remove(other.gameObject.GetComponent<UnitController>().Type);
            }
        }
    }
}