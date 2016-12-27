using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace CgfGames {

	public interface IGetQuestionsRequest {
		
		IObservable<List<IQuestion>> Send (int amount, Difficulty difficulty);
	}

	public class GetQuestionsRequest : IGetQuestionsRequest {

		public static string triviaUrl = "https://www.opentdb.com/api.php?amount={0}&difficulty={1}&type=multiple";

		public IObservable<List<IQuestion>> Send (int amount, Difficulty difficulty) {
			return ObservableWWW.Get (
				string.Format (
					triviaUrl, amount, this.Transform (difficulty)
				)
			)
			.Select(
				response => this.Transform (
					JsonUtility.FromJson<QuestionsData>(response).results, difficulty
				)
			);
		}

		private string Transform (Difficulty difficulty) {
			switch (difficulty) {
				case Difficulty.Easy:
					return "easy";
				case Difficulty.Medium:
					return "medium";
				case Difficulty.Hard:
					return "hard";
				default:
					throw new Exception ("Unknown difficulty");
			}
		}

		private List<IQuestion> Transform (QuestionData[] quesitons, Difficulty difficulty) {
			return quesitons.Select(
				quesiton => this.Transform (quesiton, difficulty)
			).ToList ();
		}

		private IQuestion Transform (QuestionData data, Difficulty difficulty) {
			return new Question (
				data.category,
				difficulty,
				data.question,
				data.correct_answer,
				new List<string> (data.incorrect_answers)
			);
		}

		[Serializable]
		private class QuestionsData {
			public QuestionData[] results;
		}

		[Serializable]
		private class QuestionData {
			public string category;
			public string difficulty;
			public string question;
			public string correct_answer;
			public string[] incorrect_answers;
		} 
	}
}