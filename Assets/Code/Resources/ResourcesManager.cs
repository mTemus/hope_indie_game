using UnityEngine;

namespace Code.Resources
{
   public class ResourcesManager : MonoBehaviour
   {
      private static Resource wood;
      private static Resource stone;

      private void Start()
      {
         wood = new Resource();
         stone = new Resource();
      }

      public static Resource Wood => wood;

      public static Resource Stone => stone;
   }
}
