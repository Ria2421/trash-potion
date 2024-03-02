//
//ボタンスクリプト
//Author：高宮祐翔
//Date:2/21
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening; //DOTween使用に必要
using System.Text;
using System.Linq;
using System.Net.Sockets;

public class ButtonPushScript :
MonoBehaviour,
IPointerEnterHandler,
    IPointerExitHandler
{
    CanvasGroup canvasGroup;
    Image image;
    Text text;
    Button button;

    [SerializeField] AudioClip ButtonSE;

    AudioSource buttonSE;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
        text = this.transform.GetChild(0).gameObject.GetComponent<Text>();
        button = GetComponent<Button>();
        buttonSE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ButtonPushSE();
    }

    /// <summary>
    /// Buttonにマウスカーソルに置かれた時
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) { return; }
        transform.DOScale(1.2f, 0.24f).SetEase(Ease.OutCubic).SetLink(gameObject);
    }
    /// <summary>
    /// Buttonからマウスカーソルが離れた時
    /// </summary>
    /// <param name="eventData"></param>
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) { return; }
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic).SetLink(gameObject);
    }

    //ボタンを押した時
    async void ButtonPushSE()
    {
        if(Input.GetMouseButtonDown(0))
        {
            // 送信処理
            string json = "1";
            byte[] buffer = Encoding.UTF8.GetBytes(json);                      // JSONをbyteに変換
            buffer = buffer.Prepend((byte)EventID.InGameFlag).ToArray();       // 送信データの先頭にイベントIDを付与
            NetworkStream stream = NetworkManager.MyTcpClient.GetStream();
            await stream.WriteAsync(buffer, 0, buffer.Length);                 // JSON送信処理
#if DEBUG
            Debug.Log("インゲーム送信");
#endif
        }
    }
        
}
