using UnityEngine;

namespace AssetStore.Wolv_Interactive.Pixel_Perfect_Retro_Camera.Scripts
{
	[ExecuteInEditMode]
	public class SnapToPixel : MonoBehaviour {
		private PixelCamera cam;
	
		float d;
	
		void Start() {
			cam = GetComponentInChildren<PixelCamera>();
		
			d = 1f / cam.pixelsPerUnit;
		}

		void LateUpdate() {
			Vector3 pos = transform.position;
			Vector3 camPos = new Vector3 (pos.x - pos.x % d, pos.y - pos.y % d, pos.z);	
			cam.transform.position = camPos;
		}
	}
}
