using UnityEngine;
using UnityEngine.UI;

namespace HopeMain.Code.GUI.UIElements
{
    /// <summary>
    /// 
    /// </summary>
    public class UIScrollYArea : UIScrollArea
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

            float newY = contentPos.y + value * elementValue;
            newY = Mathf.Clamp(newY, minValue, maxValue);
            contentTransform.localPosition = new Vector3(contentPos.x, newY, contentPos.z);
        }

        protected override void ResetArea()
        {
            Transform contentTransform = currentContent.transform;
            Vector3 contentPos = contentTransform.localPosition;
            contentTransform.localPosition = new Vector3(contentPos.x, minValue, contentPos.z);
        }

        protected override void CountAreaProperties()
        {
            float spacing = currentContent.GetComponent<VerticalLayoutGroup>().spacing;
            float elementY = currentContent.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
            elementValue = elementY + spacing;
            minValue = currentContent.transform.localPosition.y;
            maxValue = minValue * -1;
        }
    }
}
