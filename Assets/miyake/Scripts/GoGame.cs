using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoGame : MonoBehaviour
{
    //カーソル    
    public GameObject arrow;

    //startの変数
    [SerializeField] Text start;
    
    void Start()
    {
        //フォントサイズを64、色を黒に、カーソルを非表示
        arrow.SetActive(false);
        start.color = Color.black;
        start.fontSize = 64;
    }

    //マウスカーソルが乗った時
    public void OnMouseOver()
    {
        //フォントサイズを80、色を赤に、カーソルを表示
        arrow.SetActive(true);
        start.color = Color.red;
        start.fontSize = 80;
    }

    //マウスカーソルが離れた時
    public void OnMouseExit()
    {
        //フォントサイズを64、色を黒に、カーソルを非表示
        arrow.SetActive(false);
        start.color= Color.black;
        start.fontSize = 64;
    }

    public void GoGameScene()
    {
        Initiate.DoneFading();
        Initiate.Fade("Connect", Color.black, 1.5f);
    }
}
