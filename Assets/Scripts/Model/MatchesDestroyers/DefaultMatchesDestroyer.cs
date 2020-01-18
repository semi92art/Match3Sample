using System.Collections.Generic;

namespace Match3SampleModel
{
    public class DefaultMatchesDestroyer : IMatchesDestroyer
    {

        public bool CheckForNewMatches(IFigureItem[,] figureItemsTable)
        {
            bool result = false;
            int matchesCount;

            for (int i = 0; i < figureItemsTable.GetLength(0); i++)
            {
                matchesCount = 0;
                for (int j = 1; j < figureItemsTable.GetLength(1); j++)
                {
                    if (figureItemsTable[i, j - 1].FigureType == figureItemsTable[i, j].FigureType)
                    {
                        matchesCount++;
                        if (matchesCount == 3)
                        {
                            figureItemsTable[i, j - 2].SetEmpty();
                            figureItemsTable[i, j - 1].SetEmpty();
                            figureItemsTable[i, j].SetEmpty();
                        }
                        else if (matchesCount > 3)
                            figureItemsTable[i, j].SetEmpty();

                    }
                    else
                        matchesCount = 0;
                }
            }

            for (int j = 0; j < figureItemsTable.GetLength(1); j++)
            {
                matchesCount = 0;
                for (int i = 1; i < figureItemsTable.GetLength(0); i++)
                {
                    if (figureItemsTable[i - 1, j].FigureType == figureItemsTable[i, j].FigureType)
                    {
                        matchesCount++;
                        if (matchesCount == 3)
                        {
                            figureItemsTable[i - 2, j].SetEmpty();
                            figureItemsTable[i - 1, j].SetEmpty();
                            figureItemsTable[i, j].SetEmpty();
                        }
                        else if (matchesCount > 3)
                            figureItemsTable[i, j].SetEmpty();
                    }
                    else
                        matchesCount = 0;
                }
            }

            return result;
        }
    }
}
