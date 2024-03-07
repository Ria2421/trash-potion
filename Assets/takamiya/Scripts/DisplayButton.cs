using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayButton : MonoBehaviour
{

    [SerializeField] GameObject TitleButton;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("displayButton", 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void displayButton()
    {
        TitleButton.SetActive(true);
    }
}
