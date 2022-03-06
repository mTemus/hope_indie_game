using System.Collections;
using _Prototype.Code.AI.Villagers.Tasks;
using _Prototype.Code.System;
using _Prototype.Code.System.Assets;
using _Prototype.Code.World.Buildings;
using _Prototype.Code.World.Buildings.Type.Resources;
using ThirdParty.LeanTween.Framework;
using UnityEngine;

namespace _Prototype.Code.World.Resources
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceToPickUp : MonoBehaviour
    {
        [Header("Resource asset elements")]
        [SerializeField] private SpriteRenderer resourceImage;
        
        private Resource _storedResource;
        private Warehouse _warehouse;
        
        private float _fallingTime = 1.2f;
        private float _decayTimeSeconds = 60;

        private float _newMapX;
        
        public ResourcePickUp ResourcePickUpTask { get; set; }
        public Resource StoredResource => _storedResource;
        
        public bool IsRegisteredToPickUp => ResourcePickUpTask != null;

        private void RegisterResourceOnGround()
        {
            Debug.LogWarning(
                "Resource: " + _storedResource.Type + " " + _storedResource.amount + " is on the ground.");
            
            if (!(Managers.I.Buildings.GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse),
                transform.position) is Warehouse w)) return;

            _warehouse = w;
            _warehouse.RegisterResourceToPickUp(this);
            StartCoroutine(Decay());
        }

        private IEnumerator DestroyOnDelay()
        {
            StopCoroutine(Decay());
            yield return new WaitForSeconds(0.01f);
            DestroyImmediate(gameObject);
        }
        
        private IEnumerator Decay()
        {
            yield return new WaitForSeconds(_decayTimeSeconds);
            _warehouse.UnregisterResourceToPickUp(this);
            ResourcePickUpTask.RemoveResourceBeforePickUp(this);
            
            DestroyImmediate(gameObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="mapX"></param>
        public void Initialize(Resource resource, float mapX)
        {
            _newMapX = Random.Range(mapX - 2, mapX + 2);
            _storedResource = new Resource(resource);
            resourceImage.sprite = AssetsStorage.I.GetResourceIcon(_storedResource.Type);

            gameObject.LeanMove(new Vector3(mapX, 0f, 0f), _fallingTime)
                .setEaseInQuart()
                .setOnComplete(RegisterResourceOnGround);

            gameObject.LeanMove(new Vector3(_newMapX, 0, 0f), _fallingTime)
                .setEaseOutBounce();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Resource WithdrawResource()
        {
            StartCoroutine(DestroyOnDelay());
            return new Resource(_storedResource);
        }
    }
}
