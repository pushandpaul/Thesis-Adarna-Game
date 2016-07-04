using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TalasalitaanManager : MonoBehaviour {

	[System.Serializable]
	public class PartData{
		public int partIndex;
		public string summary;
		public List <Talasalitaan> talasalitaans;
		bool isFinished;
	}

	[System.Serializable]
	public class PartDataList{
		public List<PartData> partsData;
	}

	public PartDataList partDataList;

	public string talasalitaanJsonPath;

	void Start () {
		string jsonString = File.ReadAllText(Application.dataPath + talasalitaanJsonPath);

		partDataList = JsonUtility.FromJson<PartDataList>(jsonString);

		ActivateTalasalitaan("salita1");
	}

	public void ActivateTalasalitaan(string salita){
		//SearchTalasalitaan(salita).activated = true;
	}

	public Talasalitaan SearchTalasalitaan(string salita){
		/*foreach(Talasalitaan talasalitaan in talasalitaanList.talasalitaan){
			if(salita == talasalitaan.salita){
				return talasalitaan;
			}
		}
		return null;*/
		return null;
	}

	public void RewriteJson(){
		string jsonString = JsonUtility.ToJson(partDataList);
		Debug.Log(jsonString);
		File.WriteAllText(Application.dataPath + talasalitaanJsonPath, jsonString);
	}
}
