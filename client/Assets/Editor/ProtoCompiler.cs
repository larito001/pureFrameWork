using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using System.Text;
using Debug = UnityEngine.Debug;

public class ProtoCompiler : EditorWindow
{
    [MenuItem("Tools/Proto/Compile Proto Files")]
    public static void CompileProtoFiles()
    {
        string protoDir = Path.Combine(Application.dataPath, "HotUpdate/Scripts/Framework/Net/Proto");
        string outputDir = Path.Combine(Application.dataPath, "HotUpdate/Scripts/Framework/Net/Generated");

        // 确保输出目录存在
        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        // 获取所有proto文件
        string[] protoFiles = Directory.GetFiles(protoDir, "*.proto");

        foreach (string protoFile in protoFiles)
        {
            CompileProtoFile(protoFile, outputDir);
        }

        AssetDatabase.Refresh();
        Debug.Log("Proto文件编译完成!");
    }

    private static void CompileProtoFile(string protoFile, string outputDir)
    {
        try
        {
            // protoc.exe的路径，你需要根据实际情况修改
            string protocPath = "D:/Desktop/Protobuf/protoc.exe";
            
            if (!File.Exists(protocPath))
            {
                Debug.LogError($"找不到protoc编译器: {protocPath}");
                return;
            }

            // 构建编译命令
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = protocPath;
            startInfo.Arguments = $"--proto_path=\"{Path.GetDirectoryName(protoFile)}\" --csharp_out=\"{outputDir}\" \"{protoFile}\"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            // 执行编译
            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError($"编译Proto文件失败: {error}");
                }
                else if (!string.IsNullOrEmpty(output))
                {
                    Debug.Log(output);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"编译Proto文件时出错: {e.Message}");
        }
    }
} 