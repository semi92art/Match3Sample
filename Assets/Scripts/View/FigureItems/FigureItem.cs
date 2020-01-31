using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Match3SampleModel;
using Customs;

namespace Match3SampleView
{
    public class FigureItem : MonoBehaviour, IFigureItem
    {
        private Vector2Int position;
        public Vector2Int Position
        {
            get { return position; }
            set { position = value; }
        }

        public FigureItemType figureType;
        public FigureItemType FigureType
        {
            get { return figureType; }
            private set { figureType = value; }
        }

        public FigureLocation Location { get; set; }

        public bool IsInAction { get; private set; }


        private GameStatics gmst;
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private bool wasInited = false;

        private void Start()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
#if UNITY_EDITOR
            gmst = FindObjectOfType<GameStatics>();
#else
            gmst = GameStatics.Instance;
#endif
        }

        private void Update()
        {
            if (doMove)
                MoveFigureToPositionInGameLoop();
        }

        public void InitItem(int x_position, int y_position, FigureItemType type, FigureLocation location)
        {
            if (wasInited)
                return;

            Position = new Vector2Int(x_position, y_position);
            FigureType = type;
            Location = location;

            wasInited = true;
        }

        
        public void MoveFigureToPosition(int x, int y)
        {
            Position = new Vector2Int(x, y);
            var y1 = Location == FigureLocation.queue ? y + gmst.gameSessionView.BoardSize_Y : y;
            positionMoveTo = gmst.gameSessionView.GetBoardWorldPosition(x, y1);
            dt_start = System.DateTime.Now;
            speed = 10f;
            spriteRenderer.sortingOrder++;
            IsInAction = true;
            MainMenu.Instance.LockControls = true;
            doMove = true;
        }

        public void MoveFromQueueToBoard()
        {
            if (Location == FigureLocation.queue)
                Location = FigureLocation.board;
        }

        public void KillIfOnBoard()
        {
            if (Location == FigureLocation.board)
            {
                IsInAction = true;
                Location = FigureLocation.cemetery;
                figureType = FigureItemType.empty;
                animator.SetTrigger(_preload.Kill_hash);
            }
        }

        private bool doMove = false;
        private Vector3 positionMoveTo;
        private System.DateTime dt_start;
        private float speed;
        private void MoveFigureToPositionInGameLoop()
        {
            if (IsInAction && dt_start.SecondsBetween(System.DateTime.Now) < 2f)
            {
                transform.position = Vector3.Lerp(transform.position, positionMoveTo, Time.deltaTime * speed);
                if (Vector3.Distance(positionMoveTo, transform.position) < 0.001f)
                {
                    IsInAction = false;
                    doMove = false;
                    spriteRenderer.sortingOrder--;
                }
            }
            else
            {
                if (IsInAction)
                {
                    spriteRenderer.sortingOrder--;
                    IsInAction = false;
                    doMove = false;
                }
            }
        }

        private void DestroyObject()
        {
            transform.SetParent(gmst.trash.transform);
            IsInAction = false;
        }

    } 
}
