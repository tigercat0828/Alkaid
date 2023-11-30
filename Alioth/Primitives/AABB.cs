
using System.Runtime.InteropServices;

namespace Alioth.Primitives {
    public class AABB  {
        // Render Thing
        public Transform Transform;
        public Color4 Color;
        public static Shader Shader;
        protected int VAO, VBO, EBO;
        
        private float[] vertices;
        private readonly float[] indices = [0, 1, 1, 2, 2, 3, 3, 0, 0, 4, 1, 5, 2, 6, 3, 7, 4, 5, 5, 6, 6, 7, 7, 4];
        public AABB(Vector3 A, Vector3 B, Color4 color) {
            Color = color;
            vertices = [
                A.X, A.Y, A.Z,   // 0
                A.X, A.Y, B.Z,   // 1
                B.X, A.Y, B.Z,   // 2
                B.X, A.Y, A.Z,   // 3
                A.X, B.Y, A.Z,   // 4
                A.X, B.Y, B.Z,   // 5
                B.X, B.Y, B.Z,   // 6
                B.X, B.Y, A.Z,   // 7
            ];
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(float), indices, BufferUsageHint.StaticDraw);
            
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
            GL.DrawElements(PrimitiveType.Lines, indices.Length,DrawElementsType.UnsignedInt, indices);
        }
    }
}
