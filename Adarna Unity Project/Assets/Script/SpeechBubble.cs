using UnityEngine;
using System.Collections;

public class SpeechBubble : MonoBehaviour {
	Transform[] allChildren;

	void Start () {
		allChildren = GetComponentsInChildren<Transform> ();
		displayBubble (false);
	}

	public void displayBubble(bool check) {
		foreach (Transform child in allChildren) {
			setChildToActive (child);
			child.GetComponent<Renderer>().enabled = check;
		}
	}

	private void setChildToActive(Transform child) {
		if (!child.gameObject.activeInHierarchy) {
			child.gameObject.SetActive (true);
		}
	}
}
