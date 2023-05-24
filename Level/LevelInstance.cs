namespace Game
{
	public interface ILevelInstance
	{
		Level Value { get; set; }
	}

	public class LevelInstance : ILevelInstance
	{
		public Level Value { get; set; }
	}
}