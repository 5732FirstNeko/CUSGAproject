using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Tilemaps;

//半山腰太挤，你总得去山顶看看//
public class GameManager : MonoBehaviour
{
    private static GameManager Instance;
    public static GameManager instance
    {
        get
        {
            if (Instance == null)
            {
                GameObject singletonObject = new GameObject("SingletonManager");
                Instance = singletonObject.AddComponent<GameManager>();
                DontDestroyOnLoad(singletonObject);
            }
            return Instance;
        }
    }

    private Dictionary<string, Timer> timers = new Dictionary<string, Timer>();
    public List<string> timersToRemove { get; private set; } = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private string error = string.Empty;

    private void Update()
    {
        TimerManager();

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameSaveAndLoadSystem.SaveGame(out error);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GameSaveAndLoadSystem.LoadGame(out error);
        }
    }

    #region TimerManager

    private void TimerManager()
    {
        foreach (Timer timer in timers.Values)
        {
            timer.UpdateTimer();
        }

        // 遍历结束后移除计时器
        foreach (string timerName in timersToRemove)
        {
            if (timers.ContainsKey(timerName))
            {
                timers.Remove(timerName);
            }
        }
        timersToRemove.Clear();
    }

    public void StartTimer(string timerName, float duration, System.Action callback)
    {
        if (timers.ContainsKey(timerName))
        {
            StopTimer(timerName);
        }

        Timer timer = new Timer(timerName,duration, callback);
        timers.Add(timerName, timer);
    }

    public void StopTimer(string timerName)
    {
        if (timers.ContainsKey(timerName))
        {
            timers.Remove(timerName);
        }
    }

    public void PauseTimer(string timerName)
    {
        if (timers.ContainsKey(timerName))
        {
            timers[timerName].Pause();
        }
    }

    public void ReStartTimer(string timerName)
    {
        if (timers.ContainsKey(timerName))
        {
            timers[timerName].Start();
        }
    }

    public void ResumeTimer(string timerName)
    {
        if (timers.ContainsKey(timerName))
        {
            timers[timerName].Resume();
        }
    }

    public float GetRemainingTime(string timerName)
    {
        if (timers.ContainsKey(timerName))
        {
            return timers[timerName].RemainingTime;
        }
        return 0f;
    }

    #endregion

    #region TooltipManager
    public void ShowTooltip(string name,string description)
    {
        foreach (Slot slot in Inventory.instance.slots)
        {
            if (slot.name == name)
            {
                //用一个固定UI框显示文本信息，找到slot之后对文本进行赋值
            }
        }

        Debug.Log("Has select");
    }

    public void HideTooltip()
    {
        Debug.Log("Dose not select");
    }
    #endregion

    public static string GetPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null)
        {
            transform = transform.parent;
            path = $"{transform.name}/{path}";
        }
        return path;
    }
}
