using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : BaseEntity
{
    
    //属性
    public float HP = 100;
    public float maxHP=100;
    public float ATK;
    public float DEF;
    public float SPEED;
    public float CRT; //暴击率
    public float CDMG; //暴击伤害
    //移动方向
  
    public GameObject character;
    public Vector3 animationVelocity;
    public Quaternion animationRotate=Quaternion.identity;
    public abstract void Init(Vector3 pos);
    protected abstract void AddEvent();
    protected abstract void RemoveEvent();
    public Vector3 GetPos()
    {
        return character.transform.position;
    }
}
