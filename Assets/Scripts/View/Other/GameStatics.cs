using UnityEngine;
using Match3SampleModel;
using System.Collections;
using Customs;

namespace Match3SampleView
{
    public class GameStatics : MonoBehaviour
    {
        private static GameStatics instance = null;
        public static GameStatics Instance { get { return instance; } }

        public GameObject board;
        public GameObject boardItemsParent;
        public GameObject figureItemsParent;
        public GameObject trash;
        public Camera camera;

        public IGameSession gameSession;
        public IGameSessionView gameSessionView;
        public IGameSessionController gameSessionController;


        private void Awake()
        {
            if (instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;
        }

        public void InitBoard(int size_x, int size_y)
        {
#if UNITY_EDITOR
            DestroyImmediate(GameObject.Find(ConstantNames.BoardItems));
            DestroyImmediate(GameObject.Find(ConstantNames.FigureItems));
            DestroyImmediate(GameObject.Find(ConstantNames.Board));
#else
            Destroy(GameObject.Find(ConstantNames.BoardItems));
            Destroy(GameObject.Find(ConstantNames.FigureItems));
            Destroy(GameObject.Find(ConstantNames.Board));
#endif

            this.board = new GameObject(ConstantNames.Board);
            boardItemsParent = new GameObject(ConstantNames.BoardItems);
            figureItemsParent = new GameObject(ConstantNames.FigureItems);
            trash = new GameObject(ConstantNames.Trash);

            StartCoroutine(SetParents(
                boardItemsParent.transform, figureItemsParent.transform, this.board.transform));


#if UNITY_EDITOR
            DestroyImmediate(GameObject.Find(ConstantNames.GameSessionView));
            camera = FindObjectOfType<Camera>();
#else
            Destroy(GameObject.Find("GameSessionView"));
            camera = Camera.main;
#endif
            var preload = FindObjectOfType<_preload>();


            IBoardFactory boardFactory = new DefaultBoardFactory();
            var board = boardFactory.Create(size_x, size_y);
            IFigureItemsInstancerFactory figureItemsInstancerFactory = new DefaultFigureItemsInstancerFactory();
            var figureItemsInstancer = figureItemsInstancerFactory.Create();
            IGameSessionFactory gameSessionFactory = new DefaultGameSessionFactory();
            gameSession = gameSessionFactory.Create(board, figureItemsInstancer);

            var gameSessionView_obj = new GameObject(ConstantNames.GameSessionView);
            gameSessionView = gameSessionView_obj.AddComponent<DefaultGameSessionView>();

            gameSessionController = new DefaultGameSessionController(gameSession, gameSessionView);
            IBoardWorldPosition boardWorldPosition = new DefaultBoardWorldPosition(
                gameSession.Board.BoardSize_X, gameSession.Board.BoardSize_Y, 
                preload.delta_bottom_coeff, preload.delta_top_coeff, camera);
            ICommandsConverter commandsConverter = new DefaultCommandsConverter();
            gameSessionView.InitView(gameSessionController, boardWorldPosition, commandsConverter);
            gameSessionView.InitBoard();


        }

        private IEnumerator SetParents(Transform tr1, Transform tr2, Transform parent_tr)
        {
            System.DateTime dt_start = System.DateTime.Now;
            while (dt_start.SecondsBetween(System.DateTime.Now) < 0.5f)
                yield return null;

            tr1.SetParent(parent_tr);
            tr2.SetParent(parent_tr);
            yield return null;
        }
    }
}
