using UnityEngine;
using UnityEditor;

namespace Match3SampleView
{
    [CustomEditor(typeof(DefaultGameSessionView))]
    public class DefaultGameSessionViewEditor : Editor
    {
        private DefaultGameSessionView instance;

        private void OnEnable()
        {
            instance = target as DefaultGameSessionView;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (instance.BoardSize_X == 0 || instance.BoardSize_Y == 0)
                return;


            GUILayout.Label("View: ");
            for (int j = instance.BoardSize_Y - 1; j >= 0; j--)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(j.ToString());
                for (int i = 0; i < instance.BoardSize_X; i++)
                {
                    var figureType = instance.FigureItems[i, j].FigureType.ToString();
                    var figureTypeShort = new string(new char[] { figureType[0], figureType[1], figureType[2] });
                    if (figureTypeShort == "emp")
                        figureTypeShort = "!!!";
                    GUILayout.Label(figureTypeShort);

                }
                GUILayout.EndHorizontal();
            }

            PrintHorizontalIndexes();

            GUILayout.Space(10);

            GUILayout.Label("Model: ");
            for (int j = instance.BoardSize_Y - 1; j >= 0; j--)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(j.ToString());
                for (int i = 0; i < instance.BoardSize_X; i++)
                {
                    var figureType = instance.gameSession.Board.FigureItemsTable[i, j].FigureType.ToString();
                    var figureTypeShort = new string(new char[] { figureType[0], figureType[1], figureType[2] });
                    if (figureTypeShort == "emp")
                        figureTypeShort = "!!!";
                    GUILayout.Label(figureTypeShort);

                }
                GUILayout.EndHorizontal();
            }

            PrintHorizontalIndexes();
            GUILayout.Label("Mismatches:");

            for (int j = instance.BoardSize_Y - 1; j >= 0; j--)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(j.ToString());
                for (int i = 0; i < instance.BoardSize_X; i++)
                {
                    var figureType_view = instance.FigureItems[i, j].FigureType;
                    var figureType_model = instance.gameSession.Board.FigureItemsTable[i, j].FigureType;
                    string match_str = "...";
                    if (figureType_view != figureType_model)
                        match_str = "!!!";


                    GUILayout.Label(match_str);

                }
                GUILayout.EndHorizontal();
            }
        }

        private void PrintHorizontalIndexes()
        {
            GUILayout.BeginHorizontal();
            for (int i = -1; i < instance.BoardSize_X; i++)
            {
                string index_str;

                if (i == -1)
                    index_str = "   ";
                else
                {
                    if (i < 10)
                        index_str = new string(new char[] { i.ToString()[0], ' ', ' ' });
                    else
                        index_str = new string(new char[] { i.ToString()[0], i.ToString()[1], ' ' });
                }


                GUILayout.Label(index_str);
            }

            GUILayout.EndHorizontal();
        }
    }
}
