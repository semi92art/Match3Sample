using UnityEngine;


namespace Match3SampleView
{
    [ExecuteInEditMode]
    public class BoardItem : MonoBehaviour, IBoardItem
    {
        private Animator animator;
        private Vector2Int position;
        private bool isSelected;

        private GameStatics gmst;

        private void Awake()
        {
            animator = GetComponent<Animator>();

#if UNITY_EDITOR
            gmst = FindObjectOfType<GameStatics>();
#else
            gmst = GameStatics.Instance;
#endif
        }


        public Vector2Int GetPosition()
        {
            return position;
        }

        public void SetPosition(int x, int y)
        {
            position = new Vector2Int(x, y);
        }

        public bool IsSelected()
        {
            return isSelected;
        }

        public void Select(bool value)
        {
            isSelected = value;
            if (!isSelected)
            {
                StopAnimation();
                return;
            }

            StartAnimation();
        }

        private void StartAnimation()
        {
            animator.SetTrigger(_preload.Call_hash);
        }

        private void StopAnimation()
        {
            animator.SetTrigger(_preload.Back_hash);
        }

        private void OnMouseOver()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))
                MakeTurn();
#elif UNITY_ANDROID || UNITY_IOS
            if (Input.GetTouch(0))
                MakeTurn();
#endif
        }

        public void MakeTurn()
        {
            Debug.Log("Make Turn!");
            if (isSelected)
                return;

            gmst.gameSessionView.MakeTurn(position);
        }
    }
}
