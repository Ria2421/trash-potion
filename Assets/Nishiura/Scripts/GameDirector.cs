using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    //Player
    public bool[] isPlayer; 
    Player[] player;
    int nowTurn;

    //GameMode
    enum MODE
    {
        NONE=-1,
        WAIT_TURN_START,
        MOVE_SELECT,
        FIELD_UPDATE,
        WAIT_TURN_END,
        TURN_CHANGE,
    }

    //MODE 
    MODE nowMode;
    MODE nextMode;

    //待機時間定義
    float waitTime;

    //フィールド
    int[,] tileData = new int[,]
    {//手前
        {0,0,8,0,0,0,0,0,8,0,0},
        {0,2,1,1,1,1,1,1,1,2,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,1,1,1,1,1,1,1,1,1,0},
        {0,2,1,1,1,1,1,1,1,2,0},
        {0,0,4,0,0,0,0,0,4,0,0},
    };

    //プレイヤー初期配置
    int[,]initUnitData = new int[,] 
    {//手前
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,1,0,0,0,0,0,1,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
        {0,0,2,0,0,0,0,0,2,0,0},
        {0,0,0,0,0,0,0,0,0,0,0},
    };

    //プレイヤー最大数
    const int PLAYER_MAX = 12;

    //フィールド上のプレイヤー
    List<GameObject>[,] unitData;

    //プレイヤー選択モードで使う
    GameObject selectUnit;
    int oldX, oldY;


    //ボタン等のオブジェクト
    GameObject txtInfo;
    GameObject buttonTurnEnd;
    GameObject objCamera;

    // Start is called before the first frame update
    void Start()
    {
        List<int> p1rnd = getRandomList(PLAYER_MAX, PLAYER_MAX / 2);
        List<int> p2rnd = getRandomList(PLAYER_MAX, PLAYER_MAX / 2);
        int p1 = 0;
        int p2 = 0;

        unitData = new List<GameObject>[tileData.GetLength(0), tileData.GetLength(1)];

        //プレイヤー設定
        player = new Player[2];     //人数
        player[0] = new Player(isPlayer[0], 1);
        player[1] = new Player(isPlayer[1], 2);

        //タイル初期化
        for(int i = 0; i<tileData.GetLength(0); i++)
        {
            for (int j = 0; j < tileData.GetLength(1); j++)
            {
                float x =j - (tileData.GetLength(1) /2 - 0.5f);
                float z = i - (tileData.GetLength(0) / 2 - 0.5f);

                //タイル配置
                string resname = "";

                //1:NormalTile 2:GoalTile 3:1P'sGoal 4:2P's Goal
                int no = tileData[i,j];
                if (4 == no || 8 == no) no = 5;

                resname = "Cube (" + no + ")";

                resourcesInstantiate(resname, new Vector3(x, 0, z), Quaternion.identity);

                //プレイヤー配置
                unitData[i,j] = new List<GameObject>();

                Vector3 angle = new Vector3(0,0,0);
                int playerType = UnitController.TYPE_BLUE;

                List<int> unitrnd = new List<int>();

                int unitNum = -1;

                //1P配置
                resname = "Unit1";

                if(1 == initUnitData[i,j])
                {

                    unitNum = p1++;
                }
                else if (2 == initUnitData[i, j])
                {                   
                    unitNum = p2++;
                    angle.y = 180;
                }
                else
                {
                    resname = "";
                }

                //赤ユニット配置判定
                if (-1 < unitrnd.IndexOf(unitNum))
                {
                    resname = "Unit2";
                    playerType = UnitController.TYPE_RED;
                }

                GameObject unit = resourcesInstantiate(resname, new Vector3(x, 0.6f, z), Quaternion.Euler(angle));


                if(null != unit)
                {
                    unit.GetComponent<UnitController>().PlayerNo = initUnitData[i,j];
                    unit.GetComponent<UnitController>().Type = playerType;
                }
            }

            nowTurn = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SelectMode();
    }

    void SelectMode()
    {
        GameObject hitobj = null;

        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit,100))
            {
                hitobj = hit.collider.gameObject;
            }
            Debug.Log(hitobj);

        }

        if(null == hitobj) return;

        Vector3 pos = hitobj.transform.position;

        int x = (int)(pos.x + (tileData.GetLength(1) / 2 - 0.5f));
        int z = (int)(pos.z + (tileData.GetLength(0) / 2 - 0.5f));

        //ユニット選択
        if(0 < unitData[z,x].Count && player[nowTurn].PlayerNo == unitData[z, x][0].GetComponent<UnitController>().PlayerNo)
        {
            Debug.Log("aaaaaaaaaa");

            if(null != selectUnit)
            {
                selectUnit.GetComponent<UnitController>().Select(false);
            }
            selectUnit = unitData[z, x][0];
            oldX = x ;
            oldY = z;

            selectUnit.GetComponent<UnitController>().Select();
        }

        //移動先タイル選択
    }

    //ランダム配置関数(使わん)
    List<int>getRandomList(int range,int count)
    {
        List<int> ret = new List<int>();

        if(range < count)
        {
            Debug.LogError("リスト生成エラー");
            return ret;
        }

        while(true)
        {
            int no = Random.Range(0,range);

            if(-1 == ret.IndexOf(no))
            {
                ret.Add(no);
            }
            if(count <= ret.Count)
            {
                break;
            }
        }

        return ret;
    }

    GameObject resourcesInstantiate(string name,Vector3 pos, Quaternion angle)
    {
        GameObject prefab =(GameObject)Resources.Load(name);

        if(null == prefab )
        {
            return null;
        }

        GameObject ret = Instantiate(prefab, pos, angle);

        return ret;
    }

    bool movableTile(int oldx,int oldz,int x, int z)
    {
        bool ret = false;

        //差分を取得
        int dx = Mathf.Abs(oldx - x);
        int dz = Mathf.Abs(oldz - z);

        //斜め進行不可
        //if(1 <dx + dz)
        //{
        //    ret = false;
        //}

        //壁以外
        if(1 == tileData[z,x] || 2 == tileData[z,x] || player[nowTurn].PlayerNo*4 == tileData[z, x])
        {
           
            if( 0== unitData[z,x].Count) 
            { //誰もいないマス
                ret = true;
            }
            else
            {//誰かいるマス

                if (unitData[z, x][0].GetComponent<UnitController>().PlayerNo != player[nowTurn].PlayerNo)
                {//敵だった場合
                    ret = true;
                }
            }
        }

        return ret;
    }
}
