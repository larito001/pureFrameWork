using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBase : MonoBehaviour
{
    public int towerId=-1;
    private TowerBaseEntity tower;
    public void OnEnter()
    {
        tower= TowerManager.Instance.GetTowerById(towerId);
        if (tower!=null)
        {
            tower.OnEnter();
        }
    }

    public void OnExit()
    {
        tower.OnExit();
    }
    

    public void SetId(int entityID)
    {
        towerId=entityID;
        
    }
}
