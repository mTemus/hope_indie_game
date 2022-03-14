using _Prototype.Code.v001.AI;
using _Prototype.Code.v001.World.Areas;
using UnityEngine;

namespace _Prototype.Code.v001.System.Initialization
{
    public class InitializePlayer : InitializeObject
    {
        public override void InitializeMe()
        {
            Area area = Managers.I.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position));
            area.SetPlayerToArea(gameObject);
            area.SetVisitorWalkingAudio(gameObject);
            GetComponent<EntityBrain>().CurrentArea = area;
            DestroyImmediate(this);
        }
    }
}
