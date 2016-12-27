using UniRx;

namespace CgfGames {

	public enum Screen { Main, Quiz, Leaderboard }

	public class GameCtrl {

		private IGameView _view;

		public GameCtrl (IGameView view) {
			_view = view;
		}

		public void Start () {
			PlayFabManager.Login ();
			this.SetScreensInactive ();
			this.NavigateTo (Screen.Main);
		}

		private void SetScreensInactive () {
			_view.MainScreenView.SetActive (false);
			_view.QuizView.SetActive (false);
			_view.LeaderboardView.SetActive (false);
		}
		private void NavigateTo (Screen screen) {
			IObservable<Screen> nextScreenDone = null;
			switch (screen)
			{
				case Screen.Main:
					nextScreenDone = this.NewMainScreenCtrl().Start ();
					break;
				case Screen.Quiz:
					nextScreenDone = this.NewQuizCtrl().Start ();
					break;
				case Screen.Leaderboard:
					nextScreenDone = this.NewLeaderboardCtrl().Start ();
					break;
			}
			nextScreenDone
				.Subscribe (nextScreen => this.NavigateTo (nextScreen));
		}

		private IMainScreenCtrl NewMainScreenCtrl () {
			return new MainScreenCtrl (_view.MainScreenView);
		}

		private IQuizCtrl NewQuizCtrl () {
			return new QuizCtrl (
				_view.QuizView,
				new QuizRepo (),
				new QuizProgressCtrl ()
			);
		}

		private ILeaderboardCtrl NewLeaderboardCtrl () {
			return new LeaderboardCtrl (
				_view.LeaderboardView, new QuizRepo ()
			);
		}
	}
}
