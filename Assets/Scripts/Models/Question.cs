using System.Collections.Generic;

namespace CgfGames {

	public interface IQuestion {
		
		string Category { get; }

		Difficulty Difficulty { get; }

		string Wording { get; }

		string CorrectAnswer  { get; }

		List<string> IncorrectAnswers { get; }
	}

	public class Question : IQuestion {

		public string Category { get; }

		public Difficulty Difficulty { get; }

		public string Wording { get; }

		public string CorrectAnswer  { get; }

		public List<string> IncorrectAnswers { get; }

		public Question (string category, Difficulty difficulty, string wording, string correctAnswer, List<string> incorrectAnswers) {
			this.Category = category;
			this.Difficulty = difficulty;
			this.Wording = wording;
			this.CorrectAnswer = correctAnswer;
			this.IncorrectAnswers = incorrectAnswers;
		}
	}
}
