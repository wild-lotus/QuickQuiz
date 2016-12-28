using System.IO;
using UnityEngine;
using Zenject;

namespace CgfGames {

	public class EntryPoint : MonoBehaviour {

		GameCtrl _gameCtrl;

		[Inject]
		public void Construct (GameCtrl gameCtrl) {
			_gameCtrl = gameCtrl;
		}


		void Start () {
			_gameCtrl.Start ();
		}

		[ContextMenu ("Reset QuizState")]
		public void ResetQuizState () {
			File.Delete (QuizStateFileDataStore.PATH);
			Debug.Log ("QuizState file deleted");
		}
	}
}
