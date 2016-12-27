using System.Collections.Generic;
using UnityEngine;
using UniRx;
using PlayFab;
using PlayFab.ClientModels;

namespace CgfGames {
    public class PlayFabManager {
        public static readonly string StatisticName = "TestScore";
        public static string PlayFabId;

        public static void Login () {
            new LoginRequest ().Send ()
                .Subscribe (playFabId => { PlayFabId = playFabId; });
        }
    }
}
