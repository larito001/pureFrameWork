
using UnityEngine;

public class ZombieAnimatorCtrl :PoolBaseGameObject
{

    [SerializeField]
    AnimatedMeshAnimator BodyMeshAnimator;

    private readonly static string[] RandomAnimationNames = new string[]
    {
        "Die",
        "Atk",
        "Run",
        "Idel",
    };
    private void Awake()
    {
        var randomColor = new Color(Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f), Random.Range(0.00f, 1.00f), 1);

        var offsetSeconds = Random.Range(0.0f, 3.0f);
        var randomIndex = Random.Range(0, RandomAnimationNames.Length);
        var randomAnimationNames = RandomAnimationNames[randomIndex];

        BodyMeshAnimator.Play("Run", offsetSeconds);
    }

    public void EnemyDie()
    {
        BodyMeshAnimator.Play("Die",0);
    }
    public void EnemyAtk()
    {
        BodyMeshAnimator.Play("ATK", 0);
    }
    public void EnemyRun()
    {
        BodyMeshAnimator.Play("Run", 0);
    }
    public void EnemyIdel()
    {
        BodyMeshAnimator.Play("Idle", 0);
    }

    public override void ResetAll()
    {
        
    }

    public override void OnStart()
    {
    }
}
