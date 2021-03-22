using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI.UIElements
{
    public class UiXScrollArea : UiScrollArea
    {
        private void Awake()
        {
            currentContent = content[0];
            CountAreaProperties();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) 
                ChangeValue(1);
            
            if (Input.GetKeyDown(KeyCode.RightArrow)) 
                ChangeValue(-1);
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
            contentTransform.localPosition = new Vector3(minValue, contentPos.y, contentPos.z);
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
