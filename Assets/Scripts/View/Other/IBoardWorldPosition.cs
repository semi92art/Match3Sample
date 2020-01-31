using UnityEngine;

namespace Match3SampleView
{
    public interface IBoardWorldPosition
    {
        Vector3 GetBoardWorldPosition(int x, int y);
        void ScaleItemSprite(SpriteRenderer rend);
    }
}
