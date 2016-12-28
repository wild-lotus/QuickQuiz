using UniRx;

namespace CgfGames {

	public interface IQuestionDataStore {

		IObservable<IQuestion> Get (Difficulty difficulty);
	}
}