using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UniRx;

namespace CgfGames {

	public class QuizStateFileDataStore : IQuizStateDataStore {

		public static readonly string PATH = Application.persistentDataPath + "playerInfo.dat";

		public IObservable<IQuizState> Get () {
			IObservable<IQuizState> result;
			if (
				File.Exists (PATH)
			) {
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (PATH, FileMode.Open);
				IQuizState quizState = (IQuizState)bf.Deserialize (file);
				file.Close ();
				result = Observable.Return(quizState);
				// _result.OnNext ((QuizState) bf.Deserialize (file));
			} else {
				result = Observable.Return((IQuizState) QuizState.Empty);
				// _result.OnNext (new QuizState ());
			}
			// _result.OnCompleted ();
			// return _result;
			return result;
		}

		public void Set (IQuizState quizState) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (PATH, FileMode.OpenOrCreate);
			bf.Serialize (file, (QuizState)quizState);
			file.Close ();
		}
	}
}
