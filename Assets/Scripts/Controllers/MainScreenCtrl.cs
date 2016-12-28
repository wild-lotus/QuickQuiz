using Zenject;
using UniRx;

namespace CgfGames {

	public interface IMainScreenCtrl : IScreenCtrl {

		void OnStartQuizClicked ();
	}

	public class IMainScreenCtrlFactory : Factory<IMainScreenCtrl> {}


	public class MainScreenCtrl : IMainScreenCtrl {

		#region Private fields
		//======================================================================

		private readonly Subject<Screen> _done;


		private readonly IMainScreenView _view;

		#endregion

		#region Constructor
		//======================================================================

		public MainScreenCtrl (IMainScreenView view) {
			_done = new Subject<Screen> ();
			_view = view;
		}

		#endregion

		#region IMainScreenCtrl implementation
		//======================================================================

		public IObservable<Screen> Start () {
			_view.SetActive (true);
			_view.StartQuizClickedEvent += this.OnStartQuizClicked;
			_view.LeaderboardClickedEvent += this.OnLeaderboardClicked;
			return _done;
		}

		public void OnStartQuizClicked () {
			this.Close (Screen.Quiz);
		}

		public void OnLeaderboardClicked () {
			this.Close (Screen.Leaderboard);
		}
		
		// Update is called once per frame
		public void Close (Screen nextScreen) {
			_view.SetActive (false);
			_view.StartQuizClickedEvent -= this.OnStartQuizClicked;
			_view.LeaderboardClickedEvent -= this.OnLeaderboardClicked;
			_done.OnNext (nextScreen);
			_done.OnCompleted ();
		}

		#endregion
	}
}
