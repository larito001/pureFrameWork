using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YOTO
{
    public class YOTOFramework : SingletonMono<YOTOFramework>
    {
        private bool isInit=false;
        public  readonly TimeMgr timeMgr = new TimeMgr();
        public  readonly Logger logger = new Logger();
        public  readonly ToolMgr toolMgr =new ToolMgr();
        public  readonly PoolMgr poolMgr =new PoolMgr();
        public  readonly EventMgr eventMgr = new EventMgr();
        public  readonly StoreMgr storeMgr =new StoreMgr();
        public  readonly ResMgr resMgr =new ResMgr();
        public  readonly GameInputMgr gameInputMgr = new GameInputMgr();
        public  readonly SceneMgr sceneMgr = new SceneMgr();
        public  readonly CameraMgr cameraMgr = new CameraMgr();
        public  readonly UIMgr uIMgr = new UIMgr();
        public  readonly EntityMgr entityMgr = new EntityMgr();
        public  readonly NetMgr netMgr = new NetMgr();
        public  readonly DataMgr dataMgr = new DataMgr();
        public  void Init()
        {
            if (!isInit)
            {
                isInit = true;
                poolMgr.Init();
                storeMgr.Init();
                toolMgr.Init();
                logger.Init();
                resMgr.Init();
                timeMgr.Init();
                gameInputMgr.Init();
                cameraMgr.Init();
                entityMgr.Init();
                uIMgr.Init();
                sceneMgr.Init();
                dataMgr.Init();
                netMgr.Init();
             
            }
            Debug.Log("YTLOG初始化完成");
        }
    
        private void Awake()
        {
            
        }
        private void OnEnable()
        {
        
        }
        private void Start()
        {
            
        }
        private void FixedUpdate()
        {
            float dt = Time.fixedDeltaTime;
            entityMgr._FixedUpdate(dt);
            netMgr.FixUpdate(dt);
        }
        private void Update()
        {
            float dt = Time.deltaTime;
            timeMgr.Update(dt);
            poolMgr.Update(dt);
            entityMgr._Update(dt);
            sceneMgr.Update(dt);
        }
        private void LateUpdate()
        {
        }
        private void OnDestroy()
        {
            Debug.Log("YTLOG销毁完成");
            netMgr.Dispose();
            logger.OnDisable();   
        }
        private void OnGUI()
        {
            logger.OnGUI();
        }
    }
}

