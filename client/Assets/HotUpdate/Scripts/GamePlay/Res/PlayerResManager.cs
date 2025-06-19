using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResManager : Singleton<PlayerResManager>
{
    private int woodNum = 0;
    private int ironNum = 0;
    
    public void AddIronNum(int num)
    {
        if (num >= 0)
        {
            woodNum += num; 
        }
    }
    
    public int GetIronNum()
    {
        return woodNum;
    }
    public void AddWoodNum(int num)
    {
        woodNum += num;
    }
    
    public int GetWoodNum()
    {
        return woodNum;
    }



}
