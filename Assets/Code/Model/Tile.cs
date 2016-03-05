namespace RobotFactory.Model
{
	public struct Tile
	{
		public TileType Type { get; set; }
	}

	public enum TileType
	{
		Grass,
		Floor,
		Wall
	}
}