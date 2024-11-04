using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class BezierSurface
    {
        public Vector3[,] ControlPoints = new Vector3[4,4];
        public Mesh? Mesh;

        public BezierSurface() { }

        public BezierSurface(Vector3[,] controlPoints)
        {
            if (controlPoints.GetLength(0) != 4 || controlPoints.GetLength(1) != 4)
                throw new InvalidDataException("Control points must be a 4x4 array.");

            ControlPoints = controlPoints;
        }

        public void GenerateMesh(int subdivisions)
        {
            Mesh = new Mesh(ControlPoints, subdivisions);
        }

        public static BezierSurface LoadFromFile(string fileName)
        {
            BezierSurface surface = new BezierSurface();
            int count = 0;
            string[]? cords;

            using var stream = new StreamReader(fileName);
            while (!stream.EndOfStream)
            {
                cords = stream.ReadLine()?.Split(' ');
                if (cords == null) break;
                if (count > 15) throw new InvalidDataException("Control points must be a 4x4 array.");

                float X = float.Parse(cords[0]);
                float Y = float.Parse(cords[1]);
                float Z = float.Parse(cords[2]);

                Vector3 p = new Vector3(X, Y, Z);
                int i = count / 4;
                int j = count % 4;

                surface.ControlPoints[i, j] = p;
                count++;
            }

            return surface;
        }
    }
}
