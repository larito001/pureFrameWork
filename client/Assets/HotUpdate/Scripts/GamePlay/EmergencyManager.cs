using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YOTO;
using EventType = YOTO.EventType;

/// <summary>
/// 紧急事件描述类
/// </summary>
public class EmergencyEventDesc:YOTOScrollViewDataBase
{
    public EventType eventType;      // 事件类型
    public string playerId;
    public string eventName;         // 事件名称
    public string description;       // 事件描述
    public int triggerCount;         // 触发所需次数
    public float duration;           // 影响持续时间
    public System.Action<string> onTrigger;  // 触发时的回调
    public System.Action<string>  onEndTrigger;  // 触发结束的回调
}

public class EmergencyManager : SingletonMono<EmergencyManager>
{
    private bool isInitialized = false;
    private Dictionary<EventType, EmergencyEventDesc> eventDescriptions;  // 事件描述字典
    private Dictionary<EventType, int> eventCounters;                     // 事件计数器
    // private Dictionary<string, EmergencyEventEffect> activeEffects;    // 当前生效的效果
    private Dictionary<EventType, Coroutine> effectCoroutines;           // 效果持续时间协程

    // 效果变化事件
    public System.Action<EventType, Dictionary<string, float>> onEffectChanged;

    public void Init()
    {
    
        eventDescriptions = new Dictionary<EventType, EmergencyEventDesc>();
        eventCounters = new Dictionary<EventType, int>();
        effectCoroutines = new Dictionary<EventType, Coroutine>();
     
        isInitialized = true;
        RegisterDefaultEvents();
    }

    /// <summary>
    /// 注册新的紧急事件
    /// </summary>
    public void RegisterEmergencyEvent(EmergencyEventDesc eventDesc)
    {
        if (isInitialized&&!eventDescriptions.ContainsKey(eventDesc.eventType))
        {
            
            eventDescriptions.Add(eventDesc.eventType, eventDesc);
            eventCounters.Add(eventDesc.eventType, 0);
        }
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    public void TrigerEmergencyEvent(EventType eventType,string data)
    {  
        if (isInitialized&&eventDescriptions.TryGetValue(eventType, out EmergencyEventDesc desc))
        {
            eventCounters[eventType]++;
      
            if (eventCounters[eventType] >= desc.triggerCount)
            {
                TriggerEmergencyEvent(eventType,data);
                eventCounters[eventType] = 0;  // 重置计数
            }
        }
    }

    /// <summary>
    /// 重置指定事件的计数器
    /// </summary>
    public void ResetEventCounter(EventType eventType)
    {
        if (isInitialized&&eventCounters.ContainsKey(eventType))
        {
            eventCounters[eventType] = 0;
        }
    }

    /// <summary>
    /// 重置所有事件的计数器
    /// </summary>
    public void ResetAllEventCounters()
    {
        if (isInitialized && eventCounters.Count > 0)
        {
            foreach (var eventType in eventCounters.Keys)
            {
                eventCounters[eventType] = 0;
            }  
        }
   
    }

    private void RegisterDefaultEvents()
    {
        // 注册默认事件示例
        RegisterEmergencyEvent(new EmergencyEventDesc
        {
            eventType = EventType.Sprint,
            eventName = "冲刺疲劳",
            description = "连续冲刺导致体力不支",
            triggerCount = 10,
            duration = 5f,
            onTrigger = (data) =>
            {
                // YOTOFramework.Instance.eventMgr.TriggerEvent<string,float>(EventType.SetMoveSpeed,"playerID",0.7f);
                Debug.Log("Emergency event triggered111");
            },
            onEndTrigger=(data)=>{
                // YOTOFramework.Instance.eventMgr.TriggerEvent<string,float>(EventType.SetMoveSpeed,"playerID",1);
                Debug.Log("Emergency event triggered2222");
                ResetEventCounter(EventType.Sprint);
            }
        });
    }

    private void TriggerEmergencyEvent(EventType eventType,string data)
    {
        if (!eventDescriptions.ContainsKey(eventType)) return;

        var desc = eventDescriptions[eventType];
        
        // 调用事件特定的效果设置
        desc.onTrigger?.Invoke(data);

        // 如果已有相同事件的效果在生效，先移除它
        RemoveEffect(eventType,data);

        // 启动持续时间计时器
        if (desc.duration > 0)
        {
            effectCoroutines[eventType] = StartCoroutine(EffectDurationCoroutine(eventType, desc.duration,data));
        }
    }

    private IEnumerator EffectDurationCoroutine(EventType eventType, float duration,string data)
    {
        yield return new WaitForSeconds(duration);
        RemoveEffect(eventType,data);
    }

    private void RemoveEffect(EventType eventType,string data )
    {
   

        if (effectCoroutines.ContainsKey(eventType))
        {
            if (effectCoroutines[eventType] != null)
            {
                StopCoroutine(effectCoroutines[eventType]);
            }
            effectCoroutines.Remove(eventType);
            var desc = eventDescriptions[eventType];
            // 调用事件特定的效果设置
            desc.onEndTrigger?.Invoke(data);
        }
    }
    
    private void OnDestroy()
    {
        // 清理所有协程
        foreach (var coroutine in effectCoroutines.Values)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
    }
}
