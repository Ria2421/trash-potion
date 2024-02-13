using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    enum Player　
    {
        Player1,
        Player2,
        Player3,
        Player4
    }

    //Textオブジェクトを格納するためのフィールド
    [SerializeField] Text PlayerText;
    [SerializeField] Text cycleCountText;
    private Player currentPlayer = Player.Player1;// 現在のプレイヤーを表すenumの変数
    private int cycleCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //エンターキーが押されたら
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //次のプレイヤーに変更する
            currentPlayer = (Player)(((int)currentPlayer % 4) + 1);
            trunChenge();// プレイヤーテキストを更新するメソッドを呼び出す
        }
        else if (currentPlayer == Player.Player1)
        {
            cycleCount++;

            if (cycleCount ==2)
            {
                cycleCountText.text = "ターン目";
            }
        }
    }

    public void trunChenge()
    {
        // 現在のプレイヤー番号をテキストに表示する
        PlayerText.text = $"{(int)currentPlayer}P";

        //cycleCountText.text = $"{ (int)currentPlayer}ターン目";
    }
}
