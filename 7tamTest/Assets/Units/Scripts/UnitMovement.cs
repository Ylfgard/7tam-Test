using UnityEngine;
using Map;

namespace Units
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField]
        private Transform _transform;
        [SerializeField]
        private CellType _unitType;
        [SerializeField]
        private UnitBehavior _behavior;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private float _speed;
        [Header ("Sprites")]
        [SerializeField]
        private Sprite _right;
        [SerializeField]
        private Sprite _left;
        [SerializeField]
        private Sprite _up;
        [SerializeField]
        private Sprite _down;
        private MapCellKeeper _cellKeeper;
        private MapPositionCalculator _positionCalculator;
        private MapPosition _curPos;
        private MapPosition _targetPos;

        public MapCellKeeper CellKeeper => _cellKeeper;
        public MapPositionCalculator PositionCalculator => _positionCalculator;
        public MapPosition CurrentPosition => _curPos;
        private MapPosition TargetPosition => _targetPos;

        private void Awake()
        {
            _cellKeeper = FindObjectOfType<MapCellKeeper>();
            _positionCalculator = FindObjectOfType<MapPositionCalculator>();    
        }

        private void Start()
        {
            _curPos = _positionCalculator.CalculatePosition(_transform.position);
            _transform.position = _cellKeeper.Cells[_curPos.X, _curPos.Y].Center;
            _targetPos = _curPos;
            ChangeCurrentPosition();
        }

        public void MoveInDirection(Vector2 direction)
        {
            _targetPos = null;
            if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if(direction.x > 0)
                {
                    _targetPos = _positionCalculator.Position(_curPos.X + 1, _curPos.Y);
                    _spriteRenderer.sprite = _right;
                } 
                else if(direction.x < 0)
                {
                    _targetPos = _positionCalculator.Position(_curPos.X - 1, _curPos.Y);
                    _spriteRenderer.sprite = _left;
                }
            }
            else
            {
                if(direction.y > 0)
                {
                    _targetPos = _positionCalculator.Position(_curPos.X, _curPos.Y + 1);
                    _spriteRenderer.sprite = _up;
                } 
                else if(direction.y < 0)
                {
                    _targetPos = _positionCalculator.Position(_curPos.X, _curPos.Y - 1);
                    _spriteRenderer.sprite = _down;
                }
            }
            if(_targetPos == null)
            {
                _targetPos = _curPos;
                return;
            } 
            if(_cellKeeper.Cell(_targetPos).Type == CellType.Stone)
                _targetPos = _curPos;
        }

        private void FixedUpdate()
        {
            Vector3 target = _cellKeeper.Cell(_targetPos).Center;
            if(Vector2.Distance(target, _transform.position) < 0.5f)
                ChangeCurrentPosition();
            
            if(Vector2.Distance(target, _transform.position) < 0.1f) return;
            Vector3 dir = (target - _transform.position).normalized;
            _transform.position += dir * _speed * Time.deltaTime;
        }

        private void ChangeCurrentPosition()
        {
            _cellKeeper.Cells[_curPos.X, _curPos.Y].ClearCell();
            _curPos = _targetPos;
            _cellKeeper.Cells[_curPos.X, _curPos.Y].ChangeType(_unitType, _behavior);
            _spriteRenderer.sortingOrder = _cellKeeper.MapData.Rows - _curPos.Y;
        }
    }
}