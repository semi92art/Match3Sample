using UnityEngine;
using System.Collections.Generic;
using Model = Match3SampleModel;

namespace Match3SampleView
{
    public class DefaultGameSessionView : MonoBehaviour, IGameSessionView
    {

        public IBoardItem[,] BoardItems { get; private set; }

        public IFigureItem[,] FigureItems { get; private set; }
        public int BoardSize_X { get; private set; }
        public int BoardSize_Y { get; private set; }


        private Queue<IFigureItem>[] figureQueues;
        private int queue_max_length;

        private IGameSessionController gameSessionController;
        private IBoardWorldPosition boardWorldPosition;
        private ICommandsConverter commandsConverter;

        private _preload preload;
        private GameStatics gmst;
        public Model.IGameSession gameSession;


        private Vector2Int prev_selected, curr_selected;
        private PlayerAction playerAction;



        public void InitView(IGameSessionController gameSessionController, IBoardWorldPosition boardWorldPosition, ICommandsConverter commandsConverter)
        {
            this.gameSessionController = gameSessionController;
            this.gameSession = gameSessionController.GameSession;
            this.boardWorldPosition = boardWorldPosition;
            this.commandsConverter = commandsConverter;

            BoardSize_X = gameSession.Board.FigureItemsTable.GetLength(0);
            BoardSize_Y = gameSession.Board.FigureItemsTable.GetLength(1);
            prev_selected = new Vector2Int(-1, 0);
            curr_selected = new Vector2Int(-1, 0);
            playerAction = PlayerAction.Nothing;
            queue_max_length = 0;
        }

        public void MakeTurn(Vector2Int position)
        {
            prev_selected = curr_selected;
            curr_selected = position;

            switch (playerAction)
            {
                case PlayerAction.Nothing:
                    playerAction = PlayerAction.Select;
                    break;
                case PlayerAction.Select:
                    if (Mathf.Abs(curr_selected.x - prev_selected.x) == 1 && curr_selected.y == prev_selected.y)
                        playerAction = PlayerAction.Move;
                    else if (Mathf.Abs(curr_selected.y - prev_selected.y) == 1 && curr_selected.x == prev_selected.x)
                        playerAction = PlayerAction.Move;
                    break;
                default:
                    throw new System.NotImplementedException("MakeTurn function not implemented completely!");
            }

            if (playerAction == PlayerAction.Select)
            {
                if (prev_selected.x != -1)
                    BoardItems[prev_selected.x, prev_selected.y].Select(false);
                BoardItems[curr_selected.x, curr_selected.y].Select(true);
            }
            if (playerAction == PlayerAction.Move)
            {
                BoardItems[prev_selected.x, prev_selected.y].Select(false);
                BoardItems[curr_selected.x, curr_selected.y].Select(false);
                gameSession.TryMoveFigure(prev_selected.ToVec2(), curr_selected.ToVec2());
                playerAction = PlayerAction.Nothing;
            }
        }

        private BoardUpdateStage boardUpdateStage = BoardUpdateStage.AwaitingForUpdate;
        private int moveSetUpdateIndex = -1;
        private void Update()
        {
            UpdateBoardInGameLoop();
        }

        public void UpdateBoard()
        {
            commandsConverter.ConvertMoveCommans(gameSession.Moves);
            boardUpdateStage = BoardUpdateStage.HandleMove;
            moveSetUpdateIndex = 0;
        }

        private void UpdateBoardInGameLoop()
        {
            if (boardUpdateStage == BoardUpdateStage.AwaitingForUpdate)
                return;

            var moveSet = commandsConverter.MoveSets[moveSetUpdateIndex];

            switch (boardUpdateStage)
            {
                case BoardUpdateStage.HandleMove:
                    ExecuteHandleMove(moveSet);
                    break;
                case BoardUpdateStage.HandleMove_WaitForEnd:
                    ExecuteHandeMove_WaitForEnd();
                    break;
                case BoardUpdateStage.DestroyMatches:
                    ExecuteDestroyMatches(moveSet);
                    break;
                case BoardUpdateStage.DestroyMatches_WaitForEnd:
                    ExecuteDestroyMatches_WaitForEnd();
                    break;
                case BoardUpdateStage.MovesOnBoard:
                    ExecuteMovesOnBoard(moveSet);
                    break;
                case BoardUpdateStage.MovesOnBoard_WaitForEnd:
                    ExecuteMovesOnBoard_WaitForEnd();
                    break;
                case BoardUpdateStage.MovesFromQueuesToBoard:
                    ExecuteMovesFromQueuesToBoard(moveSet);
                    break;
                case BoardUpdateStage.MovesFromQueuesToBoard_WaitForEnd:
                    ExecuteMovesFromQueuesToBoard_WaitForEnd();
                    break;
                case BoardUpdateStage.MovesInQueues:
                    ExecuteMovesInQueues(moveSet);
                    break;
                case BoardUpdateStage.MovesInQueues_WaitForEnd:
                    ExecuteMovesInQueues_WaitForEnd();
                    break;
                case BoardUpdateStage.InstanceNewToQueues:
                    ExecuteInstanceNewToQueues(moveSet);
                    break;
                case BoardUpdateStage.InstanceNewToQueues_WaitForEnd:
                    ExecuteInstanceNewToQueues_WaitForEnd();
                    break;
                default:
                    throw new System.NotImplementedException("UpdateBoardInGameLoop function not implemented completely!");
            }
        }

        private void ExecuteHandleMove(MoveSet moveSet)
        {
            if (moveSet.fromBoardToBoardMoveHandle.fromPosition.x != -1)
            {
                var cmd = moveSet.fromBoardToBoardMoveHandle;
                FigureItems[cmd.fromPosition.x, cmd.fromPosition.y].MoveFigureToPosition(cmd.toPosition.x, cmd.toPosition.y);
                FigureItems[cmd.toPosition.x, cmd.toPosition.y].MoveFigureToPosition(cmd.fromPosition.x, cmd.fromPosition.y);

                IFigureItem temp = FigureItems[cmd.fromPosition.x, cmd.fromPosition.y];
                FigureItems[cmd.fromPosition.x, cmd.fromPosition.y] = FigureItems[cmd.toPosition.x, cmd.toPosition.y];
                FigureItems[cmd.toPosition.x, cmd.toPosition.y] = temp;
                boardUpdateStage = BoardUpdateStage.HandleMove_WaitForEnd;
            }
            else
                boardUpdateStage = BoardUpdateStage.DestroyMatches;
        }

        private void ExecuteHandeMove_WaitForEnd()
        {
            if (!ArePreviousMovesFinished())
                return;
            else
                boardUpdateStage = BoardUpdateStage.DestroyMatches;
        }

        private void ExecuteDestroyMatches(MoveSet moveSet)
        {
            while (moveSet.fromBoardToCemeteryMoves.Count > 0)
            {
                var cmd = moveSet.fromBoardToCemeteryMoves.Dequeue();
                FigureItems[cmd.fromPosition.x, cmd.fromPosition.y].KillIfOnBoard();
                BoardItems[cmd.fromPosition.x, cmd.fromPosition.y].Highlight();
            }

            boardUpdateStage = BoardUpdateStage.DestroyMatches_WaitForEnd;
        }

        private void ExecuteDestroyMatches_WaitForEnd()
        {
            if (!ArePreviousMovesFinished())
                return;
            else
                boardUpdateStage = BoardUpdateStage.MovesOnBoard;
        }

        private void ExecuteMovesOnBoard(MoveSet moveSet)
        {
            while (moveSet.fromBoardToBoardMoves.Count > 0)
            {
                var cmd = moveSet.fromBoardToBoardMoves.Dequeue();
                FigureItems[cmd.fromPosition.x, cmd.fromPosition.y].MoveFigureToPosition(cmd.toPosition.x, cmd.toPosition.y);
                FigureItems[cmd.toPosition.x, cmd.toPosition.y] = FigureItems[cmd.fromPosition.x, cmd.fromPosition.y];
            }

            boardUpdateStage = BoardUpdateStage.MovesOnBoard_WaitForEnd;
        }

        private void ExecuteMovesOnBoard_WaitForEnd()
        {
            if (!ArePreviousMovesFinished())
                return;
            else
                boardUpdateStage = BoardUpdateStage.MovesFromQueuesToBoard;
        }

        private void ExecuteMovesFromQueuesToBoard(MoveSet moveSet)
        {
            while (moveSet.fromQueueToBoardMoves.Count > 0)
            {
                var cmd = moveSet.fromQueueToBoardMoves.Dequeue();
                FigureItems[cmd.toPosition.x, cmd.toPosition.y] = figureQueues[cmd.fromPosition.x].Dequeue();
                FigureItems[cmd.toPosition.x, cmd.toPosition.y].MoveFromQueueToBoard();
                FigureItems[cmd.toPosition.x, cmd.toPosition.y].MoveFigureToPosition(cmd.toPosition.x, cmd.toPosition.y);

                for (int i = 0; i < figureQueues[cmd.fromPosition.x].Count; i++)
                {
                    var figureItem = figureQueues[cmd.fromPosition.x].Dequeue();
                    figureItem.MoveFigureToPosition(figureItem.Position.x, figureItem.Position.y - 1);
                    figureQueues[cmd.fromPosition.x].Enqueue(figureItem);
                }
            }

            boardUpdateStage = BoardUpdateStage.MovesFromQueuesToBoard_WaitForEnd ;
        }

        private void ExecuteMovesFromQueuesToBoard_WaitForEnd()
        {
            if (!ArePreviousMovesFinished())
                return;
            else
                boardUpdateStage = BoardUpdateStage.MovesInQueues;
        }

        private void ExecuteMovesInQueues(MoveSet moveSet)
        {
            boardUpdateStage = BoardUpdateStage.MovesInQueues_WaitForEnd;
        }

        private void ExecuteMovesInQueues_WaitForEnd()
        {
            boardUpdateStage = BoardUpdateStage.InstanceNewToQueues;
        }

        private void ExecuteInstanceNewToQueues(MoveSet moveSet)
        {
            while (moveSet.fromInstancerToQueueMoves.Count > 0)
            {
                var cmd = moveSet.fromInstancerToQueueMoves.Dequeue();
                var buffer_arr = gameSession.Board.FigureBuffers[cmd.toPosition.x].ToArray();
                int delta_size = buffer_arr.Length - figureQueues[cmd.toPosition.x].Count;
                for (int i = delta_size; i > 0; i--)
                {
                    var figure_type = buffer_arr[buffer_arr.Length - i].FigureType;
                    InstantiateQueueFigureItem(figure_type, cmd.toPosition.x, buffer_arr.Length - i);
                }
            }

            boardUpdateStage = BoardUpdateStage.InstanceNewToQueues_WaitForEnd;
        }

        private void ExecuteInstanceNewToQueues_WaitForEnd()
        {
            if (!ArePreviousMovesFinished())
                return;
            
            boardUpdateStage = BoardUpdateStage.MovesInQueues;

            if (moveSetUpdateIndex + 1 == commandsConverter.MoveSets.Count)
            {
                boardUpdateStage = BoardUpdateStage.AwaitingForUpdate;
                moveSetUpdateIndex = -1;
            }
            else
            {
                boardUpdateStage = BoardUpdateStage.HandleMove;
                moveSetUpdateIndex++;
            }
        }

        public bool ArePreviousMovesFinished()
        {
            foreach (var figureItem in FigureItems)
            {
                if (figureItem.IsInAction)
                    return false;
            }

            foreach (var figureQueue in figureQueues)
            {
                var arr = figureQueue.ToArray();
                foreach (var figureItem in arr)
                {
                    if (figureItem.IsInAction)
                        return false;
                }
            }
            return true;
        }




        public void InitBoard()
        {
            if (!GameObject.Find(ConstantNames.Board))
                new GameObject(ConstantNames.Board);

#if UNITY_EDITOR
            gmst = FindObjectOfType<GameStatics>();
            preload = FindObjectOfType<_preload>();
#else
            gmst = GameStatics.Instance;
            preload = _preload.Instance;
#endif
            InitBoardItems();
            InitFigureItems();
            InitQueueFigureItems();
        }

        public Vector3 GetBoardWorldPosition(int x, int y)
        {
            return boardWorldPosition.GetBoardWorldPosition(x, y);
        }

        private void InitBoardItems()
        {
            Customs.UnityCustoms.DestroyAllChilds(gmst.boardItemsParent);
            Customs.UnityCustoms.DestroyAllChilds(gmst.figureItemsParent);

            BoardItems = new IBoardItem[BoardSize_X, BoardSize_Y];
            for (int i = 0; i < BoardSize_X; i++)
            {
                for (int j = 0; j < BoardSize_Y; j++)
                {
                    var board_item_obj = GameObject.Instantiate(preload.boardItemPrefab) as GameObject;
                    board_item_obj.name = "board_item";
                    IBoardItem board_item = board_item_obj.GetComponent<IBoardItem>();
                    board_item.SetPosition(i, j);
                    BoardItems[i, j] = board_item;

                    board_item_obj.transform.SetParent(gmst.boardItemsParent.transform);
                    board_item_obj.transform.position = GetBoardWorldPosition(i, j);
                    boardWorldPosition.ScaleItemSprite(board_item_obj.GetComponent<SpriteRenderer>());
                }
            }
        }

        private void InitFigureItems()
        {
            FigureItems = new IFigureItem[BoardSize_X, BoardSize_Y];
            for (int i = 0; i < BoardSize_X; i++)
            {
                for (int j = 0; j < BoardSize_Y; j++)
                {
                    GameObject figure_item_obj;

                    switch (gameSession.Board.FigureItemsTable[i, j].FigureType)
                    {
                        case Model.FigureItemType.banana:
                            figure_item_obj = GameObject.Instantiate(preload.bananaFigurePrefab) as GameObject;
                            break;
                        case Model.FigureItemType.cake:
                            figure_item_obj = GameObject.Instantiate(preload.cakeFigurePrefab) as GameObject;
                            break;
                        case Model.FigureItemType.caramel:
                            figure_item_obj = GameObject.Instantiate(preload.caramelFigurePrefab) as GameObject;
                            break;
                        case Model.FigureItemType.golden_star:
                            figure_item_obj = GameObject.Instantiate(preload.goldenStarFigurePrefab) as GameObject;
                            break;
                        case Model.FigureItemType.icecream:
                            figure_item_obj = GameObject.Instantiate(preload.iceCreamFigurePrefab) as GameObject;
                            break;
                        case Model.FigureItemType.purple_cake:
                            figure_item_obj = GameObject.Instantiate(preload.purpleCakeFigurePrefab) as GameObject;
                            break;
                        default:
                            throw new System.NotImplementedException("InitFigureItems function not implemented completely!");
                    }

                    figure_item_obj.name = "figure_item";
                    IFigureItem figure_item = figure_item_obj.GetComponent<IFigureItem>();
                    FigureItems[i, j] = figure_item;

                    figure_item.InitItem(i, j, gameSession.Board.FigureItemsTable[i, j].FigureType, FigureLocation.board);
                    figure_item_obj.transform.SetParent(gmst.figureItemsParent.transform);
                    Vector3 pos = GetBoardWorldPosition(i, j);
                    figure_item_obj.transform.position = new Vector3(pos.x, pos.y, pos.z - 0.1f);
                    boardWorldPosition.ScaleItemSprite(figure_item_obj.GetComponent<SpriteRenderer>());
                }
            }
        }

        private void InitQueueFigureItems()
        {
            int length = gameSession.Board.FigureBuffers.Length;
            int height = gameSession.Board.FigureBuffers[0].Count;
            queue_max_length = height;
            figureQueues = new Queue<IFigureItem>[length];
            for (int i = 0; i < length; i++)
            {
                var buffer = gameSession.Board.FigureBuffers[i].ToArray();
                figureQueues[i] = new Queue<IFigureItem>(height);
                for (int j = 0; j < height; j++)
                {
                    InstantiateQueueFigureItem(buffer[j].FigureType, i, j);
                }
            }
        }

        private GameObject InstantiateQueueFigureItem(Model.FigureItemType figureItemType, int x_position, int y_position)
        {
            GameObject figure_item_obj;

            switch (figureItemType)
            {
                case Model.FigureItemType.banana:
                    figure_item_obj = GameObject.Instantiate(preload.bananaFigurePrefab) as GameObject;
                    break;
                case Model.FigureItemType.cake:
                    figure_item_obj = GameObject.Instantiate(preload.cakeFigurePrefab) as GameObject;
                    break;
                case Model.FigureItemType.caramel:
                    figure_item_obj = GameObject.Instantiate(preload.caramelFigurePrefab) as GameObject;
                    break;
                case Model.FigureItemType.golden_star:
                    figure_item_obj = GameObject.Instantiate(preload.goldenStarFigurePrefab) as GameObject;
                    break;
                case Model.FigureItemType.icecream:
                    figure_item_obj = GameObject.Instantiate(preload.iceCreamFigurePrefab) as GameObject;
                    break;
                case Model.FigureItemType.purple_cake:
                    figure_item_obj = GameObject.Instantiate(preload.purpleCakeFigurePrefab) as GameObject;
                    break;
                default:
                    throw new System.NotImplementedException("InitFigureItems function not implemented completely!");
            }

            figure_item_obj.name = "figure_item";

            IFigureItem figure_item = figure_item_obj.GetComponent<IFigureItem>();
            figureQueues[x_position].Enqueue(figure_item);
            figure_item.InitItem(x_position, y_position, figureItemType, FigureLocation.queue);
            figure_item_obj.transform.SetParent(gmst.figureItemsParent.transform);
            Vector3 pos = GetBoardWorldPosition(x_position, y_position + BoardSize_Y);
            figure_item_obj.transform.position = new Vector3(pos.x, pos.y, pos.z);
            boardWorldPosition.ScaleItemSprite(figure_item_obj.GetComponent<SpriteRenderer>());

            return figure_item_obj;
        }
    }
}
