using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Code.System;
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
        
        [Header("Resource Properties")]
        [SerializeField] private ResourceToGatherType type;
        
        [Header("Assets")]
        [SerializeField] private AudioClip gatheringSound;
        
        protected Resource resource;
        protected GatheringSocket[] gatheringSockets;
        protected readonly Dictionary<Villager, Task_ResourceGathering> gatherers = new Dictionary<Villager, Task_ResourceGathering>();
        
        private int maximumGatherers;
        
        public Vector2Int Size => size;
        public Vector3 PivotedPosition => transform.position + pivot;
        public ResourceToGatherType Type => type;
        public Resource Resource => resource;
        public bool CanGather =>
            gatherers.Keys.Count < maximumGatherers;

        #region AI

        public abstract void StartGathering(Villager worker);
        public abstract bool Gather(Villager worker, int socketId);
        protected abstract void DepleteResource();

        public void Initialize(ResourceToGatherData resourceToGatherData)
        {
            resource = new Resource(resourceToGatherData.ResourceType, resourceToGatherData.Amount);
            type = resourceToGatherData.ResourceToGatherType;
            maximumGatherers = resourceToGatherData.MaximumGatherers;
            gatheringSockets = new GatheringSocket[maximumGatherers];
            
            for (int i = 0; i < gatheringSockets.Length; i++) {
                AudioSource channel = gameObject.AddComponent<AudioSource>();
                channel.volume = 0.3f;
                channel.playOnAwake = false;
                channel.spatialBlend = 1f;
                channel.minDistance = 3f;
                channel.maxDistance = 10f;
                channel.clip = gatheringSound;
                
                gatheringSockets[i] = new GatheringSocket(resource, DepleteResource, channel);
            }
        }
        
        #endregion

        #region Gathering

        protected IEnumerator ClearResource()
        {
            yield return new WaitForSeconds(5f);

            Managers.I.Areas
                .GetAreaByCoords(Vector3Int.FloorToInt(transform.position))
                .RemoveResourceToGather(this);
            DestroyImmediate(gameObject);
        }
        
        protected void UnregisterGatherer(Villager worker)
        {
            gatheringSockets[gatherers.Keys.ToList().IndexOf(worker)].ResetGathering();
            gatherers.Remove(worker);
        }

        
        public int RegisterGatherer(Villager worker, Task_ResourceGathering task)
        {
            gatherers[worker] = task;
            return gatherers.Keys.ToList().IndexOf(worker);
        }
        
        #endregion
    }
}
