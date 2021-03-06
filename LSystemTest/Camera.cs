﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LSystemTest {
    public class Camera {

        //Public variables
        //********************************************************************************//
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Zoom { get; set; }
        public Vector2 Origin { get; set; }


        //Private variables
        //********************************************************************************//
        private readonly Viewport _viewport;


        //Public methods
        //********************************************************************************//
        public Camera(Viewport viewport) {
            _viewport = viewport;

            Rotation = 0;
            Zoom = 1f;
            Origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
            Position = Vector2.Zero;
        }

        public void CenterTo(Rectangle rect) {
            float selfX = Position.X;
            float selfY = Position.Y;
            float rectCenter = rect.X + (rect.Width / 2);
            selfX = rectCenter - (_viewport.Width / 2);
            selfY = rectCenter - (_viewport.Height / 2);

            if (selfX < 0)
                selfX = 0;
            if (selfY < 0)
                selfY = 0;

            Position = new Vector2(selfX, selfY);
        }

        public Matrix GetViewMatrix() {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }
    }
}
