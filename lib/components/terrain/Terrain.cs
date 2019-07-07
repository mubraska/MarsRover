using System;
using System.Collections.Generic;

namespace MarsRover.Components
{
    public class TerrainComponent
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Dictionary<string, RoverComponent> RoverDictionary { get; private set; }

        public TerrainComponent(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            this.RoverDictionary = new Dictionary<string, RoverComponent>();
        }

        public void AddRover(Rover.Position position)
        {
            if (position.X < 0 || position.Y < 0 || position.X > this.Width || position.Y > this.Height)
                throw new Exception($"Can not add rover to terrain, due to position ({position.X}, {position.Y}) exceeding the terrain.");
           
            string coordinateString = $"{position.X}-{position.Y}";

            if (this.RoverDictionary.ContainsKey(coordinateString))
                throw new Exception($"Can not add rover to terrain, due to an existing rover (Rover #{this.RoverDictionary[coordinateString].Id}).");
            
            var rover = new RoverComponent(this.RoverDictionary.Count + 1, position, this);

            this.RoverDictionary.Add(coordinateString, rover);
        }
    }
}
