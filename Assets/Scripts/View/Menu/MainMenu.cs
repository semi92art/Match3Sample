using UnityEngine;
using UnityEngine.UI;
using Match3SampleModel;

namespace Match3SampleView
{
    public class MainMenu : MonoBehaviour
    {
        private static MainMenu instance;
        public static MainMenu Instance { get { return instance; } }

        public bool LockControls { get; set; }
        public InputField size;

        private void Awake()
        {
            if (instance)
                DestroyImmediate(gameObject);
            instance = this;
        }

        public void CreateBoard()
        {
            int size_x = int.Parse(size.text);
            int size_y = int.Parse(size.text);

            IBoardFactory bf = new DefaultBoardFactory();
            GameStatics.Instance.InitBoard(size_x, size_y);
        }
    }
}
