using UnityEngine;
using UnityEngine.UI;
using Match3SampleModel;

namespace Match3SampleView
{
    public class MainMenu : MonoBehaviour
    {
        private static MainMenu instance;
        public static MainMenu Instance { get { return instance; } }

        public InputField width;
        public InputField height;

        private void Awake()
        {
            if (instance)
                DestroyImmediate(gameObject);
            instance = this;
        }

        public void CreateBoard()
        {
            int size_x = int.Parse(width.text);
            int size_y = int.Parse(height.text);

            IBoardFactory bf = new DefaultBoardFactory();
            GameStatics.Instance.InitBoard(size_x, size_y);
        }
    }
}
