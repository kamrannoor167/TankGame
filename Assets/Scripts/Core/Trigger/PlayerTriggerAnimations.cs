
using UnityEngine;

	public class PlayerTriggerAnimations : PlayerInTrigger {	
		private IAnimation[] animations;

		private void Awake() {
			animations = GetComponents<IAnimation>();
		}

		protected override void Action(GameObject player) {
			var playerPosition = player.transform.position;
			foreach (var animation in animations) {
				animation.Show(playerPosition);
			}
		}
	}
