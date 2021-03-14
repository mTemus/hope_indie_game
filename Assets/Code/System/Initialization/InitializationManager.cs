using UnityEngine;

namespace Code.System.Initialization
{
    public class InitializationManager : MonoBehaviour
    {
        [SerializeField] private InitializeObject[] buildings;
        [SerializeField] private InitializeObject[] villagers;

        private void Start()
        {
            InitializeObjects(buildings);
            InitializeObjects(villagers);
        }

        private void InitializeObjects(InitializeObject[] objects)
        {
            foreach (InitializeObject o in objects) {
                o.InitializeMe();
                DestroyImmediate(o.GetComponent<InitializeObject>());
            } 
        }
    }
}
