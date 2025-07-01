using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CtrlBase:MonoBehaviour
{
    public PlayerEntity characterBase;
    public virtual void Init(PlayerEntity character)
    {
        characterBase=character;
    }
    public abstract void YOTOUpdate(float deltaTime);
    public abstract void YOTONetUpdate();
}
