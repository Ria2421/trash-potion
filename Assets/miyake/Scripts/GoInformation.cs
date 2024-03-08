using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoInformation : MonoBehaviour
{
    //informationの変数
    [SerializeField] Text information;

    //カーソル    
    public GameObject arrow;

    void Start()
    {
        //フォントサイズを64、色を黒に、カーソルを非表示
        information.color = Color.black;
        information.fontSize = 64;
        arrow.SetActive(false);
    }

    //マウスカーソルが乗った時
    public void OnMouseOver()
    {
        //フォントサイズを80、色を赤に、カーソルを表示
        information.color = Color.red;
        information.fontSize = 80;
        arrow.SetActive(true);
    }

    //マウスカーソルが離れた時
    public void OnMouseExit()
    {
        //フォントサイズを64、色を黒に、カーソルを非表示
        information.color = Color.black;
        information.fontSize = 64;
        arrow.SetActive(false);
    }

    //遊び方説明シーン遷移
    public void HowToPlay()
    {
        Initiate.DoneFading();
        Initiate.Fade("Information", Color.black, 1.5f);
    }
}
