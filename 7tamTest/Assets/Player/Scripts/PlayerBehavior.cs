using UnityEngine;

namespace Units.Player
{
    public class PlayerBehavior : UnitBehavior
    {
        [SerializeField]
        private UnitMovement _movement;
        [SerializeField]
        private GameObject _bomb;

        public void PlantBomb()
        {
            var bomb = Instantiate(_bomb, _movement.CellKeeper.Cell(_movement.CurrentPosition).Center, Quaternion.identity);
            bomb.GetComponent<Bomb>().ActivateBomb(_movement.CurrentPosition, 
                _movement.CellKeeper, _movement.PositionCalculator);
        }

        public override void Death()
        {
            Debug.Log("Dead");
            Time.timeScale = 0;
        }
    }
}