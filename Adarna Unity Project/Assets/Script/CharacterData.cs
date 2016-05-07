using UnityEngine;
using System.Collections;

public class CharacterData : MonoBehaviour {

	public ItemToGive item;
	public Animator anim;

	void Start () {
		anim = GetComponent<Animator>();
		item = GetComponentInChildren<ItemToGive>(true);
	}
}
