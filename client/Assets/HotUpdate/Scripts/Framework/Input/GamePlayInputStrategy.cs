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
                YOTOFramework.Instance.eventMgr.TriggerEvent<float>(YOTO.EventType.Scroll, context.ReadValue<Vector2>().y);
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
                YOTOFramework.Instance.eventMgr.TriggerEvent<Vector2>(EventType.Move, context.ReadValue<Vector2>()); 
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                YOTOFramework.Instance.eventMgr.TriggerEvent<Vector2>(EventType.Move, context.ReadValue<Vector2>());
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
                YOTOFramework.Instance.eventMgr.TriggerEvent<Vector2>(YOTO.EventType.TouchMove, context.ReadValue<Vector2>());
               
            }

        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                YOTOFramework.Instance.eventMgr.TriggerEvent(EventType.Sprint);
            }
        }

        public void OnUseItem(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                YOTOFramework.Instance.eventMgr.TriggerEvent(EventType.UseItem);
            }
        }

        public void OnCooking(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                YOTOFramework.Instance.eventMgr.TriggerEvent(EventType.Cooking);
            }
        }
    }
}