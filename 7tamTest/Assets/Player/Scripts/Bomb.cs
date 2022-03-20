using UnityEngine;
using System.Collections;
using Map;

namespace Units.Player
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField]
        private Transform _transform;
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        [SerializeField]
        private float _explosionDelay;
        [SerializeField]
        private int _distance;
        private MapPosition _position;
        private MapCellKeeper _cellKeeper;
        private MapPositionCalculator _positionCalculator;

        public void ActivateBomb(MapPosition position, MapCellKeeper cellKeeper, MapPositionCalculator positionCalculator)
        {
            _position = position;
            _cellKeeper = cellKeeper;
            _positionCalculator = positionCalculator;
            _spriteRenderer.sortingOrder = _cellKeeper.MapData.Rows - position.Y;
            _transform.position = _cellKeeper.Cell(position).Center;
            StartCoroutine(ExplosionDelay());
        }

        private IEnumerator ExplosionDelay()
        {
            yield return new WaitForSeconds(_explosionDelay);
            Explosion();
        }

        private void Explosion()
        {
            DamageUnits(_positionCalculator.CellsOnLine(_position, 
                _positionCalculator.Position(_position.X + _distance, _position.Y)));
            DamageUnits(_positionCalculator.CellsOnLine(_position, 
                _positionCalculator.Position(_position.X - _distance, _position.Y)));
            DamageUnits(_positionCalculator.CellsOnLine(_position, 
                _positionCalculator.Position(_position.X, _position.Y + _distance)));
            DamageUnits(_positionCalculator.CellsOnLine(_position, 
                _positionCalculator.Position(_position.X, _position.Y - _distance)));
            Destroy(gameObject);
        }

        private void DamageUnits(MapPosition[] affectedСells)
        {
            foreach(MapPosition pos in affectedСells)
            {
                var cell = _cellKeeper.Cell(pos);
                switch(cell.Type)
                {
                    case CellType.Stone:
                    return;

                    case CellType.Player:
                    cell.Unit.Death();
                    break;
                }
            }
        }
    }
}