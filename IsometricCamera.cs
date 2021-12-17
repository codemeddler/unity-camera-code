using UnityEngine;


    public class IsometricCamera : CCamera.Module {

        public float	distance;

        private Vector3			startDirection;

        public override void UpdateCamera(out Vector3 cameraPos, out Quaternion cameraRot)
        {
            if (cameraTarget == null)
            {
                cameraPos = Vector3.zero;
                cameraRot = Quaternion.identity;
                return;
            }

            cameraPos = cameraTarget.position - startDirection * distance;
            cameraRot = Quaternion.LookRotation(cameraTarget.position - cameraPos);
        }

        public override void Initialize(Transform cameraTransform)
        {
            startDirection = cameraTransform.forward;
        }
    }

