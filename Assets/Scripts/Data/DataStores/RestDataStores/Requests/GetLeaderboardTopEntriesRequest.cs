using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using UniRx;
using PlayFab;

namespace CgfGames {

	public interface IGetLeaderboardTopEntriesRequest {
		
		IObservable<List<LeaderboardEntry>> Send ();
	}

	public class IGetLeaderboardTopEntriesRequestFactory : Factory<IGetLeaderboardTopEntriesRequest> {}

	public class GetLeaderboardTopEntriesRequest : IGetLeaderboardTopEntriesRequest {

		private readonly LeaderbarodEntryMapper _mapper;

		public GetLeaderboardTopEntriesRequest (LeaderbarodEntryMapper mapper) {
			_mapper = mapper;
		}
		public IObservable<List<LeaderboardEntry>> Send () {
			return Observable.CreateSafe<List<LeaderboardEntry>> (
				observer => {
					Debug.Log("New GetLeaderboardRequest");
					PlayFabClientAPI.GetLeaderboard (
						new PlayFab.ClientModels.GetLeaderboardRequest () {
							StatisticName = PlayFabManager.StatisticName
						},
						(result) => {
							Debug.Log("OK GetLeaderboardRequest");
							observer.OnNext (
								_mapper.Map (result.Leaderboard).ToList ()
							);
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
