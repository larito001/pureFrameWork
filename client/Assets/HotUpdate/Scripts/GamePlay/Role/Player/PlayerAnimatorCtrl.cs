using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using YOTO;

public class PlayerAnimatorCtrl : CtrlBase
{
    public const int GUN_LAYER = 1;
    public const int NONE_LAYER = 2;
    public const int MELEE_LAYER = 4;

    private Animator animator;
    LocalmotionState localmotionState = LocalmotionState.Idle;
    ArmState armState = ArmState.None;
    private float RotateSpeed = 2;
    private float reloadTimer = 0f;
    private float reloadDuration = 4f;
    public Vector3 playerMoveDir;
    private bool isRelongding = false;
    private float lerpDuration = 0.5f;
    private bool _isAiming = false;
    private float _runSpeed = 10f;
    private Vector3 _lookPos;
    private int _currentWeapon = NONE_LAYER;


    private enum LocalmotionState
    {
        Idle,
        Walk,
        Run
    }

    private enum ArmState
    {
        Gun,
        GunAim,
        Melee,
        MeleeAim,
        None,
    }
    public void SetLookPos(Vector3 lookPos)
    {
        _lookPos = lookPos;
    }
    public void SetAimingState(bool isAiming)
    {
        _isAiming = isAiming;
    }
    public void SetRunSpeed(float  speed)
    {
        _runSpeed = speed;
    }
    public void SetCurrentWeapon(int weapon)
    {
        _currentWeapon = weapon;
    }

    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }

    public void SetMoveDir(Vector3 dir)
    {
        playerMoveDir = dir;
    }
    private void SwitchPlayerState()
    {
        if (_isAiming)
        {
            if (_currentWeapon== GUN_LAYER)
            {
                armState = ArmState.GunAim;
                SetWeight(0, 1, 0);
            }
            else if (_currentWeapon == MELEE_LAYER)
            {
                armState = ArmState.MeleeAim;
                SetWeight(0, 0, 1);
            }
        }
        else if (_currentWeapon == GUN_LAYER)
        {
            armState = ArmState.Gun;
            SetWeight(0, 1, 0);
        }
        else if (_currentWeapon== MELEE_LAYER)
        {
            armState = ArmState.Melee;
            SetWeight(0, 0, 1);
        }
        else
        {
            armState = ArmState.None;
            SetWeight(1, 0, 0);
        }

        if (playerMoveDir.magnitude == 0)
        {
            localmotionState = LocalmotionState.Idle;
        }
        else
        {
            localmotionState = LocalmotionState.Run;
        }
    }

    private void SetWeight(float noneLayer, float gunLayer, float meleeLayer)
    {
        animator.SetLayerWeight(NONE_LAYER, noneLayer);
        animator.SetLayerWeight(GUN_LAYER, gunLayer);
        animator.SetLayerWeight(MELEE_LAYER, meleeLayer);
    }

    public void TryShoot()
    {
        animator.SetTrigger("ShootingTrigger");
    }

    public void TryUseMelee()
    {
        animator.SetTrigger("Melee");
    }

    private void SetAnimator(float deltaTime)
    {
        switch (localmotionState)
        {
            case LocalmotionState.Idle:
                animator.SetFloat("MoveSpeed", 0, 0.1f, deltaTime);
                animator.SetBool("isMoving", false);
                break;
            case LocalmotionState.Walk:
                animator.SetBool("isMoving", true);
                animator.SetFloat("MoveSpeed", Mathf.Min(playerMoveDir.z * _runSpeed, 2),
                    0.1f, deltaTime);
                break;
            case LocalmotionState.Run:
                animator.SetBool("isMoving", true);
                animator.SetFloat("MoveSpeed", Mathf.Min(playerMoveDir.z * _runSpeed, 2),
                    0.1f, deltaTime);
                break;
        }
        
        //手的动作
        if (armState == ArmState.GunAim)
        {
            AimState(deltaTime);
        }
        else if (armState == ArmState.Gun)
        {
            NormalState(deltaTime);
        }
        else if (armState == ArmState.MeleeAim)
        {
            AimState(deltaTime);
        }
        else if (armState == ArmState.Melee)
        {
            NormalState(deltaTime);
        }
        else if (armState == ArmState.None)
        {
            NormalState(deltaTime);
        }
    }

    private void AimState(float deltaTime)
    {
        animator.SetBool("isAming", true);
        animator.SetFloat("HorizontalSpeed", playerMoveDir.x * _runSpeed, 0.1f, deltaTime);
        if (playerMoveDir.magnitude == 0)
        {
            // 获取目标方向
            UnityEngine.Vector3 dir = _lookPos- transform.position;
            dir.y = 0f; // 保证只在水平面上旋转
            // 当前角色的正前方向
            UnityEngine.Vector3 forward =transform.forward;
            // 计算目标方向的旋转角度（弧度）
            float angleRad = Mathf.Atan2(dir.x, dir.z); // 正值表示右转，负值表示左转
            // 获取角色当前朝向的角度
            float currentRad = Mathf.Atan2(forward.x, forward.z);
            // 计算当前朝向与目标朝向的差值（夹角）
            float deltaRad = Mathf.DeltaAngle(currentRad * Mathf.Rad2Deg, angleRad * Mathf.Rad2Deg) * Mathf.Deg2Rad;
            // 将方向差值传递给动画
            animator.SetFloat("RotateSpeed", deltaRad * 5f, 0.2f, deltaTime);
            // 实际旋转角色（你也可以用 Quaternion.Slerp 替代以获得更柔和效果）
            transform.Rotate(0, deltaRad * Mathf.Rad2Deg * deltaTime * RotateSpeed, 0f);
        }

        if (_lookPos != UnityEngine.Vector3.zero)
        {
            UnityEngine.Vector3 dir = _lookPos - transform.position;
            dir.y = 0f;
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.Euler(0, angle, 0),
                deltaTime * RotateSpeed); 
        }
    }

    private void NormalState(float deltaTime)
    {
        animator.SetBool("isAming", false);
        float rad = Mathf.Atan2(playerMoveDir.x, playerMoveDir.z);
        animator.SetFloat("RotateSpeed", rad, 0.2f, deltaTime);
        transform.Rotate(0, rad * 360 * deltaTime, 0f);
    }
    
    public void ReLoad(UnityAction callback)
    {
        if (isRelongding) return;
        isRelongding = true;
        animator.SetBool("isReloding", true);
        YOTOFramework.timeMgr.DelayCall(() =>
        {
            animator.SetBool("isReloding", false);
            isRelongding = false;
        }, 4f);
        YOTOFramework.timeMgr.DelayCall(() => { callback?.Invoke(); }, 4.5f);
    }

    private void OnAnimatorMove()
    {
        characterBase.animationVelocity = animator.velocity;
        characterBase.animationRotate = animator.rootRotation;
    }

    public override void YOTOUpdate(float deltaTime)
    {
        SwitchPlayerState();
        SetAnimator(deltaTime);
    }

    public void UseGun()
    {
        if (_currentWeapon != GUN_LAYER)
            animator.Play("EquipRifle", GUN_LAYER, 0);
    }

    public void UseMelee()
    {
        if (_currentWeapon != MELEE_LAYER)
        {
            animator.Play("EquipMelee", MELEE_LAYER, 0);
        }
    }

    public override void YOTONetUpdate()
    {
    }
}