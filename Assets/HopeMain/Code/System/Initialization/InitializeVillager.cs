using HopeMain.Code.Characters.Villagers.Entity;
using HopeMain.Code.Characters.Villagers.Professions;
using HopeMain.Code.World.Areas;
using HopeMain.Code.World.Buildings.Workplace;
using HopeMain.Code.World.Buildings.Workplaces;
using UnityEngine;

namespace HopeMain.Code.System.Initialization
{
    public class InitializeVillager : InitializeObject
    {
        [SerializeField] private Data professionData;
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

            Managers.I.Professions.SetVillagerProfession(villager, professionData, workplace);
            villager.Profession.enabled = true;
            
            DestroyImmediate(this);
        }
    }
}
