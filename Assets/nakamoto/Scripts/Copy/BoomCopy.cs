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