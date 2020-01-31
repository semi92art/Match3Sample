using UnityEngine;

namespace Match3SampleView
{
    public struct MoveCommand
    {
        public FigureLocation fromLocation;
        public Vector2Int fromPosition;
        public FigureLocation toLocation;
        public Vector2Int toPosition;
        public bool handle;

        public MoveCommand(FigureLocation fromLocation, Vector2Int fromPosition, 
            FigureLocation toLocation, Vector2Int toPosition, bool handle)
        {
            this.fromLocation = fromLocation;
            this.fromPosition = fromPosition;
            this.toLocation = toLocation;
            this.toPosition = toPosition;
            this.handle = handle;
        }
    }
}