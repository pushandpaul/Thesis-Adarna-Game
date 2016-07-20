using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {

	public GameObject BackUIFader;
	private GameManager gameManager;

	void Awake(){
		BackUIFader = GameObject.FindGameObjectWithTag ("Back UI Fader");
		gameManager = FindObjectOfType<GameManager>();
	}

	public void fadeScreenToBlack(float duration){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader>().StopAllCoroutines();
			BackUIFader.GetComponent<UIFader> ().FadeIn(0, duration, false);
		}
	}

	public void fadeScreenToBlack(float duration, bool autoFadeOut, int autoFadeDelay){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader>().StopAllCoroutines();
			BackUIFader.GetComponent<UIFader> ().FadeIn(autoFadeDelay, duration, autoFadeOut);
		}
	}

	public void fadeScreenToClear(float duration){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader>().StopAllCoroutines();
			BackUIFader.GetComponent<UIFader> ().FadeOut(0, duration);
		}
	}

	public void fadeScreenToClear(float duration, int delay){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader>().StopAllCoroutines();
			BackUIFader.GetComponent<UIFader> ().FadeOut(delay, duration);
		}
	}

	public void setExitAccess(GameObject exit, bool isOpen){
		bool isDoor = false;
		bool anExit = false;

		if(exit.GetComponent<ExitManager>() != null){
			anExit = true;
			isDoor = false;
		}

		else if(exit.GetComponent<DoorHandler>() != null){
			anExit = true;
			isDoor = true;
		}

		if(anExit)
			FindObjectOfType<DoorAndExitController>().SetExitAccess(exit.name, isOpen, isDoor);
	}

	public void setExitAccess(string exitName, bool isOpen, bool isDoor){
		FindObjectOfType<DoorAndExitController>().SetExitAccess(exitName, isOpen, isDoor);
	}
		
	public void setExitAccess(string sceneName, string exitName, bool isOpen, bool isDoor){
		FindObjectOfType<DoorAndExitController>().SetExitAccess(sceneName, exitName, isOpen, isDoor);
	}

	public void setCharacterKinematic(Rigidbody2D myRigidBody, bool allow){
		myRigidBody.isKinematic = allow;
	}

	public void addToCollect(string Name, bool carryItem, AnimationClip collectAnimation, Sprite icon){
		FindObjectOfType<ItemCollectionManager> ().AddToCollect (Name, carryItem, collectAnimation, icon);
	}

	public void activateTalasalitaan (string salita){
		FindObjectOfType<TalasalitaanManager>().Activate(salita);
	}

	public void activateTalasalitaanInPart(string salita){
		FindObjectOfType<TalasalitaanManager>().ActivateInPart(salita);
	}
	public void activateTalasalitaanInPart(int part, string salita){
		FindObjectOfType<TalasalitaanManager>().ActivateInPart(salita, part);
	}

	public void LaunchTutorial(int index){
		gameManager.tutorialManager.Launch(index);
	}

	public void changePlayerSpeedLater(float speed){
		gameManager.playerSpeed = speed;
	}

	public void changePlayerSpeedNow(float speed){
		PlayerController player = FindObjectOfType<PlayerController>();
		gameManager.playerSpeed = speed;
		if(player != null){
			player.setSpeed(speed);
		}
	}

	public void changeTimeOfDayLater(string timeOfDay){
		gameManager.timeOfDay = timeOfDay.ToCharArray()[0];
	}
}
