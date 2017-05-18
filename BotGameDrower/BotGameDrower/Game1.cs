using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BotGameDrower
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        SpriteFont font14;
        Texture2D boxTexture;
        Texture2D floorTexture;
        Texture2D mouseTexture;
        Texture2D barTexture;
        Random rnd = new Random();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteEffects spriteEffect;
        //Effect fogOfWar;
        RenderTarget2D target;
        public MyMouse thisMouse;
        public const int tileHeight = 64;
        public const int tileWidth = 64;
        public Vector2 position = new Vector2(tileWidth * 100, tileHeight * 100);
        public bool wasPressedMouse = false;
        public Vector2 clickPos = new Vector2(0, 0);
        public Vector2 realPos = new Vector2(tileWidth * 100, tileHeight * 100);
        MyRectangle[,] first;
        List<MyRectangle> second = new List<MyRectangle>();
        List<MyRectangle> third = new List<MyRectangle>();
        bool [,] warFogMap;
        bool[,] boolPole;
        Tile[,] pole;
        public Vector2 ScreenCenter
        {
            get
            {
                return new Vector2(graphics.PreferredBackBufferWidth / 2.0F, graphics.PreferredBackBufferHeight / 2.0F);
            }
        }
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 1000; //graphics.PreferredBackBufferHeight * 2;
            graphics.PreferredBackBufferWidth = 2000;// graphics.PreferredBackBufferWidth * 2;
            pole = Generate(rnd,200,200);
            first = new MyRectangle[pole.GetLength(0), pole.GetLength(1)];
            int xx;
            int yy;
            do
            {
                xx = rnd.Next(200);
                yy = rnd.Next(200);
            }
            while (pole[xx, yy].isObstacle);
            thisMouse = new MyMouse(new Vector2(tileWidth * xx, tileHeight * yy), rnd);
            position = new Vector2(tileWidth * xx, tileHeight * yy);
            realPos = new Vector2(tileWidth * xx, tileHeight * yy);
            warFogMap = new bool[200,200];
            boolPole = new bool[pole.GetLength(0), pole.GetLength(1)];
            for (int x = 0;x < pole.GetLength(0); x++)
            {
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    warFogMap[x,y] = false;
                    if (!pole[x,y].isObstacle)
                    {
                        boolPole[x, y] = false;
                    }
                    else
                    {
                        boolPole[x, y] = true;
                    }
                }
            }
            drawGenerate();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }
        public static double GetAngle(Vector2 v)
        {
            double l = Math.Sqrt(v.X * v.X + v.Y * v.Y);
            if (l == 0)
            {
                return 0;
            }
            if (v.Y >= 0)
            {
                return Math.PI - Math.Asin(v.X / l);
            }
            else
            {
                return Math.Asin(v.X / l);
            }
        }
        private void drawGenerate()
        {
            while (true)
            {
                for (int i = 0; i < 10000; i++)
                {
                    int x = rnd.Next(pole.GetLength(0));
                    int y = rnd.Next(pole.GetLength(1));
                    int width = 2 + rnd.Next(3);
                    int height = 2 + rnd.Next(3);
                    if ((x + width > pole.GetLength(0)) || (y + height > pole.GetLength(1)))
                    {
                        continue;
                    }
                    else
                    {
                        for (int xx = x; xx < x + width; xx++)
                        {
                            for (int yy = y; yy < y + height; yy++)
                            {
                                if (!boolPole[xx, yy])
                                {
                                    goto Break;
                                }
                            }
                        }
                        for (int xx = x; xx < x + width; xx++)
                        {
                            for (int yy = y; yy < y + height; yy++)
                            {
                                boolPole[xx, yy] = false;
                            }
                        }
                        first[x,y] = (new MyRectangle(width, height, 0, new Vector2(x, y)));
                    }
                    goto Break1;
                    Break:;
                }
                break;
                Break1:;
            }
            for (int x = 0; x < pole.GetLength(0); x++)
            {
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    if (boolPole[x, y])
                    {
                        first[x, y] = (new MyRectangle(1, 1, 0, new Vector2(x, y)));
                    }
                }
            }
        }
        private static Tile[,] Generate(Random rnd, int width, int height)
        {
            Tile[,] pole = new Tile[width, height];
            Tile[,] pole1 = new Tile[width, height];
            for (int x = 0; x < pole.GetLength(0); x++)
            {
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    pole[x, y] = new Tile("box");
                    pole1[x, y] = new Tile("box");
                }
            }
            for (int x = 0; x < pole.GetLength(0); x++)
            {
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    if (rnd.Next(100) <= 57)
                    {
                        pole[x, y] = new Tile("box");
                        pole1[x, y] = new Tile("box");
                    }
                    else
                    {
                        pole[x, y] = new Tile("floor");
                        pole1[x, y] = new Tile("floor");
                    }
                }
            }
            for (int i = 0; i < 7; i++)
            {
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    for (int x = 0; x < pole.GetLength(0); x++)
                    {
                        int k = 0;
                        for (int yy = y - 1; yy <= y + 1; yy++)
                        {
                            for (int xx = x - 1; xx <= x + 1; xx++)
                            {
                                if (!(xx == x && yy == y) && (xx < 0 || yy < 0 || xx >= pole.GetLength(0) || yy >= pole.GetLength(1) || pole1[xx, yy].isObstacle))
                                {
                                    k++;
                                }
                            }
                        }
                        if (k >= 5)
                        {
                            pole[x, y] = new Tile("box");
                        }
                        else
                        {
                            pole[x, y] = new Tile("floor");
                        }
                    }
                }
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    for (int x = 0; x < pole.GetLength(0); x++)
                    {
                        pole1[x, y] = pole[x, y];
                    }
                }
            }
            for (int i = 0; i < 2; i++)
            {
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    for (int x = 0; x < pole.GetLength(0); x++)
                    {
                        int k = 0;
                        if (x == 0 || pole1[x - 1, y].isObstacle)
                        {
                            k++;
                        }
                        if (y == 0 || pole1[x, y - 1] .isObstacle)
                        {
                            k++;
                        }
                        if (x == pole.GetLength(0) - 1 || pole1[x + 1, y].isObstacle)
                        {
                            k++;
                        }
                        if (y == pole.GetLength(1) - 1 || pole1[x, y + 1].isObstacle)
                        {
                            k++;
                        }
                        if (k <= 1 && pole1[x, y].isObstacle)
                        {
                            pole[x, y] = new Tile("floor");
                        }
                        else
                        {
                            if (k >= 3 && !pole1[x, y].isObstacle)
                            {
                                pole[x, y] = new Tile("box");
                            }
                            else
                            {
                                pole[x, y] = pole1[x, y];
                            }
                        }
                    }
                }
                for (int y = 0; y < pole.GetLength(1); y++)
                {
                    for (int x = 0; x < pole.GetLength(0); x++)
                    {
                        pole1[x, y] = new Tile(pole[x, y].name);
                    }
                }
            }
            for (int i = 0; i < pole.GetLength(0); i++)
            {
                pole[i, pole.GetLength(1) - 1] = new Tile("box");
                pole[i, 0] = new Tile("box");
            }
            for (int i = 0; i < pole.GetLength(1); i++)
            {
                pole[pole.GetLength(0) - 1, i] = new Tile("box");
                pole[0, i] = new Tile("box");
            }
            return pole;
        }
        protected override void Initialize()
        {
            target = new RenderTarget2D(GraphicsDevice, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            boxTexture = Content.Load<Texture2D>("box");
            floorTexture = Content.Load<Texture2D>("tile4");
            mouseTexture = Content.Load<Texture2D>("mouse2");
            barTexture = Content.Load<Texture2D>("bar");
            font14 = Content.Load<SpriteFont>("font14");
            //fogOfWar = Content.Load<Effect>("fogOfWarEffect");
        }
        protected override void UnloadContent()
        {

        }
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            var mouse = Mouse.GetState();
            var keyboard = Keyboard.GetState();
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                if (wasPressedMouse)
                {
                    realPos = position - new Vector2(mouse.X, mouse.Y) + clickPos;
                    if (realPos.X > 200 * tileWidth)
                    {
                        realPos.X = 200 * tileWidth;
                    }
                    if (realPos.X < 0)
                    {
                        realPos.X = 0;
                    }
                    if (realPos.Y > 200 * tileHeight)
                    {
                        realPos.Y = 200 * tileHeight;
                    }
                    if (realPos.Y < 0)
                    {
                        realPos.Y = 0;
                    }
                }
                else
                {
                    wasPressedMouse = true;
                    clickPos = new Vector2(mouse.X, mouse.Y);
                }
            }
            else
            {
                if (wasPressedMouse)
                {
                    position -= new Vector2(mouse.X, mouse.Y) - clickPos;
                    wasPressedMouse = false;
                }
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                thisMouse.nowSpeed.Y -= thisMouse.acselleration;
                thisMouse.direction = 0;
            }
            else
            {
                if (keyboard.IsKeyDown(Keys.Down))
                {
                    thisMouse.nowSpeed.Y += thisMouse.acselleration;
                    thisMouse.direction = 2;
                }
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                thisMouse.nowSpeed.X += thisMouse.acselleration;
                thisMouse.direction = 1;
            }
            else
            {
                if (keyboard.IsKeyDown(Keys.Left))
                {
                    thisMouse.nowSpeed.X -= thisMouse.acselleration;
                    thisMouse.direction = 3;
                }
            }
            thisMouse.nowSpeed = 0.98f * thisMouse.nowSpeed;
            Break:
            if (pole[(int)((thisMouse.position + thisMouse.nowSpeed).X/tileWidth), (int)((thisMouse.position + thisMouse.nowSpeed).Y / tileHeight)].isObstacle)
            {
                thisMouse.nowSpeed *= -0.9f;
                goto Break;
            }
            double d = Math.Sqrt(thisMouse.nowSpeed.X * thisMouse.nowSpeed.X + thisMouse.nowSpeed.Y * thisMouse.nowSpeed.Y);
            if (d > thisMouse.maxSpeed)
            {
                Vector2 speed = new Vector2();
                speed.X = (float)(thisMouse.maxSpeed * thisMouse.nowSpeed.X / d);
                speed.Y = (float)(thisMouse.maxSpeed * thisMouse.nowSpeed.Y / d);
            }
            thisMouse.position += thisMouse.nowSpeed;
            for (int x = - 8; x <= 8; x++)
            {
                for (int y = - 8; y <= 8; y++)
                {
                    if (x*x + y*y < 30)
                    {
                        try
                        {
                            warFogMap[(int)(thisMouse.position.X / tileWidth) + x,(int)(thisMouse.position.Y / tileHeight) + y] = true;
                        }
                        catch
                        {}
                    }
                }
            }

            base.Update(gameTime);
        }
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            Vector2 v = new Vector2(thisMouse.position.X / tileWidth, thisMouse.position.Y / tileHeight);
            for (int x = (int)MathHelper.Max(0, v.X - 30); x < (int)MathHelper.Min(pole.GetLength(0), v.X + 30); x++)
            {
                for (int y = (int)MathHelper.Max(0, v.Y - 30); y < (int)MathHelper.Min(pole.GetLength(1), v.Y + 30); y++)
                {
                    if (!warFogMap[x,y])
                    {
                        spriteBatch.Draw(floorTexture, new Rectangle((int)(ScreenCenter.X - realPos.X + x * tileWidth), (int)(ScreenCenter.Y - realPos.Y + y * tileHeight), tileWidth, tileHeight), new Rectangle(32 * (Math.Abs(x - y) % 2), 0, 32, 32), Color.Black);
                    }
                    else if(first[x,y] != null)
                    {
                        int brightness = (int)(240 - 6 * (first[x, y].height * first[x, y].width));
                        spriteBatch.Draw(boxTexture, new Rectangle((int)(ScreenCenter.X - realPos.X + first[x, y].v1.X * tileWidth), (int)(ScreenCenter.Y - realPos.Y + first[x, y].v1.Y * tileHeight), (int)(first[x, y].width * tileWidth), (int)(first[x, y].height * tileHeight)), new Rectangle(0, 0, 80, 80), new Color(brightness, brightness, brightness));
                    }
                    else if (!pole[x,y].isObstacle)
                    {
                        spriteBatch.Draw(floorTexture,new Rectangle((int)(ScreenCenter.X - realPos.X + x * tileWidth), (int)(ScreenCenter.Y - realPos.Y + y * tileHeight),tileWidth,tileHeight), new Rectangle(32 * (Math.Abs(x - y) % 2),0,32,32),Color.White);
                    }
                }
            }
            float f = (float)GetAngle(thisMouse.nowSpeed);
            spriteBatch.Draw(mouseTexture, new Rectangle((int)(ScreenCenter.X - realPos.X + thisMouse.position.X), (int)(ScreenCenter.Y - realPos.Y + thisMouse.position.Y), tileWidth, tileHeight), new Rectangle(0, 0, 800, 800), Color.White, f,new Vector2(320,400),spriteEffect,0);
            spriteBatch.Draw(barTexture,new Vector2(600,10), new Rectangle(0,0,718,38),Color.White);
            spriteBatch.DrawString(font14,"0",new Vector2(655,15),Color.White);
            spriteBatch.DrawString(font14, "0", new Vector2(745, 15), Color.White);
            spriteBatch.DrawString(font14, "0", new Vector2(840, 15), Color.White);
            spriteBatch.DrawString(font14, "1/100", new Vector2(928, 15), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
