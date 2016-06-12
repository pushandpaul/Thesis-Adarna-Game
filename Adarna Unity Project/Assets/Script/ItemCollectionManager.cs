using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ItemCollectionManager : MonoBehaviour {


	public List <ItemToCollect> itemsToCollect;
	//private List <ItemCounter> counters;
	public Transform counterHolder;
	public Transform counterPrefab;
	public Transform itemIconPrefab;

	private int collectedCount = 0;

	//public float notCollectedItemAlpha;

	[System.Serializable]
	public class ItemToCollect{
		public string Name;
		public bool carryItem = false;
		public bool collected = false;
		public Image iconUI;

		public AnimationClip collectAnimation;
		public ItemToCollect(string Name, bool carryItem, AnimationClip collectAnimation, Image iconUI){
			this.Name = Name;
			this.carryItem = carryItem;
			this.collectAnimation = collectAnimation;
			this.iconUI = iconUI;
		}
	}

	/*[System.Serializable]
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
	}*/

	void Awake(){
		itemsToCollect = new List<ItemToCollect>();
		//counters = new List<ItemCounter>();
	}

	public void AddToCollect(string Name, bool carryItem, AnimationClip collectAnimation, Sprite icon){
		bool found = false;
		//ItemCounter itemCounter = FindItemCounter(setName);
		Transform counterUI;
		Transform imageUIHolder;
		Image iconUI;
		Image[] tempIconUIs;

		imageUIHolder = (Transform)Instantiate (itemIconPrefab, Vector3.zero, Quaternion.identity);
		iconUI = imageUIHolder.GetComponent<Image>();
		iconUI.sprite = icon; 
		imageUIHolder.SetParent (counterHolder);
		imageUIHolder.SetAsFirstSibling ();
		imageUIHolder.localScale = new Vector3 (1, 1, 1);

		itemsToCollect.Add(new ItemToCollect(Name, carryItem, collectAnimation, iconUI));
		/*if(itemCounter != null){
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
		}*/
			

	}

	public void Collect(GameObject item){
		//ItemCounter itemCounter;

		foreach(ItemToCollect itemToCollect in itemsToCollect){
			if(item.name == itemToCollect.Name && !itemToCollect.collected){
				itemToCollect.collected = true;
				collectedCount++;
				//itemCounter = FindItemCounter(itemToCollect.setName);
				//itemCounter.setCountUI(false);
				//if(itemCounter.count == 0){
					//counters.Remove(itemCounter);
					//Destroy(itemCounter.counterUI.gameObject);
					//Destroy (itemCounter.counterUI);
				//}
				StartCoroutine(StartCollect(itemToCollect, item));
				if(itemsToCollect.Count == collectedCount)
					CollectedAll();
				break;
			}
		}
	}

	public void EndCollect(){
		List<Image> imageUIs = new List<Image> ();
		int imageUIsLength = 0;
		foreach(ItemToCollect itemToCollect in itemsToCollect){
			imageUIs.Add (itemToCollect.iconUI);
			Debug.Log (itemToCollect.iconUI. name);
		}
		imageUIsLength = imageUIs.Count;
		itemsToCollect.Clear ();

		for(int i = 0; i < imageUIsLength; i++){
			//imageUIs [i].enabled = true;
			Destroy (imageUIs [i].gameObject);
		}
	}

	void CollectedAll(){
		Debug.Log("All items collected");
		this.GetComponent<ObjectiveMapper>().checkIfCurrent_misc();
		EndCollect ();
	}

	/*ItemCounter FindItemCounter(string setName){
		foreach(ItemCounter itemCounter in counters){
			if(setName == itemCounter.setName){
				return itemCounter;
			}
		}
		return null;
	}*/

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

		if(itemToCollect.iconUI != null)
			itemToCollect.iconUI.color = new Color(itemToCollect.iconUI.color.r, itemToCollect.iconUI.color.g, itemToCollect.iconUI.color.b, 1f);
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
