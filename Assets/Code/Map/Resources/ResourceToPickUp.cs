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
        [SerializeField] private SpriteRenderer resourceImage;
        
        [SerializeField] private Resource storedResource;
        private ResourcePickUpTask rput;
        private Warehouse warehouse;
        
        private float fallingTime = 1.2f;
        private float decayTimeSeconds = 60;

        private float newMapX;
        
        public Resource StoredResource => storedResource;

        public bool IsRegisteredToPickUp => rput != null;

        private void OnResourceOnGround()
        {
            Debug.LogWarning(
                "Resource: " + storedResource.Type + " " + storedResource.amount + " is on the ground.");
            
            if (!(Managers.Instance.Buildings.GetClosestBuildingOfClass(BuildingType.Resources, typeof(Warehouse),
                transform.position) is Warehouse w)) return;

            warehouse = w;
            warehouse.RegisterResourceToPickUp(this);
            StartCoroutine(Decay());
        }

        private IEnumerator DestroyOnDelay()
        {
            StopCoroutine(Decay());
            yield return new WaitForSeconds(0.3f);
            DestroyImmediate(gameObject);
        }
        
        private IEnumerator Decay()
        {
            yield return new WaitForSeconds(decayTimeSeconds);
            warehouse.UnregisterResourceToPickUp(this);
            rput.RemoveResourceBeforePickUp(this);
            
            DestroyImmediate(gameObject);
        }

        public void Initialize(Resource resource, float mapX)
        {
            newMapX = Random.Range(mapX - 2, mapX + 2);
            Vector3 groundPosition = new Vector3(mapX, 0f, 0f);
            Vector3 newGroundPosition = new Vector3(newMapX, 0, 0f);
            
            storedResource = new Resource(resource);
            resourceImage.sprite = AssetsStorage.I.GetResourceIcon(storedResource.Type);

            gameObject.LeanMove(groundPosition, fallingTime)
                .setEaseInQuart()
                .setOnComplete(OnResourceOnGround);

            gameObject.LeanMove(newGroundPosition, fallingTime)
                .setEaseOutBounce();
        }
        
        public void OnResourceRegisterToPickUp(ResourcePickUpTask rputt)
        {
            rput = rputt;
        }

        public Resource WithdrawResource()
        {
            StartCoroutine(DestroyOnDelay());
            return new Resource(storedResource);
        }
    }
}
