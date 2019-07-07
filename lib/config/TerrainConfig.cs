using System;
using System.Linq;
using MarsRover.Components;

namespace MarsRover.Config
{
    // Represents a terrain configuration line read from the input file
    public class TerrainConfig : IConfig<TerrainComponent>
    {
        public TerrainComponent Value { get; private set; }

        public TerrainConfig(string configStr)
        {
            var terrainConfig = configStr.Trim().Split(" ");

            if (terrainConfig.Count() != 2)
            {
                throw new Exception($"Invalid number of terrain commands in line {configStr}.");
            }

            if (!int.TryParse(terrainConfig[0], out int terrainWidth) && terrainWidth < 1)
            {
                throw new Exception($"Invalid terrain width {terrainConfig[0]} in line {configStr}.");
            }

            if (!int.TryParse(terrainConfig[1], out int terrainHeight) && terrainHeight < 1)
            {
                throw new Exception($"Invalid terrain height {terrainConfig[1]} in line {configStr}.");
            }

            this.Value = new TerrainComponent(terrainWidth + 1, terrainHeight + 1);
        }
    }
}