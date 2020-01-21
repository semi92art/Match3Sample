using System.Collections.Generic;

namespace Match3SampleModel
{
    public class DefaultBoardFactory : IBoardFactory
    {
        private IFigureItemsInstancer figureItemsRandomInstancer;

        public IBoard Create()
        {
            return Create(10, 10);
        }

        public IBoard Create(int size_x, int size_y)
        {
            figureItemsRandomInstancer = new DefaultFigureItemsInstancer();

            IFigureItem[,] figureItemsTable = new IFigureItem[size_x, size_y];

            for (int i = 0; i < size_x; i++)
            {
                for (int j = 0; j < size_y; j++)
                    figureItemsTable[i, j] = figureItemsRandomInstancer.InstantiateItem();
            }

            CheckForMatches(figureItemsTable);

            Queue<IFigureItem>[] figureItemBuffers = new Queue<IFigureItem>[size_x];
            for (int i = 0; i < size_x; i++)
            {
                figureItemBuffers[i] = new Queue<IFigureItem>();
                for (int j = 0; j < size_y; j++)
                    figureItemBuffers[i].Enqueue(figureItemsRandomInstancer.InstantiateItem());

            }

            return new DefaultBoard(figureItemsTable, figureItemBuffers);
        }

        private void CheckForMatches(IFigureItem[,] figureItemsTable)
        {
            if (figureItemsRandomInstancer.CheckForNewMatches(figureItemsTable, FigureAction.ReplaceRandom))
                CheckForMatches(figureItemsTable);
        }
    }
}
