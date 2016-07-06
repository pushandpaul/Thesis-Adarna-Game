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
	}

	private void Activate(Talasalitaan talasalitaan){
		if(talasalitaan != null){
			Debug.Log("Found talasalitaan '" + talasalitaan.salita + "'.");
			talasalitaan.activated = true;
		}
	}

	public void Activate(string salita){
		Activate(Search(salita));
	}

	public void ActivateInPart(string salita, int index){
		Activate(SearchInPart(salita, index));
	}

	public void ActivateInPart(string salita){
		Activate(SearchInPart(salita));
	} 


	public Talasalitaan Search(string salita){
		foreach(PartData partData in partDataList.partsData){
			foreach(Talasalitaan talasalitaan in partData.talasalitaans){
				if(talasalitaan.salita.ToLower() == salita.ToLower()){
					return talasalitaan;
				}
			}
		}
		return null;
	}

	public Talasalitaan SearchInPart(string salita){
		return SearchInPart(salita, FindObjectOfType<ObjectiveManager>().currentPartIndex);
	}


	public Talasalitaan SearchInPart(string salita, int index){
		foreach(Talasalitaan talasalitaan in partDataList.partsData[index].talasalitaans){
			if(talasalitaan.salita.ToLower() == salita.ToLower()){
				return talasalitaan;
			}
		}
		return null;
	}


	public void RewriteJson(){
		string jsonString = JsonUtility.ToJson(partDataList);
		Debug.Log(jsonString);
		File.WriteAllText(Application.dataPath + talasalitaanJsonPath, jsonString);
	}
}
