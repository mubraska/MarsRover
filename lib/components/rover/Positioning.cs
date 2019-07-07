using System;

namespace MarsRover.Components.Rover
{
    class OrientationAbbreviationAttribute : Attribute
    {
        public char Abbreviation { get; private set; }

        public OrientationAbbreviationAttribute(char abbreviation)
        {
            this.Abbreviation = abbreviation;
        }
    }

    public enum Orientation
    {
        [OrientationAbbreviation('N')]
        North = 0,
        [OrientationAbbreviation('E')]
        East = 1,
        [OrientationAbbreviation('S')]
        South = 2,
        [OrientationAbbreviation('W')]
        West = 3
    }

    public enum Rotation
    {
        Left,
        Right
    }

    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Orientation Orientation { get; set; }
        
        public Position(int x, int y, Orientation orientation)
        {
            this.X = x;
            this.Y = y;
            this.Orientation = orientation;
        }

        public string ToCoordinateString()
        {
            return $"{this.X}-{this.Y}";
        }
    }
}