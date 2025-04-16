using UnityEngine;
using UnityEditor;
using System.IO;

public class DLLMove : Editor
{
    [MenuItem("Tools/Move HotUpdate DLL")]
    public static void MoveHotUpdateDLL()
    {
        // 源文件路径
        string sourcePath = Path.Combine(Application.dataPath, FrameworkConfig.DLLPath);
        
        // 目标文件夹路径
        string targetDirectory = Path.Combine(Application.dataPath, FrameworkConfig.NewDLLPath);
        
        // 目标文件完整路径
        string targetPath = Path.Combine(targetDirectory, "HotUpdate.dll.bytes");

        try
        {
            // 确保目标目录存在
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            // 如果目标文件已存在，先删除
            if (File.Exists(targetPath))
            {
                File.Delete(targetPath);
            }

            // 复制并重命名文件
            File.Copy(sourcePath, targetPath);
            
            Debug.Log("DLL文件移动成功！");
            
            // 刷新Asset数据库以显示新文件
            AssetDatabase.Refresh();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"移动DLL文件时发生错误: {e.Message}");
        }
    }
}
