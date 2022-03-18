using System;
using UnityEngine;

namespace Map
{
    public class MapCellKeeper : MonoBehaviour
    {
        [SerializeField]
        private MapData _mapData;
        [SerializeField]
        private bool _showMap;
        [SerializeField]
        private bool _showCenters;
        private Vector2[,] _cellCenters;

        public MapData MapData => _mapData;
        public Vector2[,] CellCenters => _cellCenters;

        private void Awake()
        {
            if(_cellCenters == null) CalculateCellCenters();    
        }

        [ContextMenu ("Calculate cell centers")]
        private void CalculateCellCenters()
        {
            _cellCenters = new Vector2[_mapData.Columns, _mapData.Rows];

            float horizontalStep = (float)1 / _mapData.Columns;
            float verticalStep = (float)1 / _mapData.Rows;
            
            for(int i = 0; i < _mapData.Columns; i++)
            {
                Vector2 leftBottomColumn = Vector2.Lerp(_mapData.LeftBottomCorner, 
                    _mapData.RightBottomCorner, i * horizontalStep);
                Vector2 leftTopColumn = Vector2.Lerp(_mapData.LeftTopCorner, 
                    _mapData.RightTopCorner, i * horizontalStep);
                Vector2 rightBottomColumn = Vector2.Lerp(_mapData.LeftBottomCorner, 
                    _mapData.RightBottomCorner, (i + 1) * horizontalStep);
                Vector2 rightTopColumn = Vector2.Lerp(_mapData.LeftTopCorner, 
                    _mapData.RightTopCorner, (i + 1) * horizontalStep);
                for(int j = 0; j < _mapData.Rows; j++)
                {
                    Vector2 leftBottom = Vector2.Lerp(leftBottomColumn, 
                        leftTopColumn, j * verticalStep);
                    Vector2 leftTop= Vector2.Lerp(leftBottomColumn, 
                        leftTopColumn, (j + 1) * verticalStep);
                    Vector2 rightBottom= Vector2.Lerp(rightBottomColumn, 
                        rightTopColumn, j * verticalStep);
                    Vector2 rightTop= Vector2.Lerp(rightBottomColumn, 
                        rightTopColumn, (j + 1) * verticalStep);

                    _cellCenters[i, j] = (leftBottom + leftTop + rightBottom + rightTop) / 4;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if(_showMap == false) return;
            float horizontalStep = (float)1 / _mapData.Columns;
            float verticalStep = (float)1 / _mapData.Rows;
            for(float i = 0; i <= 1; i += horizontalStep)
            {
                Vector2 topPoint = Vector2.Lerp(_mapData.LeftTopCorner, _mapData.RightTopCorner, i);
                Vector2 bottomPoint = Vector2.Lerp(_mapData.LeftBottomCorner, _mapData.RightBottomCorner, i);
                Gizmos.DrawLine(topPoint, bottomPoint);
            }
            for(float i = 0; i <= 1; i += verticalStep)
            {
                Vector2 leftPoint = Vector2.Lerp(_mapData.LeftBottomCorner, _mapData.LeftTopCorner, i);
                Vector2 rightPoint = Vector2.Lerp(_mapData.RightBottomCorner, _mapData.RightTopCorner, i);
                Gizmos.DrawLine(leftPoint, rightPoint);
            }

            if(_cellCenters == null || _showCenters == false) return;
            Gizmos.color = Color.white;
            for(int i = 0; i < _mapData.Columns; i++)
                for(int j = 0; j < _mapData.Rows; j++)
                    Gizmos.DrawSphere(_cellCenters[i, j], 0.1f);
        }
    }

    [Serializable]
    public struct MapData
    {
        [Header ("Map Corners")]
        [SerializeField] 
        private Transform _leftBottomCorner;
        [SerializeField] 
        private Transform _leftTopCorner;
        [SerializeField] 
        private Transform _rightBottomCorner;
        [SerializeField] 
        private Transform _rightTopCorner;
        [Header ("Columns and rows")]
        [SerializeField] [Range (2, 100)]
        private int _columns;
        [SerializeField] [Range (2, 100)]
        private int _rows;

        public Vector2 LeftBottomCorner => _leftBottomCorner.position;
        public Vector2 LeftTopCorner => _leftTopCorner.position;
        public Vector2 RightBottomCorner => _rightBottomCorner.position;
        public Vector2 RightTopCorner => _rightTopCorner.position;
        public int Columns => _columns;
        public int Rows => _rows;
    }
}