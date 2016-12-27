using System;
using UnityEngine;
using UniRx;
using PlayFab;
using PlayFab.ClientModels;

namespace CgfGames {
	public class LoginRequest {
        public IObservable<string> Send () {
			return Observable.CreateSafe<string> (observer => {
				Debug.Log ("New LoginWithCustomIDRequest");
				PlayFabClientAPI.LoginWithCustomID (
					new LoginWithCustomIDRequest () {
						TitleId = PlayFabSettings.TitleId,
						CreateAccount = true,
						CustomId = SystemInfo.deviceUniqueIdentifier
					},
					(result) => {
						Debug.Log("Got PlayFabID: " + result.PlayFabId);
						if (result.NewlyCreated) {
							Debug.Log ("(New account)");
						} else {
							Debug.Log ("(Existing account)");
						}
						observer.OnNext (result.PlayFabId);
						observer.OnCompleted ();
					},
					(error) => {
						Debug.Log ("Error in LoginWithCustomIDRequest:");
						Debug.Log (error);
						observer.OnError (new Exception (error.ToString ()));
					}
				);
				return Disposable.Empty;
			});
		}
	}
}
