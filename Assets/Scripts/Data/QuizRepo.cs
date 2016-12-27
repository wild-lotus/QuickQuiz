using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using PlayFab.ClientModels;

namespace CgfGames {

	public interface IQuizRepo {
		
		IObservable<IQuizState> GetQuizState ();

		void SetQuizState (IQuizState quizState);

		IObservable<IQuestion> RandomQuestion (Difficulty difficulty);

		IObservable<LeaderboardEntry[]> GetLeaderboard ();

		IObservable<int> UpdateScore (int score);

	}

	public class QuizRepo : IQuizRepo {

		IQuizStateFile _file;

		public QuizRepo () {
			_file = new QuizStateFile ();
		}

		public IObservable<IQuizState> GetQuizState () {
			return _file.Get ();
		}

		public void SetQuizState (IQuizState quizState) {
			_file.Set (quizState);
		}

		public IObservable<IQuestion> RandomQuestion (Difficulty difficulty) {
			return new GetQuestionsRequest()
				.Send(1, difficulty)
				.Select(questions => questions[0]);
		}

		public IObservable<LeaderboardEntry[]> GetLeaderboard () {
			return DelayWhileNotLoggedIn ()
				.ContinueWith (
					Observable.CombineLatest (
						this.GetLeaderboardTopEntries (),
						this.GetPlayerLeaderboardEntry (),
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

		private IObservable<LeaderboardEntry[]> GetLeaderboardTopEntries () {
			return new GetLeaderboardRequest().Send()
				.Select (entries => this.Transform (entries));
		}

		private IObservable<LeaderboardEntry> GetPlayerLeaderboardEntry () {
			return new GetPlayerLeaderboardEntryRequest().Send()
				.Select (entry => this.Transform(entry));
		}

		private LeaderboardEntry[] Combine (LeaderboardEntry[] topEntries, LeaderboardEntry playerEntry) {
			int playerPos = playerEntry.Position;
			if (playerPos < 10) {
				topEntries[playerPos].IsPlayer = true;
			} else {
				topEntries[9] = playerEntry;
			} return topEntries;
		}

		private LeaderboardEntry[] Transform (List<PlayerLeaderboardEntry> playerLeaderboardEntries) {
			return playerLeaderboardEntries.Select (this.Transform).ToArray ();
		}

		private LeaderboardEntry Transform (PlayerLeaderboardEntry playerLeaderboardEntry) {
			return new LeaderboardEntry  () {
				Position = playerLeaderboardEntry.Position,
				Score = playerLeaderboardEntry.StatValue,
				IsPlayer = 
					playerLeaderboardEntry.PlayFabId == PlayFabManager.PlayFabId
			};
		}

		public IObservable<int> UpdateScore (int score) {
			return new UpdatePlayerStatisticRequest ().Send (score);
		}
	}
}
