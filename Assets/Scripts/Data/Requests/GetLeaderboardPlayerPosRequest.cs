using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using PlayFab;
using PlayFab.ClientModels;

namespace CgfGames {

	public class GetPlayerLeaderboardEntryRequest {

		public IObservable<PlayerLeaderboardEntry> Send () {
			return Observable.CreateSafe<PlayerLeaderboardEntry> (observer => {
				Debug.Log("New GetLeaderboardAroundPlayerRequest");
				PlayFabClientAPI.GetLeaderboardAroundPlayer (
					new PlayFab.ClientModels.GetLeaderboardAroundPlayerRequest () {
						StatisticName = PlayFabManager.StatisticName,
						MaxResultsCount = 1
					},
					(result) => {
						Debug.Log("OK GetLeaderboardAroundPlayerRequest");
						observer.OnNext (result.Leaderboard[0]);
						observer.OnCompleted ();
					},
					(error) => {
						Debug.Log("Error in GetLeaderboardAroundPlayerRequest:");
						Debug.Log (error);
						observer.OnError (new Exception (error.ToString ()));
					}
				);
				return Disposable.Empty;
			});
		}
	}
}
