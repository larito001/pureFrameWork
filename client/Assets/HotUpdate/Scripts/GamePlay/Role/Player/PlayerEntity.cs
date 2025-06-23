using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;
using YOTO;

public class PlayerEntity : CharacterBase
{
    
    public float HP;

    public float ATK;
    public float DEF;
    public float SPEED;
    public float CRT;//暴击率
    public float CDMG;//暴击伤害
    
    public PlayerAnimatorCtrl animatorCtrl;
    public PlayerMoveCtrl moveCtrl;
    public PlayerInteractionCtrl interactionCtrl;
    public Camera camera;
    public Vector2 moveInput;
    private Vector3 target;
    public bool canMove = true;
    public Vector3 orgPosition;
    
    public bool isAming = true;
    public bool isShooting = false;
    
    public Vector3 mousePoint;
    public Vector3 lookPos;
    private bool isTouching = false;
    private bool isFireing = false;
    private float waitTime = 0.5f;
    private float currentTime = 0;
    private Transform headTarget;
    private bool isInit = false;
    private float headRotationSpeed = 10;

    
    private HandRoot handPos;
    public RigBuilder builder;

    public void Hurt(float hurt)
    {
        FlyTextMgr.Instance.AddText(hurt.ToString(),character.transform.position,FlyTextType.PlayerHurt);
    }
    public GunEntity gun;
    public MeleeEntity melee;
    //todo:Gunparent
    // private NavTarget navTarget;
    public PlayerEntity() : base()
    {
        

        // navTarget=NavMapManager.Instance.GetTarget(false,new Vector3(100,0,10));
      //求Vector3.up和camera.transform.forward的夹角，然后根据三维的勾股定理，给出Height。height为直角边A，且为计算出的夹角的临边，已知夹角，已知直角三角形。知道对角的顶点位置lookPos，求另一个对角LookPos2
    }

    public override void CulculateDir()
    {
        
        Vector3 cameraForwordProjection =
            new Vector3(camera.transform.forward.x, 0, camera.transform.forward.z).normalized;
        if (!canMove)
        {
            playerMovement = Vector3.zero;
            return;
        }

        // if (isAming)
        // {
        //     playerMovement = new Vector3(moveInput.x, 0, moveInput.y);
        // }
        // else
        // {
        //  
        // }
        playerMovement = cameraForwordProjection * moveInput.y + camera.transform.right * moveInput.x;

        playerMovement = character.transform.InverseTransformVector(playerMovement);
    }

    public override void Init(Vector3 pos)
    {
        YOTOFramework.eventMgr.AddEventListener<int>(YOTO.EventType.KeyBoardNumClick,SwitchWeapon);
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.TryReload, () =>
        {
            if (!isInit) return;
            if (gun != null)
            {
                gun.Reload();   
            }

            animatorCtrl.ReLoad(() =>
            {
                if (gun != null)
                    gun.ReloadEnd();
            });
        });
        YOTOFramework.eventMgr.AddEventListener<Vector3>(YOTO.EventType.RefreshMousePos, (pos) =>
        {
            if (!isInit) return;
            mousePoint = pos;
            Vector3 cameraPos = camera.transform.position;
            Vector3 dir = mousePoint - cameraPos;
            Vector3 dirNormalized = dir.normalized;
            float angleRad = Vector3.Angle(dir, Vector3.up) * Mathf.Deg2Rad;
            float height = 1.5f;
            float len = height / Mathf.Cos(angleRad);
            lookPos = mousePoint + dirNormalized * len;
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.Fire, () =>
        { if (!isInit) return;
            isAming = true;
            isFireing = true;
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.FireRelease, () =>
        {
            if (!isInit) return;
            if (!isTouching)
            {
                isAming = false;
            }

            isFireing = false;
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.TouchPress, () =>
        {
            if (!isInit) return;
            isTouching = true;
            isAming = isTouching;
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.TouchRelease, () =>
        {
            if (!isInit) return;
            mousePoint = Vector3.zero;
            isTouching = false;
            isAming = isTouching;
        });
        orgPosition = pos;
        Debug.Log("InitPlayer");
        isRunning = true;
        YOTOFramework.eventMgr.AddEventListener<Vector2>(YOTO.EventType.Move, (ve) =>
        {
            if (!isInit) return;
            moveInput = ve;
        });

        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.Sprint, () =>
        {
            if (!isInit) return;
            isSpinting = true;
        });

        //*****************************************
        YOTOFramework.resMgr.LoadGameObject("Assets/PolygonApocalypse/Prefabs/Characters/SM_Chr_Teen_Male_01.prefab",
            Vector3.zero, Quaternion.identity, (obj, pos, rot) =>
            {
                character = UnityEngine.Object.Instantiate(obj);
                character.SetActive(true);
                character.transform.position = orgPosition;
                camera = YOTOFramework.cameraMgr.getMainCamera();
                AddComponent();
                handPos = character.GetComponentInChildren<HandRoot>();
                builder = character.GetComponent<RigBuilder>();
                headTarget = character.gameObject.transform.Find("HeadTarget");

         
                isInit = true;
                SwitchWeapon(1);
            });
    }

    private void SwitchWeapon(int index)
    {
        if (index == 1)
        {
            if (gun == null)
            {
                gun = new GunEntity(handPos); 
                gun.Init(this);
            }
            else
            {
                gun.UseWeapon();
            }

            if (melee != null)
            {
                melee.UnuseWeapon();
            }

            animatorCtrl.currentWeapon = PlayerAnimatorCtrl.GUN_LAYER; 
        }
        else if (index == 2)
        {
            if (melee == null)
            {
                melee = new MeleeEntity(handPos);
                melee.Init(this);
            }
            else
            {
                melee.UseWeapon();
            }

            if (gun != null)
            {
                gun.UnuseWeapon();
            }

            animatorCtrl.currentWeapon = PlayerAnimatorCtrl.MELEE_LAYER;
        }
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

       var mCamera= YOTOFramework.cameraMgr.getVirtualCamera("MainCameraVirtual");
       mCamera.m_Lens.FieldOfView = 60;
       mCamera.m_Follow=character.transform;
       mCamera.m_LookAt = character.transform;
      var body=  mCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
      body.m_TrackedObjectOffset= new Vector3(0, 2, 0);
      body.m_DeadZoneWidth = 0.1f;
      body.m_DeadZoneHeight = 0.1f;
      body.m_CameraDistance = 10;
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

    private float maxHeadYaw = 60f; // 最大左右旋转角度，单位是度

    public override void YOTOUpdate(float deltaTime)
    {
        if (!isInit) return;
        if (isAming)
        {
            Vector3 direction = lookPos - headTarget.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.001f)
            {
                // // 计算目标方向相对于角色前方的角度差
                // Vector3 characterForward = character.transform.forward;
                // characterForward.y = 0f;
                // characterForward.Normalize();
                //
                // Vector3 targetDirection = direction.normalized;
                // float angleToTarget = Vector3.SignedAngle(characterForward, targetDirection, Vector3.up);
                //
                // // 限制在 [-maxHeadYaw, maxHeadYaw] 之间
                // float clampedAngle = Mathf.Clamp(angleToTarget, -maxHeadYaw, maxHeadYaw);
                //
                // // 计算目标旋转（绕Y轴旋转）
                // Quaternion targetRotation = Quaternion.Euler(0f, character.transform.eulerAngles.y + clampedAngle, 0f);
                //
                // // 插值旋转，让头部平滑转向目标方向（受限）
                // headTarget.rotation = Quaternion.Slerp(
                //     headTarget.rotation,
                //     targetRotation,
                //     Time.deltaTime * headRotationSpeed
                // );
            }
        }
        else
        {
            // 没在瞄准时，默认看向正前方（归位）
            headTarget.rotation = Quaternion.Slerp(
                headTarget.rotation,
                Quaternion.Euler(0f, character.transform.eulerAngles.y, 0f),
                Time.deltaTime * headRotationSpeed
            );
        }


        if (isAming)
        {
            if (currentTime <= waitTime)
            {
                currentTime += deltaTime;
            }
        }
        else
        {
            currentTime = 0;
        }

        isShooting = false;
        if (isFireing)
        {
            if (currentTime >= waitTime)
            {
                //todo:开火，子弹间隔，帮我写，在发射子弹的地方写好todo我创建子弹就好了
                isShooting = true;
                if (gun != null)
                gun.TryShot();
                if (melee != null)
                {
                    melee.TryShot();
                }
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
        // if (character != null&&navTarget!=null)
        // {
        //     navTarget.SetPos(character.transform.position);
        // }
    
    }

    public override void YOTOOnHide()
    {
    }



    
}