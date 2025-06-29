using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DayTimeType
{
    Morning,
    MidDay,
    Afternoon,
    NightTime,
    Drak
}
public class WeatherManager : Singleton<WeatherManager>
{
    //todo:管理所有灯光
    //todo:管理游戏时间：白天，夜晚
    //todo:管理天气，晴朗，小雨，大雨
    public void Init()
    {
        
    }
    
    public void ChangeDayTime(DayTimeType time)
    {
        
    }
}
