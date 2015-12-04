using UnityEngine;
using System.Collections;

public class MasterTable : MasterTableBase<EnemyMaster>
{
    private static readonly string FilePath = "sample";
    public void Load() { Load(FilePath); }
}

public class EnemyMaster : MasterBase
{
    public string Jump { get; private set; }
    public string RoteChangeFlag { get; private set; }
    public string Route { get; private set; }
    public string text { get; private set; }
}