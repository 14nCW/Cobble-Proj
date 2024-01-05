using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class TopologyTile : Tile
{
    public int baseMovementCost;
    public List<tileDirections> directions;
}

public enum tileDirections
{
    NORTH_EAST,
    NORTH_WEST,
    SOUTH_EAST,
    SOUTH_WEST
}
