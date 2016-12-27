using UniRx;

namespace CgfGames {

	public interface IScreenCtrl {

		// Use this for initialization
		IObservable<Screen> Start ();
		
		// Update is called once per frame
		void Close (Screen nextScreen);
	}
}
