using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TalasalitaanManager : MonoBehaviour {

	[System.Serializable]
	public class PartData{
		public int partIndex;
		public string saknongNumbers;
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

	//public TalasalitaanButton talasalitaanButtonPrefab;
	public TalasalitaanButton talasalitaanButtonPrefab;
	public Transform talasalitaanButtonHolder;
	public List<TalasalitaanButton> talasalitaanBtns;
	public Transform summaryUI;
	public Transform halimbawaUI;
	public Text halimbawaUIText;

	public GameObject chapterButtonPrefab;
	public Transform chapterSelectionUI;
	public Transform chapterSelectedBtn;
	public List<Transform> chapterSelectionBtns;
	private GameObject previousChapSelectBtn;

	void Start () {
		GameObject[] chapSelectBtnHolders = GameObject.FindGameObjectsWithTag("Chapter Button");
		string jsonString = File.ReadAllText(Application.dataPath + talasalitaanJsonPath);
		talasalitaanBtns = new List<TalasalitaanButton>();

		partDataList = JsonUtility.FromJson<PartDataList>(jsonString);
		ShowBook();
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

	public void ShowBook(){
		ShowPartDataUI(FindObjectOfType<ObjectiveManager>().currentPartIndex);
	}

	public void ShowPartDataUI(int part){
		halimbawaUI.gameObject.SetActive(false);
		chapterSelectedBtn.gameObject.SetActive(true);

		if(previousChapSelectBtn != null)
			previousChapSelectBtn.SetActive(true);

		if(part == 0){
			chapterSelectedBtn.GetComponentInChildren<Text>().text = "Tutorial";
		}
		else
			chapterSelectedBtn.GetComponentInChildren<Text>().text = "Kabanata " + part;

		if(chapterSelectionBtns[part].GetSiblingIndex() < chapterSelectedBtn.GetSiblingIndex()){
			chapterSelectedBtn.SetSiblingIndex(chapterSelectionBtns[part].GetSiblingIndex() + 1);
		}
		else
			chapterSelectedBtn.SetSiblingIndex(chapterSelectionBtns[part].GetSiblingIndex());

		previousChapSelectBtn = chapterSelectionBtns[part].gameObject;
		chapterSelectionBtns[part].gameObject.SetActive(false);


		foreach(TalasalitaanButton talasalitaanBtn in talasalitaanBtns){
			Destroy(talasalitaanBtn.gameObject);
		}

		talasalitaanBtns.Clear();
			
		foreach(Talasalitaan talasalitaan in partDataList.partsData[part].talasalitaans){
			TalasalitaanButton talasalitaanButton = (TalasalitaanButton) Instantiate (talasalitaanButtonPrefab);

			talasalitaanButton.transform.SetParent(talasalitaanButtonHolder);
			talasalitaanButton.transform.localScale = new Vector3(1,1,1);
			//if(talasalitaan.activated){
				talasalitaanButton.SetTexts(talasalitaan.salita, talasalitaan.kasingKahulugan, talasalitaan.halimbawa);
			//}
			//else
				//talasalitaanButton.GetComponent<Button>().interactable = false;
			talasalitaanBtns.Add(talasalitaanButton);
		}

		summaryUI.GetComponentInChildren<Text>().text = partDataList.partsData[part].summary;
		//summaryUI.GetComponentInChildren<Image>().sprite =
	}

	public void RewriteJson(){
		string jsonString = JsonUtility.ToJson(partDataList);
		Debug.Log(jsonString);
		File.WriteAllText(Application.dataPath + talasalitaanJsonPath, jsonString);
	}
}
