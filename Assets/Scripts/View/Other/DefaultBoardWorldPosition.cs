using UnityEngine;

namespace Match3SampleView
{
    public class DefaultBoardWorldPosition : IBoardWorldPosition
    {
        private Vector2Int boardSizeInItems;
        private float delta_bottom_coeff;
        private float delta_top_coeff;
        private Camera camera;

        private Vector2 boardSizeInWorldSpace;
        private Vector3 zero_point;
        private Vector2 boardSpriteSize;


        public DefaultBoardWorldPosition(int boardSize_X, int boardSize_Y,
            float delta_bottom_coeff, float delta_top_coeff, Camera camera)
        {
            boardSizeInItems = new Vector2Int(boardSize_X, boardSize_Y);
            this.delta_bottom_coeff = delta_bottom_coeff;
            this.delta_top_coeff = delta_top_coeff;
            this.camera = camera;

            boardSizeInWorldSpace = Vector2.zero;
            zero_point = Vector3.zero;
            boardSpriteSize = Vector2.zero;

            CalculateBoardSizeInWorldSpace();
        }

        public Vector3 GetBoardWorldPosition(int x, int y)
        {
            float delta_x = boardSpriteSize.x * (float)x / (float)boardSizeInItems.x;
            return new Vector3(
                zero_point.x + boardSpriteSize.x * (float)x + boardSpriteSize.x / 2,
                zero_point.y + boardSpriteSize.y * (float)y + boardSpriteSize.y / 2,
                0);
        }

        public void ScaleItemSprite(SpriteRenderer rend)
        {
            rend.transform.localScale = Vector3.one;
            rend.transform.localScale = new Vector3(
                boardSpriteSize.x / rend.bounds.size.x, boardSpriteSize.y / rend.bounds.size.y, 1);
        }

        private void CalculateBoardSizeInWorldSpace()
        {
            var size = Customs.UnityCustoms.GetGameWindowSize();
            float b = (1 - delta_bottom_coeff - delta_top_coeff) * size.y;
            float c = (float)boardSizeInItems.x / (float)boardSizeInItems.y;
            
            if (c > size.x / b)
            {
                zero_point = camera.ScreenToWorldPoint(new Vector3(0, delta_bottom_coeff * size.y + (b - size.x / c) / 2f, 2f));
                boardSizeInWorldSpace.x = camera.ScreenToWorldPoint(new Vector3(size.x, 0, 0)).x - zero_point.x;
                boardSizeInWorldSpace.y = boardSizeInWorldSpace.x / c;
            }
            else
            {
                zero_point = camera.ScreenToWorldPoint(new Vector3((size.x - c * b) / 2f, delta_bottom_coeff * size.y, 2f));
                boardSizeInWorldSpace.y = camera.ScreenToWorldPoint(new Vector3(0, b + delta_bottom_coeff * size.y, 0)).y - zero_point.y;
                boardSizeInWorldSpace.x = c * boardSizeInWorldSpace.y;
            }

            boardSpriteSize.x = boardSizeInWorldSpace.x / (float)boardSizeInItems.x;
            boardSpriteSize.y = boardSizeInWorldSpace.x / (float)boardSizeInItems.y;

           /* Debug.Log("zero_point: " + zero_point);
            Debug.Log("boardSizeInWorldSpace: " + boardSizeInWorldSpace);
            Debug.Log("boardSizeInItems: " + boardSizeInItems);
            Debug.Log("boardSpriteSize: " + boardSpriteSize);*/
        }

        
    }
}
