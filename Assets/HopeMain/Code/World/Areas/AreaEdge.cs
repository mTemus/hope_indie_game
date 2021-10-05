using System.Collections.Generic;
using Code.AI;
using UnityEngine;

namespace Code.System.Areas
{
    public class AreaEdge : MonoBehaviour
    {
        [SerializeField] private Area area;
        [SerializeField] private List<GameObject> visitors;
        
        private void OnTriggerEnter2D(Collider2D visitor)
        {
            if (visitors.Contains(visitor.gameObject)) return;
            GameObject visitorGO = visitor.gameObject;
            visitors.Add(visitorGO);

            if (area.ContainsEntity(visitorGO)) return;
            visitorGO.GetComponent<EntityBrain>().CurrentArea.HandleLeavingEntity(visitorGO);
            visitors.Remove(visitor.gameObject);

            area.HandleEnteringEntity(visitorGO);
        }

        private void OnTriggerExit2D(Collider2D visitor)
        {
            visitors.Remove(visitor.gameObject);
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
