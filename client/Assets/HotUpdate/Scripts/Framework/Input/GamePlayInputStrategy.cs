using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
namespace YOTO
{
    public class GamePlayInputStrategy : MInput.IGamePlayActionActions
    {

        public void OnScroll(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                //Debug.Log("��ȡ����" + context.ReadValue<Vector2>());
                YOTOFramework.eventMgr.TriggerEvent<float>(YOTO.EventType.Scroll, context.ReadValue<Vector2>().y);
            }

        }

        public void OnFire(InputAction.CallbackContext context)
        {
         
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            
        }

        public void OnMove(InputAction.CallbackContext context)
        {
             if (context.phase == InputActionPhase.Performed)
            {
                YOTOFramework.eventMgr.TriggerEvent<Vector2>(EventType.Move, context.ReadValue<Vector2>()); 
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                YOTOFramework.eventMgr.TriggerEvent<Vector2>(EventType.Move, context.ReadValue<Vector2>());
            }
        }
        
        //这个只有在拖、移动的时候触发
        public void OnTouch(InputAction.CallbackContext context)
         {
         
             if (context.phase == InputActionPhase.Performed)
             {
                 Debug.Log("Touch!!!");
                 YOTOFramework.eventMgr.TriggerEvent<Vector2>(YOTO.EventType.Touch, context.ReadValue<Vector2>());
             }
         
         
         }

        //这个方法获取开始点击和结束
        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Canceled) // 松开触发事件
            {
                Debug.Log("Touch Released!!!");
                YOTOFramework.eventMgr.TriggerEvent<Vector2>(YOTO.EventType.TouchRelease, context.ReadValue<Vector2>());
            }
        }
        //public void OnTouch(InputAction.CallbackContext context)
        //{
        //    if (context.phase == InputActionPhase.Performed)
        //    {
        //        YOTOFramework.eventMgr.TriggerEvent<Vector2>(YOTO.EventType.Touch, context.ReadValue<Vector2>());
        //    }
        //}

        public void OnTouchMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                YOTOFramework.eventMgr.TriggerEvent<Vector2>(YOTO.EventType.TouchMove, context.ReadValue<Vector2>());
               
            }

        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                YOTOFramework.eventMgr.TriggerEvent(EventType.Sprint);
            }
        }

        public void OnUseItem(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                YOTOFramework.eventMgr.TriggerEvent(EventType.UseItem);
            }
        }

        public void OnCooking(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                YOTOFramework.eventMgr.TriggerEvent(EventType.Cooking);
            }
        }
    }
}