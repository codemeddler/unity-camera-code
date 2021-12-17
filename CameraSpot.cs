using UnityEngine;
using System.Collections;

public class CameraSpot : MonoBehaviour {

	public Vector3 position { get; private set; }
	public Transform defaultLookAt;
	public Quaternion rotation { get; private set; }

	void Awake()
	{
		position = transform.position;
		rotation = Quaternion.LookRotation(defaultLookAt.position - position);
	}
}
