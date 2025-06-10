using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{

    public void GenerateTower(Vector3 pos)
    {
        Debug.Log("生产防御塔");
        TowerBaseEntity ent = new TowerBaseEntity();
        ent.SetPosition(pos);
    }
}
