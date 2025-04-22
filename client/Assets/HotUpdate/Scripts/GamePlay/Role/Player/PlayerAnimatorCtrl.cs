using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using YOTO;
using Vector3 = System.Numerics.Vector3;

public class PlayerAnimatorCtrl : CtrlBase
{
    public Animator animator;
    LocalmotionState localmotionState = LocalmotionState.Idle;
    ArmState armState = ArmState.Normal;
    PlayerPose playerPose = PlayerPose.Stand;
    public float RotateSpeed=10;
    public TwoBoneIKConstraint rightHand;
    public TwoBoneIKConstraint leftHand;
    private float reloadTimer = 0f;
    private float reloadDuration = 4f;
    public enum PlayerPose
    {
        Crouch,
        Stand,
        MidAir
    }
    public enum LocalmotionState
    {
        Idle,
        Walk,
        Run
    }
    public enum ArmState
    {
        Normal,
        Aim
    }

  
    public override void Init(PlayerEntity character)
    {
        base.Init(character);
        characterBase.character.TryGetComponent<Animator>(out animator);
   
        if (!animator)
        {
            animator = characterBase.character.AddComponent<Animator>();
        }

        var Rig = characterBase.character.transform.Find("Rigs");
        rightHand = Rig.Find("RightHand").GetComponent<TwoBoneIKConstraint>();
        leftHand = Rig.Find("LeftHand").GetComponent<TwoBoneIKConstraint>();
    }
    public void SwitchPlayerState()
    {
        if (characterBase.isAming)
        {
            armState = ArmState.Aim;
        }
        else
        {
            armState = ArmState.Normal;
        }
        
        if (characterBase.playerMovement.magnitude == 0)
        {
            localmotionState = LocalmotionState.Idle;
            //Debug.Log("idel��" + characterBase.playerMovement+":"+ characterBase.playerMovement.magnitude);
        }
        else if (!characterBase.isRunning)
        {
            localmotionState = LocalmotionState.Walk;
            //Debug.Log("walk��" + characterBase.playerMovement);
        }
        else
        {
            localmotionState = LocalmotionState.Run;
            //Debug.Log("run��" + characterBase.playerMovement);
        }
        
    }
    void SetAnimator(float deltaTime)
    {
       
        
        if (playerPose == PlayerPose.Stand)
        {
            // animator.SetFloat("PlayerState", 0, 0.1f, deltaTime);
            switch (localmotionState)
            {
                case LocalmotionState.Idle:
                    animator.SetFloat("MoveSpeed", 0, 0.1f, deltaTime);
                    break;
                case LocalmotionState.Walk:
                        animator.SetFloat("MoveSpeed", characterBase.playerMovement.z * characterBase.walkSpeed, 0.5f, deltaTime);
                    break;
                case LocalmotionState.Run:
               
                    animator.SetFloat("MoveSpeed", characterBase.playerMovement.z * characterBase.runSpeed, 0.5f, deltaTime);
                    break;
            }
            
        }
        
        if (armState == ArmState.Aim)
        {
            animator.SetBool("isAming", true);
            animator.SetFloat("HorizontalSpeed", characterBase.playerMovement.x * characterBase.runSpeed, 0.5f, deltaTime);
    
            if (characterBase.lookPos != UnityEngine.Vector3.zero)
            {
                // Calculate the direction vector to rotate towards
                UnityEngine.Vector3 dir = characterBase.lookPos - characterBase.character.transform.position;
                dir.y = 0f;  // Optional: keeps the rotation on the y-axis (horizontal only)
        
                // Get the rotation angle
                float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        
                // Rotate the character towards the target direction
                characterBase.character.transform.rotation = Quaternion.Slerp(characterBase.character.transform.rotation, Quaternion.Euler(0, angle, 0), deltaTime*RotateSpeed); // You can adjust the `10f` to control the rotation speed
            }
        }
        else if (armState == ArmState.Normal)
        {
            animator.SetBool("isAming", false);
            float rad = Mathf.Atan2(characterBase.playerMovement.x, characterBase.playerMovement.z);
            animator.SetFloat("RotateSpeed", rad, 0.1f, deltaTime);
            characterBase.character.transform.Rotate(0, rad * 360 * deltaTime, 0f);
        }

    }

    private bool isRelongding = false;
    private bool isLerpingWeight = false;
    private float lerpDuration = 0.5f;
    private float lerpTimer = 0f;
    private float startWeight = 0f;
    private float targetWeight = 1f;

    public void ReLoad(UnityAction callback)
    {
        if (isRelongding) return;
        lerpTimer = 0f;
        startWeight = 1f;
        targetWeight = 0f;
        isLerpingWeight = true;
        isRelongding = true;
        animator.SetBool("isReloding",true);

        // 触发 DelayCall（执行动画时长）
        YOTOFramework.timeMgr.DelayCall(() =>
        {
            animator.SetBool("isReloding",false);
            callback?.Invoke();

            // 启动权重插值（从 0 → 1）
            lerpTimer = 0f;
            startWeight = 0f;
            targetWeight = 1f;
            isLerpingWeight = true;
            isRelongding = false;
        }, 4);
    }

    private void OnAnimatorMove()
    {
        //����״̬
        characterBase.animationVelocity = animator.velocity;
        characterBase.animationRotate = animator.rootRotation;
    }

    public override void YOTOUpdate(float deltaTime)
    {
        characterBase.CulculateDir();
        SwitchPlayerState();
        SetAnimator(deltaTime);
        if (isLerpingWeight)
        {
            lerpTimer += Time.deltaTime;
            float t = Mathf.Clamp01(lerpTimer / lerpDuration);
            float weight = Mathf.Lerp(startWeight, targetWeight, t);
            rightHand.weight = weight;
            leftHand.weight = weight;

            if (t >= 1f)
            {
                rightHand.weight = targetWeight;
                leftHand.weight = targetWeight;
                isLerpingWeight = false;

            }
        }
    }

    public override void YOTONetUpdate()
    {
        
    }
}
