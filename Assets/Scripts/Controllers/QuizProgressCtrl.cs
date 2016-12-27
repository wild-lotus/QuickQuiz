namespace CgfGames {

	public interface IQuizProgressCtrl {

		int Score (IQuizState quizState);

		Difficulty NextDifficulty (IQuizState quizState);

		bool HasLost (IQuizState quizState);
	}


	public class QuizProgressCtrl : IQuizProgressCtrl {

		public int Score (IQuizState quizState) {
			int score = 0;
			for (int i = 0; i < quizState.NumQuestions; i++) {
                switch (quizState.Difficulty(i)) {
					case Difficulty.Easy:
						score += quizState.Result(i) ? 1000 : -300;
						break;
					case Difficulty.Medium:
						score += quizState.Result(i) ? 2000 : -200;
						break;
					case Difficulty.Hard:
						score += quizState.Result(i) ? 3000 : -100;
						break;
                }
				if (score < 0) {
					score = 0;
				}
            }
			return score;
		}

		public Difficulty NextDifficulty (IQuizState quizState) {
			int numQuestions = quizState.NumQuestions;
			if (numQuestions == 0) {
				return Difficulty.Easy;
			}
			Difficulty lastDifficulty = quizState.Difficulty (numQuestions - 1);
			if (quizState.Result (numQuestions - 1) == false) {
				return lastDifficulty.Previous ();
			} else {
				if (
					numQuestions >= 2 &&
					quizState.Result (numQuestions - 2) == true &&
					quizState.Difficulty (numQuestions - 2) == lastDifficulty
				) {
					return lastDifficulty.Next ();
				} else {
					return lastDifficulty;
				}
			}
		}

		public bool HasLost (IQuizState quizState) {
			int numQuestions = quizState.NumQuestions;
			if (numQuestions < 3) {
				return false;
			}
			for (int i = 1; i <= 3; i++) {
				if (quizState.Result(numQuestions - i) == true) {
					return false;
				}
			}
			return true;
		}
	}
}
