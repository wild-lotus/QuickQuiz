using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace CgfGames {

	public class EntryPoint : MonoBehaviour {

		public GameView gameView;

		void Awake () {
			Assert.IsNotNull (gameView);
		}

		void Start () {
			new GameCtrl (gameView).Start ();
		}

		[ContextMenu ("Reset QuizState")]
		public void ResetQuizState () {
			File.Delete (QuizStateFile.PATH);
			Debug.Log ("QuizState file deleted");
		}
	}
}
