using UnityEngine;

namespace Map
{
    public class MapPositionCalculator : MonoBehaviour
    {
        [SerializeField]
        private MapCellKeeper _mapCellKeeper;

        public MapPosition CalculatePosition(Vector2 position)
        {
            int x = -1; int y = -1;
            for(int i = 1; i < _mapCellKeeper.MapData.Columns - 1; i++)
            {
                if(_mapCellKeeper.CellCenters[i, 0].x > position.x)
                {
                    x = i;
                    break;
                }
            }
            if(x == -1) x = _mapCellKeeper.MapData.Columns - 1;

            for(int i = 1; i < _mapCellKeeper.MapData.Rows; i++)
            {
                if(_mapCellKeeper.CellCenters[x, i].y > position.y)
                {
                    y = i;
                    break;
                }
            }
            if(y == -1) y = _mapCellKeeper.MapData.Rows - 1;
            
            x = -1;
            for(int i = 1; i < _mapCellKeeper.MapData.Columns; i++)
            {
                if(_mapCellKeeper.CellCenters[i, y].x > position.x)
                {
                    if(Vector2.Distance(_mapCellKeeper.CellCenters[i - 1, y], position)
                        > Vector2.Distance(_mapCellKeeper.CellCenters[i, y], position))
                    {
                        x = i;
                    }
                    else
                    {
                        x = i - 1;
                    }
                    if(i < _mapCellKeeper.MapData.Columns - 1 && Vector2.Distance(_mapCellKeeper.CellCenters[i + 1, y], position)
                        < Vector2.Distance(_mapCellKeeper.CellCenters[i, y], position))
                    {
                        x = i + 1;
                    }
                    break;
                }
            }
            if(x == -1) x = _mapCellKeeper.MapData.Columns - 1;

            y = -1;
            for(int i = 1; i < _mapCellKeeper.MapData.Rows; i++)
            {
                if(_mapCellKeeper.CellCenters[x, i].y > position.y)
                {
                    if(Vector2.Distance(_mapCellKeeper.CellCenters[x, i - 1], position)
                        > Vector2.Distance(_mapCellKeeper.CellCenters[x, i], position))
                    {
                        y = i;
                    }
                    else
                    {
                        y = i - 1;
                    }
                    if(i < _mapCellKeeper.MapData.Rows - 1 && Vector2.Distance(_mapCellKeeper.CellCenters[x, i + 1], position)
                        < Vector2.Distance(_mapCellKeeper.CellCenters[x, i], position))
                    {
                        y = i + 1;
                    }
                    break;
                }
            }
            if(y == -1) y = _mapCellKeeper.MapData.Rows - 1;
            MapPosition mapPosition = new MapPosition(x, y);
            return mapPosition;
        }
    }

    public struct MapPosition
    {
        public int X {get; private set;}
        public int Y {get; private set;}

        public MapPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}