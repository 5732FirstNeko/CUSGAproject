using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class EventCenter
{
    private static Dictionary<EventType, Delegate> m_eventTable = new Dictionary<EventType, Delegate>();

    public static void AddListener(EventType eventType, CallBack callback)
    {
        if (!m_eventTable.ContainsKey(eventType))
        {
            m_eventTable.Add(eventType, null);
        }
        Delegate d = m_eventTable[eventType];
        if (d != null && d.GetType() != callback.GetType())
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同参数的委托，当前事件所对应的委托为{1}，要添加的委托类型为{2}", eventType, d.GetType(), callback.GetType()));
        }
        m_eventTable[eventType] = (CallBack)m_eventTable[eventType] + callback;
    }

    public static void AddListener<T>(EventType eventType, CallBack<T> callback)
    {
        if (!m_eventTable.ContainsKey(eventType))
        {
            m_eventTable.Add(eventType, null);
        }
        Delegate d = m_eventTable[eventType];
        if (d != null && d.GetType() != callback.GetType())
        {
            throw new Exception(string.Format("尝试为事件{0}添加不同参数的委托，当前事件所对应的委托为{1}，要添加的委托类型为{2}", eventType, d.GetType(), callback.GetType()));
        }
        m_eventTable[eventType] = (CallBack<T>)m_eventTable[eventType] + callback;
    }

    public static void RemoveListener(EventType eventType, CallBack callback)
    {
        if (m_eventTable.ContainsKey(eventType))
        {
            Delegate d = m_eventTable[eventType];
            if (d != null)
            {
                throw new Exception(string.Format("移除监听错误，事件{0}没有对应委托", eventType));
            }
            else if (d.GetType() != m_eventTable[eventType].GetType())
            {
                throw new Exception(string.Format("移除监听错位，尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，目标委托类型为{2}", eventType, callback.GetType(), m_eventTable[eventType].GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误，不存在事件码{0}", eventType));
        }
        m_eventTable[eventType] = (CallBack)m_eventTable[eventType] - callback;
        if (m_eventTable[eventType] == null)
        {
            m_eventTable.Remove(eventType);
        }
    }

    public static void RemoveListener<T>(EventType eventType, CallBack<T> callback)
    {
        if (m_eventTable.ContainsKey(eventType))
        {
            Delegate d = m_eventTable[eventType];
            if (d != null)
            {
                throw new Exception(string.Format("移除监听错误，事件{0}没有对应委托", eventType));
            }
            else if (d.GetType() != m_eventTable[eventType].GetType())
            {
                throw new Exception(string.Format("移除监听错位，尝试为事件{0}移除不同类型的委托，当前委托类型为{1}，目标委托类型为{2}", eventType, callback.GetType(), m_eventTable[eventType].GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误，不存在事件码{0}", eventType));
        }
        m_eventTable[eventType] = (CallBack<T>)m_eventTable[eventType] - callback;
        if (m_eventTable[eventType] == null)
        {
            m_eventTable.Remove(eventType);
        }
    }

    public static void BroadCast(EventType eventType)
    {
        Delegate d;
        if (m_eventTable.TryGetValue(eventType, out d))
        {
            CallBack callBack = d as CallBack;
            if (callBack != null)
            {
                callBack();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误，事件{0}对应委托具有不同类型", eventType));
            }
        }
    }

    public static void BroadCast<T>(EventType eventType, T arg)
    {
        Delegate d;
        if (m_eventTable.TryGetValue(eventType, out d))
        {
            CallBack<T> callBack = d as CallBack<T>;
            if (callBack != null)
            {
                callBack(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误，事件{0}对应委托具有不同类型", eventType));
            }
        }
    }
}
