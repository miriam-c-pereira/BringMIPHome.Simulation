namespace BringMIPHome.Simulation
{
    public struct SimVector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public SimVector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }
    }

    public struct SimVector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public SimVector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}