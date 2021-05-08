using System;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.Map.Resources.ResourceToGather
{
    [Serializable]
    public class GatheringSocket
    {
        private readonly Resource resource;
        
        private float timeCounter;
        private float resourceCounter;

        private Action onResourceDepleted;

        public GatheringSocket(Resource resource, Action onResourceDepleted)
        {
            this.resource = resource;
            this.onResourceDepleted = onResourceDepleted;
        }

        public void GatherResource(float resourcePerFrame, Profession worker)
        {
            timeCounter += Time.deltaTime;
            resourceCounter += resourcePerFrame * Time.deltaTime;

            if (timeCounter < 1) return;
            int gatheredResource = Mathf.FloorToInt(resourceCounter);

            if (worker.CarriedResource.amount + gatheredResource > worker.Data.ResourceCarryingLimit) 
                gatheredResource = worker.Data.ResourceCarryingLimit - worker.CarriedResource.amount;
            
            int newAmount = resource.amount - gatheredResource;
            
            if (newAmount > 0) {
                resource.amount = newAmount;
                worker.CarriedResource.amount += gatheredResource;
                ResetGathering();
            }
            else {
                gatheredResource += newAmount;
                worker.CarriedResource.amount += gatheredResource;
                ResetGathering();
                onResourceDepleted.Invoke();
            }
            
            // Debug.LogError(worker.name + " gathered: " + gatheredResource + ". [" + worker.CarriedResource.amount + "/" + worker.Data.ResourceCarryingLimit + "]");
        }

        public void ResetGathering()
        {
            resourceCounter = 0f;
            timeCounter = 0;
        }
    }
}
