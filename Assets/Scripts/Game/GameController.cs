using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private Human human;
    [SerializeField] private MyWebReader myWebReader;
    [SerializeField] private List<Texture2D> textures;
    [SerializeField] private GameObject prefabWall;
    [SerializeField] private Transform spawnPointWall;
    [SerializeField] private Transform endPointWall;
    [SerializeField] private float timeWall;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip menuAudioClip;
    [SerializeField] private AudioClip mainAudioClip;
    [SerializeField] private AudioClip failAudioClip;
    [SerializeField] private AudioClip successAudioClip;
    [SerializeField] private AudioClip hoverAudioClip;
    [SerializeField] private AudioClip clickAudioClip;
    
    private MeshGenerator _meshGenerator;
    private bool _wallLogicFail;
    
    private void Awake()
    {
        _meshGenerator = new MeshGenerator(2, 2, 0.2f);
        human.Trigger += HumanOnTrigger;
        myWebReader.DataTrigger += DataTrigger;
    }

    private void Start()
    {
        audioSource.clip = menuAudioClip;
        audioSource.Play();
    }

    public void StartGame()
    {
        StartCoroutine(StartGameStart());
    }
    
    private IEnumerator StartGameStart()
    {
        audioSource.clip = mainAudioClip;
        audioSource.Play();
        yield return new WaitForSeconds(5f);
        myWebReader.NeedData();
        Generate();
    }
    
    private void DataTrigger(WallDataModel data)
    {
        Spawn(data.Data.ToArray(), 128, 128);
    }
    
    private void HumanOnTrigger(GameObject obj)
    {
        Fail();
    }
    
    private void Fail()
    {
        _wallLogicFail = true;
        audioSource.PlayOneShot(failAudioClip, 1f);
    }
    
    private void Success()
    {
        audioSource.PlayOneShot(successAudioClip, 1f);
    }
    
    private void EndWall()
    {
        if (!_wallLogicFail)
        {
            Success();
        }

        _wallLogicFail = false;
        myWebReader.NeedData();
        Generate();
    }
    
    [Button]
    private void Generate()
    {
        var tex = textures[Random.Range(0, textures.Count)];
        var pixels = tex.GetPixels();
        var data = pixels.Select(x => x.a < 0.5 ? 1 : 0).ToArray();
        Spawn(data, tex.width, tex.height);
    }
    
    [Button]
    private void Spawn(int[] data, int width, int height)
    {
        var obj = Instantiate(prefabWall);
        obj.transform.position = spawnPointWall.position;
        var meshFilter = obj.GetComponent<MeshFilter>();
        var meshCollider = obj.GetComponent<MeshCollider>();
        var mesh = _meshGenerator.GenerateCutoutMesh(data, width, height);
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        obj.transform.DOMove(endPointWall.position, timeWall).SetEase(Ease.InSine).OnComplete(() =>
        {
            EndWall();
            Destroy(obj);
        });
    }
}
