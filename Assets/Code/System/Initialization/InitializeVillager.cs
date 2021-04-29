using Code.Map.Building;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.System.Initialization
{
    public class InitializeVillager : InitializeObject
    {
        [SerializeField] private ProfessionData professionData;
        [SerializeField] private Workplace workplace;

        public Workplace Workplace
        {
            get => workplace;
            set => workplace = value;
        }
        
        public override void InitializeMe()
        {
            Villager villager = GetComponent<Villager>();
            Managers.Instance.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position))
                .AddVillager(villager);
            Managers.Instance.Professions.SetVillagerProfession(villager, professionData, workplace);
            villager.Profession.enabled = true;
        }
    }
}
