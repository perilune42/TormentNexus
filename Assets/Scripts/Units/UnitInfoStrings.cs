using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfoStrings : MonoBehaviour
{
    public static Dictionary<UnitType, UnitInfoString> Infos = new();
    [SerializeField] List<UnitInfoString> InfoStrings = new();

    private void Awake()
    {
        foreach(var unitInfo in InfoStrings) 
        {
            Infos.Add(unitInfo.Type, unitInfo);
        }
    }
}

[Serializable] public struct UnitInfoString
{
    public UnitType Type;
    [TextArea]
    public string Desc;
    public Color Color;
}