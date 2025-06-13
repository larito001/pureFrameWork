using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using YOTO;
using EventType = YOTO.EventType;

public class PlayerInteractionCtrl : CtrlBase
{
    public float distance = 10f; // 射线的检测距离
    public LayerMask interactionLayer; // 可交互的层级

    private Ray ray; // 定义射线
    private RaycastHit hitInfo; // 射线检测命中的信息
    private Collider lastHitCollider = null; // 上一帧命中的物体
    
    public override void Init(PlayerEntity character)
    {
        base.Init(character);
        interactionLayer = LayerMask.GetMask("Tools");
        
        
     
    }
    private void CookingAction()
    {


    }
    private void  UseItemAction()
    {
        Debug.Log("点击了U");
    }
    public override void YOTOUpdate(float deltaTime)
    {
        // 创建射线，从角色位置发射到前方
        ray = new Ray(transform.position + new Vector3(0, 1, 0), transform.forward);

        // 调用射线检测
        DetectObjects();
    }

    public override void YOTONetUpdate()
    {
        
    }

    private void DetectObjects()
    {
        // 使用 Physics.Raycast 进行射线检测 
        if (Physics.Raycast(ray, out hitInfo, distance, interactionLayer))
        {
            // 当前帧命中的物体
            Collider currentHitCollider = hitInfo.collider;
            
            // Debug.Log($"射线检测到物体: {currentHitCollider.gameObject.name}");

            // 如果是新进入的物体
            if (lastHitCollider != currentHitCollider)
            {
                if (lastHitCollider != null)
                {
                    Debug.Log($"物体切换: 从 {lastHitCollider.gameObject.name} 到 {currentHitCollider.gameObject.name}");
                    OnObjectExit(lastHitCollider);
                }
                OnObjectEnter(currentHitCollider);
            }

            // 更新上一帧命中的物体
            lastHitCollider = currentHitCollider;
        }
        else
        {
            // 如果当前帧没有命中任何物体，检查是否有退出的物体
            if (lastHitCollider != null)
            {
                Debug.Log($"射线没有检测到物体，上一个物体 {lastHitCollider.gameObject.name} 退出");
                OnObjectExit(lastHitCollider);
                lastHitCollider = null;
            }
        }
    }

    private void OnObjectEnter(Collider collider)
    {
      
            Debug.Log($"物体进入射线检测: {collider.gameObject.name}");
            // 在这里处理进入区域的逻辑

            // // ResourceBase info = collider.GetComponent<ResourceBase>();
            //
            // int id = info.GetID();
            // ResourceEntity res = BuildSystem.Instance.GetEntityByID(id);
            // if (res!=null)
            // {
            //     res.OnObjectEnter(characterBase);
            //     currentHitCollider = res;
            // }
            // else
            // {
            //     Debug.LogError("错误，没找到对象");
            //  } 
        
    }

    private void OnObjectExit(Collider collider)
    {
     
            // Debug.Log($"物体退出射线检测: {collider.gameObject.name}");
            // ResourceBase info = collider.GetComponent<ResourceBase>();
            //
            // int id = info.GetID();
            // ResourceEntity res = BuildSystem.Instance.GetEntityByID(id);
            //
            // res.OnObjectExit(characterBase);
            //
            // currentHitCollider = null; 
        

    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        // 在场景中动态可视化射线
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distance);
    }
}
