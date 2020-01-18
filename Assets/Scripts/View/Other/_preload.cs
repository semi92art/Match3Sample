using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class _preload : MonoBehaviour
{
    [Header("Figure Prefabs:")]
    public GameObject bananaFigurePrefab;
    public GameObject cakeFigurePrefab;
    public GameObject caramelFigurePrefab;
    public GameObject goldenStarFigurePrefab;
    public GameObject iceCreamFigurePrefab;
    public GameObject purpleCakeFigurePrefab;
    [Header("Board Prefabs:")]
    public GameObject boardItemPrefab;

    private static _preload instance = null;
    public static _preload Instange { get { return instance; } }

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += InitMenu_;
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
    }

    public void InitStatics()
    {
        GameObject obj;
        //GameStatics gmst;

#if UNITY_EDITOR
        DestroyImmediate(GameObject.Find("BOARD ITEMS"));
        DestroyImmediate(GameObject.Find("FIGURE ITEMS"));
        DestroyImmediate(GameObject.Find("BOARD"));
        DestroyImmediate(GameObject.Find("STATICS"));
#else
        Destroy(GameObject.Find("BOARD ITEMS"));
        Destroy(GameObject.Find("FIGURE ITEMS"));
        Destroy(GameObject.Find("BOARD"));
        Destroy(GameObject.Find("STATICS"));
#endif

        obj = new GameObject("STATICS");
        //gmst = obj.AddComponent<GameStatics>();
        //gmst.board = new GameObject("BOARD");
        //gmst.boardItemsParent = new GameObject("BOARD ITEMS");
        //gmst.chessItemsParent = new GameObject("CHESS ITEMS");



        //StartCoroutine(SetParents(gmst.boardItemsParent.transform, gmst.chessItemsParent.transform, gmst.board.transform));
    }

    //Костыль, т.к. по напрямую установить родителей boardItemsParent и chessItemsParent не выходит
    private IEnumerator SetParents(Transform tr1, Transform tr2, Transform parent_tr)
    {
        /*System.DateTime dt_start = System.DateTime.Now;
        while (dt_start.SecondsBetween(System.DateTime.Now) < 0.5f)
            yield return null;

        tr1.SetParent(parent_tr);
        tr2.SetParent(parent_tr);*/
        yield return null;
    }
}
