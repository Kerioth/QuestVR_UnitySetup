using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoundLoop
{
    public float startWait = 2f;
    public float molesWait = 1f;
    public List<MolesUp> cycles;
}
[Serializable]
public class MolesUp
{
    public int molesCount;
    public float delay;
}
