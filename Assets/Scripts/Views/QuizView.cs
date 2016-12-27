using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UniRx;

namespace CgfGames {

	public interface IQuizView : IScreenView {

		event Action<IQuizState> BackClickedEvent;
		
		event Action<IQuizState> ApplicationQuitEvent;

		IQuestionView QuestionView { get; }

		void SetQuizState (IQuizState quizState);

		void Loading (bool loading);

		void UpdateScore (int score);

		IObservable<int> ShowGameOver ();
	}

	public class QuizView : MonoBehaviour, IQuizView {

		private const string LOADING_TEXT = "Loading...";

		private const string GAME_OVER_TEXT = "Game Over";

		private const float GAME_OVER_TIME = 3;

		public event Action<IQuizState> BackClickedEvent;

		public event Action<IQuizState> ApplicationQuitEvent;
		
		public Text scoreText;

		public Text feedbackText;

		public GameObject loadingImgGobj;

		public GameObject questionContainerGobj;
		
		public QuestionView questionView;

		private IQuizState _quizState;


		public IQuestionView QuestionView { get { return questionView; } }

		public void SetQuizState (IQuizState quizState) {
			_quizState = quizState;
		}

		void Awake  () {
			Assert.IsNotNull (scoreText);
			Assert.IsNotNull (feedbackText);
			Assert.IsNotNull (loadingImgGobj);
			Assert.IsNotNull (questionContainerGobj);
			Assert.IsNotNull (questionView);
		}

		public void SetActive (bool active) {
			gameObject.SetActive (active);
		}

		public void Loading (bool loading) {
			if (loading) {
				feedbackText.text = LOADING_TEXT;
			}
			loadingImgGobj.SetActive (loading);
			questionContainerGobj.SetActive (!loading);
		}

		public void UpdateScore (int score) {
			scoreText.text = score.ToString ();
		}

		public IObservable<int> ShowGameOver () {
			questionContainerGobj.SetActive (false);
			feedbackText.text = GAME_OVER_TEXT;
			return Observable.Return(0)
				.Delay (TimeSpan.FromSeconds (GAME_OVER_TIME));
		}

		public void OnBackClicked () {
			if (this.BackClickedEvent != null) {
				this.BackClickedEvent (_quizState);
			}
		}

		void OnApplicationQuit () {
			if (this.ApplicationQuitEvent != null) {
				this.ApplicationQuitEvent (_quizState);
			}
		}
	}
}
