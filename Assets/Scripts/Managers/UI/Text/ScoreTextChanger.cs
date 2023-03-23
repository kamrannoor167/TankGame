using UnityEngine;


	public class ScoreTextChanger : TextChanger {
		private void OnEnable() {
			ResourceHolder.ScoreChanged += ReceiveValue;
		}

		private void Start() {
			ReceiveValue(null, ResourceHolder.Score);
		}
		
		private void OnDisable() {
			ResourceHolder.ScoreChanged -= ReceiveValue;
		}
	}
