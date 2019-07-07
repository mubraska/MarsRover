using System;
using System.Linq;
using MarsRover.Components.Rover;

namespace MarsRover.Config
{
    // Represents a rover commands configuration line read from the input file
    public class RoverCommandsConfig : IConfig<Command[]>
    {
        public Command[] Value { get; private set; }

        public RoverCommandsConfig(string configStr)
        {
            // Updates the parsed values of the config line if the input is valid.
            this.Value = configStr.Trim().Select(q =>
               {
                   if (!Enum.IsDefined(typeof(Command), (int)q))
                   {
                       throw new Exception($"Invalid rover command {q} in line {configStr}.");
                   }

                   return (Command)q;
               }).ToArray();
        }
    }
}