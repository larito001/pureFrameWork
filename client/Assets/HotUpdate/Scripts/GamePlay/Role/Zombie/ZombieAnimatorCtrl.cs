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


    public void EnemyDie()
    {
        if (state == ZombieState.Die) return;
    
        state = ZombieState.Die;
        BodyMeshAnimator.Play("Die", 0);
    }

    public void EnemyAtk()
    {
        YOTOFramework.timeMgr.DelayCall(() =>
        {
            var bullet = NormalZombieBullet.pool.GetItem(this.transform);
            bullet.Location = transform.position;
            bullet.InstanceGObj();
            bullet.FireFromTo(bullet.Location, transform.forward);
        },1.2f);
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