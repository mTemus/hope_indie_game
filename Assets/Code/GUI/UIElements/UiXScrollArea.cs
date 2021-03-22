using UnityEngine;
using UnityEngine.UI;
using NotImplementedException = System.NotImplementedException;

namespace Code.GUI.UIElements
{
    public class UiXScrollArea : UiScrollArea
    {
        private void Awake()
        {
            float spacing = content.GetComponent<HorizontalLayoutGroup>().spacing;
            float elementX = content.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
            elementValue = elementX + spacing;
            maxValue = content.transform.localPosition.x;
            minValue = maxValue * -1;
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
            Transform contentTransform = content.transform;
            Vector3 contentPos = contentTransform.localPosition;

            float newX = contentPos.x + value * elementValue;
            newX = Mathf.Clamp(newX, minValue, maxValue);
            contentTransform.localPosition = new Vector3(newX, contentPos.y, contentPos.z);
        }

        public override void ResetArea()
        {
            Transform contentTransform = content.transform;
            Vector3 contentPos = contentTransform.localPosition;
            contentTransform.localPosition = new Vector3(minValue, contentPos.y, contentPos.z);
        }
    }
}
