
using System.Runtime.InteropServices;

namespace Alioth.Primitives {
    public class AABB  {
        // Render Thing
        public Color4 Color;
        public static Shader Shader;
        protected int VAO, VBO, EBO;
        
        private float[] vertices;
        private readonly uint[] indices = [0, 1, 1, 2, 2, 3, 3, 0, 0, 4, 1, 5, 2, 6, 3, 7, 4, 5, 5, 6, 6, 7, 7, 4];
        public Vector3 A;
        public Vector3 B;
        public AABB(AABB a, AABB b, Color4 color) : this(
                new Vector3(
                    Math.Min(a.A.X, b.A.X),
                    Math.Min(a.A.Y, b.A.Y),
                    Math.Min(a.A.Z, b.A.Z)),
                new Vector3(
                    Math.Max(a.B.X, b.B.X),
                    Math.Max(a.B.Y, b.B.Y),
                    Math.Max(a.B.Z, b.B.Z)), color
            ) {
        }
        public AABB(Vector3 a, Vector3 b, Color4 color) {
            A = a;
            B = b;
            Color = color;
            vertices = [
                a.X, a.Y, a.Z,   // 0
                a.X, a.Y, b.Z,   // 1
                b.X, a.Y, b.Z,   // 2
                b.X, a.Y, a.Z,   // 3
                a.X, b.Y, a.Z,   // 4
                a.X, b.Y, b.Z,   // 5
                b.X, b.Y, b.Z,   // 6
                b.X, b.Y, a.Z,   // 7
            ];
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
            
            var vertexLocation = Shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }
        public void Draw(Camera camera) {
            GL.BindVertexArray(VAO);
            Shader.Use();
            Shader.SetVector4("uColor", Color);
            Shader.SetMatrix4("uModelMat", Matrix4.Identity);
            Shader.SetMatrix4("uViewMat", camera.GetViewMatrix());
            Shader.SetMatrix4("uProjectionMat", camera.GetProjectionMatrix());
            GL.DrawElements(PrimitiveType.Lines, indices.Length, DrawElementsType.UnsignedInt, 0);
            GL.DrawArrays(PrimitiveType.Lines, 0, 6);
        }
    }
}
