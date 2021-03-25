using Cinemachine;
using UnityEngine;

namespace Code.System.Camera
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Camera mainCamera;
        [SerializeField] private CinemachineVirtualCamera cmv;

        public void FocusCameraOn(Transform target) => 
            cmv.Follow = target;

        public void FocusCameraOnPlayer()
        {
            if (IsCameraOnPlayer()) return;
            FocusCameraOn(Managers.Instance.Player.PlayerGO.transform);
        }

        public bool IsCameraOnPlayer() =>
            cmv.Follow == Managers.Instance.Player.PlayerGO.transform;
    }
}
