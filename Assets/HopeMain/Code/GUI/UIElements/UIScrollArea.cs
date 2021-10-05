using UnityEngine;

namespace HopeMain.Code.GUI.UIElements
{
    public abstract class UIScrollArea : MonoBehaviour
    {
        [SerializeField] protected Transform[] content;

        protected Transform currentContent;
        protected float elementValue;
        protected float maxValue;
        protected float minValue;
        
        protected abstract void CountAreaProperties();
        
        public abstract void ChangeValue(int value);

        protected abstract void ResetArea();
        
        public void ChangeContent(int id)
        {
            ResetArea();
            
            currentContent.gameObject.SetActive(false);
            currentContent = content[id];
            currentContent.gameObject.SetActive(true);
            
            CountAreaProperties();
        }
    }
}
