using System.Numerics;

namespace GK1_MeshEditor
{
    internal abstract class Transform
    {
        protected Matrix4x4 _transform = Matrix4x4.Identity;

        public void Translate(float tx, float ty, float tz)
        {
            Matrix4x4 translationMatrix = Matrix4x4.CreateTranslation(tx, ty, tz);
            _transform = Matrix4x4.Multiply(_transform, translationMatrix);
        }

        public void Scale(float sx, float sy, float sz)
        {
            Matrix4x4 scaleMatrix = Matrix4x4.CreateScale(sx, sy, sz);
            _transform = Matrix4x4.Multiply(_transform, scaleMatrix);
        }

        public void Rotate(float rx, float ry, float rz)
        {
            Matrix4x4 rotationXMatrix = Matrix4x4.CreateRotationX(rx);
            Matrix4x4 rotationYMatrix = Matrix4x4.CreateRotationY(ry);
            Matrix4x4 rotationZMatrix = Matrix4x4.CreateRotationZ(rz);

            Matrix4x4 temp = Matrix4x4.Identity;
            temp = Matrix4x4.Multiply(rotationXMatrix, rotationYMatrix);
            temp = Matrix4x4.Multiply(temp, rotationZMatrix);
            _transform = Matrix4x4.Multiply(_transform, temp);
        }

        public void ResetTransform()
        {
            _transform = Matrix4x4.Identity;
        }

        public abstract void ApplyTransformations();
    }
}
