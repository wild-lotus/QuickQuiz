using Zenject;
using UniRx;

namespace CgfGames {

	public interface IQuestionCtrl {
		
		IObservable<bool> Play ();
	}

	public class IQuestionCtrlFactory : Factory<IQuestion, IQuestionCtrl> {}

	public class QuestionCtrl : IQuestionCtrl  {

		private readonly IQuestion _question;

		private readonly IQuestionView _view;

		public QuestionCtrl (IQuestionView view, IQuestion question) {
			_view = view;
			_question = question;
		}

		public IObservable<bool> Play () {
			return _view.Play (_question);
		}
	}
}
