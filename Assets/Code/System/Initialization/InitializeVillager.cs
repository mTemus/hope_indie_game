using Code.Villagers.Entity;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.System.Initialization
{
    public class InitializeVillager : InitializeObject
    {
        public override void InitializeMe()
        {
            Villager villager = GetComponent<Villager>();
            Area.Area myArea = Managers.Instance.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position));
            myArea.AddVillager(villager);
            
            villager.GetComponent<Profession>().InitializeWorkerAI();
        }
    }
}
