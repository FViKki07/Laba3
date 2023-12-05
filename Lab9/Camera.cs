﻿using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lab9
{
    internal class Camera
    {

        public PointZ Position { get; set; }
        public double Rotation { get; set; }
        public PointZ forward { get; set; }
        public PointZ Up { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public float alpha;
        public float AspectRatio;


        public Camera(PointZ Position, double Rotation, PointZ forward, int width, int height)
        { 
            
            this.Position = Position;
            if(Math.Abs(Rotation) < 90)
                this.Rotation = Rotation;
            this.forward = forward;

            this.Up = new PointZ(0, 1, 0);
            this.Width = width;
            this.Height = height;
            this.AspectRatio = Width / (float)Height;
            this.alpha = 45;
        }

        public Transform LookAt()
        {
            var f = (forward + Position).Normalize();
            var s = PointZ.CrossProduct(Up, f).Normalize();
            var u = PointZ.CrossProduct(f, s);

            var a = PointZ.DotProduct(s, Position);
            var b = PointZ.DotProduct(u, Position);
            var c = PointZ.DotProduct(f, Position);

            return new Transform(new double[,] {

                { s.X, s.Y, s.Z, 0 },
                { u.X, u.Y, u.Z, 0},
                {-f.X, -f.Y, -f.Z,0 },
                {-a, -b, -c, 1  }
            });
        }
        public void move(PointZ posOffset)
        {
            Position += posOffset;
            forward += posOffset;
        }

        public void recalcTarget(PointZ currentRotation)
        {
            if (currentRotation.Y > 89.0f)
                currentRotation.Y = 89.0f;
            if (currentRotation.Y < -89.0f)
                currentRotation.Y = -89.0f;

            forward.X = Math.Cos((currentRotation.X / 180 * Math.PI)) * Math.Cos(currentRotation.Y / 180 * Math.PI);
            forward.Y = Math.Sin(currentRotation.Y / 180 * Math.PI);
            forward.Z = Math.Sin(currentRotation.X / 180 * Math.PI) * Math.Cos(currentRotation.Y / 180 * Math.PI);

            //requiredRecalcView = true;
        }
    }
}
