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

	public Sprite dayap;
	public ItemToGive itemToGive;

	public Flowchart flowchart;

	private float targetSize;
	public float addSize;
	private float currentSize;

	private bool success;
	private bool dialoguePlayed;

	public float defaultDuration;
	public float changeDuration;

	public bool toIncrease = true;

	public int dayapCount = 7;

	// Use this for initialization
	void Start () {
		player = FindObjectOfType<PlayerController>();
		levelLoader = FindObjectOfType<LevelLoader>();
		bar = barUI.GetComponent<Image>();
		objectiveMapper = this.GetComponent<ObjectiveMapper>();
		timer = FindObjectOfType<Timer>();

		timer.startTimer();
	}
	
	// Update is called once per frame
	void Update () {
		if(timer.checkIfOnGoing()){
			if(dayapCount > 0 && Input.GetKeyDown(KeyCode.E)){
				increaseSize();
				dayapCount--;
				dayapUI[dayapCount].GetComponent<UIFader>().FadeTo(1f, 0.5f);
				player.GetComponentInChildren<Animator>().Play("Give Item");
				if(itemToGive.GetComponent<SpriteRenderer>().sprite != dayap)
					itemToGive.setItem(dayap);
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
				endMinigame("Game Over");
			}
		}
		else{
			success = true;
			endMinigame("Success!");
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

	private void endMinigame(string message){
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
