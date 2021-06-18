using Code.Map.Building.Workplaces;
using Code.System.Areas;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.System.Initialization
{
    public class InitializeVillager : InitializeObject
    {
        [SerializeField] private Villager_ProfessionData villagerProfessionData;
        [SerializeField] private Workplace workplace;

        public Workplace Workplace
        {
            get => workplace;
            set => workplace = value;
        }
        
        public override void InitializeMe()
        {
            Villager villager = GetComponent<Villager>();
            Area area = Managers.I.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position));
            area.AddVillager(villager);
            area.SetVisitorWalkingAudio(villager.gameObject);

            Managers.I.Professions.SetVillagerProfession(villager, villagerProfessionData, workplace);
            villager.Profession.enabled = true;
        }
    }
}
