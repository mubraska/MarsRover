using System;
using System.IO;
using System.Linq;
using MarsRover.Config;

namespace MarsRover
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Gets all configuration lines as a string array
                string[] lines = File.ReadAllLines("config.txt");

                // Parse the first configuration line for terrain value
                var terrainConfig = new TerrainConfig(lines[0]);

                var roverConfigLines = lines.Skip(1).ToArray();

                // Validates rover configuration line count which should be even
                if (roverConfigLines.Count() % 2 == 1)
                    throw new Exception("Invalid number of rover commands in file.");

                // Gets the configuration lines which have even indices to group with the odd ones and parse them to create rover configuration values
                var evenRoverConfigLines = roverConfigLines.Where((p, index) => index % 2 == 0);

                var roverConfig = evenRoverConfigLines.Select((p, index) =>
                {
                    var roverPositionConfig = new RoverPositionConfig(p);
                    var roverCommandsConfig = new RoverCommandsConfig(roverConfigLines[index * 2 + 1]);

                    return new
                    {
                        Position = roverPositionConfig,
                        Commands = roverCommandsConfig
                    };
                });

                var terrain = terrainConfig.Value;

                // Add rovers to the terrain
                foreach (var rc in roverConfig)
                {
                    terrain.AddRover(rc.Position.Value);
                    terrain.RoverDictionary[$"{rc.Position.Value.X}-{rc.Position.Value.Y}"].SetCommands(rc.Commands.Value);
                }

                // Consume commands of each rover respectively
                foreach (var rover in terrain.RoverDictionary.ToList())
                {
                    try
                    {
                        rover.Value.ConsumeCommands();
                        Console.WriteLine(rover.Value.ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
