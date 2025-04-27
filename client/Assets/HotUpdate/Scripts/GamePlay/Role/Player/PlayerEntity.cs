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

    private GunEntity gun;
    //todo:Gunparent
    // private NavTarget navTarget;
    public PlayerEntity() : base()
    {
        // navTarget=NavMapManager.Instance.GetTarget(false,new Vector3(100,0,10));
    
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
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.TryReload, () =>
        {
            gun.canFire = false;
            animatorCtrl.ReLoad(() =>
            {
                gun.canFire = true;
            });
        });
        YOTOFramework.eventMgr.AddEventListener<Vector3>(YOTO.EventType.RefreshMousePos, (pos) =>
        {
            if (!isInit) return;
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
        YOTOFramework.eventMgr.AddEventListener<Vector2>(YOTO.EventType.Move, (ve) => { moveInput = ve; });

        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.Sprint, () => { isSpinting = true; });

        //*****************************************
        YOTOFramework.resMgr.LoadGameObject("Assets/PolygonApocalypse/Prefabs/Characters/SM_Chr_Teen_Male_01.prefab",
            Vector3.zero, Quaternion.identity, (obj, pos, rot) =>
            {
                character = UnityEngine.Object.Instantiate(obj);
                character.transform.position = orgPosition;
                camera = YOTOFramework.cameraMgr.getMainCamera();
                AddComponent();
                handPos = character.GetComponentInChildren<HandRoot>();
                builder = character.GetComponent<RigBuilder>();
                headTarget = character.gameObject.transform.Find("HeadTarget");
                isInit = true;
                gun = new GunEntity(animatorCtrl.leftHand, handPos);
                gun.Init(this);
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
                // 计算目标方向相对于角色前方的角度差
                Vector3 characterForward = character.transform.forward;
                characterForward.y = 0f;
                characterForward.Normalize();

                Vector3 targetDirection = direction.normalized;
                float angleToTarget = Vector3.SignedAngle(characterForward, targetDirection, Vector3.up);

                // 限制在 [-maxHeadYaw, maxHeadYaw] 之间
                float clampedAngle = Mathf.Clamp(angleToTarget, -maxHeadYaw, maxHeadYaw);

                // 计算目标旋转（绕Y轴旋转）
                Quaternion targetRotation = Quaternion.Euler(0f, character.transform.eulerAngles.y + clampedAngle, 0f);

                // 插值旋转，让头部平滑转向目标方向（受限）
                headTarget.rotation = Quaternion.Slerp(
                    headTarget.rotation,
                    targetRotation,
                    Time.deltaTime * headRotationSpeed
                );
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
                gun.TryShot();
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


    public override void SetPosition(Vector3 pos)
    {
    }

    public override void SetRotation(Quaternion rot)
    {
    }
}