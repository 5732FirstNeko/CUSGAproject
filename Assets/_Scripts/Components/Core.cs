using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

//半山腰太挤，你总得去山顶看看//
public class Core : MonoBehaviour
{
    public readonly List<CoreComponent> coreComponents = new List<CoreComponent>();

    public void LogicUpdate()
    {
        foreach (var component in coreComponents)
        {
            component.LogicUpdate();
        }
    }

    public void AddCoreComponent(CoreComponent coreComponent)
    {
        if (!coreComponents.Contains(coreComponent))
        {
            coreComponents.Add(coreComponent);
        }
    }

    public T GetCoreComponent<T>() where T : CoreComponent
    {
        var comp = coreComponents.OfType<T>().FirstOrDefault();

        if (comp)
            return comp;

        comp = GetComponentInChildren<T>();

        if (comp)
            return comp;

        Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        return null;
    }

    private void Update()
    {
        LogicUpdate();
    }
}
