using System;
using UnityEngine;

namespace CgfGames {

	public interface IMainScreenView : IScreenView {

		event Action StartQuizClickedEvent;

		event Action LeaderboardClickedEvent;
	}

	public class MainScreenView : MonoBehaviour, IMainScreenView {

		public event Action StartQuizClickedEvent;

		public event Action LeaderboardClickedEvent;

		void Awake () {
		}

		public void SetActive (bool active) {
			gameObject.SetActive (active);
		}

		public void OnStartQuizClicked () {
			if (this.StartQuizClickedEvent != null) {
				this.StartQuizClickedEvent ();
			}
		}

		public void OnLeaderboardClicked () {
			if (this.LeaderboardClickedEvent != null) {
				this.LeaderboardClickedEvent ();
			}
		}

		public void OnQuitClicked () {
			Application.Quit ();
		}
	}
}
