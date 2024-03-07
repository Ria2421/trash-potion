using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    //quitの変数
    [SerializeField] Text quit;

    //カーソル    
    public GameObject arrow;

    void Start()
    {
        //フォントサイズを64、色を黒に、カーソルを非表示
        quit.color = Color.black;
        quit.fontSize = 64;
        arrow.SetActive(false);
    }

    //マウスカーソルが乗った時
    public void OnMouseOver()
    {
        //フォントサイズを80、色を赤に、カーソルを表示
        quit.color = Color.red;
        quit.fontSize = 80;
        arrow.SetActive(true);
    }

    //マウスカーソルが離れた時
    public void OnMouseExit()
    {
        //フォントサイズを64、色を黒に、カーソルを非表示
        quit.color = Color.black;
        quit.fontSize = 64;
        arrow.SetActive(false);
    }

    //ゲーム終了処理
    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();//ゲームプレイ終了
#endif
    }
}
