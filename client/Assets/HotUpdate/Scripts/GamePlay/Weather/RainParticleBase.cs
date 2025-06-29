using System.Collections;
using UnityEngine;

public class RainParticleBase : MonoBehaviour
{
    private ParticleSystem ps;
    private float originalRate;
    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        var emission = ps.emission;
        originalRate=emission.rateOverTime.constant;
    }
    public void PlayRain()
    {
        if (ps == null) return;
        var emission = ps.emission;
        emission.rateOverTime = originalRate;
        gameObject.SetActive(true);
        ps.Play();
    }
    public void StopAndFadeOut()
    {
        if (ps == null) return;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        var emission = ps.emission;
        emission.rateOverTime = 0;

        while (ps.IsAlive(true))
        {
            yield return null;
        }

        gameObject.SetActive(false);
    }
}