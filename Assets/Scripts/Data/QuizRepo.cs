using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;

namespace CgfGames {

	public interface IQuizRepo {
		
		IObservable<IQuizState> GetQuizState ();

		void SetQuizState (IQuizState quizState);

		IObservable<IQuestion> RandomQuestion (Difficulty difficulty);

		IObservable<List<LeaderboardEntry>> GetLeaderboard ();

		IObservable<int> UpdateScore (int score);

	}

	public class QuizRepo : IQuizRepo {

		IQuizStateDataStore _quizStateDataStore;

		IQuestionDataStore _questionDataStore;

		ILeaderboardDataStore _leaderboardDataStore;

		public QuizRepo (IQuizStateDataStore quizStateDataStore, IQuestionDataStore questionDataStore, ILeaderboardDataStore leaderboardDataStore) {
			_quizStateDataStore = quizStateDataStore;
			_questionDataStore = questionDataStore;
			_leaderboardDataStore = leaderboardDataStore;
		}

		public IObservable<IQuizState> GetQuizState () {
			return _quizStateDataStore.Get ();
		}

		public void SetQuizState (IQuizState quizState) {
			_quizStateDataStore.Set (quizState);
		}

		public IObservable<IQuestion> RandomQuestion (Difficulty difficulty) {
			return _questionDataStore.Get (difficulty);
		}

		public IObservable<List<LeaderboardEntry>> GetLeaderboard () {
			return DelayWhileNotLoggedIn ()
				.ContinueWith (
					Observable.CombineLatest (
						_leaderboardDataStore.GetTopEntries (),
						_leaderboardDataStore.GetPlayerEntry (),
						this.Combine
					)
				);
		}

		private IObservable<long> DelayWhileNotLoggedIn () {
			return Observable
				.Timer (TimeSpan.Zero, TimeSpan.FromSeconds (0.5))
				.TakeWhile (_ => {
					return PlayFabManager.PlayFabId == null;
				})
				.DefaultIfEmpty (0);
		}

		private List<LeaderboardEntry> Combine (List<LeaderboardEntry> topEntries, LeaderboardEntry playerEntry) {
			int playerPos = playerEntry.Position;
			if (playerPos < 10) {
				LeaderboardEntry tempEntry = topEntries[playerPos];
				tempEntry.IsPlayer = true;
				topEntries[playerPos] = tempEntry;
			} else {
				topEntries[9] = playerEntry;
			}
			return topEntries;
		}

		public IObservable<int> UpdateScore (int score) {
			return _leaderboardDataStore.SetPlayerScore (score);
		}
	}
}
