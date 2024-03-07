//==============================================
//Autor:三宅歩人
//Day:3/7
//タイトル画面処理
//==============================================
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{    
    GameObject networkManager;   
    
    // Start is called before the first frame update
    void Start()
    {
        if(networkManager = GameObject.Find("NetworkManager"))
        {   // NetworkManagerオブジェクトが存在する時は破棄
            Destroy(networkManager);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
