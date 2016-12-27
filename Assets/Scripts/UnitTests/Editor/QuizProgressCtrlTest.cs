using NUnit.Framework;

namespace CgfGames {

	public class QuizProgressCtrlTest {

		[Test]
		public void CanContinueOnEmptyState () {
			QuizProgressCtrl ctrl = new QuizProgressCtrl ();
			Assert.False (ctrl.HasLost (new QuizState ()));
		}

		[Test]
		public void CanContinueOnLessThan3Results () {
			QuizProgressCtrl ctrl = new QuizProgressCtrl ();

			IQuizState _1fState = 
				new QuizState ().Next (Difficulty.Easy, false);
			Assert.False (ctrl.HasLost (_1fState));

			IQuizState _2fState = _1fState.Next (Difficulty.Easy, false);
			Assert.False (ctrl.HasLost (_2fState));
		}	

		[Test]
		public void CanLoseOn3IncorrectsFromEmpty () {		
			QuizProgressCtrl ctrl = new QuizProgressCtrl ();

			Assert.True (
				ctrl.HasLost (
					new QuizState ()
					.Next (Difficulty.Easy, false)
					.Next (Difficulty.Easy, false)
					.Next (Difficulty.Easy, false)
				)
			);
		}

		[Test]
		public void CanContinueOn1IncorrectFromNonEmpty () {		
			QuizProgressCtrl ctrl = new QuizProgressCtrl ();

			Assert.False (
				ctrl.HasLost (
					new QuizState ()
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Easy, false)
				)
			);
		}

		[Test]
		public void CanContinueOn2IncorrectsFromNonEmpty () {		
			QuizProgressCtrl ctrl = new QuizProgressCtrl ();

			Assert.False (
				ctrl.HasLost (
					new QuizState ()
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Easy, false)
					.Next (Difficulty.Easy, false)
				)
			);
		}

		[Test]
		public void CanLoseOn3IncorrectsFromNonEmpty () {		
			QuizProgressCtrl ctrl = new QuizProgressCtrl ();

			Assert.True (
				ctrl.HasLost (
					new QuizState ()
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Easy, false)
					.Next (Difficulty.Easy, false)
					.Next (Difficulty.Easy, false)
				)
			);
		}

		[Test]
		public void CanReturnNextDifficultyOnEmpty () {	
			QuizProgressCtrl ctrl = new QuizProgressCtrl ();
			
			Assert.AreEqual (
				ctrl.NextDifficulty (new QuizState ()),
				Difficulty.Easy
			);
		}

		[Test]
		public void CanReturnPreviousDifficultyOnIncorrect () {	
			QuizProgressCtrl ctrl = new QuizProgressCtrl ();
			
			Assert.AreEqual (
				ctrl.NextDifficulty (
					new QuizState ()
					.Next (Difficulty.Easy, false)),
				Difficulty.Easy
			);

			Assert.AreEqual (
				ctrl.NextDifficulty (
					new QuizState ()
					.Next (Difficulty.Medium, false)),
				Difficulty.Easy
			);

			Assert.AreEqual (
				ctrl.NextDifficulty (
					new QuizState ()
					.Next (Difficulty.Hard, false)),
				Difficulty.Medium
			);
		}

		[Test]
		public void CanReturnPreviousDifficultyOn2Corrects () {	
			QuizProgressCtrl ctrl = new QuizProgressCtrl ();
			
			Assert.AreEqual (
				ctrl.NextDifficulty (
					new QuizState ()
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Easy, true)),
				Difficulty.Medium
			);

			Assert.AreEqual (
				ctrl.NextDifficulty (
					new QuizState ()
					.Next (Difficulty.Medium, true)
					.Next (Difficulty.Medium, true)),
				Difficulty.Hard
			);

			Assert.AreEqual (
				ctrl.NextDifficulty (
					new QuizState ()
					.Next (Difficulty.Hard, true)
					.Next (Difficulty.Hard, true)),
				Difficulty.Hard
			);
		}

				[Test]
		public void CanReturnSameDifficultyOn1CorrectSameDifficulty () {	
			QuizProgressCtrl ctrl = new QuizProgressCtrl ();
			
			Assert.AreEqual (
				ctrl.NextDifficulty (
					new QuizState ()
					.Next (Difficulty.Easy, true)
					.Next (Difficulty.Medium, true)),
				Difficulty.Medium
			);

			Assert.AreEqual (
				ctrl.NextDifficulty (
					new QuizState ()
					.Next (Difficulty.Hard, false)
					.Next (Difficulty.Medium, true)),
				Difficulty.Medium
			);
		}
	}
}
