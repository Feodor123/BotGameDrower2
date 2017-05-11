using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotGameDrower
{
    class Tile
    {
        public string name;
        public bool isObstacle = false;
        public Tile(string name)
        {
            this.name = name;
            if (name == "box")
            {
                isObstacle = true;
            }
            else if (name == "floor")
            {
                isObstacle = false;
            }
        }
    }
}
