using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Fungus;

public class DayapMinigame : MonoBehaviour {
	public Bar barUI;
	private Image bar;
	private Timer timer;
	private PlayerController player;
	private ObjectiveMapper objectiveMapper;
	private LevelLoader levelLoader;
	public Image[] dayapUI;
	private ObjectiveManager objectiveManager;
	private UIFader objectiveUIFader;

	public Sprite dayap;
	public ItemToGive itemToGive;

	public Flowchart flowchart;

	private float targetSize;
	public float addSize;
	private float currentSize;

	private bool success;
	private bool dialoguePlayed;

	private bool timerStarted = false;
	public float timerDuration;
	public float defaultDuration;
	public float changeDuration;

	public bool toIncrease = true;

	public int dayapCount = 7;

	void Start () {
		player = FindObjectOfType<PlayerController>();
		levelLoader = FindObjectOfType<LevelLoader>();
		bar = barUI.GetComponent<Image>();
		objectiveMapper = this.GetComponent<ObjectiveMapper>();
		objectiveManager = FindObjectOfType<ObjectiveManager>();
		objectiveUIFader = objectiveManager.objectivePanelFader;
		timer = FindObjectOfType<Timer>();

		//timer.startTimer();
		//endMiniGame("End on Start");
	}
	void Update () {
		if(objectiveUIFader.canvasGroup.alpha > 0)
			return;
		else if(objectiveUIFader.canvasGroup.alpha == 0 && !timerStarted){
			startMiniGame();
			timerStarted = true;
		}

		if(timer.checkIfOnGoing()){
			if(dayapCount > 0 && Input.GetKeyDown(KeyCode.E)){
				increaseSize();
				dayapCount--;
				dayapUI[dayapCount].GetComponent<UIFader>().FadeTo(1f, 0.5f);
				player.camera.zoomInZoomOut(2.5f, 3, true);
				player.GetComponentInChildren<Animator>().Play("Sugatan Sarili with Dayap");
				//if(itemToGive.GetComponent<SpriteRenderer>().sprite != dayap)
					//itemToGive.setItem(dayap);
			}
			else if(dayapCount == 0 && !dialoguePlayed){
				flowchart.ExecuteBlock("Ubos na Dayap");
				dialoguePlayed = true;
			}
			if(toIncrease){
				bar.fillAmount = Mathf.MoveTowards(bar.fillAmount, this.targetSize, Time.deltaTime * changeDuration);
				if(bar.fillAmount == this.targetSize)
					decreaseSize();
			}
			else{
				bar.fillAmount = Mathf.MoveTowards(bar.fillAmount, this.targetSize, Time.deltaTime * defaultDuration);
			}
			if(bar.fillAmount == 0f){
				success = false;
				endMiniGame("Game Over");
			}
		}
		else{
			success = true;
			endMiniGame("Success!");
		}
	}

	private void increaseSize(){
		toIncrease = true;
		this.targetSize = bar.fillAmount;
		this.targetSize += addSize;
		if(this.targetSize > 1f)
			this.targetSize = 1f;
	}

	private void decreaseSize(){
		toIncrease = false;
		this.targetSize = 0f;
	}


	private void startMiniGame(){
		timer.startTimer(timerDuration);
	}

	private void endMiniGame(string message){
		Debug.Log("Mingame Ends");
		Debug.Log(message);
		timer.stopTimer();
		this.enabled = false;
		if(success){
			objectiveMapper.checkIfCurrent_misc();
			levelLoader.Levels = LevelLoader.LevelSelect.ForestPedrasPlatas;
		}
			
		else{
			levelLoader.Levels = LevelLoader.LevelSelect.MinigameDayap;
		}
		levelLoader.launchScene();
	}


}
