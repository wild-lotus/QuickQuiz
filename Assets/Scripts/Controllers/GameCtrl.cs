using Zenject;
using UniRx;

namespace CgfGames {

	public enum Screen { Main, Quiz, Leaderboard }

	public class GameCtrl {

		private IGameView _view;

		private IMainScreenCtrlFactory _mainScreenCtrlFactory;

		private IQuizCtrlFactory _quizCtrlFactory;

		private ILeaderboardCtrlFactory _leaderboardCtrlFactory;

		public GameCtrl (IGameView view, IMainScreenCtrlFactory mainScreenCtrlFactory, IQuizCtrlFactory quizCtrlFactory, ILeaderboardCtrlFactory leaderboardCtrlFactory ) {
			_view = view;
			_mainScreenCtrlFactory = mainScreenCtrlFactory;
			_quizCtrlFactory = quizCtrlFactory;
			_leaderboardCtrlFactory = leaderboardCtrlFactory;
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
					nextScreenDone = _mainScreenCtrlFactory.Create ().Start ();
					break;
				case Screen.Quiz:
					nextScreenDone = _quizCtrlFactory.Create ().Start ();
					break;
				case Screen.Leaderboard:
					nextScreenDone = _leaderboardCtrlFactory.Create ().Start ();
					break;
			}
			nextScreenDone
				.Subscribe (nextScreen => this.NavigateTo (nextScreen));
		}
	}
}
