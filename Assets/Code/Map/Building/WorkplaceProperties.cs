using UnityEngine;

namespace Code.Map.Building
{
    public enum WorkplaceType
    {
        Gathering, Production, Services
    }
    
    [CreateAssetMenu(fileName = "Workplace Properties", menuName = "Game Data/Map Objects/Workplace Properties", order = 1)]
    public class WorkplaceProperties : ScriptableObject
    {
        //TODO: ADD Workplace properties while creating building and initiating building
        
        [Header("Workplace properties")]
        [SerializeField] private int workers;
        [SerializeField] private int haulers;
        [SerializeField] private WorkplaceType type;
        
        public int Workers => workers;
        public int Haulers => haulers;
        public WorkplaceType Type => type;
    }
}