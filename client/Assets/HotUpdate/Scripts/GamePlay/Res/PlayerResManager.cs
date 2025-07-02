using System.Collections;
using System.Collections.Generic;
using YOTO;

public class PlayerResManager : Singleton<PlayerResManager>
{
    private int woodNum = 999;
    private int ironNum = 999;

    public bool CheckWoodEnough(int num)
    {
        return woodNum >= num;

    }
    public bool CheckIronEnough(int num)
    {
        return ironNum >= num;

    }

    public void UseWoodRes(int num)
    {
        if (CheckWoodEnough(num))
        {
            woodNum -= num; 
            YOTOFramework.eventMgr.TriggerEvent(EventType.RefreshResInfo);
        }
  
    }
    public void UseIronRes(int num)
    {
        if (CheckIronEnough(num))
        {
            ironNum -= num; 
            YOTOFramework.eventMgr.TriggerEvent(EventType.RefreshResInfo);
        }
  
    }
    public void AddIronNum(int num)
    {
        if (num >= 0)
        {
            ironNum += num; 
        }
        YOTOFramework.eventMgr.TriggerEvent(EventType.RefreshResInfo);
    }
    
    public int GetIronNum()
    {
        return ironNum;
    }
    public void AddWoodNum(int num)
    {
        if (num >= 0)
        {
            woodNum += num; 
        }
    
        YOTOFramework.eventMgr.TriggerEvent(EventType.RefreshResInfo);
    }
    
    public int GetWoodNum()
    {
        return woodNum;
    }



}
