using UnityEngine;

namespace AssetStore.Wolv_Interactive.Pixel_Perfect_Retro_Camera.Scenes.Example
{
	public class Spin : MonoBehaviour {
		void Update () {
			transform.Rotate(0, 90f * Time.deltaTime, 0);
		}
	}
}
