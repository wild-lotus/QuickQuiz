using System.Collections.Generic;
using UniRx;

namespace CgfGames {

	public class LeaderboardRestDataStore : ILeaderboardDataStore {

		private readonly IGetLeaderboardTopEntriesRequestFactory _getLeaderboardTopEntriesRequestFactory;

		private readonly IGetLeaderboardPlayerEntryRequestFactory _getLeaderboardPlayerEntryRequestFactory;

		private readonly ISetLeaderboardPlayerScoreRequestFactory _setLeaderboardPlayerScoreRequestFactory;

		public LeaderboardRestDataStore (IGetLeaderboardTopEntriesRequestFactory getLeaderboardTopEntriesRequestFactory, IGetLeaderboardPlayerEntryRequestFactory getLeaderboardPlayerEntryRequestFactory, ISetLeaderboardPlayerScoreRequestFactory setLeaderboardPlayerScoreRequestFactory) {
			_getLeaderboardTopEntriesRequestFactory = getLeaderboardTopEntriesRequestFactory;
			_getLeaderboardPlayerEntryRequestFactory = getLeaderboardPlayerEntryRequestFactory;
			_setLeaderboardPlayerScoreRequestFactory = setLeaderboardPlayerScoreRequestFactory;
		}

		public IObservable<LeaderboardEntry> GetPlayerEntry () {
			return _getLeaderboardPlayerEntryRequestFactory.Create ().Send ();
		}

		public IObservable<List<LeaderboardEntry>> GetTopEntries () {
			return _getLeaderboardTopEntriesRequestFactory.Create ().Send ();
		}

		public IObservable<int> SetPlayerScore (int score) {
			return _setLeaderboardPlayerScoreRequestFactory.Create ()
				.Send (score);
		}
	}
}
