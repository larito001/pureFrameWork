using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MapBlock
{
    public int distance;//距离
    public Vector3 pos;//坐标
    public Vector3 dir;//方向

}
public class NavMapManager : MonoBehaviour
{
    private static readonly int X=100;
    private static readonly int Y=100;
    private static readonly int BLOCKWIDTH=3;
    private static readonly Vector3 LEFTPOS = new Vector3(0,0,0);
    public static int ID=0;
    
    
    Dictionary<int ,NavTarget> _targetDic =new Dictionary<int ,NavTarget>();
    Dictionary<int,MapInfo> _mapDic = new Dictionary<int, MapInfo>();
    public NavTarget GetTarget(bool isStatic,Vector3 orgPos)
    {
        var temp =new NavTarget(ID,orgPos,this);
        _targetDic.Add(ID,temp);
        var mapInfo = new MapInfo();
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

    

  
    
    
}

public class MapInfo
{
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
    }
    private void OnDrawGizmos()
    {
        if (m_map == null) return;

        Gizmos.color = Color.green;

        foreach (var row in m_map)
        {
            foreach (var block in row)
            {
                Gizmos.DrawWireCube(block.pos, Vector3.one);
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
    private void UpdateMap()
    {
      //todo：根据targetPos计算
        
    }
}
