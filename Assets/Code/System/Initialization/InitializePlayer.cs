using Code.AI;
using Code.System.Areas;
using UnityEngine;

namespace Code.System.Initialization
{
    public class InitializePlayer : InitializeObject
    {
        public override void InitializeMe()
        {
            Area area = Managers.I.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position));
            area.SetPlayerToArea(gameObject);
            area.SetVisitorWalkingAudio(gameObject);
            GetComponent<EntityBrain>().CurrentArea = area;
        }
    }
}