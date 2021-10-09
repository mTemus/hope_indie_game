using System;
using HopeMain.Code.Characters.Villagers.Professions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HopeMain.Code.World.Resources.ResourceToGather
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class GatheringSocket
    {
        private readonly Resource _resource;
        
        private float _timeCounter;
        private float _resourceCounter;
        private AudioSource _gatherChannel;

        private Action _resourceDepleted;
        
        public GatheringSocket(Resource resource, Action resourceDepleted, AudioSource gatherChannel)
        {
            _resource = resource;
            _resourceDepleted = resourceDepleted;
            _gatherChannel = gatherChannel;
        }

        private void PlayGatheringSound()
        {
            if (_gatherChannel.isPlaying) return;
            _gatherChannel.pitch = Random.Range(0.8f, 1f);
            _gatherChannel.Play((ulong) 0.5f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourcePerFrame"></param>
        /// <param name="worker"></param>
        public void GatherResource(float resourcePerFrame, Profession worker)
        {
            PlayGatheringSound();
            _timeCounter += Time.deltaTime;
            _resourceCounter += resourcePerFrame * Time.deltaTime;
            
            if (_timeCounter < 1) return;

            if (_gatherChannel.isPlaying) _gatherChannel.Stop();
            int gatheredResource = Mathf.FloorToInt(_resourceCounter);

            if (worker.CarriedResource.amount + gatheredResource > worker.Data.ResourceCarryingLimit) 
                gatheredResource = worker.Data.ResourceCarryingLimit - worker.CarriedResource.amount;
            
            int newAmount = _resource.amount - gatheredResource;
            
            if (newAmount > 0) {
                _resource.amount = newAmount;
                worker.CarriedResource.amount += gatheredResource;
                ResetGathering();
            }
            else {
                gatheredResource += newAmount;
                worker.CarriedResource.amount += gatheredResource;
                ResetGathering();
                _resourceDepleted.Invoke();
            }
            
            // Debug.LogError(worker.name + " gathered: " + gatheredResource + ". [" + worker.CarriedResource.amount + "/" + worker.Data.ResourceCarryingLimit + "]");
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetGathering()
        {
            _resourceCounter = 0f;
            _timeCounter = 0;
        }
    }
}
