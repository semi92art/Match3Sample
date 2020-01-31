
using System.Collections.Generic;

namespace Match3SampleView
{
    public interface ICommandsConverter
    {
        List<MoveSet> MoveSets { get; }
        MoveCommand ConvertMoveCommand(string cmd);
        void ConvertMoveCommans(Queue<string> commands);
    }
}
