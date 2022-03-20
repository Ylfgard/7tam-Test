using UnityEngine;

namespace Map
{
    public class StoneSpawner : MonoBehaviour
    {
        [SerializeField]
        private MapCellKeeper _cellKeeper;
        [SerializeField]
        private GameObject _stonePrefab;
        [SerializeField]
        private Transform _level;

        public void SpawnStones()
        {
            for (int i = 0; i < _cellKeeper.MapData.Columns; i++)
            {
                if(i % 2 == 0) continue;
                for (int j = 0; j < _cellKeeper.MapData.Rows; j++)
                {
                    if(j % 2 == 0) continue;
                    GameObject stone = Instantiate(_stonePrefab, _cellKeeper.Cells[i, j].Center, Quaternion.identity, _level);
                    stone.GetComponent<SpriteRenderer>().sortingOrder = _cellKeeper.MapData.Rows - j;
                    _cellKeeper.Cells[i, j].ChangeType(CellType.Stone, null);
                }
            }
        }
    }
}