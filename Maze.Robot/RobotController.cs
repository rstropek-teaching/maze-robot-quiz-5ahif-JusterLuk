using Maze.Library;
using System.Collections.Generic;
using System.Drawing;

namespace Maze.Solver
{
    /// <summary>
    /// Moves a robot from its current position towards the exit of the maze
    /// </summary>
    public class RobotController
    {
        private IRobot robot;
        private bool reachedEnd = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RobotController"/> class
        /// </summary>
        /// <param name="robot">Robot that is controlled</param>
        public RobotController(IRobot robot)
        {
            // Store robot for later use
            this.robot = robot;
        }

        // Tip: Don't make members public if they are only used inside of
        //      this class. Use a private member instead.
        public HashSet<Point> alreadyChecked { get; private set; }

        /// <summary>
        /// Moves the robot to the exit
        /// </summary>
        /// <remarks>
        /// This function uses methods of the robot that was passed into this class'
        /// constructor. It has to move the robot until the robot's event
        /// <see cref="IRobot.ReachedExit"/> is fired. If the algorithm finds out that
        /// the exit is not reachable, it has to call <see cref="IRobot.HaltAndCatchFire"/>
        /// and exit.
        /// </remarks>
        public void MoveRobotToExit()
        {
            Point startPoint = new Point(0, 0); 
            alreadyChecked = new HashSet<Point>();

            robot.ReachedExit += (_, __) => reachedEnd = true;

            CheckPoint(startPoint);

            //No End in Sight...
            if (reachedEnd == false) 
                robot.HaltAndCatchFire();
        }
        public void CheckPoint(Point currentPoint)
        {
            // General tip: Don't write `if (something == true)...` or `if (something == false)...`.
            //              Prefer `if (something)...` or `if (!something)...` instead.

            // Have I already been there? End Reached?
            if (alreadyChecked.Contains(currentPoint) == false && reachedEnd == false)
            {
                // We have visited this Point
                alreadyChecked.Add(currentPoint);
                // Can he move left?
                if (robot.TryMove(Direction.Left) == true )
                {
                    // Is Point left valid?
                    // Tip: Avoid repeating the data type left and right of the assignment (in this
                    //      case `Point`). Prefer `var` instead.
                    Point newtestpoint = new Point(currentPoint.X - 1, currentPoint.Y);
                    // Try this new Point
                    CheckPoint(newtestpoint);
                    // We found no end so lets go back
                    if (reachedEnd == false) { robot.Move(Direction.Right); }
                }

                if (reachedEnd == false && robot.TryMove(Direction.Right) == true)
                {
                    Point newtestpoint = new Point(currentPoint.X + 1, currentPoint.Y);
                    CheckPoint(newtestpoint);
                    if (reachedEnd == false) { robot.Move(Direction.Left); }
                }

                if (reachedEnd == false && robot.TryMove(Direction.Down) == true)
                {
                    Point newtestpoint = new Point(currentPoint.X, currentPoint.Y + 1);
                    CheckPoint(newtestpoint);
                    if (reachedEnd == false) { robot.Move(Direction.Up); }
                }

                if (reachedEnd == false && robot.TryMove(Direction.Up) == true)
                {
                    Point newtestpoint = new Point(currentPoint.X, currentPoint.Y - 1);
                    CheckPoint(newtestpoint);
                    if (reachedEnd == false) { robot.Move(Direction.Down); }
                }
            }
        }
    }
}
