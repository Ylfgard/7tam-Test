using UnityEngine;

namespace Map
{
    public class MapPositionCalculator : MonoBehaviour
    {
        [SerializeField]
        private MapCellKeeper _cellKeeper;

        public MapPosition Position(int x, int y)
        {
            if(x < 0) x = 0;
            if(x >= _cellKeeper.MapData.Columns)x = _cellKeeper.MapData.Columns - 1;
            if(y < 0) y = 0;
            if(y >= _cellKeeper.MapData.Rows) y = _cellKeeper.MapData.Rows - 1;
            return new MapPosition(x, y);
        }

        public MapPosition[] CellsOnLine(MapPosition begin, MapPosition end)
        {
            int distance;
            bool XLine;
            if(begin.X == end.X)
            {
                distance = end.Y - begin.Y;
                XLine = false;  
            }
            else
            {
                distance = end.X - begin.X;
                XLine = true;
            }
            MapPosition[] positions = new MapPosition[Mathf.Abs(distance) + 1];

            int i = 0;
            positions[i] = begin;
            int step = (int)Mathf.Sign(distance);
            while(i != distance)
            {
                i += step;
                if(XLine) positions[Mathf.Abs(i)] = Position(begin.X + i, begin.Y);
                else positions[Mathf.Abs(i)] = Position(begin.X, begin.Y + i);
            }
            foreach(MapPosition pos in positions)
                Debug.Log(pos.X + " " + pos.Y);
            return positions;
        } 

        public MapPosition CalculatePosition(Vector2 position)
        {
            int x = -1; int y = -1;
            for(int i = 0; i < _cellKeeper.MapData.Columns; i++)
            {
                if(_cellKeeper.Cells[i, 0].Center.x >= position.x)
                {
                    x = i;
                    break;
                }
            }
            if(x == -1) x = _cellKeeper.MapData.Columns - 1;

            for(int i = 0; i < _cellKeeper.MapData.Rows; i++)
            {
                if(_cellKeeper.Cells[x, i].Center.y > position.y)
                {
                    y = i;
                    break;
                }
            }
            if(y == -1) y = _cellKeeper.MapData.Rows - 1;
            
            x = -1;
            for(int i = 1; i < _cellKeeper.MapData.Columns; i++)
            {
                if(_cellKeeper.Cells[i, y].Center.x > position.x)
                {
                    if(Vector2.Distance(_cellKeeper.Cells[i - 1, y].Center, position)
                        > Vector2.Distance(_cellKeeper.Cells[i, y].Center, position))
                    {
                        x = i;
                    }
                    else
                    {
                        x = i - 1;
                    }
                    if(i < _cellKeeper.MapData.Columns - 1 && Vector2.Distance(_cellKeeper.Cells[i + 1, y].Center, position)
                        < Vector2.Distance(_cellKeeper.Cells[i, y].Center, position))
                    {
                        x = i + 1;
                    }
                    break;
                }
            }
            if(x == -1) x = _cellKeeper.MapData.Columns - 1;

            y = -1;
            for(int i = 1; i < _cellKeeper.MapData.Rows; i++)
            {
                if(_cellKeeper.Cells[x, i].Center.y > position.y)
                {
                    if(Vector2.Distance(_cellKeeper.Cells[x, i - 1].Center, position)
                        > Vector2.Distance(_cellKeeper.Cells[x, i].Center, position))
                    {
                        y = i;
                    }
                    else
                    {
                        y = i - 1;
                    }
                    if(i < _cellKeeper.MapData.Rows - 1 && Vector2.Distance(_cellKeeper.Cells[x, i + 1].Center, position)
                        < Vector2.Distance(_cellKeeper.Cells[x, i].Center, position))
                    {
                        y = i + 1;
                    }
                    break;
                }
            }
            if(y == -1) y = _cellKeeper.MapData.Rows - 1;
            return new MapPosition(x, y);
        }
    }

    public class MapPosition
    {
        public int X;
        public int Y;

        public MapPosition(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}