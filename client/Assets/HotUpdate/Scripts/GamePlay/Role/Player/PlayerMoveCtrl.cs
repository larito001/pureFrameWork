
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using YOTO;

public class PlayerMoveCtrl : CtrlBase
{
    Rigidbody rigidbody=null;
    Vector3 velocity;
    IEnumerator sprintIE;
    IEnumerator fvxIE;
    List<VFXBase> vfxs= new List<VFXBase>();
    private bool canMove = true;

    private Vector3 _outVolocity;
    private Quaternion _outQuaternion;
    public void SetRig(Rigidbody rig)
    {
        rigidbody = rig;
    }

    public void SetVolocity(Vector3 v)
    {
        _outVolocity = v;
    }
    public void SetRotation(Quaternion q)
    {
        _outQuaternion = q;
    }
    public override void YOTOUpdate(float deltaTime)
    {
        if (!canMove)
        {
            return;
        }
        
        if (rigidbody)
        {
      
            velocity = _outVolocity;
            velocity.y = rigidbody.velocity.y;
            rigidbody.velocity = velocity;
            rigidbody.rotation = _outQuaternion;
        }

    }

    public override void YOTONetUpdate()
    {
        
    }
}
