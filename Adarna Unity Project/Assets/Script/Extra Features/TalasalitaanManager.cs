using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TalasalitaanManager : MonoBehaviour {

	[System.Serializable]
	public class PartData{
		public int partIndex;
		public string title;
		public string saknongNumbers;
		public string summary;
		public List <Talasalitaan> talasalitaans;
		public bool isFinished;
	}

	[System.Serializable]
	public class PartDataList{
		public List<PartData> partsData;
	}

	public PartDataList partDataList;

	public string talasalitaanJsonPath;
	public string backupJsonPath;
	private string backupJson;

	//public TalasalitaanButton talasalitaanButtonPrefab;
	public TalasalitaanButton talasalitaanButtonPrefab;
	public Transform talasalitaanButtonHolder;
	public List<TalasalitaanButton> talasalitaanBtns;
	public Text partTitleUIText;
	public Transform summaryUI;
	public Image summaryImage;
	public Text saknongUIText;
	public Transform halimbawaUI;
	public Text halimbawaUIText;

	public GameObject chapterButtonPrefab;
	public Transform chapterSelectionUI;
	public Transform chapterSelectedBtn;
	public List<Transform> chapterSelectionBtns;
	private GameObject previousChapSelectBtn;

	public Transform hintPanel;
	private bool hintViewed = false;

	private GameManager gameManager;
	private ObjectiveManager objectiveManager;
	private AudioSource notificationSound;

	private UIFader myUIFader;
	public int newSalitaCount;
	public GameObject notif;

	public Sprite[] thumbnails; 

	void Awake () {
		GameObject[] chapSelectBtnHolders = GameObject.FindGameObjectsWithTag("Chapter Button");
		string jsonString = "";
		string actualPath = Path.Combine(Application.persistentDataPath, talasalitaanJsonPath + ".json");
		backupJson = File.ReadAllText(Application.dataPath + backupJsonPath);


		if(!File.Exists(actualPath)){
			//File.WriteAllText(Application.persistentDataPath + talasalitaanJsonPath, backupJson);
			File.WriteAllText(actualPath, backupJson);
		}

		jsonString = File.ReadAllText (actualPath);

		myUIFader = this.GetComponent<UIFader>();
		gameManager = FindObjectOfType<GameManager>();
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		talasalitaanBtns = new List<TalasalitaanButton>();
		notificationSound = GetComponent<AudioSource> ();

		partDataList = JsonUtility.FromJson<PartDataList>(jsonString);
		//Activate("Subyang");
		//Activate("Magniig");
		//ShowBook();

		foreach(PartData partData in partDataList.partsData){
			foreach(Talasalitaan talasalitaan in partData.talasalitaans){
				if(talasalitaan.newlyActivated){
					
					newSalitaCount++;
				}
			}
		}

		if(newSalitaCount > 0){
			notif.SetActive (true);
			notif.GetComponentInChildren<Text>().text = newSalitaCount.ToString();
		}

	}

	private void Activate(Talasalitaan talasalitaan){


		if(talasalitaan != null){
			Debug.Log("Found talasalitaan '" + talasalitaan.salita + "'.");
			if(!talasalitaan.activated){
				newSalitaCount++;
				notif.GetComponentInChildren<Text>().text = newSalitaCount.ToString();
				notif.SetActive(true);
				notificationSound.time = 1.1f;
				notificationSound.Play();
				talasalitaan.activated = true;
				talasalitaan.newlyActivated = true;

			}
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
		if(!gameManager.pauseRan){
			StartCoroutine(BookController(true));
			ShowPartDataUI(gameManager.latestPartIndex);
		}
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
			if(talasalitaan.activated){
				talasalitaanButton.Init(talasalitaan);
			}
			else{
				talasalitaanButton.button.interactable = false;
				talasalitaanButton.transform.SetAsLastSibling();
			}
				
			talasalitaanBtns.Add(talasalitaanButton);
		}

		setupSummaryData(part);

		//summaryUI.GetComponentInChildren<Image>().sprite =
	}

	void setupSummaryData(int part){
		string summaryText = partDataList.partsData[part].summary;
		string newText = "";
		string firstLetterFormat = "";
		Text summaryUIText = summaryUI.GetComponentInChildren<Text>();

		if(gameManager.latestPartIndex > part /*&& partDataList.partsData[part].isFinished*/){
			partTitleUIText.text = partDataList.partsData[part].title;
			firstLetterFormat = "<size=" + (summaryUIText.fontSize + 6) + "><b>" + summaryText.Substring(0,1) + "</b></size>";
			newText =  firstLetterFormat + summaryText.Substring(1, summaryText.Length-1);
			saknongUIText.text = partDataList.partsData[part].saknongNumbers;
			summaryUIText.text = newText;
			summaryImage.enabled = true;
			summaryImage.sprite = thumbnails [part];
		} 
		else if(gameManager.latestPartIndex == part){
			partTitleUIText.text = partDataList.partsData[part].title;
			saknongUIText.text = partDataList.partsData[part].saknongNumbers;
			summaryUI.GetComponentInChildren<Text>().text = "????";
			summaryImage.enabled = true;
			summaryImage.sprite = thumbnails [part];
		}
		else{
			partTitleUIText.text = "????";
			summaryUI.GetComponentInChildren<Text>().text = "????";
			saknongUIText.text = "????";
			summaryImage.enabled = false;
		}
	}

	public void ShowHint(){
		hintViewed = !hintViewed;
		hintPanel.gameObject.SetActive(hintViewed);
	}

	public void ExitBook(){
		bool thereIsNew = false;
		newSalitaCount = 0;

		foreach(PartData partData in partDataList.partsData){
			foreach(Talasalitaan talasalitaan in partData.talasalitaans){
				if(talasalitaan.newlyActivated){
					newSalitaCount ++;
				}
			}
		}

		if(newSalitaCount > 0){
			thereIsNew = true;
			notif.GetComponentInChildren<Text>().text = newSalitaCount.ToString();
		}

		notif.SetActive(thereIsNew);

		StartCoroutine(BookController(false));
	}

	public void RewriteJson(){
		string jsonString = JsonUtility.ToJson(partDataList);
		Debug.Log(jsonString);
		File.WriteAllText(Application.dataPath + talasalitaanJsonPath, jsonString);
	}

	public void ResetPartData(){
		newSalitaCount = 0;
		notif.SetActive (false);
		partDataList = JsonUtility.FromJson<PartDataList>(backupJson);
		File.WriteAllText(Application.dataPath + talasalitaanJsonPath, backupJson);
	}

	IEnumerator BookController(bool isLaunch){
		if(isLaunch){
			gameManager.pauseMenu.SetActive(false);
			gameManager.pauseRan = true;
			gameManager.blurredCam.gameObject.SetActive(true);
			myUIFader.FadeIn(0, 5f, false);
			while(myUIFader.canvasGroup.alpha < 1){
				yield return null;
			}
			gameManager.pause(true);
		}

		else{
			hintViewed = false;
			hintPanel.gameObject.SetActive(false);
			myUIFader.FadeOut(0, 5f);
			gameManager.pause(false);
			while(myUIFader.canvasGroup.alpha > 0){
				yield return null;
			}
			gameManager.pauseMenu.SetActive(true);
			gameManager.pauseRan = false;
			gameManager.blurredCam.gameObject.SetActive(false);

		}
	}
}
