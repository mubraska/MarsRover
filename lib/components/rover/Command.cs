using System.Collections.Generic;

namespace MarsRover.Components.Rover {
    public enum Command
    {
        Move = (int)'M',
        RotateLeft = (int)'L',
        RotateRight = (int)'R'
    }

     public class CommandQueue : Queue<Command>
    {
        public override string ToString()
        {
            return string.Join(", ", this.ToArray());
        }
    }
}