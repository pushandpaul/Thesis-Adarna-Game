using UnityEngine;
using System.Collections;

public class CharacterData : MonoBehaviour {

	public ItemToGive item;
	public Animator anim;
	public bool allowSave = true;

	void Awake () {
		anim = GetComponent<Animator>();
		item = GetComponentInChildren<ItemToGive>(true);
	}
}
