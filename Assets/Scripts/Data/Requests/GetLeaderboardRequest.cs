using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using PlayFab;
using PlayFab.ClientModels;

namespace CgfGames {

	public class GetLeaderboardRequest {

		public IObservable<List<PlayerLeaderboardEntry>> Send () {
			return Observable.CreateSafe<List<PlayerLeaderboardEntry>> (
				observer => {
					Debug.Log("New GetLeaderboardRequest");
					PlayFabClientAPI.GetLeaderboard (
						new PlayFab.ClientModels.GetLeaderboardRequest () {
							StatisticName = PlayFabManager.StatisticName
						},
						(result) => {
							Debug.Log("OK GetLeaderboardRequest");
							// result.Leaderboard[0].
							observer.OnNext (result.Leaderboard);
							observer.OnCompleted ();
						},
						(error) => {
							Debug.Log("Error in GetLeaderboardRequest:");
							Debug.Log (error);
							observer.OnError (new Exception (error.ToString ()));
						}
					);
					return Disposable.Empty;
				}
			);
		}
	}
}
