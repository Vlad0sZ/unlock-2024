using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverAudioClip;
    [SerializeField] private AudioClip clickAudioClip;
    [SerializeField] private UnityEvent onClick;

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(hoverAudioClip);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(clickAudioClip);
        onClick?.Invoke();
    }
}
