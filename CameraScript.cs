using UnityEngine;
using System.Collections;

public class CCamera : MonoBehaviour {

	CameraSpot[] cameraSpots;
	ParticleEmitter emitter;

	void Awake()
	{
		int i = 1;
		int spotCount = GameObject.FindGameObjectsWithTag("CameraSpot").Length;

		cameraSpots = new CameraSpot[spotCount];

		while (GameObject.Find("CameraSpot" + i.ToString()) != null && i <= spotCount)
		{
			cameraSpots[i-1] = GameObject.Find("CameraSpot" + i.ToString()).GetComponent<CameraSpot>();
			i++;
		}

		transform.position = cameraSpots[0].position;
		transform.rotation = cameraSpots[0].rotation;

		emitter = GetComponentInChildren<ParticleEmitter>();
	}

	void Start()
	{
		GetComponent<Camera>().orthographicSize *= UIScaler.uiScaleHeight;
	}

	public void SwitchCamera(int spot)
	{
		transform.position = cameraSpots[spot-1].position;
		transform.rotation = cameraSpots[spot-1].rotation;
	}

	public void EnableTransition()
	{
		emitter.emit = true;
		AudioManager.Manager.PlayFX(AudioManager.AudioType.BUBBLESOUND);
		Invoke("DisableTransition", 2.0f);
	}

	void DisableTransition()
	{
		emitter.emit = false;
	}
}
