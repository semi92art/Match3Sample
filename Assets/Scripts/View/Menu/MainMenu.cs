using System.Text;
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
        public Animator logAnimator;
        private Text logText;

        private bool isShowingLog = false;

        private void Awake()
        {
            if (instance)
                DestroyImmediate(gameObject);
            instance = this;
        }

        private void Start()
        {
            logText = logAnimator.GetComponent<Text>();
            logText.text = string.Empty;
            logText.fontSize = 8;
            Application.logMessageReceived += Application_logMessageReceived;
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            var sb = new StringBuilder();
            sb.Append(condition);
            sb.Append("\n");
            sb.Append(stackTrace);
            sb.Append("\n");
            logText.text = sb.ToString();
        }

        public void CreateBoard()
        {
            int size_x = int.Parse(size.text);
            int size_y = int.Parse(size.text);

            IBoardFactory bf = new DefaultBoardFactory();
            GameStatics.Instance.InitBoard(size_x, size_y);
        }

        public void ShowLog()
        {
            if (!isShowingLog)
                logAnimator.SetTrigger("call");
            else
                logAnimator.SetTrigger("back");
        }
    }
}
