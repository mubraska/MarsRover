using System;
using System.Linq;
using MarsRover.Components.Rover;

namespace MarsRover.Config
{
    // Represents a rover positioning configuration line read from the input file
    public class RoverPositionConfig : IConfig<Position>
    {
        public Position Value { get; private set; }

        public RoverPositionConfig(string configStr)
        {
            var roverPositionConfig = configStr.Trim().Split(" ").Where(p => !string.IsNullOrWhiteSpace(p)).ToArray();

            if (roverPositionConfig.Count() != 3)
                throw new Exception($"Invalid number of rover position commands in line {configStr}.");

            if (!int.TryParse(roverPositionConfig[0], out int x))
                throw new Exception($"Invalid rover coordinate X {roverPositionConfig[0]} in line {configStr}.");

            if (!int.TryParse(roverPositionConfig[1], out int y))
                throw new Exception($"Invalid rover coordinate Y {roverPositionConfig[1]} in line {configStr}.");

            if (!char.TryParse(roverPositionConfig[2], out char orientationChar))
                throw new Exception($"Invalid rover orientation {roverPositionConfig[2]} in line {configStr}.");

            Orientation? orientation = default(Orientation?);

            // Gets the orientation value by the custom abbreviation attribute defined in the Orientation enum
            orientation = Enum.GetValues(typeof(Orientation))
                                    .OfType<Orientation>()
                                    .FirstOrDefault(q => q.GetAttribute<OrientationAbbreviationAttribute>().Abbreviation == orientationChar);

            if (!orientation.HasValue)
                throw new Exception($"Invalid rover orientation {orientationChar} in line {configStr}.");

            this.Value = new Position(x, y, orientation.Value);
        }
    }
}