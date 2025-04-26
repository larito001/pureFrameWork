using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YOTO;

public class MapBlock
{
    public int distance;//距离
    public Vector3 pos;//坐标
    public Vector3 dir;//方向

}
public class NavMapManager : SingletonMono<NavMapManager>
{
    private static readonly int X=100;
    private static readonly int Y=100;
    private static readonly int BLOCKWIDTH=3;
    private static readonly Vector3 LEFTPOS = new Vector3(0,0,0);
    public static int ID=0;

    private void Start()
    {
        YOTOFramework.timeMgr.LoopCall(() =>
        {
            UpdateAllMap();
        },0.5f);
    }

    Dictionary<int ,NavTarget> _targetDic =new Dictionary<int ,NavTarget>();
    Dictionary<int,MapInfo> _mapDic = new Dictionary<int, MapInfo>();
    public NavTarget GetTarget(bool isStatic,Vector3 orgPos)
    {
        var temp =new NavTarget(ID,orgPos,this);
        _targetDic.Add(ID,temp);
        var mapInfo = new MapInfo(this);
        mapInfo.InitMap(X,Y,BLOCKWIDTH,LEFTPOS,temp,!isStatic);
        _mapDic.Add(ID,mapInfo);
        ID++;
        return temp;
    }
    public NavAgent GetAgent(bool isStatic,Vector3 orgPos)
    {
        var temp = new NavAgent(this);
        return temp;
    }
    private void UpdateAllMap()
    {
        foreach (var map in _mapDic.Values)
        {
            map.TryUpdateMap();
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var map in _mapDic.Values)
        {
            map.OnDrawGizmos();
        }
    }


}

public class MapInfo
{
    private NavMapManager _manager;
    public MapInfo(NavMapManager manager)
    {
        _manager=manager;
    }
    
    private int x;
    private int y;
    private int blockWidth;
    private Vector3 lefTopPos;
    List<List<MapBlock>> m_map;
    NavTarget _target;
    private bool _needUpdate; 
    public void InitMap(int x, int y,int width,Vector3 leftTopPos,NavTarget target,bool needUpdate)
    {
        _needUpdate=needUpdate;
        m_map=new List<List<MapBlock>>();
        this.blockWidth=width;
        _target=target;
        this.x = x;
        this.y = y;
        this.lefTopPos = leftTopPos;
        for (int i = 0; i < x; i++)
        {
            var Line = new List<MapBlock>();
         
            for (int j = 0; j < y; j++)
            {
                var block = new MapBlock();
                block.pos=lefTopPos+new Vector3(i*blockWidth,0,j*blockWidth);
                Line.Add(block);
            }
            m_map.Add(Line);
        }

        UpdateMap();
        DebugDistance();
    }
    public void OnDrawGizmos()
    {
        if (m_map == null) return;

        foreach (var row in m_map)
        {
            foreach (var block in row)
            {
                // 画格子
                if (block.distance == 0)
                {
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.color = Color.green;
                }

                float heightOffset = block.distance * 0.2f;
                Vector3 pos = block.pos + new Vector3(0, heightOffset, 0);
                // Gizmos.DrawWireCube(block.pos, Vector3.one);
                // Gizmos.DrawWireCube(pos, Vector3.one);

                // 画方向箭头
                if (block.dir != Vector3.zero)
                {
                    Gizmos.color = Color.blue; // 画向量时用蓝色
                    Vector3 start = pos;
                    Vector3 end = start + block.dir * 1.5f; // 向量长度可调（这里是1.5）

                    Gizmos.DrawLine(start, end);

                    // 画箭头头部（小三角）
                    Vector3 right = Quaternion.LookRotation(block.dir) * Quaternion.Euler(0, 150, 0) * Vector3.forward;
                    Vector3 left = Quaternion.LookRotation(block.dir) * Quaternion.Euler(0, -150, 0) * Vector3.forward;
                    Gizmos.DrawLine(end, end + right * 0.3f);
                    Gizmos.DrawLine(end, end + left * 0.3f);
                }
            }
        }
      
    }

    public void TryUpdateMap()
    {
        if (_needUpdate)
        {
            UpdateMap();
        }  
    }
    
    //算法核心：
    private void UpdateMap()
    {

        CulDistance();

        CulVector();

        if (Input.GetKeyDown(KeyCode.P))
        {
            DebugDistance(); // 按P键输出distance和direction
        }
    }


    private void CulDistance()
    {
        if (_target == null) return;

        
        Vector3 targetPos = _target.GetPosition();
        MapBlock targetBlock = GetMapPos(targetPos);
        // 初始化所有格子的距离为一个很大值（比如无穷大）
        for (var i = 0; i < m_map.Count; i++)
        {
            for (int j = 0; j <  m_map[i].Count; j++)
            {
                m_map[i][j].distance = int.MaxValue;
            }
            
        }
       

        // BFS准备
        Queue<MapBlock> queue = new Queue<MapBlock>();
        targetBlock.distance = 0;
        queue.Enqueue(targetBlock);

        // 定义8个方向
        int[] dirX = { 0, 0, 1, -1, 1, 1, -1, -1 };
        int[] dirY = { 1, -1, 0, 0, 1, -1, 1, -1 };

        while (queue.Count > 0)
        {
            MapBlock current = queue.Dequeue();
            Vector3 relativePos = current.pos - lefTopPos;
            int cx = Mathf.RoundToInt(relativePos.x / blockWidth);
            int cy = Mathf.RoundToInt(relativePos.z / blockWidth);

            for (int d = 0; d < 8; d++)
            {
                int nx = cx + dirX[d];
                int ny = cy + dirY[d];

                if (nx >= 0 && nx < x && ny >= 0 && ny < y)
                {
                    MapBlock neighbor = m_map[nx][ny];
                    if (neighbor.distance > current.distance + 1)
                    {
                        neighbor.distance = current.distance + 1;
                        queue.Enqueue(neighbor);
                    }
                }
            }
        }
        
        
    }

    private void CulVector()
    {
        if (_target == null || m_map == null) return;

        int rows = m_map.Count;
        if (rows == 0) return;
        int cols = m_map[0].Count;

        // 8个方向 (上下左右 + 4个斜角)
        int[] dx = { -1, 1, 0, 0, -1, -1, 1, 1 };
        int[] dy = { 0, 0, -1, 1, -1, 1, -1, 1 };

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                MapBlock currentBlock = m_map[i][j];
                MapBlock nearestBlock = null;
                int minDistance = currentBlock.distance; // 初始为当前块的distance

                for (int d = 0; d < 8; d++)
                {
                    int ni = i + dx[d];
                    int nj = j + dy[d];

                    if (ni >= 0 && ni < rows && nj >= 0 && nj < cols)
                    {
                        MapBlock neighbor = m_map[ni][nj];
                        if (neighbor.distance < minDistance) // 仅考虑更小的distance
                        {
                            minDistance = neighbor.distance;
                            nearestBlock = neighbor;
                        }
                    }
                }

                if (nearestBlock != null)
                {
                    Vector3 dir = (nearestBlock.pos - currentBlock.pos).normalized;
                    currentBlock.dir = dir;
                }
                else
                {
                    currentBlock.dir = Vector3.zero;
                }
            }
        }
    }
    public void DebugDistance()
    {
        return;
        if (m_map == null) return;

        // 文件路径：可以自定义路径，这里以应用程序的数据路径作为示例
        string filePath = Path.Combine(Application.dataPath, "MapDistanceAndDirection.txt");

        // 使用 StreamWriter 来写入文件
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            // 遍历每一行
            for (int j = y - 1; j >= 0; j--) // 从上往下遍历
            {
                string distanceRow = ""; // 存储当前行的distance
                string directionRow = ""; // 存储当前行的方向向量

                // 遍历每一列
                for (int i = 0; i < x; i++) // 从左往右遍历
                {
                    int dist = m_map[i][j].distance;
                    Vector3 dir = m_map[i][j].dir;

                    // 拼接当前格子的distance，格式化为两位数
                    distanceRow += dist.ToString("D2") + " ";

                    // 拼接当前格子的dir向量，取 x, y, z 并格式化为小数点后一位
                    directionRow += $"({dir.x:F1},{dir.y:F1},{dir.z:F1}) " ;
                }

                // 将distance和direction行写入文件
                writer.WriteLine("Distance: " + distanceRow.Trim());
                writer.WriteLine("Direction: " + directionRow.Trim());
                writer.WriteLine(); // 添加空行分隔每一行
            }
        }

        // 输出文件路径到控制台，方便查看文件生成位置
        Debug.Log($"Data has been written to: {filePath}");
    }

    private MapBlock GetMapPos(Vector3 pos)
    {
        Vector3 relativePos = pos - lefTopPos;

        int i = Mathf.RoundToInt(relativePos.x / blockWidth);
        int j = Mathf.RoundToInt(relativePos.z / blockWidth);

        // 保证i,j不越界
        i = Mathf.Clamp(i, 0, x - 1);
        j = Mathf.Clamp(j, 0, y - 1);

        return m_map[i][j];
    }
}
