using UnityEngine;
using YOTO;

public enum ZombieState
{
    Die,
    Atk,
    Run,
    Idel
}

public class ZombieAnimatorCtrl : MonoBehaviour
{
    [SerializeField] AnimatedMeshAnimator BodyMeshAnimator;

    public ZombieState state = ZombieState.Idel;
    private ZombieEntity zombieEntity;

    public void Init(ZombieEntity zombieEntity)
    {
        this.zombieEntity = zombieEntity;
    }

    public void EnemyDie()
    {
        if (state == ZombieState.Die) return;

        state = ZombieState.Die;
        BodyMeshAnimator.Play("Die", 0);
    }

    public void EnemyAtk()
    {
        zombieEntity.StopNav();
        YOTOFramework.timeMgr.DelayCall(() =>
        {
            if (!zombieEntity.isDie)
            {
                zombieEntity.GenerateZombieBullet();
       
            }
        }, 1.2f);
        YOTOFramework.timeMgr.DelayCall(() =>
        {
            if (!zombieEntity.isDie)
                zombieEntity.StartNav();
        }, 1.5f);

        state = ZombieState.Atk;
        // Debug.Log("Atk");
        BodyMeshAnimator.Play("Atk", 0);
    }

    public void EnemyRun()
    {
        if (state == ZombieState.Run) return;
        state = ZombieState.Run;

        // Debug.Log("Run");
        BodyMeshAnimator.Play("Run", 0);
    }

    public void EnemyIdel()
    {
        if (state == ZombieState.Idel) return;
        state = ZombieState.Idel;
        // Debug.Log("Idel");
        BodyMeshAnimator.Play("Idel", 0);
    }
}