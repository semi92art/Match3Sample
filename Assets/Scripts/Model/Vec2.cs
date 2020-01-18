using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3SampleModel
{
    public struct Vec2
    {
        public int x;
        public int y;

        public Vec2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Vec2 v2_1, Vec2 v2_2)
        {
            return v2_1.x == v2_2.x && v2_1.y == v2_2.y;
        }

        public static bool operator !=(Vec2 v2_1, Vec2 v2_2)
        {
            return v2_1.x != v2_2.x || v2_1.y != v2_2.y;
        }
    }
}
