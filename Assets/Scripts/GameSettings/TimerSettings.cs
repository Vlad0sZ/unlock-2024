using Game;
using TMPro;
using UnityEngine;

public class TimerSettings : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private GameController gameController;
    
    private void Awake()
    {
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    
    private void OnDropdownValueChanged(int selectedIndex)
    {
        switch (selectedIndex)
        {
            case 1:
                gameController.SetTime(60*10);
                break;
            case 2:
                gameController.SetTime(60*5);
                break;
            default:
                gameController.SetTime(60*2);
                break;
        }
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
}
