using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class ParamData
{
    public ParamType Type;
    public int Value;
    public Vector2Int MinMax;
    public UnityEvent<int> OnValueChange;

    public void SetValue(int value)
    {
        Value = Mathf.Clamp(value, MinMax.x, MinMax.y);
        OnValueChange?.Invoke(Value);
    }
}


public enum ParamType
{
    Gold,
    Health
}