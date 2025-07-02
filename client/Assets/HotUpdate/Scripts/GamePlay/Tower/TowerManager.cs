using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    Dictionary<int, TowerBaseEntity> towers = new Dictionary<int, TowerBaseEntity>();
    public void Init()
    {
        var org = GameObject.Find("PlayerOrgPos");
        for (int i = 0; i < 5; i++)
        {
            GenerateTowerBase(org.transform.position+new Vector3(0,-4,0)+new Vector3(5,0,0)*i); 
        }
    }
    public void GenerateTowerBase(Vector3 pos)
    {
        var tower = TowerBaseEntity.pool.GetItem(Vector3.zero);
        tower.Location = pos;
        tower.InstanceGObj();
        towers.Add(tower._entityID, tower);
    }
    
    public TowerBaseEntity GetTowerById(int id)
    {
        if (towers.ContainsKey(id))
        {
            return towers[id];
        }

        return null;
    }

    public override void Unload()
    {
        foreach (var towerBaseEntity in towers)
        {
            towerBaseEntity.Value.Free();
        }

        base.Unload();
    }
}