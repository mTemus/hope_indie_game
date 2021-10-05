using System;
using HopeMain.Code.Characters.Villagers.Profession;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HopeMain.Code.World.Resources.ResourceToGather
{
    [Serializable]
    public class GatheringSocket
    {
        private readonly Resource resource;
        
        private float timeCounter;
        private float resourceCounter;
        private AudioSource gatherChannel;

        private Action onResourceDepleted;
        
        public GatheringSocket(Resource resource, Action onResourceDepleted, AudioSource gatherChannel)
        {
            this.resource = resource;
            this.onResourceDepleted = onResourceDepleted;
            this.gatherChannel = gatherChannel;
        }

        private void PlayGatheringSound()
        {
            if (gatherChannel.isPlaying) return;
            gatherChannel.pitch = Random.Range(0.8f, 1f);
            gatherChannel.Play((ulong) 0.5f);
        }

        public void GatherResource(float resourcePerFrame, Villager_Profession worker)
        {
            PlayGatheringSound();
            timeCounter += Time.deltaTime;
            resourceCounter += resourcePerFrame * Time.deltaTime;
            
            if (timeCounter < 1) return;

            if (gatherChannel.isPlaying) gatherChannel.Stop();
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
