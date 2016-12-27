using UniRx;

namespace CgfGames {

	public interface ILeaderboardCtrl : IScreenCtrl {}

	public class LeaderboardCtrl : ILeaderboardCtrl {

		private readonly Subject<Screen> _done;

		private readonly ILeaderboardView _view;

		private readonly IQuizRepo _repo;

		public LeaderboardCtrl (ILeaderboardView view, IQuizRepo repo) {
			_done = new Subject<Screen> ();
			_view = view;
			_repo = repo;
		}

		// Use this for initialization
		public IObservable<Screen> Start () {
			_view.SetActive (true);
			_view.Loading (true);
			_view.BackClickedEvent += OnBackClicked;
			_repo.GetLeaderboard ()
				.Subscribe (entries => this.OnLeaderboardLoaded (entries));
			return _done;
		}

		private void OnLeaderboardLoaded (LeaderboardEntry[] entries) {
			_view.Loading (false);
			_view.ShowLeaderboard (entries);
		}

		private void OnBackClicked () {
			this.Close (Screen.Main);
		}
		
		// Update is called once per frame
		public void Close (Screen nextScreen) {
			_view.SetActive (false);
			_view.BackClickedEvent -= OnBackClicked;
			_done.OnNext (nextScreen);
			_done.OnCompleted ();
		}
	}
}
