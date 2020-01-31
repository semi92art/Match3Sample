using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match3SampleModel
{
    public class DefaultBoard : IBoard
    {
        public IFigureItem[,] FigureItemsTable { get; private set; }
        public Queue<IFigureItem>[] FigureBuffers { get; private set; }
        public int BoardSize_X { get { return FigureItemsTable.GetLength(0); } }
        public int BoardSize_Y { get { return FigureItemsTable.GetLength(1); } }

        public DefaultBoard(IFigureItem[,] FigureItemsTable, Queue<IFigureItem>[] FigureBuffers)
        {
            this.FigureItemsTable = FigureItemsTable;
            this.FigureBuffers = FigureBuffers;
        }

        public bool TryMoveFigure(Vec2 from, FigureMoveType figureMoveType)
        {
            Vec2 to = from;
            switch (figureMoveType)
            {
                case FigureMoveType.Left:
                    to.x--;
                    break;
                case FigureMoveType.Top:
                    to.y++;
                    break;
                case FigureMoveType.Right:
                    to.x++;
                    break;
                case FigureMoveType.Bottom:
                    to.y--;
                    break;
                default:
                    throw new System.NotImplementedException("TryMoveFigure function not implemented completely!");
            }

            if (FigureItemsTable[to.x, to.y].FigureType != FigureItemType.no_access)
            {
                IFigureItem temp = FigureItemsTable[from.x, from.y];
                FigureItemsTable[from.x, from.y] = FigureItemsTable[to.x, to.y];
                FigureItemsTable[to.x, to.y] = temp;
                return true;
            }
            else
                return false;


        }

        public void AddFiguresFromBuffers()
        {
            IFigureItemsInstancer figureItemsRandomInstancer = new DefaultFigureItemsInstancer();
            int x_length = FigureItemsTable.GetLength(0);
            int y_length = FigureItemsTable.GetLength(1);
            for (int i = 0; i < x_length; i++)
            {
                for (int j = 0; j < y_length - 1; j++)
                {
                    if (FigureItemsTable[i, j].IsEmpty())
                    {
                        for (int k = j; k < y_length - 1; k++)
                        {
                            FigureItemsTable[i, k] = FigureItemsTable[i, k + 1];
                            FigureItemsTable[i, k + i].SetEmpty();
                        }

                        FigureItemsTable[i, y_length - 1] = FigureBuffers[i].Dequeue();
                        FigureBuffers[i].Enqueue(figureItemsRandomInstancer.InstantiateItem());
                    }
                }
            }
        }
    }
}
