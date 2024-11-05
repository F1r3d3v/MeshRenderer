using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GK1_MeshEditor
{
    internal class Util
    {
        public static Vector3 CartesianToBaricentric(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 v0 = b - a;
            Vector3 v1 = c - a;
            Vector3 v2 = p - a;
            float d00 = Vector3.Dot(v0, v0);
            float d01 = Vector3.Dot(v0, v1);
            float d11 = Vector3.Dot(v1, v1);
            float d20 = Vector3.Dot(v2, v0);
            float d21 = Vector3.Dot(v2, v1);
            float denom = d00 * d11 - d01 * d01;
            float alpha = (d11 * d20 - d01 * d21) / denom;
            float beta = (d00 * d21 - d01 * d20) / denom;
            float gamma = 1.0f - alpha - beta;

            return new Vector3(alpha, beta, gamma);
        }

    }
}
