using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (CanvasScaler))]
[ExecuteInEditMode]
public class SwitchMatchModeScript : MonoBehaviour {

	public static event Action<int> OrientationChangedEvent; 

	private CanvasScaler _scaler;

	private float _oldAspect;

	// Use this for initialization
	void Awake () {
		_scaler = GetComponent<CanvasScaler> ();
		_oldAspect = _scaler.referenceResolution.x / _scaler.referenceResolution.y;
	}
	
	
	
	void Update () {
		float aspect = Screen.width / Screen.height;
		if ((int)aspect < 1 != (int)_oldAspect < 1) {
			_oldAspect = aspect;
			int orientation =  Mathf.Min ((int)aspect, 1);
			_scaler.matchWidthOrHeight = orientation;
			Vector2 newRef = new Vector2 (
				_scaler.referenceResolution.y, _scaler.referenceResolution.x
			);
			_scaler.referenceResolution = newRef;
			if (OrientationChangedEvent != null) {
				OrientationChangedEvent (orientation);
			}
		}
		// Debug.Log ();
	}

	[ContextMenu ("Switch")]
	public void Switch () {
		CanvasScaler scaler = GetComponent<CanvasScaler> ();
		Vector2 newRef = new Vector2 (
			scaler.referenceResolution.y, scaler.referenceResolution.x
		);
		scaler.referenceResolution = newRef;

		scaler.matchWidthOrHeight = 1 - scaler.matchWidthOrHeight;
	}
}
