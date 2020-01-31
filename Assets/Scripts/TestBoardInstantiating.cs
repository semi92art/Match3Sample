using UnityEngine;

using Match3SampleView;

[ExecuteInEditMode]
public class TestBoardInstantiating : MonoBehaviour
{
    public int size_x = 10;
    public int size_y = 10;

    public void InstantiateDefaultBoard()
    {
        var preload = FindObjectOfType<_preload>();
        preload.InitStatics();
        var gmst = FindObjectOfType<GameStatics>();
        gmst.InitBoard(size_x, size_y);
    }
}


