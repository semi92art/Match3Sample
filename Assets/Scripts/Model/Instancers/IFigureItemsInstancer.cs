using System.Collections.Generic;

namespace Match3SampleModel
{
    public interface IFigureItemsInstancer
    {
        Queue<string> Moves { get; set; }
        IFigureItem InstantiateItem();
        bool CheckForNewMatches(IFigureItem[,] figureItemsTable, FigureAction figureAction);
        void FillEmptyItemsFromQueues(IBoard board);
    }
}
