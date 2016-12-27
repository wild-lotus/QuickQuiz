using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace CgfGames {

	public interface ILeaderboardView : IScreenView {

		event Action BackClickedEvent;

		void Loading (bool loading);

		void ShowLeaderboard (LeaderboardEntry[] entries);
	}

	public class LeaderboardView : MonoBehaviour, ILeaderboardView {

		public event Action BackClickedEvent;

		public GameObject loadingImgGobj;

		public GameObject leaderboardGobj;

		void Awake () {
			Assert.IsNotNull (leaderboardGobj);
			Assert.IsNotNull (loadingImgGobj);
		}

		public void SetActive (bool active) {
			gameObject.SetActive (active);
		}

		public void Loading (bool loading) {
			loadingImgGobj.SetActive (loading);
			leaderboardGobj.SetActive (!loading);
		}

		public void ShowLeaderboard (LeaderboardEntry[] entries) {
			Text[] texts = leaderboardGobj.GetComponentsInChildren<Text> ();
			for (int i = 0; i < entries.Length; i++) {
				texts [2 * i].text = (entries[i].Position + 1) + ".";
				texts [2 * i + 1].text = entries[i].Score.ToString ();
				if (entries [i].IsPlayer) {
					texts [2 * i].color = Color.green;
					texts [2 * i + 1].color = Color.green;
				} else {
					texts [2 * i].color = Color.white;
					texts [2 * i + 1].color = Color.white;
				}
			}
		}


		public void OnBackClicked () {
			if (this.BackClickedEvent != null) {
				this.BackClickedEvent ();
			}
		}
	}
}
