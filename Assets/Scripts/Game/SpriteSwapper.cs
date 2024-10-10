using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwapper : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private Image image;
    [SerializeField] private List<Sprite> sprites;
    
    private int _index;

    private void Start()
    {
        StartCoroutine(DoSwapping());
    }

    private IEnumerator DoSwapping()
    {
        var time = new WaitForSeconds(delay);
        while (true)
        {
            _index = (_index + 1) % sprites.Count;
            image.sprite = sprites[_index];
            yield return time;
        }
        // ReSharper disable once IteratorNeverReturns
    }
}
