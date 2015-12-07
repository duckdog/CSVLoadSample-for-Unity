using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ScenarioSetter : MonoBehaviour {


	public enum Route
	{
		Main = 0,
		A = 1,
		B = 2,
		C = 3,

		NULL =-1,//Minigameに遷移の予定

	}


	//テキスト一行分に格納されるデータ一覧
	//テキストデータ、※表示時間、次に表示するルート,アニメーション命令
	//追加予定:効果音、CV等の命令
	public struct Scenariodate
	{

		public string _text_date;
		public float _time;
		public Route _next_route;

		public Scenariodate(string text_date,float time ,Route next_route = Route.Main)
		{
			this._text_date = text_date;
			//_change_flag = change_flag;
			this._next_route = next_route;
			this._time = time;
		}
	}

	
	//メインシナリオ

	//テキスト格納用
	List<Scenariodate> _Main = new List<Scenariodate>();
	List<Scenariodate> _A = new List<Scenariodate>();
	List<Scenariodate> _B = new List<Scenariodate>();
	List<Scenariodate> _C = new List<Scenariodate>();

	string[] _main_route_text;
	string[] _A_route_text;
	string[] _B_route_text;
	string[] _C_route_text;

	Route _next_route;
	string _current_text;
	float _timer;
	int[,] _current_text_number = new int[4,1];
	public int CurrentTextNumber_A{
		get {return _current_text_number[(int)Route.A,0]; }
		set { _current_text_number [(int)Route.A, 0] = value;}
	}
	public int CurrentTextNumber_B{
		get {return _current_text_number[(int)Route.B,0]; }
		set { _current_text_number [(int)Route.B, 0] = value;}
	}
	public int CurrentTextNumber_C{
		get {return _current_text_number[(int)Route.C,0]; }
		set { _current_text_number [(int)Route.C, 0] = value;}
	}

	int _main_text_number = 0;
	int _a_text_number = 0;
	int _b_text_number = 0;
	int _c_text_number = 0;

	//フォント用変数
    public GUISkin s_Skin;
	GUIStyle Style;
	GUIStyleState State;

	public float[] timer;

	int gameroot = 0;

	// Use this for initialization
	void Start () {


		Style = new GUIStyle();
		State = new GUIStyleState();

		//CSVデータから、テキストデータ等を読み込む
		var MasterTable = new CSVMasterTable();
		MasterTable.Load();
		foreach (var Master in MasterTable.All)
		{

			switch((Route)Master.CurrentRoute){

			case Route.Main:


				_Main.Add(new Scenariodate(Master.Scenario,Master.WatchTime,(Route)Master.NextRoute));

				break;
			case Route.A:

				_A.Add(new Scenariodate(Master.Scenario,Master.WatchTime,(Route)Master.NextRoute));


				break;
			case Route.B:

				_B.Add(new Scenariodate(Master.Scenario,Master.WatchTime,(Route)Master.NextRoute));


				break;
			case Route.C:

				_C.Add(new Scenariodate(Master.Scenario,Master.WatchTime,(Route)Master.NextRoute));


				break;

			}

		}

		_A.Add(new Scenariodate("ENDTEXT",0,Route.Main));
		_B.Add(new Scenariodate("ENDTEXT",0,Route.Main));
		_C.Add(new Scenariodate("ENDTEXT",0,Route.Main));

		_timer = _Main [0]._time;
		_current_text = _Main [0]._text_date;
		_next_route = Route.Main;

		//StartCoroutine(WaitTimeAndGo());

	}


	// Update is called once per frame
	void Update () {


				
	    	if (_timer > 0) {
			
			_timer -= Time.deltaTime;

		} else {

			//テキストデータを更新
			UpdateScenerio (_next_route);
			/*			switch (_next_route) {

			case Route.Main:


				_current_text = _Main [_current_text_number [(int)Route.Main, 0]]._text_date;
				_timer = _Main [_current_text_number [(int)Route.Main, 0]]._time;
				_next_route = _Main [_current_text_number [(int)Route.Main, 0]]._next_route;
				_current_text_number [(int)Route.Main, 0]++;
				break;

			case Route.A:

				_current_text = _A [_current_text_number [(int)Route.A, 0]]._text_date;
				_timer = _A [_current_text_number [(int)Route.A, 0]]._time;
				_next_route = _A [_current_text_number [(int)Route.A, 0]]._next_route;
				_current_text_number [(int)Route.A, 0]++;

				break;

			case Route.B:
				_current_text = _B [_current_text_number [(int)Route.B, 0]]._text_date;
				_timer = _B [_current_text_number [(int)Route.B, 0]]._time;
				_next_route = _B [_current_text_number [(int)Route.B, 0]]._next_route;
				_current_text_number [(int)Route.B, 0]++;

				break;
			case Route.C:
				Debug.Log ("C " + _current_text_number [(int)Route.C, 0]);
				_current_text = _C [_current_text_number [(int)Route.C, 0]]._text_date;
				_timer = _C [_current_text_number [(int)Route.C, 0]]._time;
				_next_route = _C [_current_text_number [(int)Route.C, 0]]._next_route;
				_current_text_number [(int)Route.C, 0]++;

				break;

			}
*/
		}



		//ミニゲーム等で、テキスト表示を一旦中止.
		//デバッグキーがなくなったら、スイッチ分の中に入れるべき
		if (_next_route == Route.NULL && _timer <= 0) {
		

			_current_text = "";
			//Fix: α版用デバッグキー　右クリックで、メインシナリオに遷移
			if (Input.GetMouseButtonDown (0)) {
				_next_route = Route.Main;
				UpdateScenerio (_next_route);
			}

			if (Input.GetKeyDown(KeyCode.A)) {
				_next_route = Route.A;
				UpdateScenerio (_next_route);

				do{
					//シナリオをスキップ//次の分岐のテキストまで配列番号を更新
					CurrentTextNumber_B++;
				}
				while((_B[CurrentTextNumber_B]._next_route != Route.Main));
				
				do{
					//シナリオをスキップ//次の分岐のテキストまで配列番号を更新
					if(CurrentTextNumber_C >= _C.Count) break;
					CurrentTextNumber_C++;
				}
				while((_C[CurrentTextNumber_C]._next_route != Route.Main));

			}
			if (Input.GetKeyDown(KeyCode.B)) {
				_next_route = Route.B;
				UpdateScenerio (_next_route);
				do{
					//シナリオをスキップ//次の分岐のテキストまで配列番号を更新
					CurrentTextNumber_A++;
				}
				while(( _A[CurrentTextNumber_A]._next_route != Route.Main));
				
				do{
					//シナリオをスキップ//次の分岐のテキストまで配列番号を更新
					if(CurrentTextNumber_C >= _C.Count) break;
					CurrentTextNumber_C++;
				}
				while((_C[CurrentTextNumber_C]._next_route != Route.Main));

			}
			if (Input.GetKeyDown(KeyCode.C)) {
				_next_route = Route.C;
				UpdateScenerio (_next_route);


				do{
					//シナリオをスキップ//次の分岐のテキストまで配列番号を更新
					CurrentTextNumber_A++;
				}
				while((_A[CurrentTextNumber_A]._next_route != Route.Main));
				
				do{
					//シナリオをスキップ//次の分岐のテキストまで配列番号を更新
					CurrentTextNumber_B++;
				}
				while((_B[CurrentTextNumber_B]._next_route != Route.Main));
			}
		
		
		}



	}


	void UpdateScenerio(Route route)
	{
		switch (route) {
		case Route.Main:
			_current_text = _Main [_current_text_number [(int)route, 0]]._text_date;
			_timer        = _Main [_current_text_number [(int)route, 0]]._time;
			_next_route   = _Main [_current_text_number [(int)route, 0]]._next_route;
			_current_text_number [(int)route, 0]++;
			break;
		case Route.A:
			_current_text = _A [_current_text_number [(int)route, 0]]._text_date;
			_timer        = _A [_current_text_number [(int)route, 0]]._time;
			_next_route   = _A [_current_text_number [(int)route, 0]]._next_route;
			_current_text_number [(int)route, 0]++;
			break;
		case Route.B:
			_current_text = _B [_current_text_number [(int)route, 0]]._text_date;
			_timer        = _B [_current_text_number [(int)route, 0]]._time;
			_next_route   = _B [_current_text_number [(int)route, 0]]._next_route;
			_current_text_number [(int)route, 0]++;
			break;
		case Route.C:
			_current_text = _C [_current_text_number [(int)route, 0]]._text_date;
			_timer        = _C [_current_text_number [(int)route, 0]]._time;
			_next_route   = _C [_current_text_number [(int)route, 0]]._next_route;
			_current_text_number [(int)route, 0]++;
			break;
		}
	}

	IEnumerator WaitTimeAndGo()
	{
		for (int i = 0; i < timer.Length;i++ )
		{
			Debug.Log(timer);
			yield return new WaitForSeconds(timer[i]);
			//text_number++;
		}
	}

	void OnGUI()
	{

		GUI.skin = s_Skin;

		Style.fontSize = Screen.height/22;
		Style.normal = State;

		State.textColor = Color.black;

		GUI.Label(new Rect(Screen.width/25, Screen.height / 2 + Screen.height / 4, 0, Screen.height / 10), _current_text, Style);

	}
}
