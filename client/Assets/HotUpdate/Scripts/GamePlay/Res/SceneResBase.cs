
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneResBase : MonoBehaviour
{
   
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

        resEntity.Collect();

    }
}
