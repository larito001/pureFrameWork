// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using YOTO;
//
// public class PlayerNetCtrl : CtrlBase
// {
//
//     public Vector3 lastPos=new Vector3(0,0,0);
//     public Quaternion lastRot=Quaternion.identity;
//     public void ChangePlayerMoveDir(Vector3 currentPos,Quaternion rotation,Vector3 dir)
//     {
//         
//         characterBase.playerMovement=dir;
//         lastPos = currentPos;
//         lastRot=rotation;
//     }
//     
//     public override void YOTOUpdate(float deltaTime)
//     {
//         
//     }
//
//     public override void YOTONetUpdate()
//     {
//     
//         if (!YOTOFramework.netMgr.isLogin)
//         {
//             return;
//         }
//     }
// }
