using NUnit.Framework;

namespace CgfGames {

	public class QuizStateTest {

		[Test]
		public void CanReturnNumQuestions () {

			QuizState emptyState = new QuizState ();
			Assert.AreEqual (emptyState.NumQuestions, 0);

			IQuizState _1State = emptyState.Next (Difficulty.Easy, true);
			Assert.AreEqual (_1State.NumQuestions, 1);

			IQuizState _2State = _1State.Next (Difficulty.Easy, true);
			Assert.AreEqual (_2State.NumQuestions, 2);

			IQuizState _3State = _2State.Next (Difficulty.Easy, true);
			Assert.AreEqual (_3State.NumQuestions, 3);
		}

		[Test]
		public void CanReturnDifficulties () {

			IQuizState _1eState = new QuizState ().Next (Difficulty.Easy, true);
			Assert.AreEqual (_1eState.Difficulty(0), Difficulty.Easy);

			IQuizState _1e2mState = _1eState.Next (Difficulty.Medium, true);
			Assert.AreEqual (_1e2mState.Difficulty(0), Difficulty.Easy);
			Assert.AreEqual (_1e2mState.Difficulty(1), Difficulty.Medium);

			IQuizState _1e2m3hState = _1e2mState.Next (Difficulty.Hard, true);
			Assert.AreEqual (_1e2m3hState.Difficulty(0), Difficulty.Easy);
			Assert.AreEqual (_1e2m3hState.Difficulty(1), Difficulty.Medium);
			Assert.AreEqual (_1e2m3hState.Difficulty(2), Difficulty.Hard);
		}

				[Test]
		public void CanReturnResults () {

			IQuizState _1tState = new QuizState ().Next (Difficulty.Easy, true);
			Assert.AreEqual (_1tState.Result(0), true);

			IQuizState _1t2fState = _1tState.Next (Difficulty.Medium, false);
			Assert.AreEqual (_1t2fState.Result(0), true);
			Assert.AreEqual (_1t2fState.Result(1), false);

			IQuizState _1t2f3tState = _1t2fState.Next (Difficulty.Hard, true);
			Assert.AreEqual (_1t2f3tState.Result(0), true);
			Assert.AreEqual (_1t2f3tState.Result(1), false);
			Assert.AreEqual (_1t2f3tState.Result(2), true);
		}
	}
}
