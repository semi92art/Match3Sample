
using System.Collections.Generic;

namespace Match3SampleModel
{
    public interface IMatchesDestroyer
    {
        bool CheckForNewMatches(IFigureItem[,] figureItemsTable);
    }
}
