using System;
using UnityEngine;

	[RequireComponent(typeof(Camera))]
	public class CCamera : MonoBehaviour {

		[Serializable]
		public abstract class Module : MonoBehaviour
		{
			protected Transform cameraTarget;
			public bool smoothCamera;
			public float smoothMoveSpeed;
			public float smoothRotationSpeed;

			public void SetCameraTarget(Transform newTarget)
			{
				cameraTarget = newTarget;
			}

			public virtual void UpdateCamera(out Vector3 cameraPos, out Quaternion cameraRot)
			{
				if (!cameraTarget)
				{
					cameraPos = Vector3.zero;
					cameraRot = Quaternion.identity;
					return;
				}
				cameraPos = cameraTarget.position;
				cameraRot = cameraTarget.rotation;            
			}

			public virtual void Initialize(Transform cameraTransform)
			{

			}
		}

		public Transform			cameraTarget;

		[SerializeField]
		public Module[]		cameras;

		private Module				activeCamera;

		private int							activeCameraIndex;

		private void Awake()
		{
			if (cameras == null
			    || cameras[0] == null)
			{
				Debug.LogWarning("No default camera module set up. Destroying " + gameObject + " because of this.");
				Destroy(gameObject);
				return;
			}
			
			activeCameraIndex = 0;
			SwitchToCamera(activeCameraIndex);
		}

		private void SwitchToCamera(int index)
		{
			cameras[index].SetCameraTarget(cameraTarget);
			cameras[index].Initialize(transform);
			activeCamera = cameras[index];
		}

		private void LateUpdate()
		{
			if (!activeCamera) return;
			var cameraPos = transform.position;
			var cameraRot = transform.rotation;
			activeCamera.UpdateCamera(out cameraPos, out cameraRot);
			if (!activeCamera.smoothCamera)
			{
				transform.position = cameraPos;
				transform.rotation = cameraRot;
			}
			else
			{
				transform.position = Vector3.Lerp(transform.position, cameraPos, Time.deltaTime * activeCamera.smoothMoveSpeed);
				transform.rotation = Quaternion.Lerp(transform.rotation, cameraRot, Time.deltaTime * activeCamera.smoothRotationSpeed);
			}
		}

		public void SwitchCamera()
		{
			if (activeCameraIndex < cameras.Length)
				activeCameraIndex++;
			else
				activeCameraIndex = 0;
		
			SwitchToCamera(activeCameraIndex);
		}
	}

