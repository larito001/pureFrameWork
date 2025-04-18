
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
namespace YOTO
{
    public class ResMgr
    {
        //Dictionary<int ,BaseLoader> loaders= new Dictionary<int ,BaseLoader>(2);
        //ʹ�ö���أ�
        private ResLoader<T> CreateLoader<T>()
        {
            //ȡ��
            return YOTOFramework.poolMgr.GetObjectPool(ObjectPoolType.Loader).Get<ResLoader<T>>(); 
        }
        private void RecycleLoader<T>(ResLoader<T> baseLoader)
        {
            //�Ż�
            YOTOFramework.poolMgr.GetObjectPool(ObjectPoolType.Loader).Set<ResLoader<T>>(baseLoader);
        }
        public void Init()
        {
 
        }
     
        //����ؼ���
        public void LoadUI(string key,Action<GameObject> callBack)
        {
            ResLoader<GameObject> loader = CreateLoader<GameObject>();
            if (!loader.isRecycle)
            {

                //todo: ������Դ
                loader.LoadAsync<GameObject>(key,
                    (t) =>
                    {
                        //Debug.Log("������UIiiiiiiiiii");
                        if (t.Status == AsyncOperationStatus.Succeeded)
                        {
                          //  UIPageBase obj = UnityEngine.Object.Instantiate(t.Result).GetComponent<UIPageBase>();
                            callBack(t.Result);
                        }
                        RecycleLoader(loader);

                    });



            }

     

        }

        //public void LoadAudio()
        //{
        //    ResLoader<> loader = CreateLoader();
        //    //todo:������Դ
        //    RecycleLoader(loader);

        //}

        //�����
        public void LoadGameObject(string path,Vector3 pos,Quaternion rotation, Action<GameObject, Vector3,Quaternion> callBack)
        {
            ResLoader<GameObject> loader = CreateLoader<GameObject>();
            //todo:������Դ
            if (!loader.isRecycle)
            {
                //todo: ������Դ
                loader.LoadAsync<GameObject>(path,
                    (t) =>
                    {
                   
                        if (t.Status == AsyncOperationStatus.Succeeded)
                        {
                            //Debug.Log("��������Ʒ"+t.Result);
                            //  UIPageBase obj = UnityEngine.Object.Instantiate(t.Result).GetComponent<UIPageBase>();
                            callBack(t.Result,pos, rotation);
                        }
                        else
                        {
                            Debug.LogError("加载失败");
                        }
                        RecycleLoader(loader);

                    });
            }
        }

    }
}