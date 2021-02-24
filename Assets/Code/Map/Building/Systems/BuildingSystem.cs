using Code.System.Area;
using Code.System.Grid;
using UnityEngine;

namespace Code.Map.Building.Systems
{
    public class BuildingSystem : MonoBehaviour
    {
        [SerializeField] private GameObject warehouse;
        
        void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            Area.Instance.Grid.GetXY(Utilities.Utilities.GetMouseWorldPosition2D(), out int x, out int y);
            Cell targetCell = Area.Instance.Grid.GetCellAt(x, y);

            if (targetCell.CanBuild()) {
                GameObject building = Instantiate(warehouse, Area.Instance.Grid.GetWorldPosition(x, y), Quaternion.identity);
                targetCell.SetBuildingAtCell(building.transform);
            }
            else {
                Debug.LogWarning("Can't build at: [" + x + ", " + y + "].");
            }
            
        }
    }
}
