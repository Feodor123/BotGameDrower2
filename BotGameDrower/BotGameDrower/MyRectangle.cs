using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BotGameDrower
{
    class MyRectangle
    {
        public double width;
        public double height;
        public double alf;
        public Vector2 v1;
        public Vector2 v2;
        public Vector2 v3;
        public Vector2 v4;
        public MyRectangle(double width,double height,double alf,Vector2 pos)
        {
            this.width = width;
            this.height = height;
            this.alf = alf;
            v1 = pos;
            v2 = new Vector2((float)(pos.X + Math.Cos(alf) * width),(float)(pos.Y + Math.Sin(alf) * width));
            v3 = new Vector2((float)(pos.X + Math.Sin(alf) * height), (float)(pos.Y - Math.Cos(alf) * width));
            v4 = new Vector2(v2.X - v1.X + v3.X, v3.Y - v1.Y + v2.Y);
        }
        private bool IncludePoint(Vector2 v)
        {
            if (((v2.Y - v1.Y)*(v.X - v1.X)/(v2.X - v1.X) >= v.Y - v1.Y)
                &&((v4.Y - v3.Y) * (v.X - v3.X) / (v4.X - v3.X) <= v.Y - v3.Y)
                &&((v3.X - v1.X) * (v.Y - v1.Y) / (v3.Y - v1.Y) <= v.X - v1.X)
                &&((v4.X - v2.X) * (v.Y - v2.Y) / (v4.Y - v2.Y) >= v.X - v2.X))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IncludeRec(MyRectangle rec)
        {
            if (IncludePoint(rec.v1)&& IncludePoint(rec.v2)&& IncludePoint(rec.v3)&& IncludePoint(rec.v4))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Intersect(MyRectangle rec1,MyRectangle rec2)
        {
            if (rec2.IncludePoint(rec1.v1) || rec2.IncludePoint(rec1.v2) || rec2.IncludePoint(rec1.v3) || rec2.IncludePoint(rec1.v4)
                || rec1.IncludePoint(rec2.v1) || rec1.IncludePoint(rec2.v2) || rec1.IncludePoint(rec2.v3) || rec1.IncludePoint(rec2.v4))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
