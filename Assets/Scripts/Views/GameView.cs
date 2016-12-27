using UnityEngine;
using UnityEngine.Assertions;

namespace CgfGames {

	public interface IGameView {

		IMainScreenView MainScreenView { get; }

		IQuizView QuizView { get; }

		ILeaderboardView LeaderboardView { get; }
	}

	public class GameView : MonoBehaviour, IGameView {

		public MainScreenView mainScreenView;

		public QuizView quizView;

		public LeaderboardView leaderboardView;

		public IMainScreenView MainScreenView { get { return mainScreenView; } }

		public IQuizView QuizView { get { return quizView; } }

		public ILeaderboardView LeaderboardView { 
			get { return leaderboardView; } 
		}

		void Awake () {
			Assert.IsNotNull (mainScreenView);
			Assert.IsNotNull (quizView);
			Assert.IsNotNull (leaderboardView);
		}
	}
}
