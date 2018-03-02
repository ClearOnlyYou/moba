using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Tianyu;
using UnityStandardAssets.ImageEffects;
public class Upgrade : GUIBase
{
    public GUISingleLabel currentLevel;
    public GUISingleLabel levelCapMax;
    public GUISingleLabel currentStrength;
    public GUISingleLabel strengthCapMax;
    public GUISingleLabel currentFriends;
    public GUISingleLabel friendsCapMax;
    public GUISingleLabel level;
    public GUISingleLabel lastStrength;
    public GUISingleLabel NextStrength;
    public GUISingleMultList multList;
    private GameObject go;
    private Transform newOpenLabel;
    List<UnLockFunctionNode> functionList = new List<UnLockFunctionNode>();
    Vector3[] posArr = new Vector3[] { new Vector3(-49f, -145f, 0), new Vector3(-91f, -145f, 0), new Vector3(-139f, -145f, 0) };
    UISprite mask;

    public static Upgrade _instance;
    public Upgrade()
    {
        _instance = this;
    }

    public override UIPanleID GetUIKey()
    {
        return UIPanleID.Upgrade;
    }
    protected override void OnLoadData()
    {
        base.OnLoadData();
        this.State = EnumObjectState.Ready;
        Show();
    }
    protected override void Init()
    {

        base.Init();
        _instance = this;

        newOpenLabel = transform.Find("NewOpenLabel");
        mask = this.transform.Find("Mask").GetComponent<UISprite>();
        UIEventListener.Get(mask.gameObject).onClick += OnMaskClick;
    }
    protected override void ShowHandler()
    {
        if(Globe.isSaoDang)
        {
            lastStrength.text = (playerData.GetInstance().beforeStrength - Globe.autoScenceCount*6).ToString();//֮ǰ������
            //if(Globe.autoScenceCount)
        }
        else
        {
            lastStrength.text = (playerData.GetInstance().beforeStrength).ToString();//֮ǰ������
        }
        currentLevel.text = playerData.GetInstance().beforePlayerLevel.ToString();//֮ǰ�ĵȼ�
        levelCapMax.text = playerData.GetInstance().selfData.level.ToString();//���ڵĵȼ�
        level.text = playerData.GetInstance().selfData.level.ToString();
        //lastStrength.text = playerData.GetInstance().beforeStrength.ToString();//֮ǰ������
        NextStrength.text = playerData.GetInstance().baginfo.strength + "";//���ڵ�����
        Debug.Log("����ս�ӵȼ�  " + playerData.GetInstance().beforePlayerLevel + "=====>" + playerData.GetInstance().selfData.level);
        //int levelCha = playerData.GetInstance().selfData.level - playerData.GetInstance().beforePlayerLevel;
        //int rewardStrength = 0;
        //if (levelCha>0)
        //{
        //    for (int i =0;i<levelCha;i++)
        //    {
        //        if (FSDataNodeTable<PlayerLevelUpNode>.GetSingleton().DataNodeList.ContainsKey(playerData.GetInstance().beforePlayerLevel))
        //        {
        //            rewardStrength = FSDataNodeTable<PlayerLevelUpNode>.GetSingleton().DataNodeList[playerData.GetInstance().beforePlayerLevel + i].rewardPower;

        //        }
        //    }
        //    NextStrength.text = rewardStrength + playerData.GetInstance().beforeStrength + "";
        //}

        if (FSDataNodeTable<PlayerLevelUpNode>.GetSingleton().DataNodeList.ContainsKey(playerData.GetInstance().beforePlayerLevel))
        {
            //֮ǰ�ȼ�����������
            currentStrength.text = FSDataNodeTable<PlayerLevelUpNode>.GetSingleton().DataNodeList[playerData.GetInstance().beforePlayerLevel].maxPower.ToString();
            //֮ǰ�ȼ��ĺ�������
            currentFriends.text = FSDataNodeTable<PlayerLevelUpNode>.GetSingleton().DataNodeList[playerData.GetInstance().beforePlayerLevel].maxFiend.ToString();
        }
        if (FSDataNodeTable<PlayerLevelUpNode>.GetSingleton().DataNodeList.ContainsKey(playerData.GetInstance().selfData.level))
        {
            //���ڵȼ�����������
            strengthCapMax.text = FSDataNodeTable<PlayerLevelUpNode>.GetSingleton().DataNodeList[playerData.GetInstance().selfData.level].maxPower.ToString();
            //���ڵȼ��ĺ�������
            friendsCapMax.text = FSDataNodeTable<PlayerLevelUpNode>.GetSingleton().DataNodeList[playerData.GetInstance().selfData.level].maxFiend.ToString();
        }
        if (Camera.main.GetComponent<Blur>() != null)
        {
            Camera.main.GetComponent<Blur>().enabled = true;
        }
        InitData();
        
        
    }
    private void InitData()
    {
        functionList.Clear();
        foreach (UnLockFunctionNode node in FSDataNodeTable<UnLockFunctionNode>.GetSingleton().DataNodeList.Values)
        {
            if (node.unlock_system_type == 1 && node.condition_parameter == playerData.GetInstance().selfData.level)
            {
                functionList.Add(node);
            }
        }
        if (functionList.Count > 0)
        {
            newOpenLabel.gameObject.SetActive(true);
            multList.gameObject.SetActive(true);
            if (functionList.Count >= posArr.Length)
            {
                multList.transform.localPosition = posArr[posArr.Length - 1];
            }
            else
            {
                multList.transform.localPosition = posArr[functionList.Count-1];
            }
            multList.InSize(functionList.Count, functionList.Count);
            multList.Info(functionList.ToArray());
        }
        else
        {
            newOpenLabel.gameObject.SetActive(false);
            multList.gameObject.SetActive(false);
        }
    }
    public void RefreshCurrentStrength()
    {
        if (NextStrength!=null)
        {
            NextStrength.text = playerData.GetInstance().baginfo.strength + "";//���ڵ�����
        }
        
    }
    public void OnMaskClick(GameObject go)
    {
        GuideManager.Single().SetObject(this.gameObject);
        Control.HideGUI(this.GetUIKey());
        if (Camera.main.GetComponent<Blur>() != null)
        {
            Camera.main.GetComponent<Blur>().enabled = false;
        }
    }

}
