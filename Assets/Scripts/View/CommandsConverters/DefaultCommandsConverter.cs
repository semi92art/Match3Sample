using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Match3SampleView
{
    public class MoveSet
    {
        public Queue<MoveCommand> fromBoardToCemeteryMoves;
        public MoveCommand fromBoardToBoardMoveHandle;
        public Queue<MoveCommand> fromBoardToBoardMoves;
        public Queue<MoveCommand> fromQueueToBoardMoves;
        public Queue<MoveCommand> fromInstancerToQueueMoves;

        public MoveSet()
        {
            fromBoardToBoardMoveHandle = new MoveCommand();
            fromBoardToBoardMoveHandle.fromPosition = new Vector2Int(-1, 0);
            fromBoardToCemeteryMoves = new Queue<MoveCommand>();
            fromBoardToBoardMoves = new Queue<MoveCommand>();
            fromQueueToBoardMoves = new Queue<MoveCommand>();
            fromInstancerToQueueMoves = new Queue<MoveCommand>();
        }
    }

    public class DefaultCommandsConverter : ICommandsConverter
    {
        public List<MoveSet> MoveSets { get; private set; }

        public DefaultCommandsConverter()
        {
            MoveSets = new List<MoveSet>();
        }

        public MoveCommand ConvertMoveCommand(string cmd) //examples: q1,5_b2,6 b0,0_b0,1
        {
            FigureLocation fromLocation, toLocation;
            Vector2Int fromPosition, toPosition;
            bool handle = false;

            var split = cmd.Split('_');
            var from_str = split[0];
            var to_str = split[1];
            if (from_str[0] == 'q')
                fromLocation = FigureLocation.queue;
            else if (from_str[0] == 'b' || from_str[0] == 'h')
                fromLocation = FigureLocation.board;
            else if (from_str[0] == 'c')
                fromLocation = FigureLocation.cemetery;
            else if (from_str[0] == 'z')
                fromLocation = FigureLocation.instancer;
            else
                throw new System.NotImplementedException("ConvertMoveCommand function not implemented completely! (fromLocation)");

            if (from_str[0] == 'h')
                handle = true;

            if (to_str[0] == 'q')
                toLocation = FigureLocation.queue;
            else if (to_str[0] == 'b')
                toLocation = FigureLocation.board;
            else if (to_str[0] == 'c')
                toLocation = FigureLocation.cemetery;
            else if (to_str[0] == 'z')
                toLocation = FigureLocation.instancer;
            else
                throw new System.NotImplementedException("ConvertMoveCommand function not implemented completely! (toLocation)");

            from_str = from_str.Replace(from_str[0].ToString(), string.Empty);
            to_str = to_str.Replace(to_str[0].ToString(), string.Empty);

            var from_pos_split = from_str.Split(',');
            var to_pos_split = to_str.Split(',');
            fromPosition = new Vector2Int(int.Parse(from_pos_split[0]), int.Parse(from_pos_split[1]));
            toPosition = new Vector2Int(int.Parse(to_pos_split[0]), int.Parse(to_pos_split[1]));

            return new MoveCommand(fromLocation, fromPosition, toLocation, toPosition, handle);
        }

        public void ConvertMoveCommans(Queue<string> commands)
        {
            MoveSets.Clear();
            int k = -1;
            while (commands.Count > 0)
            {
                k++;
                var cmd_str = commands.Dequeue();
                MoveCommand cmd = ConvertMoveCommand(cmd_str);

                switch (cmd.fromLocation)
                {
                    case FigureLocation.instancer:
                        if (cmd.toLocation == FigureLocation.queue)
                            MoveSets.Last().fromInstancerToQueueMoves.Enqueue(cmd);
                        break;
                    case FigureLocation.queue:
                        if (cmd.toLocation == FigureLocation.board)
                        {
                            if (MoveSets.Last().fromInstancerToQueueMoves.Count > 0)
                                MoveSets.Add(new MoveSet());

                            MoveSets.Last().fromQueueToBoardMoves.Enqueue(cmd);
                        }
                        break;
                    case FigureLocation.board:
                        if (cmd.toLocation == FigureLocation.board)
                        {
                            if (cmd.handle)
                                MoveSets.Add(new MoveSet());

                            if (k > 0 && MoveSets.Last().fromQueueToBoardMoves.Count > 0)
                                MoveSets.Add(new MoveSet());

                            if (cmd.handle)
                                MoveSets.Last().fromBoardToBoardMoveHandle = cmd;
                            else if (k > 0)
                                MoveSets.Last().fromBoardToBoardMoves.Enqueue(cmd);

                        }
                        else if (cmd.toLocation == FigureLocation.cemetery)
                        {
                            if (MoveSets.Last().fromBoardToBoardMoves.Count > 0 || MoveSets.Last().fromQueueToBoardMoves.Count > 0)
                                MoveSets.Add(new MoveSet());
                            MoveSets.Last().fromBoardToCemeteryMoves.Enqueue(cmd);
                        }
                        break;
                    default:
                        throw new System.NotImplementedException("ConvertMoveCommans function not implemented completely! (fromLocation)");
                }
            }

        }

    }
}
