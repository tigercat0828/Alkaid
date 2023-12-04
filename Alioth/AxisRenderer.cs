namespace Alioth {
    internal class AxisRenderer {
        private int VAO, VBO;
        public static Shader Shader;
        public AxisRenderer() {

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            var vertexLocation = Shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var colorLoocation = Shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLoocation);
            GL.VertexAttribPointer(colorLoocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));

        }
        public void Draw(Camera camera) {
            GL.BindVertexArray(VAO);
            Shader.Use();
            Shader.SetMatrix4("uModelMat", Matrix4.Identity);
            Shader.SetMatrix4("uViewMat", camera.GetViewMatrix());
            Shader.SetMatrix4("uProjectionMat", camera.GetProjectionMatrix());
            
            GL.DrawArrays(PrimitiveType.Lines, 0, 6);
            
        }
        private readonly float[] vertices = {
            10, 0 ,0, 1, 0, 0,
            0, 0 ,0, 1, 0, 0,
            0, 10, 0, 0, 1, 0,
            0, 0, 0, 0, 1, 0,
            0, 0, 10, 0, 0, 1,
            0, 0, 0, 0, 0, 1,
        };
    }
}
