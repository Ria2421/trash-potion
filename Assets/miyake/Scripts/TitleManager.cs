using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    [SerializeField] EventSystem eventSystem;      //イベントシステム
    bool select = false;            //ボタンが選択されたらtrue
    public GameObject arrow;        //カーソル
    GameObject selectObject;
    [SerializeField] Text start;          //startの変数
    [SerializeField] Text quit;           //qiotの変数
    // Start is called before the first frame update
    void Start()
    {
        start.color = Color.black;
        quit.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        if(select == false)
        {
            
            if(eventSystem.currentSelectedGameObject != null)
            {
                selectObject = eventSystem.currentSelectedGameObject.gameObject;
            }
            //もしボタンが選択されていたら
            if(this.gameObject == selectObject)
            {
                //矢印を表示
                arrow.SetActive(true);
                select = true;

                //選ばれた方のテキストの色と大きさを変える
                start.color= Color.red;
                quit.color= Color.black;
                start.fontSize = 80;
                quit.fontSize = 64;
            }
        }
        else
        {
            if (eventSystem.currentSelectedGameObject != null)
            {
                selectObject = eventSystem.currentSelectedGameObject.gameObject;
            }
            //もしボタンが選択から外れたら
            if (this.gameObject != selectObject)
            {
                //矢印を非表示
                arrow.SetActive(false);
                select = false;

                //選ばれてない方のテキストの色と大きさを変える
                start.color = Color.black;
                quit.color = Color.red;
                start.fontSize = 64;
                quit.fontSize = 80;
            }
        }
    }
}
