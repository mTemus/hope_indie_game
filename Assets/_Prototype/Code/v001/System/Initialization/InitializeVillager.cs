using _Prototype.Code.v001.Characters.Villagers.Entity;
using _Prototype.Code.v001.Characters.Villagers.Professions;
using _Prototype.Code.v001.World.Areas;
using _Prototype.Code.v001.World.Buildings.Workplaces;
using UnityEngine;

namespace _Prototype.Code.v001.System.Initialization
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
