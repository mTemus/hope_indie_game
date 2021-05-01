using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.Villagers.Entity;
using Code.Villagers.Tasks;
using UnityEngine;

namespace Code.Map.Resources.ResourceToGather
{
    public enum ResourceToGatherType
    {
        Single, 
        Spot
    }
    
    public abstract class ResourceToGather : MonoBehaviour
    {
        [Header("World Properties")]
        [SerializeField] private Vector2Int size;
        [SerializeField] private Vector3 pivot;
        
        protected Resource resource;
        protected GatheringSocket[] gatheringSockets;
        protected Dictionary<Villager, ResourceGatheringTask> gatherers = new Dictionary<Villager, ResourceGatheringTask>();

        private ResourceToGatherType type;
        private int maximumGatherers;
        
        public Vector2Int Size => size;
        public Vector3 PivotedPosition => transform.position + pivot;
        public ResourceToGatherType Type => type;
        public Resource Resource => resource;

        public abstract void OnGatherStart(Villager worker);
        public abstract bool Gather(Villager worker, int socketId);
        protected abstract void OnResourceDepleted();

        protected IEnumerator ClearResource()
        {
            yield return new WaitForSeconds(5f);
            
            //TODO: remove resource data from it's area!
            DestroyImmediate(gameObject);
        }
        
        public void Initialize(ResourceToGatherData resourceToGatherData)
        {
            resource = new Resource(resourceToGatherData.ResourceType, resourceToGatherData.Amount);
            type = resourceToGatherData.ResourceToGatherType;
            maximumGatherers = resourceToGatherData.MaximumGatherers;
            gatheringSockets = new GatheringSocket[maximumGatherers];
            
            for (int i = 0; i < gatheringSockets.Length; i++) 
                gatheringSockets[i] = new GatheringSocket(resource, OnResourceDepleted);
        }
        
        public int RegisterGatherer(Villager worker, ResourceGatheringTask task)
        {
            gatherers[worker] = task;
            return gatherers.Keys.ToList().IndexOf(worker);
        }

        public void UnregisterGatherer(Villager worker)
        {
            gatheringSockets[gatherers.Keys.ToList().IndexOf(worker)].ResetGathering();
            gatherers.Remove(worker);
        }
        
        public bool CanGather() =>
            gatherers.Keys.Count < maximumGatherers;
    }
}
