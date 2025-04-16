using UnityEngine;
using System.IO;
using System.Collections.Generic;
namespace YOTO
{
    [System.Serializable]
    public class ItemPosData
    {
        public string name;
        public Vector3 pos;
        public Quaternion rot;
    }
    [System.Serializable]
    public class ItemDataList
    {
       public List<ItemPosData> itemPosDatas = new List<ItemPosData>();
    }
    public class StoreMgr
    {
        //�����޸����޸ı��ӿ�
        //��д
        public void Init()
        {

        }
        public void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }

        public void SetString(string key, string value)
        {
            PlayerPrefs.SetString(key, value);
        }

        public float GetFloat(string key, float def)
        {
            return PlayerPrefs.GetFloat(key, def);
        }

        public int GetInt(string key, int def)
        {
            return PlayerPrefs.GetInt(key, def);
        }

        public string GetString(string key, string def)
        {
            return PlayerPrefs.GetString(key, def);
        }
        public void DeleteKey(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }
        public void Save()
        {
            PlayerPrefs.Save();
        }
        public void ClearAll(bool isReady = false)
        {
            if (isReady)
            {
                PlayerPrefs.DeleteAll();
            }
   
        }
        public  void SaveJson<T>(T data, string filePath, bool prettyPrint = false)
        {
            try
            {
                string json = JsonUtility.ToJson(data, prettyPrint);
                string directory = Path.GetDirectoryName(filePath);

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(filePath, json);
                Debug.Log($"[JsonManager] �����ѱ��浽: {filePath}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[JsonManager] ���� JSON ʱ����: {e.Message}");
            }
        }

        public  T LoadJson<T>(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Debug.LogWarning($"[JsonManager] �ļ�������: {filePath}");
                    return default;
                }

                string json = File.ReadAllText(filePath);
                T data = JsonUtility.FromJson<T>(json);
                Debug.Log($"[JsonManager] �����Ѵ� {filePath} ��ȡ��");
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[JsonManager] ��ȡ JSON ʱ����: {e.Message}");
                return default;
            }
        }
    }
}