namespace CodeRetreat
{
    public class Teleport
    {
        public Point Location { get; }
        public int TeleportIndex { get; }

        public Teleport(Point location, int teleportIndex)
        {
            Location = location;
            TeleportIndex = teleportIndex;
        }
    }
}