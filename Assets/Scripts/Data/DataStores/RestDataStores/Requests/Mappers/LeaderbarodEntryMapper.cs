using UnityEngine;
using PlayFab.ClientModels;

namespace CgfGames {

	public class LeaderbarodEntryMapper : Mapper<PlayerLeaderboardEntry, LeaderboardEntry> 
	 {

		public override LeaderboardEntry Map (PlayerLeaderboardEntry playerLeaderboardEntry) {
			return new LeaderboardEntry  () {
				Position = playerLeaderboardEntry.Position,
				Score = playerLeaderboardEntry.StatValue,
				IsPlayer = 
					playerLeaderboardEntry.PlayFabId == PlayFabManager.PlayFabId
			};
		}
	}
}
