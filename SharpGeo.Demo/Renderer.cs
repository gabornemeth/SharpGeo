//
// Renderer.cs
//
// Author:
//    Gabor Nemeth (gabor.nemeth.dev@gmail.com)
//
//    Copyright (C) 2016, Gabor Nemeth
//

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace SharpGeo.Demo
{
    class Renderer
    {
        private Bitmap _surface;

        public Renderer(Bitmap surface)
        {
            _surface = surface;
        }

        public void DrawLine(Point origin, Point endpoint, Color color)
        {
            DrawLine(origin.X, origin.Y, endpoint.X, endpoint.Y, color);
        }

        /// <summary>
        /// Bresenham line drawing
        /// </summary>
        /// <param name="originX"></param>
        /// <param name="originY"></param>
        /// <param name="endPointX"></param>
        /// <param name="endPointY"></param>
        /// <param name="color"></param>
        public void DrawLine(int originX, int originY, int endPointX, int endPointY, Color color)
        {
            var deltaX = endPointX - originX;
            var deltaY = endPointY - originY;
            var error = 0.0;

            // Note the below fails for completely vertical lines.
            var deltaError = Math.Abs(deltaY / deltaX);

            var y = originY;
            for (var x = originX; x < endPointX; x++)
            {
                _surface.SetPixel(x, y, color);
                error = error + deltaError;
                if (error >= 0.5)
                {
                    ++y;
                    error -= 1.0;
                }
            }
        }
    }
}
