//---------------------------------------------------------------
//
// �l�b�g���[�N�}�l�[�W���[ [NetworkManager.cs ]
// Author:Kenta Nakamoto
// Data:2024/02/08
// Update:2024/02/28
//
//---------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Text;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class NetworkManager : MonoBehaviour
{
    //------------------------------------------------------------------------------
    // �t�B�[���h ----------------------------------------

    /// <summary>
    /// �N���C�A���g�쐬
    /// </summary>
    TcpClient tcpClient;

    /// <summary>
    /// �ڑ���IP�A�h���X
    /// </summary>
    const string ipAddress = "127.0.0.1";

    /// <summary>
    /// �|�[�g�ԍ�
    /// </summary>
    const int portNum = 20001;

    /// <summary>
    /// ����M�T�C�Y
    /// </summary>
    const int dataSize = 1024;

    /// <summary>
    /// �ڑ����i�[�p
    /// </summary>
    public static NetworkStream stream;

    /// <summary>
    /// �����̃v���C���[�ԍ�
    /// </summary>
    public static int MyNo
    {  get; private set; }

    /// <summary>
    /// �}�b�`���O���e�L�X�g
    /// </summary>
    [SerializeField] GameObject matchingText;

    /// <summary>
    /// �ҋ@��UI
    /// </summary>
    [SerializeField] GameObject readyUI;

    /// <summary>
    /// ���@���w�����̃I�u�W�F�N�g
    /// </summary>
    [SerializeField] GameObject[] arrowYouObjs;

    /// <summary>
    /// �S�v���C���[�̖��O�i�[ 
    /// </summary>
    [SerializeField] GameObject[] playerNames;

    /// <summary>
    /// �S�v���C���[�̑ҋ@��Ԋi�[�p
    /// </summary>
    [SerializeField] GameObject[] playerStatus;

    /// <summary>
    /// ���O���͗pUI
    /// </summary>
    [SerializeField] GameObject nameInput;

    /// <summary>
    /// ���������{�^��
    /// </summary>
    [SerializeField] GameObject completeButton;

    /// <summary>
    /// ���C���X���b�h�ɏ������s���˗��������
    /// </summary>
    SynchronizationContext context;

    //------------------------------------------------------------------------------
    // ���\�b�h ------------------------------------------

    /// <summary>
    /// �����֐�
    /// </summary>
    async void Start()
    {
        context = SynchronizationContext.Current;

        // �ڑ������̎��s
        await StartClient(ipAddress, portNum);

        // �V�[���J�ڎ���NetworkManager��j�����Ȃ��悤�ɐݒ�
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// �N���C�A���g�ڑ�����
    /// </summary>
    /// <param name="ipaddress"></param>
    /// <param name="port"></param>
    /// <returns></returns>
    private async Task StartClient(string ipaddress, int port)
    {
        // �T�[�o�[�֐ڑ�
        try
        {
            //�N���C�A���g�쐬
            tcpClient = new TcpClient();

            // ����M�^�C���A�E�g�ݒ� (msec)
            tcpClient.SendTimeout = 1000;
            tcpClient.ReceiveTimeout = 1000;

            // �T�[�o�[�֐ڑ��v��
            await tcpClient.ConnectAsync(ipAddress, port);

            // �T�[�o�[����PL�ԍ�����M�ҋ@
            byte[] recvBuffer = new byte[dataSize];                                    // ����M�f�[�^�i�[�p
            stream = tcpClient.GetStream();                                            // �N���C�A���g�̃f�[�^����M�Ɏg��NetworkStream���擾
            int length = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);     // ��M�f�[�^�̃o�C�g�����擾

            // ��M�f�[�^����C�x���gID�����o��
            int eventID = recvBuffer[0];

            // ��M�f�[�^���當��������o��
            byte[] bufferJson = recvBuffer.Skip(1).ToArray();                            // 1�o�C�g�ڂ��X�L�b�v
            string recevieString = Encoding.UTF8.GetString(bufferJson, 0, length - 1);   // ��M�f�[�^�𕶎���ɕϊ�

            // ������PL No��ۑ�
            MyNo = int.Parse(recevieString[0].ToString());

            // �}�b�`���O�e�L�X�g���\��
            matchingText.SetActive(false);

            // �ҋ@��UI��\��
            readyUI.SetActive(true);

            // ������PL�ԍ��̏�ɖ��\��
            arrowYouObjs[MyNo - 1].SetActive(true);

            // ��M�p�X���b�h�̋N��
            Thread thread = new Thread(new ParameterizedThreadStart(RecvProc));
            thread.Start(tcpClient);
#if DEBUG
            // �����{�^�����̗L����
            Debug.Log("�ҋ@��ʕ\��");
#endif
        }
        catch (Exception ex)
        {
            // �G���[������
            Debug.Log(ex);
        }
    }

    /// <summary>
    /// ��M�p�X���b�h
    /// </summary>
    /// <param name="arg"></param>
    async void RecvProc(object arg)
    {
        TcpClient tcpClient = (TcpClient)arg;

        NetworkStream stream = tcpClient.GetStream();

        while (true)
        {
            // ��M�ҋ@����
            byte[] recvBuffer = new byte[dataSize];
            int length = await stream.ReadAsync(recvBuffer, 0, recvBuffer.Length);

            // �ڑ��ؒf�`�F�b�N
            if(length <= 0)
            {
                
            }

            // ��M�f�[�^����C�x���gID�����o��
            int eventID = recvBuffer[0];

            // ��M�f�[�^����JSON����������o��
            byte[] bufferJson = recvBuffer.Skip(1).ToArray();     // 1�o�C�g�ڂ��X�L�b�v
            string jsonString = System.Text.Encoding.UTF8.GetString(bufferJson, 0, length - 1);

            context.Post(_ =>
            {
                switch (eventID)
                {
                    case (int)EventID.UserData: // �ePL�̖��O�\������

                        // Json�f�V���A���C�Y
                        UserData userData = JsonConvert.DeserializeObject<UserData>(jsonString);

                        // ���M����Ă���PLNo�̖��O���N���C�A���g�̃e�L�X�g�ɔ��f������
                        playerNames[userData.PlayerNo - 1].GetComponent<Text>().text = userData.UserName;

                        break;

                    case (int)EventID.CompleteFlag: // �ePL�̏��������\������

                        // Json�f�V���A���C�Y
                        userData = JsonConvert.DeserializeObject<UserData>(jsonString);

                        // �����\��
                        playerStatus[userData.PlayerNo - 1].GetComponent<Text>().text = "��������";

                        break;

                    case (int)EventID.InSelectFlag: // �I����ʑJ�ڏ���
                        NextScene("ModeSelection");
                        break;

                    case (int)EventID.InGameFlag: // �Q�[����ʑJ�ڏ���
                        NextScene("IGC");
                        break;

                    default: 
                        break;
                }
            }, null);
        }

        // �ʐM��ؒf
        tcpClient.Close();
    }

    /// <summary>
    /// ���[�U�[�����M����
    /// </summary>
    public async void sendUserData()
    {
        //���M�p���[�U�[�f�[�^�̍쐬
        UserData userData = new UserData();
        userData.UserName = nameInput.GetComponent<InputField>().text;   // ���͂��ꂽ���O���i�[
        userData.PlayerNo = MyNo;                                        // �����̃v���C���[�ԍ����i�[

        // ���M�f�[�^��JSON�V���A���C�Y
        string json = JsonConvert.SerializeObject(userData);

        // ���M����
        byte[] buffer = Encoding.UTF8.GetBytes(json);                // JSON��byte�ɕϊ�
        buffer = buffer.Prepend((byte)EventID.UserData).ToArray();   // ���M�f�[�^�̐擪�ɃC�x���gID��t�^
        await stream.WriteAsync(buffer, 0, buffer.Length);           // JSON���M����

        // ���̓t�B�[���h�̖�����
        nameInput.SetActive(false);

        // ���������{�^���̗L����
        completeButton.SetActive(true);
    }

    /// <summary>
    /// ���������t���O���M����
    /// </summary>
    public async void SendComplete()
    {
        // ���������{�^���̔�\��
        completeButton.SetActive(false);

        UserData userData = new UserData();
        userData.IsReady = true;                 // ���������t���O��true��
        userData.PlayerNo = MyNo;                // �����̃v���C���[�ԍ����i�[

        // ���M�f�[�^��JSON�V���A���C�Y
        string json = JsonConvert.SerializeObject(userData);

        // ���M����
        byte[] buffer = Encoding.UTF8.GetBytes(json);                    // JSON��byte�ɕϊ�
        buffer = buffer.Prepend((byte)EventID.CompleteFlag).ToArray();   // ���M�f�[�^�̐擪�ɃC�x���gID��t�^
        await stream.WriteAsync(buffer, 0, buffer.Length);               // JSON���M����

#if DEBUG
        // ���M�ҋ@�����\��
        Debug.Log("�����������M");
#endif
    }

    /// <summary>
    /// �V�[���J�ڏ���
    /// </summary>
    /// <param name="sceneName"> �J�ڂ������V�[���� </param>
    private void NextScene(string sceneName)
    {
        /* �t�F�[�h���� (��)  
            ( "�V�[����",�t�F�[�h�̐F, ����);  */
        Initiate.DoneFading();
        Initiate.Fade(sceneName, Color.black, 1.5f);
    }
}