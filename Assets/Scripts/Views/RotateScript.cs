using UnityEngine;

public class RotateScript : MonoBehaviour {

	public float rotationSpeed;

	private Transform _trans;

	// Use this for initialization
	void Awake () {
		_trans = transform;
	}
	
	// Update is called once per frame
	void Update () {
		_trans.Rotate (0, 0, rotationSpeed * Time.deltaTime);
	}
}
