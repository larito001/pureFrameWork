
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager> {
    private PlayerEntity player;
    
    public void Init(Transform org)
    {
        player = new PlayerEntity();
        player.Init(org.position);
   
    }

    public Vector3 GetPlayerPos()
    {
        return player.GetPlayerPos();
    }
    public override void Unload()
    {
        base.Unload();
        player.Free();
        player = null;
    }
}
