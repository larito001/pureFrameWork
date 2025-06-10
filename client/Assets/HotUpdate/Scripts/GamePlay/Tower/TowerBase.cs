using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBase : MonoBehaviour
{
    public GameObject canavs;
    public Button towerButton1;

    public void OnEnter()
    {
        canavs.SetActive(true);
    }

    public void OnExit()
    {
        canavs.SetActive(false);
        
    }

    private void Start()
    {
        towerButton1.onClick.AddListener(Generate);
    }

    private void Generate()
    {
        TowerManager.Instance.GenerateTower(this.transform.position);
        //todo：生成防御塔，使用框架生成
    }
}
