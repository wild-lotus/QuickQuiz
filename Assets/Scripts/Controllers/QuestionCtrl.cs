using UniRx;

namespace CgfGames {

	public interface IQuestionCtrl {
		
		IObservable<bool> Play ();

		void OnAnswer ();
	}

	public class QuestionCtrl {

		private readonly IQuestion _question;

		private readonly IQuestionView _view;

		public QuestionCtrl (IQuestionView view, IQuestion question) {
			_question = question;
			_view = view;
		}

		public IObservable<bool> Play () {
			return _view.Play (_question);
		}
	}
}
