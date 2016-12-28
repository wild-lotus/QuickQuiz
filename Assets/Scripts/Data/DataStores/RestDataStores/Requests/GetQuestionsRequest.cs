using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;

namespace CgfGames {

	public interface IGetQuestionsRequest {
		
		IObservable<IQuestion> Send (Difficulty difficulty);
	}

	public class IGetQuestionsRequestFactory : Factory<IGetQuestionsRequest> {}

	public class GetQuestionsRequest : IGetQuestionsRequest {

		private readonly QuestionMapper _mapper;

		public GetQuestionsRequest () {
			_mapper = new QuestionMapper ();
		}

		private const string triviaUrl = "https://www.opentdb.com/api.php?amount=1&difficulty={0}&type=multiple";

		public IObservable<IQuestion> Send (Difficulty difficulty) {
			return ObservableWWW.Get (
				string.Format (
					triviaUrl, _mapper.Map (difficulty)
				)
			)
			.Select(
				response => _mapper.Map (
					JsonUtility.FromJson<QuestionsData>(response).results
				).ElementAt (0)
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

		private class QuestionMapper : Mapper<QuestionData, IQuestion> {

			public override IQuestion Map (QuestionData data) {
				return new Question (
					data.category,
					this.Map (data.difficulty),
					data.question,
					data.correct_answer,
					new List<string> (data.incorrect_answers)
				);
			}

			public Difficulty Map (string difficulty) {
				switch (difficulty) {
					case "easy":
						return Difficulty.Easy;
					case "medium":
						return Difficulty.Medium;
					case "hard":
						return Difficulty.Hard;
					default:
						throw new Exception ("Unknown difficulty");
				}
			}

			public string Map (Difficulty difficulty) {
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
		}	
	}
}