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
    public float HP = 100;
    float maxHP=100;
    public float ATK;
    public float DEF;
    public float SPEED;
    public float CRT; //暴击率
    public float CDMG; //暴击伤害

    private PlayerAnimatorCtrl animatorCtrl;
    public PlayerMoveCtrl moveCtrl;
    public PlayerInteractionCtrl interactionCtrl;
    public Camera camera;
    public Vector2 moveInput;
    private Vector3 target;
    public bool canMove = true;
    public Vector3 orgPosition;

    // public bool isAming = true;
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
    public RigBuilder builder;
    public bool isAimEnd = false;
    private int currentWeapon = PlayerAnimatorCtrl.NONE_LAYER;
    private HandRoot handPos;
    private Transform charCameraPos;
    public bool isWaiting { get; private set; }
    private Outline outline;

    public void Hurt(float hurt)
    {
     
        HP -= hurt;
        if (HP <= 0)
        {
            HP = 0;
        }
        
        float healthPercentage = HP / maxHP;

        // Interpolate between red (HP=0) and green (HP=maxHP)
        outline.OutlineColor = Color.Lerp(Color.red, Color.green, healthPercentage);

        YOTOFramework.sceneMgr.cameraCtrl.AddShake(0.1f);
        FlyTextMgr.Instance.AddText(hurt.ToString(), character.transform.position, FlyTextType.PlayerHurt);
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
            animatorCtrl.SetMoveDir(playerMovement);
            return;
        }
        
        playerMovement = cameraForwordProjection * moveInput.y + camera.transform.right * moveInput.x;

        playerMovement = character.transform.InverseTransformVector(playerMovement);
        animatorCtrl.SetMoveDir(playerMovement);
    }

    public override void Init(Vector3 pos)
    {
        YOTOFramework.eventMgr.AddEventListener<int>(YOTO.EventType.KeyBoardNumClick, SwitchWeapon);
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
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.Fire, () =>
        {
            if (!isInit) return;
            isAiming = true;
            isFireing = true;
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.FireRelease, FireRelease);
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.TouchPress, () =>
        {
            if (!isInit) return;
            isTouching = true;
            isAiming = isTouching;
        });
        YOTOFramework.eventMgr.AddEventListener(YOTO.EventType.TouchRelease, TouchReless);
        orgPosition = pos;
        Debug.Log("InitPlayer");
        YOTOFramework.eventMgr.AddEventListener<Vector2>(YOTO.EventType.Move, (ve) =>
        {
            if (!isInit) return;
            moveInput = ve;
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
                builder = character.GetComponent<RigBuilder>();
                handPos = character.GetComponentInChildren<HandRoot>();
                headTarget = character.gameObject.transform.Find("HeadTarget");
                builder.Build();
                outline = character.GetComponent<Outline>();
                isInit = true;
                SwitchWeapon(1);
            });
    }

    private void FireRelease()
    {
        if (!isInit) return;
        if (!isTouching)
        {
            isAiming = false;
        }

        isFireing = false;
    }

    private bool isAiming = false;
    private void TouchReless()
    {
        if (!isInit) return;
        mousePoint = Vector3.zero;
        isTouching = false;
        isAiming = isTouching;
    }

    private void SwitchWeapon(int index)
    {
        if (isWaiting) return;

        isWaiting = true;
        YOTOFramework.timeMgr.DelayCall(() => { isWaiting = false; }, 0.7f);
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
                animatorCtrl.UseGun();
            }

            if (melee != null)
            {
                melee.UnuseWeapon();
            }

            currentWeapon = PlayerAnimatorCtrl.GUN_LAYER;
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
                animatorCtrl.UseMelee();
            }

            if (gun != null)
            {
                gun.UnuseWeapon();
            }

            currentWeapon = PlayerAnimatorCtrl.MELEE_LAYER;
        }
    }

    public override void AddComponent()
    {
        Debug.Log("AddComponent");
        animatorCtrl = character.AddComponent<PlayerAnimatorCtrl>();
        moveCtrl = character.AddComponent<PlayerMoveCtrl>();
        interactionCtrl = character.AddComponent<PlayerInteractionCtrl>();


        animatorCtrl.Init(this);
        animatorCtrl.SetAnimator(character.GetComponent<Animator>());
        moveCtrl.Init(this);
        moveCtrl.SetRig(character.GetComponent<Rigidbody>());
        interactionCtrl.Init(this);
        Debug.Log("AddComponent Finish");

        var mCamera = YOTOFramework.cameraMgr.getVirtualCamera("MainCameraVirtual");
        mCamera.m_Lens.FieldOfView = 60;
        charCameraPos = GameObject.Find("CharCameraPos").transform;

        mCamera.m_Follow = charCameraPos;
        mCamera.m_LookAt = charCameraPos;
        var body = mCamera.AddCinemachineComponent<CinemachineFramingTransposer>();
        body.m_TrackedObjectOffset = new Vector3(0, 2, 0);
        body.m_DeadZoneWidth = 0f;
        body.m_DeadZoneHeight = 0f;
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
        CulculateDir();
        animatorCtrl.SetAimingState(isAiming);
        animatorCtrl.SetCurrentWeapon(currentWeapon);
        moveCtrl.SetVolocity(this.animationVelocity);
        moveCtrl.SetRotation(this.animationRotate);
        if (!isInit) return;
        headTarget.rotation = Quaternion.Slerp(
            headTarget.rotation,
            Quaternion.Euler(0f, character.transform.eulerAngles.y, 0f),
            Time.deltaTime * headRotationSpeed
        );
        if (isAiming)
        {
            Vector3 cameraPos = camera.transform.position;
            Vector3 dir = mousePoint - cameraPos;
            Vector3 dirNormalized = dir.normalized;
            float angleRad = Vector3.Angle(dir, Vector3.up) * Mathf.Deg2Rad;
            float height = 1.5f;
            float len = height / Mathf.Cos(angleRad);
            lookPos = mousePoint + dirNormalized * len;
            animatorCtrl.SetLookPos(lookPos);
            //todo:偏移镜头charCameraPos（Transform），移动到  character.transform.position+character.transform.forward *3的圆形范围内
            if (currentWeapon == PlayerAnimatorCtrl.GUN_LAYER)
            {
                charCameraPos.position = Vector3.Lerp(charCameraPos.position,
                    character.transform.position + character.transform.forward * 3, Time.deltaTime * 10);
            }
            else
            {
                charCameraPos.position = Vector3.Lerp(charCameraPos.position, character.transform.position,
                    Time.deltaTime * 10);
            }


            if (currentTime <= waitTime)
            {
                currentTime += deltaTime;
                isAimEnd = false;
            }

            if (currentTime >= waitTime)
            {
                isAimEnd = true;
            }
        }
        else
        {
            charCameraPos.position =
                Vector3.Lerp(charCameraPos.position, character.transform.position, Time.deltaTime * 10);
            currentTime = 0;
            isAimEnd = false;
        }


        isShooting = false;

        if (isFireing && !isWaiting)
        {
            if (currentTime >= waitTime)
            {
                //todo:开火，子弹间隔，帮我写，在发射子弹的地方写好todo我创建子弹就好了
                isShooting = true;
                if (gun != null)
                    gun.TryShot(() =>
                    {
                        animatorCtrl.TryShoot();
                    });
                if (melee != null)
                {
                    melee.TryShot(() =>
                    {
                        animatorCtrl.TryUseMelee();
                    });
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