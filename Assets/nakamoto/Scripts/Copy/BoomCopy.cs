//
// ポーション爆発スクリプト
// Name:西浦晃太 Date:02/26
// Update:02/29
//
using UnityEngine;
using static TMPro.Examples.ObjectSpin;

public class BoomCopy : MonoBehaviour
{
    /// <summary>
    /// 爆発エフェクトのプレハブ
    /// </summary>
    public GameObject explosionPrefab; 

    /// <summary>
    /// ゲームディレクター
    /// </summary>
    GameDirector gameDirector;

    /// <summary>
    /// 死亡するプレイヤータイプ
    /// </summary>
    int[] type = {2};

    /// <summary>
    /// ポーションの種類
    /// </summary>
    PotionType potionType;

    void Start()
    {
        potionType = new PotionType();
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirector>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { //指定番号のプレイヤーを殺害(番号はサーバから取得)
            BoomPotion(type);
        }
    }

    /// <summary>
    /// 爆発処理
    /// </summary>
    /// <param name="unitType"></param>
    void BoomPotion(int[] unitType)
    { 
        GameObject explosion = Instantiate(explosionPrefab, this.transform.position, Quaternion.identity);
        explosion.transform.position += new Vector3(0f, 0.5f, 0f);

        this.gameObject.transform.position += new Vector3(0f, -50f, 0f);

        Invoke("PotionKill",0.2f);

        for(int i = 0; i < unitType.Length; i++)
        {
            switch(potionType.PotionTypes)
            { //ポーション別処理
                case TYPE.BOMB:     //ボムの場合
                    gameDirector.DestroyUnit(unitType[i]);
                    break;

                case TYPE.CRUSTER:  //クラスターの場合
                    gameDirector.DestroyUnit(unitType[i]);
                    break;

                case TYPE.REFRESH:   //リフレッシュの場合
                    gameDirector.BuffUnit(unitType[i],TYPE.REFRESH);
                    break;

                case TYPE.INVISIBLE: //無敵の場合
                    gameDirector.BuffUnit(unitType[i],TYPE.INVISIBLE);
                    break;

                case TYPE.MUSCLE:   //筋力の場合
                    gameDirector.BuffUnit(unitType[i],TYPE.MUSCLE);
                    break;

                case TYPE.ICE:      //アイスの場合
                    gameDirector.DebuffUnit(unitType[i],TYPE.ICE);
                    break;

                case TYPE.CURSE:    //呪いの場合
                    gameDirector.DebuffUnit(unitType[i],TYPE.CURSE);
                    break;

                case TYPE.SOUR:     //スッパイ場合
                    gameDirector.DebuffUnit(unitType[i],TYPE.SOUR);
                    break;

                default:
                    break;
            }
        }
    }

    /// <summary>
    /// ポーションオブジェクト破壊処理
    /// </summary>
    void PotionKill()
    {
        //ポーションを破壊
        Destroy(this.gameObject);
    }
}