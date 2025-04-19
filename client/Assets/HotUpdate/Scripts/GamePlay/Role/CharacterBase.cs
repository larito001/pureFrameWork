using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : BaseEntity
{
    public bool isRunning;
    public bool isSpinting;
    public float walkSpeed = 2.5f;
    public float runSpeed = 10f;


    public Vector3 playerMovement;
    public GameObject character;
    public Vector3 animationVelocity;
    public Quaternion animationRotate=Quaternion.identity;

    protected CharacterBase()
    {
        //Debug.Log("���츸��");
    }

    public abstract void Init(Vector3 pos);
    public abstract void CulculateDir();
    public abstract void AddComponent();
    //public abstract void Start();
    //public abstract void Update(float deltaTime);
    //public abstract void FixedUpdate(float deltaTime);
    public abstract void DontMove();
    public abstract void CanMove();
    public abstract void Dispose();

    public Vector3 GetPos()
    {
        return character.transform.position;
    }
}
