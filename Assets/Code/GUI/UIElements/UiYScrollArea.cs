using UnityEngine;
using UnityEngine.UI;

namespace Code.GUI.UIElements
{
    public class UiYScrollArea : UiScrollArea
    {
        private void Awake()
        {
            float spacing = content.GetComponent<VerticalLayoutGroup>().spacing;
            float elementY = content.GetChild(0).GetComponent<RectTransform>().sizeDelta.y;
            elementValue = elementY + spacing;
            minValue = content.transform.localPosition.y;
            maxValue = minValue * -1;
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) 
                ChangeValue(-1);
            
            if (Input.GetKeyDown(KeyCode.DownArrow)) 
                ChangeValue(1);
        }

        public override void ChangeValue(int value)
        {
            Transform contentTransform = content.transform;
            Vector3 contentPos = contentTransform.localPosition;

            float newY = contentPos.y + value * elementValue;
            newY = Mathf.Clamp(newY, minValue, maxValue);
            contentTransform.localPosition = new Vector3(contentPos.x, newY, contentPos.z);
        }

        public override void ResetArea()
        {
            Transform contentTransform = content.transform;
            Vector3 contentPos = contentTransform.localPosition;
            contentTransform.localPosition = new Vector3(contentPos.x, minValue, contentPos.z);
        }
    }
}
