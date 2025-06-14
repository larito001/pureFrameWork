
using UnityEngine;

public enum ZombieState
{
    Die,
    Atk,
    Run,
    Idel
}
public class ZombieAnimatorCtrl :PoolBaseGameObject
{

    [SerializeField]
    AnimatedMeshAnimator BodyMeshAnimator;

    public ZombieState state = ZombieState.Idel;
    // private readonly static string[] RandomAnimationNames = new string[]
    // {
    //     "Die",
    //     "Atk",
    //     "Run",
    //     "Idel",
    // };
    // private void Awake()
    // {
    //     var randomColor = new Color(Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f), 1);
    //
    //     var offsetSeconds = Random.Range(0.0f, 3.0f);
    //     var randomIndex = Random.Range(0, RandomAnimationNames.Length);
    //     var randomAnimationNames = RandomAnimationNames[randomIndex];
    //
    //     BodyMeshAnimator.Play("Run", offsetSeconds);
    // }

    public void EnemyDie()
    {
        if (state == ZombieState.Die) return;
        state = ZombieState.Die;
        BodyMeshAnimator.Play("Die",0);
    }
    public void EnemyAtk()
    {
        if (state == ZombieState.Atk) return;
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

    public override void ResetAll()
    {
        
    }

    public override void OnStart()
    {
    }
}
