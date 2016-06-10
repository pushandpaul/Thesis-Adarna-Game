using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemCollectionManager : MonoBehaviour {


	public List <ItemToCollect> itemsToCollect;
	private List <ItemCounter> counters;
	public Transform counterHolder;
	public Transform counterPrefab;

	[System.Serializable]
	public class ItemToCollect{
		public string Name;
		public string setName;
		public bool carryItem = false;
		public bool collected = false;

		public AnimationClip collectAnimation;
		public ItemToCollect(string Name, string setName, bool carryItem, AnimationClip collectAnimation){
			this.Name = Name;
			this.setName = setName;
			this.carryItem = carryItem;
			this.collectAnimation = collectAnimation;
		}
	}

	[System.Serializable]
	public class ItemCounter{
		public string setName;
		public int count = 0;

		public Transform counterUI;
		private Text countUI;

		public ItemCounter(string setName, Transform counterUI, Text countUI){
			this.setName = setName;
			this.counterUI = counterUI;
			this.countUI = countUI;
			setCountUI(true);
		}

		public void setCountUI(bool increment){
			if(increment){
				count ++;
			}
			else{
				count--;
			}
			countUI.text = count.ToString();
		}
	}

	void Awake(){
		itemsToCollect = new List<ItemToCollect>();
		counters = new List<ItemCounter>();
	}

	public void AddToCollect(string Name, string setName, bool carryItem, AnimationClip collectAnimation, Sprite icon){
		bool found = false;
		ItemCounter itemCounter = FindItemCounter(setName);
		Transform counterUI;
		Image[] tempIconUIs;

		itemsToCollect.Add(new ItemToCollect(Name, setName, carryItem, collectAnimation));

		if(itemCounter != null){
			itemCounter.setCountUI(true);
		}

		else{
			counterUI = (Transform) Instantiate (counterPrefab, Vector3.zero, Quaternion.identity);
			counterUI.SetParent(counterHolder);
			counterUI.SetAsFirstSibling();
			counterUI.localScale = new Vector3(1,1,1);
			tempIconUIs = counterUI.GetComponentsInChildren<Image>();

			foreach(Image tempIconUI in tempIconUIs){
				if(tempIconUI.transform != counterUI){
					tempIconUI.sprite = icon;
					break;
				}
			}
			counters.Add(new ItemCounter(setName, counterUI, counterUI.GetComponentInChildren<Text>()));
		}
	}

	public void Collect(GameObject item){
		ItemCounter itemCounter;

		foreach(ItemToCollect itemToCollect in itemsToCollect){
			if(item.name == itemToCollect.Name){
				itemToCollect.collected = true;
				itemCounter = FindItemCounter(itemToCollect.setName);
				itemCounter.setCountUI(false);
				if(itemCounter.count == 0){
					counters.Remove(itemCounter);
					Destroy(itemCounter.counterUI);
				}
				itemsToCollect.Remove(itemToCollect);
				StartCoroutine(StartCollect(itemToCollect, item));
				if(itemsToCollect.Count == 0)
					CollectedAll();
				break;
			}
		}
	}

	void CollectedAll(){
		Debug.Log("All items collected");
		this.GetComponent<ObjectiveMapper>().checkIfCurrent_misc();
	}

	ItemCounter FindItemCounter(string setName){
		foreach(ItemCounter itemCounter in counters){
			if(setName == itemCounter.setName){
				return itemCounter;
			}
		}
		return null;
	}

	IEnumerator StartCollect(ItemToCollect itemToCollect, GameObject item){
		PlayerController player = FindObjectOfType<PlayerController>();
		Animator anim = player.GetComponentInChildren<Animator>();
		GameManager gameManager = FindObjectOfType<GameManager>();

		player.canMove = false;
		player.item.setItem(item.GetComponent<SpriteRenderer>().sprite);
		anim.Play(itemToCollect.collectAnimation.name);

		StartCoroutine(DelayDestroyItem(player.item, item));
		yield return new WaitForSeconds(itemToCollect.collectAnimation.length);
		player.canMove = true;

		if(!itemToCollect.carryItem){
			player.setPlayerState(gameManager.initPlayerIdleStateHash);
		}
			
		else
			player.setPlayerState(anim.GetCurrentAnimatorStateInfo(0).shortNameHash);		
	}

	IEnumerator DelayDestroyItem(ItemToGive playerItem, GameObject item){
		SaveAndDestroy saveAndDestroy = FindObjectOfType<SaveAndDestroy>();
		SpriteRenderer itemRenderer = playerItem.GetComponent<SpriteRenderer>();
		itemRenderer.enabled = false;
		while(!itemRenderer.enabled){
			yield return null;
		}
		saveAndDestroy.saveAndDestroy(item);
	}
}
