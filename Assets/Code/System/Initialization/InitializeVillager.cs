using System;
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

            switch (villager.Profession.Type) {
                case ProfessionType.Unemployed:
                    villager.Profession.Initialize();
                    break;
                
                case ProfessionType.Builder:
                case ProfessionType.Lumberjack:
                case ProfessionType.WorkplaceHauler:
                case ProfessionType.GlobalHauler:
                    villager.Profession.Initialize();
                    break;

                default:
                    throw new Exception("No such profession type: " + villager.Profession.Type + " to initialize!");
            }
            
            villager.Profession.Workplace.HireWorker(villager.Profession);
            villager.UI.ProfessionName.text = villager.Profession.Type.ToString();
        }
    }
}
