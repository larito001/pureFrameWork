using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasRenderer))]
public class UGUIFloatingTextBatch : PoolBaseGameObject
{
    public Font font ; // 字体
    public Material fontMaterial; // 字体的材质
    public Color color = Color.white; // 文本颜色
    public float floatSpeed = 50f; // 基础飘动速度
    public float fadeDuration = 10f; // 淡出时长
    public int numberOfTexts = 1000; // 生成的文本数量
    public int currentNum = 1;
    private Mesh mesh;
    private CanvasRenderer canvasRenderer;
    private float timer = 0;
    public List<string> texts = new List<string>();

    // 每个字符的独立行为
    private Vector3[] positions;
    private Color[] colors;
    private float[] alphas; // 用于控制每个字符的透明度


    int totalCharacters;
    Vector3[] vertices; // 每个字符4个顶点
    Vector2[] uvs;
    int[] triangles; // 每个字符2个三角形
    Color[] meshColors;
    Vector3 org;
    void Start()
    {
        Debug.Log("生成");
        currentNum = texts.Count;
        org = transform.position;
        // 为每个 "Hello" 生成网格信息
        totalCharacters = 10 * numberOfTexts;
        vertices = new Vector3[totalCharacters * 4]; // 每个字符4个顶点
        uvs = new Vector2[vertices.Length];
        triangles = new int[totalCharacters * 6]; // 每个字符2个三角形
        meshColors = new Color[vertices.Length];

        // 初始化位置和颜色数据
        positions = new Vector3[numberOfTexts];
        colors = new Color[numberOfTexts];
        alphas = new float[numberOfTexts];

        for (int i = 0; i < numberOfTexts; i++)
        {
            // 随机初始位置
            positions[i] = new Vector3(Random.Range(-500, 500), Random.Range(-300, 300), 0);
            colors[i] = color; // 初始化颜色为全白
            alphas[i] = 1f; // 初始透明度为1（不透明）
        }

        // 获取CanvasRenderer组件
        canvasRenderer = GetComponent<CanvasRenderer>();

        // 生成初始的字体网格
        GenerateMesh();

        // 设置材质：如果没有指定字体材质，使用字体自带的默认材质
        if (fontMaterial != null)
        {
            canvasRenderer.SetMaterial(fontMaterial, null);
        }
        else
        {
            fontMaterial = font.material;
            canvasRenderer.SetMaterial(fontMaterial, null);
        }
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        timer += Time.deltaTime;

        //如果时间超过淡出时长，销毁物体
        if (timer >= fadeDuration)
        {
            transform.position = org;
            timer = 0;
        }

    }
    //重绘
    public void GenerateMesh()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
        }
        float xOffset = 0f;
        int vertexOffset = 0;
        int triangleOffset = 0;

        for (int i = 0; i < texts.Count; i++)
        {
            // 为每个 "Hello" 设置网格
            Vector3 startPosition = positions[i];

            for (int j = 0; j < texts[i].Length; j++)
            {
                char c = texts[i][j];
                font.RequestCharactersInTexture(c.ToString(), font.fontSize, FontStyle.Normal);
                font.GetCharacterInfo(c, out CharacterInfo characterInfo, font.fontSize);

                // 设置每个字符的顶点
                float xMin = startPosition.x + xOffset + characterInfo.minX;
                float xMax = startPosition.x + xOffset + characterInfo.maxX;
                float yMin = startPosition.y + characterInfo.minY;
                float yMax = startPosition.y + characterInfo.maxY;

                vertices[vertexOffset] = new Vector3(xMin, yMin, 0);
                vertices[vertexOffset + 1] = new Vector3(xMin, yMax, 0);
                vertices[vertexOffset + 2] = new Vector3(xMax, yMax, 0);
                vertices[vertexOffset + 3] = new Vector3(xMax, yMin, 0);

                // 设置UV坐标
                uvs[vertexOffset] = characterInfo.uvBottomLeft;
                uvs[vertexOffset + 1] = characterInfo.uvTopLeft;
                uvs[vertexOffset + 2] = characterInfo.uvTopRight;
                uvs[vertexOffset + 3] = characterInfo.uvBottomRight;

                // 设置初始颜色
                meshColors[vertexOffset] = colors[i];
                meshColors[vertexOffset + 1] = colors[i];
                meshColors[vertexOffset + 2] = colors[i];
                meshColors[vertexOffset + 3] = colors[i];

                // 设置三角形
                triangles[triangleOffset] = vertexOffset;
                triangles[triangleOffset + 1] = vertexOffset + 1;
                triangles[triangleOffset + 2] = vertexOffset + 2;
                triangles[triangleOffset + 3] = vertexOffset;
                triangles[triangleOffset + 4] = vertexOffset + 2;
                triangles[triangleOffset + 5] = vertexOffset + 3;

                vertexOffset += 4;
                triangleOffset += 6;

                xOffset += characterInfo.advance; // 下一个字符的X偏移量
            }

            // 重置 xOffset，开始下一组 "Hello"
            xOffset = 0f;
        }

        // 将生成的顶点、UV坐标、三角形和颜色赋值给网格
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.colors = meshColors;

        // 使用CanvasRenderer将网格设置为UI元素
        canvasRenderer.SetMesh(mesh);
    }

    public override void ResetAll()
    {
    
    }

    //刷新，暂时不用
    //void UpdateMesh()
    //{
    //    Vector3[] vertices = mesh.vertices;
    //    Color[] meshColors = mesh.colors;

    //    float xOffset = 0f;
    //    int vertexOffset = 0;

    //    for (int i = 0; i < numberOfTexts; i++)
    //    {
    //        Vector3 startPosition = positions[i];

    //        for (int j = 0; j < helloText.Length; j++)
    //        {
    //            font.GetCharacterInfo(helloText[j], out CharacterInfo characterInfo, font.fontSize);

    //            float xMin = startPosition.x + xOffset + characterInfo.minX;
    //            float xMax = startPosition.x + xOffset + characterInfo.maxX;
    //            float yMin = startPosition.y + characterInfo.minY;
    //            float yMax = startPosition.y + characterInfo.maxY;

    //            vertices[vertexOffset] = new Vector3(xMin, yMin, 0);
    //            vertices[vertexOffset + 1] = new Vector3(xMin, yMax, 0);
    //            vertices[vertexOffset + 2] = new Vector3(xMax, yMax, 0);
    //            vertices[vertexOffset + 3] = new Vector3(xMax, yMin, 0);

    //            // 更新颜色
    //            meshColors[vertexOffset] = colors[i];
    //            meshColors[vertexOffset + 1] = colors[i];
    //            meshColors[vertexOffset + 2] = colors[i];
    //            meshColors[vertexOffset + 3] = colors[i];

    //            vertexOffset += 4;
    //            xOffset += characterInfo.advance;
    //        }

    //        // 重置 xOffset
    //        xOffset = 0f;
    //    }

    //    // 更新网格的顶点和颜色
    //    mesh.vertices = vertices;
    //    mesh.colors = meshColors;

    //    // 更新CanvasRenderer
    //    canvasRenderer.SetMesh(mesh);
    //}
}