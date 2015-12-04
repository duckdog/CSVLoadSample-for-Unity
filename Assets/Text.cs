using UnityEngine;
using System.Collections;

public class Text : MonoBehaviour {

   


// Use this for initialization
void Start () {

        var MasterTable = new MasterTable();

        MasterTable.Load();
        foreach (var Master in MasterTable.All)
        {
            Debug.Log(Master.text);
        }

    }

    // Update is called once per frame
    void Update () {
	
	}
}
