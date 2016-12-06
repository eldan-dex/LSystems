using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LSystemTest {
    class LSystem {
        public string State { get; private set; }
        public int Step { get; private set; }
        public double Angle { get; private set; }

        private List<Rule> rules;

        //Ctors
        public LSystem() {
            rules = new List<Rule>();
        }

        public LSystem(string template) : this() {
            SetTemplate(template);
        }

        public LSystem(string template, int stepLength, double angle) : this() {
            SetTemplate(template);
            SetStepLength(stepLength);
            SetAngle(angle);
        }

        public LSystem(string template, List<Rule> rules) {
            this.rules = rules;
            SetTemplate(template);
        }
        public LSystem(string template, int stepLength, double angle, List<Rule> rules) : this() {
            SetTemplate(template);
            SetStepLength(stepLength);
            SetAngle(angle);
        }


        //Preparation
        public void SetTemplate(string template) {
            State = template;
        }

        public void SetStepLength(int stepLength) {
            Step = stepLength;
        }

        public void SetAngle(double angle) {
            Angle = angle;
        }

        public void AddRule(Rule newRule) {
            rules.Add(newRule);
        }


        //Iteration
        public string ApplyRule(string element) {
            foreach (Rule rule in rules)
                if (rule.Element == element)
                    return rule.Replace;
            return element; //or empty string?
        }

        public void Iterate() {
            string newState = "";
            foreach (char c in State) {
                newState += ApplyRule(c.ToString());
            }
            State = newState;
        }

        public void Iterate(int times) {
            for (int i = 0; i < times; ++i)
                Iterate();
        }

        //Rules
        public class Rule {
            public string Element { get; private set; }
            public string Replace { get; private set; }

            public Rule(string element, string replace) {
                Element = element;
                Replace = replace;
            }
        }
    }

    class Turtle {
        class Moment {
            public Vector2 currentPosition;
            public double angle;

            public Moment() {
            }

            public Moment(Moment other) {
                currentPosition = other.currentPosition;
                angle = other.angle;
            }

            public Moment(Vector2 position, double angle) {
                currentPosition = position;
                this.angle = angle;
            }
        }
        
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Texture2D lineTexture;
        Moment now;
        Moment defaultMoment;
        Stack<Moment> time;
        Color currentColor;

        public Turtle(GraphicsDevice device, SpriteBatch spriteBatch, Vector2 startingPosition, double startingAngle, Color currentColor) {
            this.device = device;
            this.spriteBatch = spriteBatch;
            this.currentColor = currentColor;
            defaultMoment = new Moment(startingPosition, startingAngle);

            lineTexture = new Texture2D(device, 1, 1);
            lineTexture.SetData(new Color[] { Color.White });
        }

        //Draws the whole L-System to a RenderTarget2D
        public RenderTarget2D DrawSystem(LSystem System) {
            time = new Stack<Moment>();
            now = new Moment(defaultMoment);
            RenderTarget2D target = new RenderTarget2D(device, Game.canvasW, Game.canvasH);
            device.SetRenderTarget(target);
            device.Clear(Color.Black);
            Random r = new Random((int)DateTime.Now.Ticks);

            foreach (char c in System.State) {
                if (c == 'F' || c == 'G' || c == 'H')
                    MoveTurtle(System.Step);

                else if (c == '0')
                    currentColor = Color.White;

                else if (c == '1')
                    currentColor = Color.Brown;

                else if (c == '2')
                    currentColor = Color.Green;

                else if (c == '3')
                    currentColor = Color.Red;

                else if (c == '4')
                    currentColor = Color.Blue;

                else if (c == '+')
                    now.angle -= System.Angle;

                else if (c == '-')
                    now.angle += System.Angle;

                else if (c == '[')
                    time.Push(new Moment(now));

                else if (c == ']')
                    now = time.Pop();

            }

            device.SetRenderTarget(null);

            return target;
        }

        private void MoveTurtle(int length) {
            Vector2 newPos = now.currentPosition + (length * GeometryHelper.AngleToVector(GeometryHelper.DegToRad(now.angle)));
            //GraphicsHelper.DrawLine(spriteBatch, lineTexture, now.currentPosition, newPos, currentColor); //for drawing lines (conventional l-systems)
            GraphicsHelper.DrawSquares(spriteBatch, lineTexture, now.currentPosition, newPos, currentColor, length);
            now.currentPosition = newPos; //move next line to start inside parent?
        }
    }
}
