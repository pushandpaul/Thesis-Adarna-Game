using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LayuninUIManager : MonoBehaviour {
	public static string lastTriggered;
	private bool allowPanelPress;
	private bool closing;

	public enum CloseControl{
		PressAnywhere,
		PressW,
		PressA,
		PressS,
		PressD,
		PressE,
	}
	private GameManager gameManager;

	public UIFader layuninUI;
	public Text title;
	public Text description;
	public Text closeInstruction;
	private KeyCode closeKey;

	private bool showMini;
	public UIFader layuninMiniUI;
	public Text layuninMiniText;

	void Awake(){
		lastTriggered = "";
		gameManager = FindObjectOfType<GameManager>();
	}

	void Start(){
		
		//Launch("This is my title", "hope you like it", CloseControl.PressAnywhere, true);
	}

	void Update(){
		if(layuninUI.canvasGroup.alpha == 0f)
			return;

		if(Input.GetKeyDown(closeKey) && !allowPanelPress && !closing){
			Close();
			closing = true;
		}
	}

	public void Launch(string title, string description, CloseControl closeControl, bool showMini){
		StartCoroutine(Launching(title, description, closeControl, showMini));
	}

	IEnumerator Launching(string title, string description, CloseControl closeControl, bool showMini){
		string closeKeyTemp = "";
		this.title.text = title;
		this.description.text = description;
		this.closeInstruction.text = "(" + closeInstruction + ")";
		this.showMini = showMini;

		gameManager.pauseMenu.SetActive(false);
		if(closeControl == CloseControl.PressAnywhere){
			this.GetComponent<Button>().interactable = true;
			allowPanelPress = true;
			closeKeyTemp = "Mag-click kahit saaan";
		}
		else{
			this.GetComponent<Button>().interactable = false;
			allowPanelPress = false;

			switch(closeControl){
			case(CloseControl.PressW): closeKey = KeyCode.W;
				closeKeyTemp = "Pindutin ang 'W'";
				break;
			case(CloseControl.PressA): closeKey = KeyCode.A;
				closeKeyTemp = "Pindutin ang 'A'";
				break;
			case(CloseControl.PressS): closeKey = KeyCode.S;
				closeKeyTemp = "Pindutin ang 'S'";
				break;
			case(CloseControl.PressD): closeKey = KeyCode.D;
				closeKeyTemp = "Pindutin ang 'D'";
				break;
			case(CloseControl.PressE): closeKey = KeyCode.E;
				closeKeyTemp = "Pindutin ang 'E'";
				break;
			}
		}
		this.closeInstruction.text = "(" + closeKeyTemp + " upang tumuloy...)";
		layuninUI.canvasGroup.alpha = 1f;
		layuninUI.canvasGroup.interactable = true;
		layuninUI.canvasGroup.blocksRaycasts = true;
		gameManager.blurredCam.gameObject.SetActive(true);

		foreach(GameObject HUD in GameObject.FindGameObjectsWithTag("HUD")){
			HUD.GetComponent<CanvasGroup>().alpha = 0f;
		}

		while(FindObjectOfType<ScreenFader>().alpha > 0){
			yield return null;
		}

		gameManager.pause(true);
	}
		
	public void Close(){
		StartCoroutine(Closing());
	}

	IEnumerator Closing(){
		layuninUI.FadeOut(0, 5f);
		gameManager.pause(false);
		while(layuninUI.canvasGroup.alpha > 0){
			yield return null;
		}

		closing = false;
		gameManager.blurredCam.gameObject.SetActive(false);
		gameManager.pauseMenu.SetActive(true);
		if(showMini){
			layuninMiniText.text = description.text;
			layuninMiniUI.FadeIn(10, 5f, true);
		}

		foreach(GameObject HUD in GameObject.FindGameObjectsWithTag("HUD")){
			HUD.GetComponent<CanvasGroup>().alpha = 1f;
		}
	}
}
