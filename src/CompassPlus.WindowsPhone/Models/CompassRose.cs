namespace CompassPlus.Models
{
    public class CompassRose
    {
        public int Points { get; set; }
        public string Name { get; set; }

        public const string Rose8Name = "PointRose8";
        public const string Rose360Name = "PointRose360";
        public const string Rose6000Name = "MilsRose6000";
        public const string Rose6400Name = "MilsRose6400";

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
