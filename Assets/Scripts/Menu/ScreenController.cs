using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public abstract class ScreenController : MonoBehaviour
    {
        [System.Serializable]
        public class ButtonController
        {
            public Button button;
            public GameObject objectToOpen;
            public GameObject objectToClose;

            public void OnButtonClicked()
            {
                if(objectToClose)
                    objectToClose.SetActive(false);
                
                if (objectToOpen)
                    objectToOpen.SetActive(true);
            }
        }


        [SerializeField] private ButtonController[] screenButtons;

        protected virtual void Start()
        {
            foreach (var buttonController in screenButtons)
            {
                if (buttonController.button == null)
                    continue;

                buttonController.button.onClick.AddListener(buttonController.OnButtonClicked);
            }
        }


        protected virtual void OnDestroy()
        {
            foreach (var buttonController in screenButtons)
            {
                if (buttonController.button == null)
                    continue;

                buttonController.button.onClick.RemoveListener(buttonController.OnButtonClicked);
            }
        }
    }
}