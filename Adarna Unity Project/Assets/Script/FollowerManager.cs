using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowerManager : MonoBehaviour {

	private FollowTarget[] localFollowers;
	public List<FollowTarget> activeFollowers;
	private PlayerController player;
	private GameManager gameManager;

	public void setActiveFollowers () {
		gameManager = FindObjectOfType<GameManager>();
		player = FindObjectOfType<PlayerController>();
		activeFollowers = new List<FollowTarget>();
		localFollowers = FindObjectsOfType<FollowTarget>();
		List <FollowTarget> unactiveFollowers = new List<FollowTarget>();
		if(localFollowers.Length > 0 && gameManager.FollowerNames.Count > 0){
			foreach(string followerName in gameManager.FollowerNames){
				for(int i = 0; i< localFollowers.Length; i++){
					if(localFollowers[i].name == followerName){
						activeFollowers.Add(localFollowers[i]);
						localFollowers[i].setIsFollowing(true);
						localFollowers[i].enabled = true;
					}
					else{
						unactiveFollowers.Add(localFollowers[i]);
					}
				}
			}
			setFollowerPositions();
		}
	}

	//Follower Methods
	void setFollowerPositions(){
		int i = 0;
		int j = 0;
		float xPosition = 0f;
		Transform followerTransform;
		if(activeFollowers != null){
			foreach(FollowTarget activeFollower in activeFollowers){
				i+=2;
				j+=3;
				activeFollower.thisConstructor(player.moveSpeed, j, player.transform, player.transform.localScale);
				if(player.transform.localScale.x < 0)
					xPosition = player.transform.position.x + i;
				else if(player.transform.localScale.x > 0)
					xPosition = player.transform.position.x - i;
				followerTransform = activeFollower.transform;
				followerTransform.position = new Vector3(xPosition, player.transform.position.y, player.transform.position.z);
			}
		}
	}

	void setFollowerDistances(){
		int i = 0;

		if(activeFollowers != null){
			foreach(FollowTarget activeFollower in activeFollowers){
				i += 3;
				activeFollower.distanceLimit = i;
			}
		}
	}

	public void activateFollower(FollowTarget follower){
		follower.enabled = true;
		follower.setIsFollowing(true);
		if(!activeFollowers.Contains(follower))
			activeFollowers.Add(follower);
	}

	public void removeFollower(FollowTarget follower, bool isDestroyed){
		foreach(FollowTarget activeFollower in activeFollowers){
			if(activeFollower == follower){
				activeFollower.enabled = false;
				activeFollowers.Remove(activeFollower);
				if(isDestroyed)
					Destroy(follower.gameObject);
				break;
			}
		}
	}

	public void removeFollowers(bool isDestroyed){	
		foreach (FollowTarget activeFollower in activeFollowers) {
			activeFollower.enabled = false;
			if (isDestroyed)
				Destroy (activeFollower);
		}

		activeFollowers.Clear ();
	}

	void removeUnactives(List <FollowTarget> unactiveFollowers){
		foreach(FollowTarget unactiveFollower in unactiveFollowers){
			Destroy(unactiveFollower.gameObject);
		}
	}

	public void updateFollowerList(){
		if(gameManager != null){
			gameManager.FollowerNames = new List<string> ();
		}
		//gameManager.FollowerNames.Clear();
		foreach(FollowTarget activeFollower in activeFollowers){
			gameManager.FollowerNames.Add(activeFollower.name);
		}
	}
}
