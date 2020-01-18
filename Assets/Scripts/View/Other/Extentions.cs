using UnityEngine;
using Match3SampleModel;

namespace Match3SampleView
{
    public static class Extentions
    {
        public static Vec2 ToVec2(this Vector2Int v)
        {
            return new Vec2(v.x, v.y);
        }

        public static Vector2Int ToVector2Int(this Vec2 v)
        {
            return new Vector2Int(v.x, v.y);
        }
    }
}
