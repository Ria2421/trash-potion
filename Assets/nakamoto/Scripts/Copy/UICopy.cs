//
// UI�X�N���v�g
// Name:���Y�W�� Date:2/13
//
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICopy: MonoBehaviour
{ 
    /// <summary>
    /// �N��������
    /// </summary>
    private void Awake()
    {
        // UI�V�[���̒ǉ�
        SceneManager.LoadScene("UICopy", LoadSceneMode.Additive);
    }
}