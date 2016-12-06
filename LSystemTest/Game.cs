using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace LSystemTest {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont debugFont;
        KeyboardState keyboardState;

        Camera camera;
        LSystem system;
        Turtle turtle;
        RenderTarget2D render;

        public const int canvasW = 4096;
        public const int canvasH = 4096;

        bool iEnabled;

        int step = 10;
        int angle = 22;
        string axiom = "F";
        List<LSystem.Rule> rules;

        public Game(string[] args) {
            rules = new List<LSystem.Rule>();
            try {
                if (args.Length > 1)
                    step = System.Convert.ToInt32(args[1]);
                if (args.Length > 2)
                    angle = System.Convert.ToInt32(args[2]);
                if (args.Length > 3)
                    axiom = args[3];

                int cnt = 4;
                while (cnt < args.Length)
                {
                    string[] rspl = args[cnt].Split('=');
                    if (rspl.Length > 1)
                        rules.Add(new LSystem.Rule(rspl[0], rspl[1]));
                    ++cnt;
                }
            }
            catch
            {
                step = 10;
                angle = 22;
                rules.Add(new LSystem.Rule("F", "1FF-[2-F+F-F]+[2+F-F+F]"));
            }

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            graphics.PreferredBackBufferWidth = 1280; //X
            graphics.PreferredBackBufferHeight = 720; //Y
            graphics.ApplyChanges();

            camera = new Camera(GraphicsDevice.Viewport);
            camera.CenterTo(new Rectangle(canvasW/2, canvasH/2, 1, 1));

            /*system = new LSystem("FX", 20, 25);
            system.AddRule(new LSystem.Rule("F", "1FF-[2-F+F]+[2+F-F]"));
            system.AddRule(new LSystem.Rule("X", "1FF+[2+F]+[2-F]"));*/
            system = new LSystem(axiom, step, angle);
            foreach (LSystem.Rule r in rules)
                system.AddRule(r);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        
            turtle = new Turtle(GraphicsDevice, spriteBatch, new Vector2(100, canvasH / 2), -90, Color.White);
            render = turtle.DrawSystem(system);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            //Arrow keys for manual camera controls
            if (keyboardState.IsKeyDown(Keys.Right))
                camera.Position += new Vector2(10f, 0);

            if (keyboardState.IsKeyDown(Keys.Left))
                camera.Position -= new Vector2(10f, 0);

            if (keyboardState.IsKeyDown(Keys.Down))
                camera.Position += new Vector2(0, 10f);

            if (keyboardState.IsKeyDown(Keys.Up))
                camera.Position -= new Vector2(0, 10f);

            if (keyboardState.IsKeyDown(Keys.OemPlus))
                camera.Zoom += 0.01f;

            if (keyboardState.IsKeyDown(Keys.OemMinus))
                camera.Zoom -= 0.01f;

            if (keyboardState.IsKeyDown(Keys.I) && iEnabled)
            {
                system.Iterate();
                render = turtle.DrawSystem(system);
                iEnabled = false;
            }

            if (keyboardState.IsKeyUp(Keys.I))
                iEnabled = true;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var viewMatrix = camera.GetViewMatrix();

            spriteBatch.Begin(transformMatrix: viewMatrix);
            spriteBatch.Draw(render, new Rectangle(0, 0, canvasW, canvasH), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
