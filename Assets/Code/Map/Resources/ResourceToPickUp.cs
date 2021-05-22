using System.Collections;
using Code.Map.Building;
using Code.Map.Building.Buildings.Types.Resources;
using Code.System;
using Code.Villagers.Tasks;
using ThirdParty.LeanTween.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.Map.Resources
{
    public class ResourceToPickUp : MonoBehaviour
    {
        [Header("Resource asset elements")]
        [SerializeField] private SpriteRenderer resourceImage;
        
        private Resource storedResource;
        private Warehouse warehouse;
        
        private float fallingTime = 1.2f;
        private float decayTimeSeconds = 60;

        private float newMapX;
        
        public Task_ResourcePickUp TaskResourcePickUp { get; set; }
        public Resource StoredResource => storedResource;
        
        public bool IsRegisteredToPickUp => TaskResourcePickUp != null;

        private void RegisterResourceOnGround()
        {
            Debug.LogWarning(
                "Resource: " + storedResource.Type + " " + storedResource.amount + " is on the ground.");
            
            if (!(Managers.I.Buildings.GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse),
                transform.position) is Warehouse w)) return;

            warehouse = w;
            warehouse.RegisterResourceToPickUp(this);
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
            yield return new WaitForSeconds(decayTimeSeconds);
            warehouse.UnregisterResourceToPickUp(this);
            TaskResourcePickUp.RemoveResourceBeforePickUp(this);
            
            DestroyImmediate(gameObject);
        }

        public void Initialize(Resource resource, float mapX)
        {
            newMapX = Random.Range(mapX - 2, mapX + 2);
            storedResource = new Resource(resource);
            resourceImage.sprite = AssetsStorage.I.GetResourceIcon(storedResource.Type);

            gameObject.LeanMove(new Vector3(mapX, 0f, 0f), fallingTime)
                .setEaseInQuart()
                .setOnComplete(RegisterResourceOnGround);

            gameObject.LeanMove(new Vector3(newMapX, 0, 0f), fallingTime)
                .setEaseOutBounce();
        }
        
        public Resource WithdrawResource()
        {
            StartCoroutine(DestroyOnDelay());
            return new Resource(storedResource);
        }
    }
}
