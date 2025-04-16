using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Net;

public class UpLoadToServer : EditorWindow
{
    private string serverUrl = FrameworkConfig.UploadPath;
    private string deleteUrl = FrameworkConfig.DeletePath;
    private string loginUrl = FrameworkConfig.LoginPath;
    private string sourcePath = FrameworkConfig.PackPath;
    private Vector2 scrollPosition;
    private bool isUploading = false;
    private string statusMessage = "";
    
    // 登录相关
    private string username = "admin";
    private string password = "";
    private bool isLoggedIn = false;
    private HttpClient httpClient;
    private HttpClientHandler handler;
    private CookieContainer cookieContainer;

    [MenuItem("Tools/文件上传工具")]
    public static void ShowWindow()
    {
        GetWindow<UpLoadToServer>("文件上传工具");
    }

    private void OnEnable()
    {
        cookieContainer = new CookieContainer();
        handler = new HttpClientHandler 
        { 
            UseCookies = true,
            AllowAutoRedirect = true,
            CookieContainer = cookieContainer
        };
        httpClient = new HttpClient(handler);
        httpClient.DefaultRequestHeaders.Add("User-Agent", "Unity Editor Upload Tool");
    }

    private void OnDisable()
    {
        httpClient?.Dispose();
        handler?.Dispose();
    }

    private void OnGUI()
    {
        GUILayout.Label("文件上传配置", EditorStyles.boldLabel);
        
        if (!isLoggedIn)
        {
            DrawLoginUI();
        }
        else
        {
            DrawMainUI();
        }

        if (!string.IsNullOrEmpty(statusMessage))
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(statusMessage, MessageType.Info);
        }
    }

    private void DrawLoginUI()
    {
        GUILayout.Label("登录", EditorStyles.boldLabel);
        username = EditorGUILayout.TextField("用户名:", username);
        password = EditorGUILayout.PasswordField("密码:", password);

        if (GUILayout.Button("登录"))
        {
            Login();
        }
    }

    private void DrawMainUI()
    {
        sourcePath = EditorGUILayout.TextField("源文件路径:", sourcePath);
        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();
        
        if (!isUploading)
        {
            if (GUILayout.Button("开始上传"))
            {
                UploadFiles();
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("删除远端文件"))
            {
                if (EditorUtility.DisplayDialog("警告", 
                    "确定要删除远端所有文件吗？此操作不可撤销！", 
                    "确定", "取消"))
                {
                    DeleteRemoteFiles();
                }
            }
            GUI.backgroundColor = Color.white;

            if (GUILayout.Button("退出登录"))
            {
                Logout();
            }
        }
        
        EditorGUILayout.EndHorizontal();

        if (isUploading)
        {
            EditorGUILayout.HelpBox("正在上传中...", MessageType.Info);
        }
    }

    private async void Login()
    {
        try
        {
            Debug.Log("开始登录流程...");
            var formData = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };

            var content = new FormUrlEncodedContent(formData);
            
            // 设置请求头
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Unity Editor Upload Tool");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");

            // 先获取登录页面以获取任何必要的cookie
            var initialResponse = await httpClient.GetAsync(loginUrl);
            Debug.Log($"初始页面请求状态: {initialResponse.StatusCode}");

            // 发送登录请求
            var loginResponse = await httpClient.PostAsync(loginUrl, content);
            Debug.Log($"登录请求状态码: {loginResponse.StatusCode}");
            var responseContent = await loginResponse.Content.ReadAsStringAsync();
            Debug.Log($"登录响应内容: {responseContent}");

            // 检查响应内容是否包含登录成功的标志
            if (responseContent.Contains("退出登录") && responseContent.Contains("上传文件"))
            {
                // 获取所有cookies进行调试
                var cookies = cookieContainer.GetCookies(new Uri(FrameworkConfig.BaseUrl));
                Debug.Log($"Cookie数量: {cookies.Count}");
                foreach (Cookie cookie in cookies)
                {
                    Debug.Log($"Cookie: {cookie.Name} = {cookie.Value}");
                }

                isLoggedIn = true;
                statusMessage = "登录成功！";
                Debug.Log("登录成功");
            }
            else if (responseContent.Contains("error=true") || responseContent.Contains("登录失败"))
            {
                statusMessage = "登录失败：用户名或密码错误";
                Debug.LogError("登录失败：用户名或密码错误");
            }
            else
            {
                statusMessage = "登录失败：未能识别的响应内容";
                Debug.LogError("登录失败：未能识别的响应内容");
            }
        }
        catch (System.Exception ex)
        {
            statusMessage = "登录出错：" + ex.Message;
            Debug.LogError($"登录错误: {ex}");
        }
        Repaint();
    }

    private async void Logout()
    {
        try
        {
            // 调用服务器登出接口
            var logoutResponse = await httpClient.GetAsync(FrameworkConfig.LogoutPath);
            Debug.Log($"登出响应状态: {logoutResponse.StatusCode}");
            
            // 清理旧的HTTP客户端
            httpClient.Dispose();
            handler.Dispose();

            // 重新创建HTTP客户端
            cookieContainer = new CookieContainer();
            handler = new HttpClientHandler 
            { 
                UseCookies = true,
                AllowAutoRedirect = true,
                CookieContainer = cookieContainer
            };
            httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Unity Editor Upload Tool");

            // 重置所有状态
            isLoggedIn = false;
            password = "";
            GUI.FocusControl(null); // 清除当前焦点
            statusMessage = "已退出登录";
        }
        catch (System.Exception ex)
        {
            statusMessage = "登出错误：" + ex.Message;
            Debug.LogError($"登出错误: {ex}");
            
            // 即使发生错误也要重置客户端和状态
            try
            {
                httpClient.Dispose();
                handler.Dispose();

                cookieContainer = new CookieContainer();
                handler = new HttpClientHandler 
                { 
                    UseCookies = true,
                    AllowAutoRedirect = true,
                    CookieContainer = cookieContainer
                };
                httpClient = new HttpClient(handler);
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Unity Editor Upload Tool");

                // 重置所有状态
                isLoggedIn = false;
                password = "";
                GUI.FocusControl(null); // 清除当前焦点
            }
            catch
            {
                // 忽略清理过程中的错误
            }
        }
        
        // 强制重绘窗口
        EditorApplication.delayCall += () => 
        {
            Repaint();
        };
    }

    private async void UploadFiles()
    {
        if (!isLoggedIn)
        {
            statusMessage = "请先登录！";
            return;
        }

        if (!Directory.Exists(sourcePath))
        {
            statusMessage = "源文件路径不存在！";
            return;
        }

        isUploading = true;
        statusMessage = "开始上传...";
        
        try
        {
            string[] files = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
            
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                statusMessage = $"正在上传: {fileName}";
                
                Repaint();

                using (var fileStream = File.OpenRead(filePath))
                {
                    var content = new MultipartFormDataContent();
                    var fileContent = new StreamContent(fileStream);
                    content.Add(fileContent, "file", fileName);

                    var response = await httpClient.PostAsync(serverUrl, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.Found)
                        {
                            isLoggedIn = false;
                            throw new System.Exception("登录已过期，请重新登录");
                        }
                        throw new System.Exception($"上传文件 {fileName} 失败: {response.StatusCode}");
                    }
                }
            }
            
            statusMessage = "所有文件上传完成！";
        }
        catch (System.Exception ex)
        {
            statusMessage = $"上传出错: {ex.Message}";
            Debug.LogError($"上传错误: {ex}");
        }
        finally
        {
            isUploading = false;
            Repaint();
        }
    }

    private async void DeleteRemoteFiles()
    {
        if (!isLoggedIn)
        {
            statusMessage = "请先登录！";
            return;
        }

        isUploading = true;
        statusMessage = "正在删除远端文件...";
        Repaint();

        try
        {
            var response = await httpClient.DeleteAsync(deleteUrl);
            if (response.IsSuccessStatusCode)
            {
                statusMessage = "远端文件删除成功！";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Found)
            {
                isLoggedIn = false;
                statusMessage = "登录已过期，请重新登录";
            }
            else
            {
                throw new System.Exception($"删除失败: {response.StatusCode}");
            }
        }
        catch (System.Exception ex)
        {
            statusMessage = $"删除出错: {ex.Message}";
            Debug.LogError($"删除错误: {ex}");
        }
        finally
        {
            isUploading = false;
            Repaint();
        }
    }
}

