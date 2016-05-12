using UnityEngine;
using System.Collections;

public class CharacterData : MonoBehaviour {

	public ItemToGive item;
	public Animator anim;

	public bool allowSave = true;
	public bool saveThis = false;

	public int stateHashID;
	public Sprite heldItem;
	public string state;

	void Awake () {
		anim = GetComponent<Animator>();
		item = GetComponentInChildren<ItemToGive>(true);
	}

	public void Init(string state){
		this.stateHashID = Animator.StringToHash(state);
		this.heldItem = item.getItem();
	}

	public void Init(string state, Sprite heldItem){
		this.stateHashID = Animator.StringToHash(state);
		this.heldItem = heldItem;
	}
}
