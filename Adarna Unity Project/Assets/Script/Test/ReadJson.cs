using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class ReadJson : MonoBehaviour {

	public string jsonString;
	public JsonData myJsonData;
	public JsonData myJsonData2;


	void Start () {
		jsonString = File.ReadAllText(Application.dataPath + "/Resources/Text/test.json");
		myJsonData = JsonMapper.ToObject(jsonString);

		Debug.Log(myJsonData["talasalitaan"][0]["salita"]);

		File.WriteAllText(Application.dataPath + "/Resources/Text/test2.json", jsonString);
	}
}