using System;
using UnityEngine;
using Zenject;
using UniRx;
using PlayFab;
using PlayFab.ClientModels;

namespace CgfGames {

	public interface IGetLeaderboardPlayerEntryRequest {

		IObservable<LeaderboardEntry> Send ();
	}

	public class IGetLeaderboardPlayerEntryRequestFactory : Factory<IGetLeaderboardPlayerEntryRequest> {}

	public class GetLeaderboardPlayerEntryRequest : IGetLeaderboardPlayerEntryRequest {

		private readonly LeaderbarodEntryMapper _mapper;

		public GetLeaderboardPlayerEntryRequest (LeaderbarodEntryMapper mapper) {
			_mapper = mapper;
		}

		public IObservable<LeaderboardEntry> Send () {
			return Observable.CreateSafe<LeaderboardEntry> (observer => {
				Debug.Log("New GetLeaderboardAroundPlayerRequest");
				PlayFabClientAPI.GetLeaderboardAroundPlayer (
					new PlayFab.ClientModels.GetLeaderboardAroundPlayerRequest () {
						StatisticName = PlayFabManager.StatisticName,
						MaxResultsCount = 1
					},
					(result) => {
						Debug.Log("OK GetLeaderboardAroundPlayerRequest");
						observer.OnNext (_mapper.Map (result.Leaderboard[0]));
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
