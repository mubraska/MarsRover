using System;
using System.Collections.Generic;
using MarsRover.Components.Rover;

namespace MarsRover.Components
{
    public class RoverComponent
    {
        // The terrain that the component is staying on.
        private TerrainComponent _terrain;

        // The queue containing the command set of the rover.
        private CommandQueue _commandQueue;
        public int Id { get; private set; }
        public Position Position { get; private set; }

        public RoverComponent(int id, Position position, TerrainComponent terrain)
        {
            this.Id = id;
            this.Position = position;
            this._terrain = terrain;
            this._commandQueue = new CommandQueue();
        }

        // Adds the given command set to the command queue.
        public void SetCommands(IEnumerable<Command> commands)
        {
            foreach (var command in commands)
            {
                this._commandQueue.Enqueue(command);
            }
        }

        // Values in Orientation goes clockwise therefore adding 1 goes clockwise and substracting 1 goes counter clockwise in compass position
        private void Rotate(Rotation rotation)
        {
            int orientationCount = Enum.GetValues(typeof(Orientation)).Length;
            int orientation = (int)this.Position.Orientation;

            switch (rotation)
            {
                case Rotation.Left:
                    orientation = (orientation - 1) % orientationCount;
                    break;
                case Rotation.Right:
                    orientation = (orientation + 1) % orientationCount;
                    break;
                default:
                    break;
            }

            // For cases like going from North to West which is 0 to 3
            if (orientation < 0)
                orientation += orientationCount;

            this.Position.Orientation = (Orientation)orientation;
        }

        private void Move()
        {
            int x = this.Position.X;
            int y = this.Position.Y;
            string positionKey = this.Position.ToCoordinateString();
            string newPositionKey = string.Empty;

            switch (this.Position.Orientation)
            {
                case Orientation.North:
                    y += 1;
                    break;
                case Orientation.East:
                    x += 1;
                    break;
                case Orientation.South:
                    y -= 1;
                    break;
                case Orientation.West:
                    x -= 1;
                    break;
                default:
                    break;
            }

            // Validation for going out of bounds
            if (x < 0 || y < 0 || x > this._terrain.Width - 1 || y > this._terrain.Height - 1)
            {
                string message = $"Rover #{this.Id} stopped moving due to coordinates ({x},{y}) being out of bounds.";

                if (this._commandQueue.Count > 0)
                    message += $" Could not execute remaining commands ({this._commandQueue.ToString()})";

                throw new Exception(message);
            }
                
            newPositionKey = $"{x}-{y}";
            
            // Validation for collision
            if (this._terrain.RoverDictionary.ContainsKey(newPositionKey))
            {
                string message = $"Rover #{this.Id} stopped moving at position ({this.Position.X}, {this.Position.Y}) due to potential collision with Rover #{this._terrain.RoverDictionary[newPositionKey].Id} on coordinates ({x},{y}).";

                if (this._commandQueue.Count > 0)
                    message += $" Could not execute remaining commands ({this._commandQueue.ToString()})";

                throw new Exception(message);
            }

            // Set new position of the rover
            this.Position.X = x;
            this.Position.Y = y;

            // Update position of the rover in the terrain
            this._terrain.RoverDictionary.Remove(positionKey);
            this._terrain.RoverDictionary.Add(newPositionKey, this);
        }

        // Apply the commands in the queue one by one till they're all consumed
        public void ConsumeCommands()
        {
            while (this._commandQueue.Count > 0)
            {
                Command command = this._commandQueue.Dequeue();

                switch (command)
                {
                    case Command.Move:
                        this.Move();
                        break;
                    case Command.RotateLeft:
                        this.Rotate(Rotation.Left);
                        break;
                    case Command.RotateRight:
                        this.Rotate(Rotation.Right);
                        break;
                    default:
                        break;
                }
            }
        }

        public override string ToString()
        {
            return $"{this.Position.X} {this.Position.Y} {this.Position.Orientation.GetAttribute<OrientationAbbreviationAttribute>().Abbreviation}";
        }
    }
}