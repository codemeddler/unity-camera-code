using System.Collections.Generic;
using System.Linq;
using UnityEngine;

	public class CXRayView : MonoBehaviour {

		public Material alphaMaterial;

		public LayerMask blockLayer;
		public string blockTag;
		private Dictionary<GameObject, Material> hiddenObjects;

		private void Awake()
		{
			hiddenObjects = new Dictionary<GameObject, Material>();
		}

		private void LateUpdate()
		{
			var checkDir = PlayerController.ControlledCharacter.transform.position - transform.position;
			var checkDistance = checkDir.magnitude;
			checkDir.Normalize();
		
			var blockers = Physics.RaycastAll(transform.position, checkDir, checkDistance, blockLayer);

			if(blockers.Length > 0)
			{
				foreach (var t in blockers)
				{
					var hitObject = t.transform.gameObject;
				
					if (hitObject.tag != blockTag)
						continue;

					if (hiddenObjects.ContainsKey(hitObject))
						continue;

					var renderer = hitObject.GetComponentInChildren<Renderer>();
					if (!renderer) continue;
					var oldMaterial = renderer.sharedMaterial;
					hiddenObjects.Add(hitObject, oldMaterial);
					renderer.material = alphaMaterial;
					renderer.material.mainTexture = oldMaterial.mainTexture;
				}
			}

			var keys = new GameObject[hiddenObjects.Keys.Count];
			hiddenObjects.Keys.CopyTo(keys, 0);

			foreach (var t in keys)
			{
				var bFound = blockers.Select(t1 => t1.transform.gameObject).Any(hitObject => hitObject == t);

				if (bFound) continue;
				Material restoredMaterial;
				hiddenObjects.TryGetValue(t, out restoredMaterial);
				var renderer = t.GetComponentInChildren<Renderer>();
				renderer.material = restoredMaterial;
				hiddenObjects.Remove(t);
			}
		}
	}

