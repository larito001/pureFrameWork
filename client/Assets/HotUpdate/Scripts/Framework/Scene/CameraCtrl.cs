using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YOTO;

public class CameraCtrl
{
    CinemachineVirtualCamera vCamera;
    Vector3 moveDirection;
    Vector3 currentVelocity; // ��¼��ǰ������ٶ�
    float moveSpeed = 3f; // �϶�ʱ���ƶ��ٶ�
    float drag = 0.95f; // ��ק����ٶ�˥��ϵ��
    bool isDragging = false; // ����Ƿ������϶�
    GraphicRaycaster graphicRaycaster;
    PointerEventData pointerEventData;
    public CameraCtrl()
    {
        vCamera = YOTOFramework.Instance.cameraMgr.getVirtualCamera("MainCameraVirtual");
        GameObject cameraDir = GameObject.Find("CameraDir");
        vCamera.transform.position = cameraDir.transform.position;
        vCamera.transform.rotation = cameraDir.transform.rotation;
        vCamera.m_Lens.FieldOfView = 30;
        YOTOFramework.Instance.eventMgr.AddEventListener<Vector2>(YOTO.EventType.Touch, Touch);
        //YOTOFramework.eventMgr.AddEventListener<Vector2>(YOTO.EventType.TouchMove, CameraMove);
        YOTOFramework.Instance.eventMgr.AddEventListener<float>(YOTO.EventType.Scroll, CameraSclae);
        graphicRaycaster = YOTOFramework.Instance.uIMgr.UIRoot.GetComponent<GraphicRaycaster>();
        pointerEventData = new PointerEventData(EventSystem.current);
    }

    private void CameraMove(Vector2 dir)
    {
        // ��ʼ�϶��������϶����
        isDragging = true;

        // ��ȡ�������ǰ���򣨺���y�ᣬ��Ϊ��ˮƽ�ƶ���
        Vector3 forward = new Vector3(vCamera.transform.forward.x, 0, vCamera.transform.forward.z).normalized;
        // ��ȡ������ҷ��򣨺���y�ᣬ��Ϊ��ˮƽ�ƶ���
        Vector3 right = new Vector3(vCamera.transform.right.x, 0, vCamera.transform.right.z).normalized;

        // �����϶�������ǰ�ƶ�ʱ�������ǰ�����������ƶ�ʱ����������ҷ���
        moveDirection = -forward * dir.y - right * dir.x;

        // ���µ�ǰ�ٶ�
        currentVelocity = moveDirection * moveSpeed;

        // �����ƶ����
        vCamera.transform.position += currentVelocity * Time.deltaTime;
    }

    private void Touch(Vector2 pos)
    {
 
        Vector3 dir=new Vector3(pos.x,pos.y, vCamera.m_Lens.NearClipPlane);
        //Debug.Log("TOUCH" + dir);
        Ray ray = YOTOFramework.Instance.cameraMgr.getMainCamera().ScreenPointToRay(dir);
        Ray uiRay = YOTOFramework.Instance.cameraMgr.getUICamera().ScreenPointToRay(dir);
        RaycastHit hitInfo;//

        pointerEventData.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();

        // ִ��UI���߼��
        graphicRaycaster.Raycast(pointerEventData, results);
        if (results.Count > 0)
        {
            //Debug.Log("TOUCH" + hitInfo.point);
            //BuildSystem.Instance.mapCtrl.SetPos(hitInfo.point);
            //BattleSystem.Instance.playerEntity?.MoveTarget(hitInfo.point);
            Debug.Log("�㵽UI��");
        }
        else
        {
            Debug.Log("���߼��");
            if (Physics.Raycast(ray, out hitInfo,1000 ,LayerMask.GetMask("Ground")))
            {
            
         
            }
        }
   
        //����һ�����ߣ��򵽵ذ���
    }

    private void CameraSclae(float sclale)
    {
        if (sclale >= 0)
        {
            vCamera.m_Lens.FieldOfView -= 1;
        }
        else
        {
            vCamera.m_Lens.FieldOfView += 1;
        }
    }
    // ���������Ҫÿ֡����
    public void Update(float dt)
    {
        // �����ǰû���϶�
        if (!isDragging)
        {
            // ��ק�ɿ��󣬼����ƶ����𽥼���
            if (currentVelocity.magnitude > 0.01f)
            {
                vCamera.transform.position += currentVelocity * dt;
                // �ٶ���˥��
                currentVelocity *= drag;
            }
        }
        else
        {
            // �϶������󣬽������Ϊfalse���������϶����ֲ���Ҫ���ƶ�
            isDragging = false;
        }
    }
}
