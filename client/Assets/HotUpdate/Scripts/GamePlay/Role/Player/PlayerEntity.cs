using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using YOTO;

public class PlayerEntity : CharacterBase
{
    
    public PlayerAnimatorCtrl animatorCtrl;
    public PlayerMoveCtrl moveCtrl;
    public PlayerInteractionCtrl interactionCtrl;
    public Camera camera;
    public Vector2 moveInput;
    private Vector3 target; 
    public bool canMove = true;
    public Vector3 orgPosition;
    public bool isAming = false;
    public Vector3 lookPos;
    private bool isTouching = false;
    private bool isFireing = false;
    private float waitTime = 0.5f;
    private float currentTime = 0;
    //todo:Gunparent
    public PlayerEntity(): base()
    {
    }
    public override void  CulculateDir()
    {
  
        Vector3 cameraForwordProjection = new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z).normalized;
        if (!canMove)
        {
            playerMovement=Vector3.zero;
         return;   
        }

        if (isAming)
        {
            playerMovement = new Vector3(moveInput.x, 0, moveInput.y);  
        }
        else
        {
            playerMovement = cameraForwordProjection * moveInput.y + camera.transform.right * moveInput.x; 
        }
       
       
       playerMovement = character.transform.InverseTransformVector(playerMovement);


    }
    public override void Init(Vector3 pos)
    {
        YOTOFramework.eventMgr.AddEventListener<Vector3>(YOTO.EventType.RefreshMousePos, (pos) =>
        {
            lookPos = pos;
            lookPos.y = character.transform.position.y;
 
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.Fire, () =>
        {
            isAming = true;
            isFireing = true;
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.FireRelease, () =>
        {
            if (!isTouching)
            {
                isAming = false;
         
            }
            isFireing = false;
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.TouchPress, () =>
        {
            isTouching = true;
            isAming = isTouching;
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.TouchRelease, () =>
        {
            lookPos = Vector3.zero;
            isTouching = false;
            isAming = isTouching;
        });
        orgPosition = pos;
        Debug.Log("InitPlayer");
        isRunning = true;
        YOTOFramework.eventMgr.AddEventListener<Vector2>(YOTO.EventType.Move, (ve) =>
        {
            moveInput = ve;
        });
 
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.Sprint, () =>
        {
            isSpinting = true;
        });
     
        //*****************************************
        YOTOFramework.resMgr.LoadGameObject("Assets/PolygonApocalypse/Prefabs/Characters/SM_Chr_Teen_Male_01.prefab", Vector3.zero,Quaternion.identity, (obj,pos,rot) =>
            {
           
                character = UnityEngine.Object.Instantiate(obj);
                character.transform.position = orgPosition;
                camera = YOTOFramework.cameraMgr.getMainCamera();
                AddComponent();

            });
    } 
    public override void AddComponent()
    {
        Debug.Log("AddComponent");
        animatorCtrl = character.AddComponent<PlayerAnimatorCtrl>();
        moveCtrl = character.AddComponent<PlayerMoveCtrl>();
        interactionCtrl = character.AddComponent<PlayerInteractionCtrl>();

        
        animatorCtrl.Init(this);
        moveCtrl.Init(this);
        interactionCtrl.Init(this);
        Debug.Log("AddComponent Finish");
    }

    public override void DontMove()
    {
        canMove = false;
    }

    public override void CanMove()
    {
        canMove = true;
    }


    public override void Dispose()
    {
        GameObject.Destroy(character);
    }

 
    protected override void YOTOOnload()
    {

    }

    public override void YOTOStart()
    {

    }

    public override void YOTOUpdate(float deltaTime)
    {
        if (isAming)
        {
            if (currentTime<=waitTime)
            {
                currentTime += deltaTime;
            }   
        }
        else
        {
            currentTime = 0;
        }
        if (isFireing)
        {
            if (currentTime>=waitTime)
            {
                //todo:开火，子弹间隔，帮我写，在发射子弹的地方写好todo我创建子弹就好了
                Debug.Log("开火！！");
                //Gun.TryShot();
            }
        }
       
      
    
        
        
        if (animatorCtrl)
        {
            animatorCtrl.YOTOUpdate(deltaTime);
        }
        if (moveCtrl)
        {
            moveCtrl.YOTOUpdate(deltaTime);
        }
        if (interactionCtrl)
        {
            interactionCtrl.YOTOUpdate(deltaTime);
        }
        //CharacterBase ch = BattleSystem.Instance.getCharByName("enemy1");
        //if (ch != null && character)
        //{
        //    (ch as EnemyEntity).MoveTarget(character.transform.position);
        //}
    }

    public override void YOTONetUpdate()
    {
     
    }

    public override void YOTOFixedUpdate(float deltaTime)
    {
    }

    public override void YOTOOnHide()
    {
        
    }



 
    public override void SetPosition(Vector3 pos)
    {
    }

    public override void SetRotation(Quaternion rot)
    {
    
    }
}
