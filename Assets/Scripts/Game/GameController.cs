using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Game;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private Timer timer;
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
    
    private MeshGenerator _meshGenerator;
    private bool _wallLogicFail;
    private bool _isGameFinished;
    private GameObject _currentWall;
    
    private void Awake()
    {
        _meshGenerator = new MeshGenerator(2, 2, 0.04f);
        human.Trigger += HumanOnTrigger;
        myWebReader.DataTrigger += DataTrigger;
        timer.OnStop += TimerOnOnStop;
    }
    
    private void TimerOnOnStop()
    {
        FinishGame();
    }
    
    private void FinishGame()
    {
        _isGameFinished = true;
        myWebReader.GameEnd();
    }

    private void Start()
    {
        audioSource.clip = menuAudioClip;
        audioSource.Play();
    }
    
    private void Update()
    {
        if (_currentWall != null)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Destroy(_currentWall);
                _currentWall = null;
                myWebReader.NeedData();
            }
        }
    }
    
    public void StartGame()
    {
        StartCoroutine(StartGameStart());
    }
    
    private IEnumerator StartGameStart()
    {
        audioSource.clip = mainAudioClip;
        audioSource.Play();
        myWebReader.GameStart();
        yield return new WaitForSeconds(5f);
        timer.StartTimer();
        myWebReader.NeedData();
        //Generate();
    }
    
    private void DataTrigger(WallDataModel data)
    {
        // TODO 
        // Spawn(data.Data.ToArray(), 128, 128);
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
        if(!_isGameFinished)
        {
            myWebReader.NeedData();
        }
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
        _currentWall = obj;
        obj.transform.DOMove(endPointWall.position, timeWall).SetEase(Ease.InSine).OnComplete(() =>
        {
            EndWall();
            Destroy(obj);
        });
    }
}
