using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    Dictionary<int, TowerBaseEntity> towers = new Dictionary<int, TowerBaseEntity>();

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
}