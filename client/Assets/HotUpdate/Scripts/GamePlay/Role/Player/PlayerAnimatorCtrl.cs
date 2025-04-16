using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorCtrl : CtrlBase
{
    public Animator animator;
    LocalmotionState localmotionState = LocalmotionState.Idle;
    ArmState armState = ArmState.Normal;
    PlayerPose playerPose = PlayerPose.Stand;
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
    }
    public void SwitchPlayerState()
    {
       
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
   
        
            armState = ArmState.Normal;
        
    }
    void SetAnimator(float deltaTime)
    {
       
        
        if (playerPose == PlayerPose.Stand)
        {
            animator.SetFloat("PlayerState", 0, 0.1f, deltaTime);
            switch (localmotionState)
            {
                case LocalmotionState.Idle:
                    animator.SetFloat("MoveSpeed", 0, 0.1f, deltaTime);
                    break;
                case LocalmotionState.Walk:
                        animator.SetFloat("MoveSpeed", characterBase.playerMovement.magnitude * characterBase.walkSpeed, 0.1f, deltaTime);
                    
                    break;
                case LocalmotionState.Run:
               
                    animator.SetFloat("MoveSpeed", characterBase.playerMovement.magnitude * characterBase.runSpeed, 0.1f, deltaTime);
                    break;
            }
        }
     

        if (armState == ArmState.Normal)
        {
            float rad = Mathf.Atan2(characterBase.playerMovement.x, characterBase.playerMovement.z);
            animator.SetFloat("RotateSpeed", rad, 0.1f, deltaTime);
            ////Debug.Log("rad:" + rad);
            characterBase.character.transform.Rotate(0, rad * 360 * deltaTime, 0f);
        }
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
    }

    public override void YOTONetUpdate()
    {
        
    }
}
