using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneResType
{
    Wood,
    Iron,
}
public class SceneResManager : Singleton<SceneResManager>
{
 
    Dictionary<int, SceneResEntity> resList = new Dictionary<int, SceneResEntity>();

    public void Init()
    {
        var org = GameObject.Find("PlayerOrgPos");
        for (int i = 0; i < 5; i++)
        {
            GenerateResAt(SceneResType.Wood, org.transform.position + new Vector3(0, -4, 5) + new Vector3(5, 0, 0) * i);
            GenerateResAt(SceneResType.Iron, org.transform.position+new Vector3(0,-4,10) + new Vector3(5, 0, 0) * i);
        }
    }
    public void GenerateResAt(SceneResType resType,Vector3 pos)
    {
        SceneResEntity res=null;
        if (resType == SceneResType.Wood)
        {
            res=WoodResEntity.pool.GetItem(pos);
        }else if (resType == SceneResType.Iron)
        {
            res=IronResEntity.pool.GetItem(pos);
        }

        if (res != null)
        {
            res.Location = pos;
            res.InstanceGObj();
            resList.Add(res._entityID, res); 
        }

    }

    public SceneResEntity GetResById(int id)
    {
        if (resList.ContainsKey(id))
        {
            return resList[id];
        }

        return null;
    }
}
