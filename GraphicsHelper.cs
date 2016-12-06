using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Randio_2 {
    class GraphicsHelper {

        //Public methods
        //********************************************************************************//
        public static void FillRectangle(Texture2D texture, Color fill) {
            Color[] color = new Color[texture.Width * texture.Height];
            texture.GetData(color); //Texture size limit is 4096*4096. Textures bigger than that crash when being converted to Color[]

            for (int i = 0; i < texture.Width * texture.Height; ++i)
                color[i] = fill;

            texture.SetData(color);
        }

        public static void OutlineRectangle(Texture2D texture, Color outline, int outlineWidth) {
            Color[] color = new Color[texture.Width * texture.Height];
            texture.GetData(color);

            int index = 0;
            for (int y = 0; y < texture.Height; ++y) {
                for (int x = 0; x < texture.Width; ++x) {
                    if (y < outlineWidth || x < outlineWidth || y > texture.Height - outlineWidth-1 || x > texture.Width - outlineWidth-1)
                        color[index] = outline;
                    ++index;
                }
            }
            texture.SetData(color);
        }

        public static void OutlineRectangleSide(Texture2D texture, Color outline, int outlineWidth, bool leftSide, bool topSide, bool rightSide, bool bottomSide) {
            Color[] color = new Color[texture.Width * texture.Height];
            texture.GetData(color);

            int index = 0;
            for (int y = 0; y < texture.Height; ++y) {
                for (int x = 0; x < texture.Width; ++x) {
                    if ((topSide && y < outlineWidth) || (leftSide && x < outlineWidth) || (bottomSide && y > texture.Height - outlineWidth-1) || (rightSide && x > texture.Width - outlineWidth-1))
                        color[index] = outline;
                    ++index;
                }
            }
            texture.SetData(color);
        }

        public static void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 a, Vector2 b) {
            Vector2 edge = b - a;

            // calculate angle to rotate line
            double angle = Math.Atan2(edge.Y, edge.X);

            spriteBatch.Begin();
            spriteBatch.Draw(texture, new Rectangle((int)a.X, (int)a.Y, (int)edge.Length(), 1), null, Color.White, (float)angle, new Vector2(0, 0), SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}
