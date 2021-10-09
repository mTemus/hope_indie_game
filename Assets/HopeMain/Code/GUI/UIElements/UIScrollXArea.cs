using UnityEngine;
using UnityEngine.UI;

namespace HopeMain.Code.GUI.UIElements
{
    /// <summary>
    /// 
    /// </summary>
    public class UIScrollXArea : UIScrollArea
    {
        private void Awake()
        {
            currentContent = content[0];
            CountAreaProperties();
        }

        public override void ChangeValue(int value)
        {
            Transform contentTransform = currentContent.transform;
            Vector3 contentPos = contentTransform.localPosition;

            float newX = contentPos.x + value * elementValue;
            newX = Mathf.Clamp(newX, minValue, maxValue);
            contentTransform.localPosition = new Vector3(newX, contentPos.y, contentPos.z);
        }

        protected override void ResetArea()
        {
            Transform contentTransform = currentContent.transform;
            Vector3 contentPos = contentTransform.localPosition;
            contentTransform.localPosition = new Vector3(maxValue, contentPos.y, contentPos.z);
        }

        protected override void CountAreaProperties()
        {
            float spacing = currentContent.GetComponent<HorizontalLayoutGroup>().spacing;
            float elementX =currentContent.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
            elementValue = elementX + spacing;
            maxValue = currentContent.transform.localPosition.x;
            minValue = maxValue * -1;
        }
    }
}
