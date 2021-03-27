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
                    villager.Profession.InitializeUnemployedAI();
                    break;
                
                case ProfessionType.Builder:
                case ProfessionType.Lumberjack:
                    villager.Profession.InitializeWorkerAI();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
