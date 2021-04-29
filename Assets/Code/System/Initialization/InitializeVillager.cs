using Code.Map.Building;
using Code.System.Areas;
using Code.Villagers.Entity;
using Code.Villagers.Professions;
using UnityEngine;

namespace Code.System.Initialization
{
    public class InitializeVillager : InitializeObject
    {
        [SerializeField] private ProfessionType professionType;
        [SerializeField] private Workplace workplace;

        public Workplace Workplace
        {
            get => workplace;
            set => workplace = value;
        }
        
        public override void InitializeMe()
        {
            Villager villager = GetComponent<Villager>();
            Area myArea = Managers.Instance.Areas.GetAreaByCoords(Vector3Int.FloorToInt(transform.position));
            myArea.AddVillager(villager);
            Managers.Instance.Professions.SetVillagerProfession(villager, professionType, workplace);
            villager.Profession.enabled = true;
        }
    }
}
