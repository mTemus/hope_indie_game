using UnityEngine;

namespace _Prototype.Code.v001.System.Initialization
{
    public class InitializationManager : MonoBehaviour
    {
        [SerializeField] private InitializeObject[] buildings;
        [SerializeField] private InitializeObject[] villagers;
        [SerializeField] private InitializeObject[] resourcesToGather;
        [SerializeField] private InitializeObject[] player;

        private void Start()
        {
            InitializeObjects(player);
            InitializeObjects(resourcesToGather);
            InitializeObjects(buildings);
            InitializeObjects(villagers);
        }

        private void InitializeObjects(InitializeObject[] objects)
        {
            foreach (InitializeObject o in objects) 
                o.InitializeMe();
        }
    }
}
