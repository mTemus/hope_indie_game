using UnityEngine;

namespace _Prototype.Code.v001.GUI.UIElements
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class UIScrollArea : MonoBehaviour
    {
        [SerializeField] protected Transform[] content;

        protected Transform currentContent;
        protected float elementValue;
        protected float maxValue;
        protected float minValue;
        
        protected abstract void CountAreaProperties();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public abstract void ChangeValue(int value);

        protected abstract void ResetArea();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
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
