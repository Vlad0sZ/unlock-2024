using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Menu;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private MenuController finishGameObject;
        [SerializeField] private Human human;
        [SerializeField] private MyWebReader myWebReader;
        [SerializeField] private SunMover sunMover;
        [SerializeField] private CloudController cloud;
        [SerializeField] private float timeWall;
        [SerializeField] private Timer timer;
        [SerializeField] private GameObject prefabWall;
        [SerializeField] private Transform spawnPointWall;
        [SerializeField] private Transform endPointWall;
        [SerializeField] private AudioController audioController;
        [SerializeField] private UITextUserData userText;
        [SerializeField] private List<Texture2D> textures;

        private MeshGenerator _meshGenerator;
        private bool _wallLogicFail;
        private bool _isGameFinished;
        private GameObject _currentWall;
        private WallDataModel _currentWallModel;

        private PlayerScore _playerScore;

        private void Awake()
        {
            _meshGenerator = new MeshGenerator(2, 2, 0.04f);
            _playerScore = new PlayerScore();

            myWebReader.DataTrigger += DataTrigger;
            human.Trigger += HumanOnTrigger;
            timer.OnStop += TimerOnOnStop;
        }

        private void TimerOnOnStop() =>
            FinishGame();

        private void FinishGame()
        {

            _isGameFinished = true;
            DestroyWall();
        
            audioController.PlayFinished();
            finishGameObject.ShowFinishGame(_playerScore.GetScores());
            GameStateController.CurrentState = GameState.FinalScreen;
        }

        private void Update()
        {
            if (_currentWall == null)
                return;
            
            if (!Input.GetKeyUp(KeyCode.Space)) 
                return;
            
            DestroyWall();
            myWebReader.NeedData();
        }

        public void StartGame()
        {
            _isGameFinished = false;
            _wallLogicFail = false;
            userText.ClearText();
            
            StartCoroutine(StartGameStart());
        }

        private IEnumerator StartGameStart()
        {
            sunMover.StartMove();
            cloud.StartMove();
            GameStateController.CurrentState = GameState.Game;
        
            yield return new WaitForSeconds(1f);
            timer.StartTimer();
            myWebReader.NeedData();
        }

        private void DataTrigger(WallDataModel data)
        {
            _currentWallModel = data;
            Spawn(data.Data);
            userText.SetText(data.UserName, data.TaskText);
        }

        private void HumanOnTrigger(GameObject obj) =>
            Fail();

        private void Fail()
        {
            _wallLogicFail = true;
            _playerScore.AddScore(_currentWallModel, 0);
            audioController.PlayFail(4f);
        }

        private void Success()
        {
            _playerScore.AddScore(_currentWallModel, 1);
            audioController.PlaySuccess(4f);
        }
    

        private void EndWall()
        {
            if (!_wallLogicFail) 
                Success();

            _wallLogicFail = false;
        
            if (!_isGameFinished) 
                myWebReader.NeedData();
        }

        [Button]
        private void Generate()
        {
            var tex = textures[Random.Range(0, textures.Count)];
            Spawn(tex);
        }

        [Button]
        private void Spawn(Texture2D texture2D)
        {
            var obj = Instantiate(prefabWall);
            obj.transform.position = spawnPointWall.position;
            var meshFilter = obj.GetComponent<MeshFilter>();
            var meshCollider = obj.GetComponent<MeshCollider>();
            var mesh = _meshGenerator.GenerateCutoutMesh(texture2D);
            meshFilter.mesh = mesh;
            meshCollider.sharedMesh = mesh;
            _currentWall = obj;
            obj.transform.DOMove(endPointWall.position, timeWall).SetEase(Ease.InSine).OnComplete(() =>
            {
                EndWall();
                Destroy(obj);
            });
        }

        private void DestroyWall()
        {
            try
            {
                if (_currentWall != null)
                    Destroy(_currentWall);

                _currentWall = null;
                _currentWallModel = null;
            }
            catch
            {
                // ignored
            }
        }
    }
}