using UnityEngine;

namespace Match3SampleView
{
    public interface IBoardItem
    {
        void SetPosition(int x, int y);
        Vector2Int GetPosition();
        bool IsSelected();
        void Select(bool value);
        void Highlight();
    }
}
