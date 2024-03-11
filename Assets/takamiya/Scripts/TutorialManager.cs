//==================================================
//
//チュートリアルの実装
//Author：高宮祐翔
//date:3/7
//===================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialManager : MonoBehaviour
{
    //表示する画像を入れる
    [SerializeField] GameObject[] TutorialImages;

    int imageNum;//今何枚目が表示されているかの変数




    // Start is called before the first frame update
    void Start()
    {
        imageNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //左クリックが押されたら
        if (Input.GetMouseButtonDown(0))
        {
            //最後の画像が表示されていたら
            if (imageNum == TutorialImages.Length - 1)
            {
                Initiate.Fade("Title", Color.black, 1.0f);
            }
            else
            {
                //今表示されている画像を非表示にする
                TutorialImages[imageNum].SetActive(false);

                imageNum++;

                //次の画像を表示する
                TutorialImages[imageNum].SetActive(true);
            }
        }
        //右クリックが押されたら
        else if(Input.GetMouseButtonDown(1))
        {
            //一番最初の画像だったら
            if (imageNum == 0)
            {
                Initiate.Fade("Title", Color.black, 1.0f);
            }
            else
            {
                //今表示されている画像を非表示にする
                TutorialImages[imageNum].SetActive(false);

                imageNum--;

                //前の画像を表示する
                TutorialImages[imageNum].SetActive(true);
            }
        }
    }
        
}
