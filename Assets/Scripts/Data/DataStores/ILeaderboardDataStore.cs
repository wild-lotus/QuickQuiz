using System.Collections.Generic;
using UniRx;

namespace CgfGames {

	public interface ILeaderboardDataStore {

		IObservable<LeaderboardEntry> GetPlayerEntry ();

		IObservable<List<LeaderboardEntry>> GetTopEntries ();

		IObservable<int> SetPlayerScore (int score);
	}
}