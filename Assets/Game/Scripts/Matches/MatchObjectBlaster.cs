using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Scripts.Grids;

namespace Game.Scripts.Matches
{
    public class MatchObjectBlaster
    {
        private readonly GridBoard _gridBoard;
        private readonly Dictionary<int, int> _columnSpawnCountDictionary = new Dictionary<int, int>();

        public MatchObjectBlaster(GridBoard gridBoard)
        {
            _gridBoard = gridBoard;
        }

        public async Task<Dictionary<int,int>> BlastMatchingObjectsAndGetBlastDictionary(List<GridCoordinates> matchingCoordinatesList)
        {
            ResetColumnBlastDictionary();
            var blastTasks = new List<Task>();
            foreach (var matchingCoordinates in matchingCoordinatesList)
            {
                _columnSpawnCountDictionary[matchingCoordinates.X]++;
                AddFallCountsToUpperObjects(matchingCoordinates);
                var matchingObject = _gridBoard.GetMatchObjectFromCoordinates(matchingCoordinates);
                blastTasks.Add(matchingObject.Blast());
            }
            await Task.WhenAll(blastTasks);
            return _columnSpawnCountDictionary;
        }

        private void AddFallCountsToUpperObjects(GridCoordinates gridCoordinates)
        {
            for (int i = gridCoordinates.Y + 1; i < GridBoard.GridSize; i++)
            {
                var upperObjectCoordinates = new GridCoordinates { X = gridCoordinates.X, Y = i };
                var upperObject = _gridBoard.GetMatchObjectFromCoordinates(upperObjectCoordinates);
                upperObject.AddFallCount();
            }
        }

        private void ResetColumnBlastDictionary()
        {
            for (int i = 0; i < GridBoard.GridSize; i++)
            {
                _columnSpawnCountDictionary[i] = 0;
            }
        }
    }
}