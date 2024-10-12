using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [AddComponentMenu("UI/Round Button", 30)]
    public class RoundButton : Button
    {
        [SerializeField] private Image icon;
        [SerializeField] private ColorBlock iconColorBlock = ColorBlock.defaultColorBlock;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            base.DoStateTransition(state, instant);
            if (icon)
                ChangeIconColor(state, instant);
        }

        private void ChangeIconColor(SelectionState state, bool instant)
        {
            var colorState = GetColor(state);
            var duration = instant ? 0f : iconColorBlock.fadeDuration;
            icon.CrossFadeColor(colorState, duration, true, true);
        }

        private Color GetColor(SelectionState byState) =>
            byState switch
            {
                SelectionState.Normal => iconColorBlock.normalColor,
                SelectionState.Highlighted => iconColorBlock.highlightedColor,
                SelectionState.Pressed => iconColorBlock.pressedColor,
                SelectionState.Selected => iconColorBlock.selectedColor,
                SelectionState.Disabled => iconColorBlock.disabledColor,
                _ => throw new ArgumentOutOfRangeException(nameof(byState), byState, null)
            };
    }
}