using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int missionNum = 0;
    List<string> missionUIStr=new List<string>();
    string[] missionStr = new string[5] {"","","","","" };
    string[] missionList = new string[5]
    { "怪獣のチャージを止めろ\n",
            "",
            "",
            "",
            ""
    };
    [SerializeField]
    Text missionUI;
    GameObject enemy;
    enemyAI EAI;
    int buildingNum, buildingMaxNum;
    // Start is called before the first frame update
    void Start()
    {
        buildingMaxNum = checkBuilding();
        missionStr[0] = "";
        missionUI.text = missionStr[0];
        enemy = GameObject.FindGameObjectWithTag("enemy");
        EAI = enemy.GetComponent<enemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        buildingNum = checkBuilding();
        CheckEvent();
        
    }

    public void CheckEvent()
    {
        missionUIStr.Clear();
        if (EAI.aiState == enemyAI.EnemyAiState.CHARGING)
        {
            //Debug.Log("mission発生");
            missionUIStr.Add(missionList[0]);
          //  missionStr[0] = missionList[0];
            
        }
        if (buildingNum > 0)
        {
            missionUIStr.Add("街を守れ！（"+buildingNum+"/"+buildingMaxNum+"）\n");
        }
        ChangeText();
    }
    void ChangeText()
    {
        missionUI.text = "";
        for (int i = 0; i < missionUIStr.Count; i++)
        {
            missionUI.text += missionUIStr[i];
        }
        
    }

    int checkBuilding()
    {
        GameObject[] building;
        building = GameObject.FindGameObjectsWithTag("building");
        return building.Length;
    }
    //void addMissionText(string a)
    //{
    //    for (int i = 0; i < missionUIStr.Count; i++)
    //    {
    //        if (missionStr[i] == null)
    //        {
    //            missionStr[i] = a;
    //            break;
    //        }
    //    }
    //}

}
