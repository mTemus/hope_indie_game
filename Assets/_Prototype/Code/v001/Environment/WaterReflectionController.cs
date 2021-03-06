using UnityEngine;

namespace _Prototype.Code.v001.Environment
{
    /// <summary>
    /// 
    /// </summary>
    public class WaterReflectionController : MonoBehaviour
    {
        [SerializeField] private Transform target;

        void Update()
        {
            Transform myTransform = transform;
            Vector3 myPosition = myTransform.position;
            myTransform.position = new Vector3(target.transform.position.x, myPosition.y, myPosition.z);
        }
    }
}
