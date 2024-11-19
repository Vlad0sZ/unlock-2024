using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSettings
{
    public class SettingsBadge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject badgeObject;
        [SerializeField] private GameObject hoverHint;

        private void OnEnable()
        {
            ActivateBadge(false);
            SetHintActive(false);
        }


        public void OnPointerEnter(PointerEventData eventData) => SetHintActive(true);

        public void OnPointerExit(PointerEventData eventData) => SetHintActive(false);

        private void SetHintActive(bool isActive)
        {
            if (hoverHint)
                hoverHint.SetActive(isActive);
        }

        public void ActivateBadge(bool isActive)
        {
            if (badgeObject)
                badgeObject.SetActive(isActive);
        }
    }
}