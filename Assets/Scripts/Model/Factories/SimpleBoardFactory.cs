using System.Collections.Generic;

namespace Match3SampleModel
{
    public class SimpleBoardFactory : IBoardFactory
    {
        public IBoard CreateBoard()
        {
            IFigureItemsRandomInstancer figureItemsRandomInstancer = new DefaultFigureItemsRandomInstancer();

            int size_x = 10;
            int size_y = 10;

            IFigureItem[,] figureItemsTable = new IFigureItem[size_x, size_y];

            for (int i = 0; i < size_x; i++)
            {
                for (int j = 0; j < size_y; j++)
                    figureItemsTable[i, j] = figureItemsRandomInstancer.InstantiateItem();
            }

            Queue<IFigureItem>[] figureItemBuffers = new Queue<IFigureItem>[size_x];
            for (int i = 0; i < size_x; i++)
            {
                figureItemBuffers[i] = new Queue<IFigureItem>();
                for (int j = 0; j < size_y; j++)
                    figureItemBuffers[i].Enqueue(figureItemsRandomInstancer.InstantiateItem());
                
            }

            return new DefaultBoard(figureItemsTable, figureItemBuffers);
        }
    }
}
