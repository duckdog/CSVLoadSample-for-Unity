using UnityEngine;
using System.Collections;

public class CSVMasterTable : MasterTableBase<CSVScenarioData> 
{
	//FileName(Path)

    private static readonly string FilePath = "sample";
    public void Load() { Load(FilePath); }
}
public enum CharacterAnimator 
{
	Stay,
	Jump,
	LeftMove,
	RightMove,

}

public enum CharacterState
{
	Normal,
	Smile,
	Angry,
	Happend,

	Null,
}


public class CSVScenarioData : MasterBase
{
	/*   public string Jump { get; private set; }
    public string RoteChangeFlag { get; private set; }
    public string Route { get; private set; }
    public string text { get; private set; }*/

	public string Scenario { get; private set; }
	public int CurrentRoute{ get; private set; }
	public int NextRoute   { get; private set; }
	public float WatchTime { get; private set; }
	public CharacterAnimator Animation { get; private set; }
	public CharacterState State { get; private set; }

}

