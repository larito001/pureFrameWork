using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXBase : PoolBaseGameObject
{
    ParticleSystem par;
    public override void ResetAll()
    {
     
    }
    public void PlayVFX()
    {
        if (par == null)
        {
            par = gameObject.GetComponent<ParticleSystem>();
        }
        par.Play();
      

    }
}
