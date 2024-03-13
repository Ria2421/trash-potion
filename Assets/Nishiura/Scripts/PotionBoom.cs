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
    /// オーディオソース
    /// </summary>
    [SerializeField] AudioSource audioSource;

    /// <summary>
    /// 爆発時SE
    /// </summary>
    [SerializeField] AudioClip boomSE;      

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
        bombCnt = 6;
        potionType = new PotionType();
        gameDirector = GameObject.Find("GameDirector").GetComponent<GameDirectorCopy>();
    }

    void Update()
    {
        if (bombCnt == 0)
        { //指定番号のプレイヤーを殺害(番号はサーバから取得)
            audioSource.PlayOneShot(boomSE);
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
            if (other.gameObject != null)
            {   //nullチェック
                if (!deadList.Contains(other.gameObject.GetComponent<UnitController>().Type))
                {   // ぶつかったプレイヤーのNoを取得
                    deadList.Add(other.gameObject.GetComponent<UnitController>().Type);
                }
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
            if (other.gameObject != null)
            {   //nullチェック
                if (deadList.Contains(other.gameObject.GetComponent<UnitController>().Type))
                {   // ぶつかったプレイヤーのNoを消す
                    deadList.Remove(other.gameObject.GetComponent<UnitController>().Type);
                }
            }
        }
    }
}