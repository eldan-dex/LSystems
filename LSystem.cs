using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Randio_2 {
    class LSystem {
        public string State { get; private set; }

        private List<Rule> rules;

        //Ctors
        public LSystem() {
            rules = new List<Rule>();
        }

        public LSystem(string template) : this() {
            SetTemplate(template);
        }

        public LSystem(string template, List<Rule> rules) {
            this.rules = rules;
            SetTemplate(template);
        }


        //Preparation
        public void SetTemplate(string template) {
            State = template;
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
        Vector2 currentPosition;
        double angle;

        SpriteBatch spriteBatch;
        GraphicsDevice device;

        Texture2D lineTexture;

        public Turtle(GraphicsDevice device, SpriteBatch spriteBatch, Vector2 startingPosition, double startingAngle) {
            this.device = device;
            this.spriteBatch = spriteBatch;
            currentPosition = startingPosition;
            angle = startingAngle;

            lineTexture = new Texture2D(device, 1, 1);
            lineTexture.SetData(new Color[] { Color.White });
        }

        //Draws the whole L-System to a RenderTarget2D
        public RenderTarget2D DrawSystem(LSystem System) {
            RenderTarget2D target = new RenderTarget2D(device, 2000, 2000);
            device.SetRenderTarget(target);
            device.Clear(Color.Black);

            foreach (char c in System.State) {
                if (c == 'F' || c == 'G')
                    MoveTurtle(80);

                else if (c == '+')
                    angle += 60;

                else if (c == '-')
                    angle -= 60;
            }

            device.SetRenderTarget(null);

            return target;
        }

        private void MoveTurtle(int length) {
            Vector2 newPos = currentPosition + (length * GeometryHelper.AngleToVector(GeometryHelper.DegToRad(angle)));
            GraphicsHelper.DrawLine(spriteBatch, lineTexture, currentPosition, newPos);
            currentPosition = newPos;
        }
    }
}
