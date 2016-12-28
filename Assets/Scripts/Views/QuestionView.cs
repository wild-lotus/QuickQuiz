using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UniRx;

namespace CgfGames {

	public interface IQuestionView {

		IObservable<bool> Play (IQuestion question);
	}

	public class QuestionView : MonoBehaviour, IQuestionView {

		private const string CORRECT_TEXT = "Correct!";

		private const string INCORRECT_TEXT = "Incorrect!";

		private const string TIMEOUT_TEXT = "Time out!";

		private const string CATEGORY_TEXT = "Category: {0}";

		private const string DIFFICULTY_TEXT = "Difficulty: {0}";

		private const int ANSWER_TIME = 10;

		private const float POST_ANSWER_TIME = 2;

		private readonly Color CORRECT_COLOR_HINT = new Color (0.8f, 1f, 0.8f);

		private event Action<int> AnswerClickedEvent;

		public Text feedbackText;
		
		public Text wordingText;

		public Text[] answersTexts;

		public Button[] answersButtons;

		public Text catgoryText;

		public Text difficultyText;

		private Subject<bool> _result;

		private int _correctAnswerPos;

		private float _timer;

		void Awake  () {
			Assert.IsNotNull (feedbackText);
			Assert.IsNotNull (wordingText);
			Assert.IsNotNull (answersTexts);
			Assert.IsTrue (answersTexts.Length > 0);
			Assert.IsNotNull (answersButtons);
			Assert.IsTrue (answersButtons.Length > 0);
			Assert.IsNotNull (catgoryText);
			Assert.IsNotNull (difficultyText);
			enabled = false;
		}

		public IObservable<bool> Play (IQuestion question) {
			this.ShowQuestion (question);
			return Observable.Merge<int> (
				this.StartTimer (),
				Observable.FromEvent<int> (
					h => AnswerClickedEvent += h, h => AnswerClickedEvent -= h
				)
			)
			.First ()
			.SelectMany (pos => this.Feedback (pos));
		}

		private void ShowQuestion (IQuestion question) {
			wordingText.text = question.Wording;
			this.EnableButtons (true);
			answersTexts[0].text = question.CorrectAnswer;
			answersTexts[1].text = question.IncorrectAnswers[0];
			answersTexts[2].text = question.IncorrectAnswers[1];
			answersTexts[3].text = question.IncorrectAnswers[2];
			catgoryText.text = string.Format (CATEGORY_TEXT, question.Category);
			difficultyText.text = 
				 string.Format (DIFFICULTY_TEXT, question.Difficulty);
			_correctAnswerPos = this.ShuffleAnswers ();
			Debug.Log ("Correct answer: " + _correctAnswerPos);
		}

		private int ShuffleAnswers () {
			int correctAnswerPos = UnityEngine.Random.Range(
				0, answersTexts.Length
			);  
			string value = answersTexts[correctAnswerPos].text;  
			answersTexts[correctAnswerPos].text = answersTexts[0].text;  
			answersTexts[0].text = value;  
			return correctAnswerPos;
		}

		private IObservable<int> StartTimer () {
			return Observable.Timer (TimeSpan.Zero, TimeSpan.FromSeconds (1))
				.Do (t => feedbackText.text = (ANSWER_TIME - t).ToString ())
				.Skip (ANSWER_TIME)
				.Select (_ => -1);
		}

		public void OnAnswerClicked (int pos) {
			if (this.AnswerClickedEvent != null) {
				this.AnswerClickedEvent (pos);
			}
		}

		private IObservable<bool> Feedback (int pos) {
			if (pos == -1) {
				return this.TimeOutFeedback ();
			} else if (pos == _correctAnswerPos) {
				return this.CorrectFeedback ();
			} else {
				return this.IncorrectFeedback (pos);
			}
		}

		private IObservable<bool> CorrectFeedback () {
			return Observable.Return (true)
				.Do (_ => {
					this.EnableButtons (false);
					feedbackText.text = CORRECT_TEXT;
					answersButtons[_correctAnswerPos].image.color = Color.green;
				})
				.Delay (TimeSpan.FromSeconds (POST_ANSWER_TIME))
				.Do (_ => {
					answersButtons[_correctAnswerPos].image.color = Color.white;
				});
		}

		private IObservable<bool> IncorrectFeedback (int pos) {
			return Observable.Return (false)
				.Do (_ => {
					this.EnableButtons (false);
					feedbackText.text = INCORRECT_TEXT;
					answersButtons[pos].image.color = Color.red;
					answersButtons[_correctAnswerPos].image.color = 	
						CORRECT_COLOR_HINT;
				})
				.Delay (TimeSpan.FromSeconds (POST_ANSWER_TIME))
				.Do (correct => {
					answersButtons[pos].image.color = Color.white;
					answersButtons[_correctAnswerPos].image.color = Color.white;
				});
		}

		private IObservable<bool> TimeOutFeedback () {
			return Observable.Return (false)
				.Do (_ => {
					this.EnableButtons (false);
					feedbackText.text = TIMEOUT_TEXT;
					answersButtons[_correctAnswerPos].image.color = CORRECT_COLOR_HINT;
				})
				.Delay (TimeSpan.FromSeconds (POST_ANSWER_TIME))
				.Do (_ => 
					answersButtons[_correctAnswerPos].image.color = Color.white
				);
		}

		private void EnableButtons (bool enable) {
			foreach (Button button in answersButtons) {
				button.interactable = enable;
			}
		}
	}
}
