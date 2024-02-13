//
// UIスクリプト
// Name:西浦晃太 Date:2/13
//
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{ 
    /// <summary>
    /// 起動時処理
    /// </summary>
    private void Awake()
    {
        // UIシーンの追加
        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }
}
