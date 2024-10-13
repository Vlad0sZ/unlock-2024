using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private MeshRenderer meshRenderer;

    private void Awake() =>
        meshRenderer.material = new Material(meshRenderer.material);

    public void StartMove() =>
        StartCoroutine(Move());

    private IEnumerator Move()
    {
        while (true)
        {
            var material = meshRenderer.material;
            material.mainTextureOffset = new Vector2(material.mainTextureOffset.x + speed * Time.deltaTime,
                material.mainTextureOffset.y);
            yield return null;
        }
        // ReSharper disable once IteratorNeverReturns
    }
}