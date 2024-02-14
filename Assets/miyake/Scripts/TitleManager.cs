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


    // Start is called before the first frame update
    void Start()
    {

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
            }
        }
    }
}
