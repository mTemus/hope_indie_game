using System.Collections.Generic;
using Code.Player.Brain;
using Code.System.Assets;
using Code.Villagers.Brain;
using UnityEngine;

namespace Code.System.Areas
{
    public class AreaEdge : MonoBehaviour
    {
        [SerializeField] private Area area;
        [SerializeField] private List<GameObject> visitors;
        
        private void OnTriggerEnter2D(Collider2D visitor)
        {
            if (!visitors.Contains(visitor.gameObject)) 
                visitors.Add(visitor.gameObject);
        }

        private void OnTriggerExit2D(Collider2D visitor)
        {
            if (!visitors.Contains(visitor.gameObject)) return;
            GameObject visitorGO = visitor.gameObject;

            switch (visitor.tag) {
                case "Player":
                    if (area.IsPlayerInArea) return;
                    Managers.I.Areas.SetPlayerToArea(area, visitor.gameObject);
                    visitorGO
                        .GetComponent<Player_Brain>().Sounds
                        .SetWalkingAudioClip(AssetsStorage.I.
                            GetAudioClipByName(AssetSoundType.Walking, area.name.ToLower()));
                    
                    visitors.Remove(visitor.gameObject);
                    break;
                
                case "Villager":
                    visitorGO
                        .GetComponent<Villager_Brain>().Sounds
                        .SetWalkingAudioClip(AssetsStorage.I
                            .GetAudioClipByName(AssetSoundType.Walking, area.name.ToLower()));
                    visitors.Remove(visitorGO);
                    break;
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Vector3 areaSize = new Vector3(1, 2, 0);

            if (name.Contains("Left")) 
                Gizmos.DrawWireCube(transform.position - new Vector3(areaSize.x, -2 ,0) * 0.5f, areaSize);
            else 
                Gizmos.DrawWireCube(transform.position + areaSize * 0.5f, areaSize);
        }
    }
}
