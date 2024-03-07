using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NextImage : MonoBehaviour
{
    //[SerializeField] GameObject[] slides;//inspectorから表示したい画像を貼ったImageオブジェクトをドラッグ＆ドロップで設定


    //int head = 0;//今何枚目を表示しているのかという変数


    // Start is called before the first frame update
    void Start()
    {
        //if (slides.Length != head)
        //{
        //    slides[head].SetActive(true);//一枚目を表示
        //}
        //for (int i = head + 1; i < slides.Length; i++)
        //{
        //    slides[i].SetActive(false);//他のスライドを非表示
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    slides[head].SetActive(false);//今表示しているものを非表示
        //    if (slides.Length != ++head)//もしスライドがまだあるなら
        //        slides[head].SetActive(true);//次のスライドを表示
        //}
    }
}
