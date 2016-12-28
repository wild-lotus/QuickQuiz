using UniRx;

namespace CgfGames {

	public class QuestionRestDataStore : IQuestionDataStore {

		private readonly IGetQuestionsRequestFactory _getQuestionsRequestFactory;

		public QuestionRestDataStore (IGetQuestionsRequestFactory getQuestionsRequestFactory) {
			_getQuestionsRequestFactory = getQuestionsRequestFactory;
		}

		public IObservable<IQuestion> Get (Difficulty difficulty) {
			return _getQuestionsRequestFactory.Create ().Send (difficulty);
		}
	}
}
