using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BotGameDrower
{
    public class MyMouse
    {
        Color color;
        public Vector2 position;
        public float maxSpeed = 10;
        public int lightRahge = 30;
        public float acselleration = 0.1f;
        public Vector2 nowSpeed = new Vector2(0,0);
        public int direction;
        ////0///
        ///3*1//
        ////2///
        public int HP;
        public MyMouse(Vector2 position,Random rnd)
        {
            this.position = position;
            direction = rnd.Next(4);
        }
    }
}
