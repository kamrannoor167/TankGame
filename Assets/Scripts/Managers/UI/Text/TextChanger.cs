using TMPro;
using UnityEngine;
using UnityEngine.UI;


	public class TextChanger : MonoBehaviour {
		[Header("Configuration")]
	

		private Text _text;
		private int _count;
		private int _displayedCount;


		private int Count {
			get => _count;
			set {
				_count = value;

			UpdateText();

			}
		}

		private void Awake() {
			_text = GetComponent<Text>();
		}

		protected void ReceiveValue(object sender, int newCount) {
			Count = newCount;
		}

		private void UpdateText() {

		_text.text = Count.ToString();
		}
	}
