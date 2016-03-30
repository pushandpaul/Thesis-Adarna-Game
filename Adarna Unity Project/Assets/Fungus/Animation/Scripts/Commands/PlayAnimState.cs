using UnityEngine;
using System;
using System.Collections;

namespace Fungus
{
	[CommandInfo("Animation", 
	             "Play Anim State", 
	             "Plays a state of an animator according to the state name")]
	[AddComponentMenu("")]
	public class PlayAnimState : Command 
	{
        [Tooltip("Reference to an Animator component in a game object")]
		public Animator animator;

		[Tooltip("Name of the state you want to play")]
		public string stateName;

        [Tooltip("Layer to play animation on")]
        public int layer=-1;

        [Tooltip("Start time of animation")]
        public float time=0f;

        public override void OnEnter()
		{
			if (animator != null)
			{
                animator.Play(stateName,layer,time);
			}

			Continue();
		}

		public override string GetSummary()
		{
			if (animator == null)
			{
				return "Error: No animator selected";
			}

            return animator.name + " (" + stateName + ")";
		}

		public override Color GetButtonColor()
		{
			return new Color32(170, 204, 169, 255);
		}
    }
    
}


