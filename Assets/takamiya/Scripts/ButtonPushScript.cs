using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening; //DOTween使用に必要

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

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) { return; }
        transform.DOScale(1.2f, 0.24f).SetEase(Ease.OutCubic).SetLink(gameObject);
    }
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable) { return; }
        transform.DOScale(1f, 0.24f).SetEase(Ease.OutCubic).SetLink(gameObject);
    }

    void ButtonPushSE()
    {
        if(Input.GetMouseButtonDown(0))
        {
            /* フェード処理 (黒)
            ( "シーン名",フェードの色, 速さ);*/
            Initiate.DoneFading();
            Initiate.Fade("IGC", Color.black, 1.5f);
            buttonSE.PlayOneShot(ButtonSE);
        }
        
    }
        
}
