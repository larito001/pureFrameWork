using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VirtualSceneBase 
{
    public abstract void OnAdd();
    public abstract void Onload();
    public abstract void OnInit();
    public abstract void UnLoad();
    public abstract void Update(float dt);


}
