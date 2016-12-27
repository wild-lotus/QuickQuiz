using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using PlayFab;
using PlayFab.ClientModels;

namespace CgfGames {

	public class UpdatePlayerStatisticRequest {
		public IObservable<int> Send (int value) {
			return Observable.CreateSafe<int> (observer => {
				Debug.Log("New UpdatePlayerStatisticsRequest");
				PlayFabClientAPI.UpdatePlayerStatistics (
					new PlayFab.ClientModels.UpdatePlayerStatisticsRequest () {
						Statistics = new List<StatisticUpdate> {
							new StatisticUpdate () {
								StatisticName = PlayFabManager.StatisticName,
								Value = value
							}
						}
					},
					(result) => {
						Debug.Log("OK UpdatePlayerStatisticsRequest");
						observer.OnNext (0);
						observer.OnCompleted ();
					},
					(error) => {
						Debug.Log("Error in UpdatePlayerStatisticsRequest:");
						Debug.Log (error);
						observer.OnError (new Exception (error.ToString ()));
					}
				);
				return Disposable.Empty;
			});
		}
	}
}
