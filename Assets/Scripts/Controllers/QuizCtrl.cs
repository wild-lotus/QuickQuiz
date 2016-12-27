using UnityEngine;
using UniRx;

namespace CgfGames {
	
	public interface IQuizCtrl : IScreenCtrl {

		void PlayNewQuestion (IQuizState quizState);

		void OnQuestionAnswered (IQuizState quizState, IQuestion question, bool correctAnswer);

		void OnBackClicked (IQuizState quizState);
	}

	public class QuizCtrl : IQuizCtrl {

		#region Private fields
		//======================================================================

		private readonly Subject<Screen> _done;

		private readonly CompositeDisposable _disposables;

		private readonly IQuizView _view;

		private readonly IQuizRepo _repo;

		private readonly IQuizProgressCtrl _quizProgressCtrl;

		#endregion

		#region Constructor
		//======================================================================

		public QuizCtrl (IQuizView view, IQuizRepo repo, IQuizProgressCtrl quizProgressCtrl) {
			_done = new Subject<Screen> ();
			_disposables = new CompositeDisposable();
			_view = view;
			_repo = repo;
			_quizProgressCtrl = quizProgressCtrl;
		}

		#endregion

		#region IQuizCtrl implementation
		//======================================================================

		public IObservable<Screen> Start () {
			_view.SetActive (true);
			_view.Loading (true);
			_view.ApplicationQuitEvent += this.OnApplicationQuit;
			_view.BackClickedEvent += this.OnBackClicked;
			_repo.GetQuizState ()
				.Subscribe(quizState => this.PlayNewQuestion (quizState))
				.AddTo(_disposables);;
			return _done;
		}

		public void PlayNewQuestion (IQuizState quizState) {
			_view.Loading (true);
			_view.SetQuizState (quizState);
			_view.UpdateScore (_quizProgressCtrl.Score (quizState));
			_repo.RandomQuestion (
				_quizProgressCtrl.NextDifficulty (quizState)
			)
				.Subscribe (
					question => this.OnQuestionLoaded (quizState, question)
				)
				.AddTo(_disposables);
		}

		private void OnQuestionLoaded (IQuizState quizState, IQuestion question) {
			_view.Loading (false);
			new QuestionCtrl (
				_view.QuestionView,
				question
			).Play ()
				.Subscribe (correctAnswer => 
					this.OnQuestionAnswered (quizState, question, correctAnswer)
				)
				.AddTo(_disposables);
		}

		public void OnQuestionAnswered (IQuizState quizState, IQuestion question, bool correctAnswer) {
			IQuizState nextState = quizState.Next (question.Difficulty, correctAnswer);
			if (_quizProgressCtrl.HasLost (nextState)) {
				this.Lost (nextState);
			} else {
				this.PlayNewQuestion (nextState);
			}
		}

		private void Lost (IQuizState lostState) {
			_repo.SetQuizState (new QuizState ());
			int score = _quizProgressCtrl.Score (lostState);
			Debug.Log ("GameOver: " + score);
			_repo.UpdateScore (score).Subscribe ().AddTo(_disposables);
			_view.UpdateScore (score);
			_view.ShowGameOver ()
				.Subscribe (_ => {
					this.Close (Screen.Main);
				})
				.AddTo(_disposables);
		}

		public void OnBackClicked (IQuizState quizState) {
			_repo.SetQuizState (quizState);
			this.Close (Screen.Main);
		}

		public void OnApplicationQuit (IQuizState quizState) {
			_repo.SetQuizState (quizState);
		}

		public void Close (Screen nextScreen) {
			_disposables.Dispose ();
			_view.SetActive (false);
			_view.ApplicationQuitEvent -= this.OnApplicationQuit;
			_view.BackClickedEvent -= this.OnBackClicked;
			_done.OnNext (nextScreen);
			_done.OnCompleted ();
		}

		#endregion
	}
}
