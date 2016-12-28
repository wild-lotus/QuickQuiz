using System;
using System.Collections.Generic;

namespace CgfGames {

	public interface IQuizState {

		int NumQuestions { get; }

		Difficulty Difficulty (int pos);

		bool Result (int pos);

		IQuizState Next (Difficulty difficulty, bool result);
	}

	[Serializable]
	public class QuizState : IQuizState {

		public List<QuestionResult> results;

		public static QuizState Empty = new QuizState ();

		public QuizState () {
			results = new List<QuestionResult> ();
		}

		private QuizState (List<QuestionResult> results) {
			this.results = new List<QuestionResult>(results);
		}

		public int NumQuestions { get { return results.Count; } }

		public Difficulty Difficulty (int pos) {
			return results[pos].Difficulty;
		}

		public bool Result (int pos) {
			return results[pos].Result;
		}

		public IQuizState Next (Difficulty difficulty, bool result) {
			results.Add (new QuestionResult (difficulty, result));
			return new QuizState (results);
		}

		[Serializable]
		public struct QuestionResult {
			
			public QuestionResult (Difficulty difficulty, bool result) {
				this.Difficulty = difficulty;
				this.Result = result;
			}

			public Difficulty Difficulty;

			public bool Result;
		}
	}
}
