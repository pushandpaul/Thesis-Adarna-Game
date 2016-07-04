using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {

	public GameObject BackUIFader;

	void Awake(){
		BackUIFader = GameObject.FindGameObjectWithTag ("Back UI Fader");
	}

	public void fadeScreenToBlack(float duration){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader> ().FadeIn(0, duration, false);
		}
	}

	public void fadeScreenToBlack(float duration, bool autoFadeOut, int autoFadeDelay){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader> ().FadeIn(autoFadeDelay, duration, autoFadeOut);
		}
	}

	public void fadeScreenToClear(float duration){
		if(BackUIFader != null){
			BackUIFader.GetComponent<UIFader> ().FadeOut(0, duration);
		}
	}

	public void fadeScreenToClear(float duration, int delay){
		if(BackUIFader != null){
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
		FindObjectOfType<TalasalitaanManager>().ActivateTalasalitaan(salita);
	}
}
