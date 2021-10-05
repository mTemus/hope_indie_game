using Cinemachine;
using UnityEngine;

namespace HopeMain.Code.System.Camera
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
            FocusCameraOn(Managers.I.Player.PlayerGO.transform);
        }

        public bool IsCameraOnPlayer() =>
            cmv.Follow == Managers.I.Player.PlayerGO.transform;
    }
}
