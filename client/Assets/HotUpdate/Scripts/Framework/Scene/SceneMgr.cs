    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YOTO
{
    public enum Scenes
    {
        Start,
        Normal,
       
    }
    public class SceneMgr
    {

        CameraCtrl cameraCtrl;

        Dictionary<Scenes, VirtualSceneBase> scenesMap = new Dictionary<Scenes, VirtualSceneBase>() {
            { Scenes.Normal, new NormalScene()},
            { Scenes.Start, new StartScene()}
        };
        private VirtualSceneBase currentScene=null;
        public void Init()
        {
            cameraCtrl = new CameraCtrl();
        }
        public void LoadScene(Scenes scene)
        {
            if (currentScene!=null)
            {
                currentScene.UnLoad();
            }
            if (!scenesMap.ContainsKey(scene))
            {
                Debug.Log("YTLOG:�������ó���");
                return;
            }


            currentScene = scenesMap[scene];
            // YOTOFramework.uIMgr.ClearUI();
            scenesMap[scene].Onload();
            scenesMap[scene].OnAdd();
            scenesMap[scene].OnInit();
         

        }
        public void Update(float dt)
        {
            if (cameraCtrl!=null)
            {
                cameraCtrl.Update(UnityEngine.Time.deltaTime);
            }

            foreach (var virtualSceneBase in scenesMap)
            {
                virtualSceneBase.Value.Update(dt);
            }
        }
    }
}

