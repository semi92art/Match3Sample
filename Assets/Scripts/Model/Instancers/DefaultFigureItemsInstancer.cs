using System.Collections.Generic;
using System.Linq;

namespace Match3SampleModel
{
    public class DefaultFigureItemsInstancer : IFigureItemsInstancer
    {
        public Queue<string> Moves { get; set; }

        private System.Random randomizer;

        public DefaultFigureItemsInstancer()
        {
            randomizer = new System.Random();
            Moves = new Queue<string>();
        }

        public IFigureItem InstantiateItem()
        {
            int possibleFigureTypesCount = System.Enum.GetNames(typeof(FigureItemType)).Length - 2; //minus empty and no_access
            FigureItemType figureItemType = (FigureItemType)randomizer.Next(0, possibleFigureTypesCount);

            IFigureItemsFactory figureItemsFactory = new FigureItemsFactory();
            return figureItemsFactory.CreateFigureItem(figureItemType);
        }

        public bool CheckForNewMatches(IFigureItem[,] figureItemsTable, FigureAction figureAction)
        {
            bool result = false;
            int matchesCount;
            int boardSize_X = figureItemsTable.GetLength(0);
            int boardSize_Y = figureItemsTable.GetLength(1);

            List<Vec2> itemsToSetEmpty = new List<Vec2>();

            for (int i = 0; i < boardSize_X; i++)
            {
                matchesCount = 0;
                for (int j = 1; j < boardSize_Y; j++)
                {
                    if (figureItemsTable[i, j - 1].FigureType == figureItemsTable[i, j].FigureType)
                    {
                        matchesCount++;
                        if (matchesCount >= 2)
                        {
                            switch (figureAction)
                            {
                                case FigureAction.ReplaceRandom:
                                    figureItemsTable[i, j] = InstantiateItem();
                                    break;
                                case FigureAction.SetEmpty:
                                    if (matchesCount == 2)
                                    {
                                        itemsToSetEmpty.Add(new Vec2(i, j - 2));
                                        itemsToSetEmpty.Add(new Vec2(i, j - 1));
                                    }
                                    itemsToSetEmpty.Add(new Vec2(i, j));
                                    break;
                                default:
                                    throw new System.NotImplementedException("CheckForNewMatches functions not implemented completely!");
                            }

                            result = true;
                        }
                    }
                    else
                        matchesCount = 0; 
                }
            }

            for (int j = 0; j < boardSize_Y; j++)
            {
                matchesCount = 0;
                for (int i = 1; i < boardSize_X; i++)
                {
                    if (figureItemsTable[i - 1, j].FigureType == figureItemsTable[i, j].FigureType)
                    {
                        matchesCount++;
                        if (matchesCount >= 2)
                        {
                            switch (figureAction)
                            {
                                case FigureAction.ReplaceRandom:
                                    figureItemsTable[i, j] = InstantiateItem();
                                    break;
                                case FigureAction.SetEmpty:
                                    if (matchesCount == 2)
                                    {
                                        itemsToSetEmpty.Add(new Vec2(i - 1, j));
                                        itemsToSetEmpty.Add(new Vec2(i - 2, j));
                                    }
                                    itemsToSetEmpty.Add(new Vec2(i, j));
                                    break;
                                default:
                                    throw new System.NotImplementedException("CheckForNewMatches functions not implemented completely!");
                            }

                            result = true;
                        }
                    }
                    else
                        matchesCount = 0;
                }
            }

            itemsToSetEmpty = itemsToSetEmpty.Distinct().ToList();

            foreach (var item in itemsToSetEmpty)
            {
                figureItemsTable[item.x, item.y].SetEmpty();
                Moves.Enqueue("b" + item.x + "," + item.y + "_c0,0");
            }

            return result;
        }

        public void FillEmptyItemsFromQueues(IBoard board)
        {
            int board_height = board.FigureItemsTable.GetLength(1);
            List<string> commands = new List<string>();

            for (int i = 0; i < board.BoardSize_X; i++)
            {
                for (int j = 0; j < board.BoardSize_Y; j++)
                {
                    if (board.FigureItemsTable[i, j].IsEmpty())
                    {
                        int k = 0;
                        while (k + j < board_height && board.FigureItemsTable[i, k + j].IsEmpty())
                            k++;


                        //Move figures down to empty spaces on board
                        for (int m = j; m < board_height - k; m++)
                        {
                            board.FigureItemsTable[i, m] = board.FigureItemsTable[i, m + k];
                            commands.Add("b" + i + "," + (m + k) + "_" + "b" + i + "," + m);
                        }

                        //Move figures from queue to board
                        for (int m = board_height - k; m < board_height; m++)
                        {
                            board.FigureItemsTable[i, m] = board.FigureBuffers[i].Dequeue();
                            commands.Add("q" + i + "," + "0" + "_" + "b" + i + "," + m);
                            board.FigureBuffers[i].Enqueue(InstantiateItem());
                            commands.Add("z" + i + "," + "0" + "_" + "q" + i + "," + "0");
                        }

                        break;
                    }
                }
            }

            var sorted_commands = from c in commands orderby c select c;

            foreach (var command in sorted_commands)
            {
                Moves.Enqueue(command);
            }
        }
    }
}
