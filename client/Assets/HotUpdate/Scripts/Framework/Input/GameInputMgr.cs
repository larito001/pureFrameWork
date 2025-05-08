using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
namespace YOTO{
    public class GameInputMgr 
    {
        public MInput input;
        private GamePlayInputStrategy gamePlayInputStrategy;
        private BuildStrategy buildStrategy;
        
        public GamePlayInputStrategy GamePlayInput
        {
            get{
            return gamePlayInputStrategy;
            }
        }
        public BuildStrategy BuildInput
        {
            get
            {
                return buildStrategy;
            }
        }
        public void Init()
        {

            //模拟触摸
          
            input =new MInput();
            gamePlayInputStrategy=new GamePlayInputStrategy();
            buildStrategy=new BuildStrategy();
            input.GamePlayAction.SetCallbacks(gamePlayInputStrategy);
            //EnableBuildInput();
            EnableGamePlayInput();

        }
        public void EnableGamePlayInput()
        {
            input.GamePlayAction.Enable();
   
        }
        public void DisableGamePlayInput()
        {
            input.GamePlayAction.Disable();
        }
  
    
    }   

}
