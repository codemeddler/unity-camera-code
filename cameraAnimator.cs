using UnityEngine;
using System.Collections;

public class cameraAnimator : MonoBehaviour {

	public static cameraAnimator Instance {
		get {
			return instance;
		}
	}

	static cameraAnimator instance;

	string currentCameraAnimation;
	string newCameraAnimation;

	public Transform BattleCameraTransform;
	public Transform MenuCameraTransform;
	public Transform MetaCameraTransform;
	public Transform ShopCameraTransform;
	public Transform CodexCameraTransform;

	public float CameraTransitionTime;

	public AnimationCurve EasingCurve;

	Transform ownTrans;

	bool bTransitioning;

	void Awake() {
		instance=this;
		ownTrans=this.transform;
		currentCameraAnimation="";
		newCameraAnimation="";
		bTransitioning=false;
	}

	public void TriggerCameraTransition( string animationToTrigger ) {
		if( bTransitioning ) {
			StopAllCoroutines();
			currentCameraAnimation=newCameraAnimation;
			bTransitioning=false;
		}
		newCameraAnimation=animationToTrigger;
	}

	public bool IsCameraTransitionPlaying() {
		return bTransitioning;
	}

	void LateUpdate() {
		if( newCameraAnimation==currentCameraAnimation )
			return;

		if( bTransitioning )
			return;

		Transform targetTransform=null;
		switch( newCameraAnimation ) {
			case "TO_BATTLE_CAMERA":
				targetTransform=BattleCameraTransform;
				bTransitioning=true;
				StartCoroutine( TransitionToPosition( ownTrans,targetTransform,CameraTransitionTime ) );
				break;
			case "TO_MENU_CAMERA":
				targetTransform=MenuCameraTransform;
				bTransitioning=true;
				StartCoroutine( TransitionToPosition( ownTrans,targetTransform,CameraTransitionTime ) );
				break;
			case "TO_META_CAMERA":
				targetTransform=MetaCameraTransform;
				bTransitioning=true;
				StartCoroutine( TransitionToPosition( ownTrans,targetTransform,CameraTransitionTime ) );
				break;
			case "TO_SHOP_CAMERA":
				targetTransform=ShopCameraTransform;
				bTransitioning=true;
				StartCoroutine( TransitionToPosition( ownTrans,targetTransform,CameraTransitionTime ) );
				break;
			case "TO_CODEX_CAMERA":
				targetTransform=CodexCameraTransform;
				bTransitioning=true;
				StartCoroutine( TransitionToPosition( ownTrans,targetTransform,CameraTransitionTime ) );
				break;
			case "":
				break;
		}

		if( targetTransform==null ) {
			currentCameraAnimation=newCameraAnimation;
			return;
		}
	}

	private IEnumerator TransitionToPosition( Transform movingTrans,Transform moveToTrans,float duration ) {
		float animationTime=0;
		var startPos=movingTrans.position;
		var startRot=movingTrans.rotation;
		var endPos=moveToTrans.position;
		var endRot=moveToTrans.rotation;
		var animationTimeLength=EasingCurve[ EasingCurve.length-1 ].time;

		while( animationTime<animationTimeLength ) {
			animationTime+=( Time.deltaTime/duration );
			movingTrans.position=Vector3.Lerp( startPos,endPos,EasingCurve.Evaluate( animationTime ) );
			movingTrans.rotation=Quaternion.Slerp( startRot,endRot,EasingCurve.Evaluate( animationTime ) );
			yield return null;
		}

		bTransitioning=false;
		currentCameraAnimation=newCameraAnimation;
	}
}
