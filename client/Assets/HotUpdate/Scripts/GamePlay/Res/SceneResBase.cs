
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneResBase : MonoBehaviour
{
    private int canCollectNum = 3;
    public SceneResEntity resEntity
    {
        get;
        private set;
    }
    public void Init(SceneResEntity entity)
    {
        resEntity=entity;
    }

    public void CollectOnce()
    {
        canCollectNum--;
        FlyTextMgr.Instance.AddText("wood+1", this.transform.position, FlyTextType.Normal);
        PlayerResManager.Instance.AddWoodNum(1);
        if (canCollectNum == 0)
        {
            resEntity.Remove();
        }
    }
}
