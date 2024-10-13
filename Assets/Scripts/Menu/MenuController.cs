using System;
using System.Collections;
using UnityEngine;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private ScreenController[] screens;

        public IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            for (var i = 1; i < screens.Length; i++)
            {
                var screen = screens[i];
                screen.gameObject.SetActive(false);
            }
        }
    }
}