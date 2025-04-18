
using System.Collections.Generic;
using Proto;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using YOTO;
using EventType = YOTO.EventType;

//控制游戏模式（场景）
public class GameRoot : SingletonMono<GameRoot>
{

    public void Init()
    {
        FlyTextMgr.Instance.Init();
        TouchSimulation.Enable();
        YOTOFramework.sceneMgr.LoadScene(Scenes.Start);
        Debug.Log("GameRoot 加载完成");
    }

    public void StartGame()
    {
        Debug.Log("GameRoot StartGame");

    }
    public void Gaming()
    {
        Debug.Log("GameRoot Gaming");
    }

}
