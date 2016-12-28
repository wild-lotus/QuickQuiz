using UniRx;

namespace CgfGames {

	public interface IQuizStateDataStore {

		IObservable<IQuizState> Get ();

		void Set (IQuizState quizState);
	}
}