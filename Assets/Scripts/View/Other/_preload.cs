using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Customs;

namespace Match3SampleView
{

    public class _preload : MonoBehaviour
    {
        private static _preload instance = null;
        public static _preload Instance { get { return instance; } }

        [Header("Figure Prefabs:")]
        public GameObject bananaFigurePrefab;
        public GameObject cakeFigurePrefab;
        public GameObject caramelFigurePrefab;
        public GameObject goldenStarFigurePrefab;
        public GameObject iceCreamFigurePrefab;
        public GameObject purpleCakeFigurePrefab;
        [Header("Board Prefabs:")]
        public GameObject boardItemPrefab;
        [Header("Board Relative Indents:")]
        public float delta_bottom_coeff = 0.1f;
        public float delta_top_coeff = 0.25f;

        public static int Call_hash;
        public static int Call2_hash;
        public static int Back_hash;
        public static int Kill_hash;




        private void Awake()
        {
            if (instance)
            {
                DestroyImmediate(gameObject);
                return;
            }
            instance = this;

            Call_hash = Animator.StringToHash("call");
            Call2_hash = Animator.StringToHash("call2");
            Back_hash = Animator.StringToHash("back");
            Kill_hash = Animator.StringToHash("kill");

            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += InitMenu_;
            SceneManager.LoadScene(1);
        }

        private void DestroyAllExceptThis()
        {
            foreach (var item in FindObjectsOfType<GameObject>())
            {
                if (item != gameObject)
                {
#if UNITY_EDITOR
                    DestroyImmediate(item);
#else
                    Destroy(item);
#endif
                }
            }
        }

        public void InitMenu_(Scene sc0, Scene sc1)
        {
            if (sc1.buildIndex != 1)
                return;

            InitStatics();
            //GameStatics.Instance.InitBoard();
        }

        public void InitStatics()
        {
            GameObject obj;

#if UNITY_EDITOR
            DestroyImmediate(GameObject.Find(ConstantNames.BoardItems));
            DestroyImmediate(GameObject.Find(ConstantNames.FigureItems));
            DestroyImmediate(GameObject.Find(ConstantNames.Board));
            DestroyImmediate(GameObject.Find(ConstantNames.StaticInstances));
#else
            Destroy(GameObject.Find(ConstantNames.BoardItems));
            Destroy(GameObject.Find(ConstantNames.FigureItems));
            Destroy(GameObject.Find(ConstantNames.Board));
            Destroy(GameObject.Find(ConstantNames.StaticInstances));
#endif

            new GameObject(ConstantNames.StaticInstances).AddComponent<GameStatics>();
        }

        
    }
}