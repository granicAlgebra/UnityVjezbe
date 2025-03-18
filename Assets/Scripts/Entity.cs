using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public List<ParamData> Params;

    public ParamData GetParam(ParamType type)
    {
        foreach (ParamData param in Params)
        {
            if (param.Type == type)
            {
                return param;
            }
        }

        Debug.LogWarning($"{name} does not have param {type}");
        return null;
    }

    public bool ChangeParam(ParamType type, int value)
    {
        var paramData = GetParam(type);
        if (paramData != null) 
        { 
            paramData.SetValue(paramData.Value + value);
            return true;
        }
        return false;
    }
}
