using System.Runtime.InteropServices;

namespace Alioth.Primitives {
    public class Primitive : IDisposable {
        public Transform Transform;
        public Color4 Color;
        public static Shader Shader;

        protected int VAO, VBO, EBO;

        public Primitive(Transform transform, Color4 color) {
            Transform = transform; 
            Color = color;
        }
        public virtual void GetAABBPoints(out Vector3 A, out Vector3 B) {
            A = new();
            B = new();
        }
        public void Dispose() {
            GL.DeleteBuffer(VBO);
            GL.DeleteBuffer(EBO);
            GL.DeleteVertexArray(VAO);
        }
        public virtual void Draw(Camera camera) {
            GL.BindVertexArray(VAO);
            Shader.Use();
            Shader.SetVector4("uColor", Color);
            var model = Matrix4.Identity
                * Matrix4.CreateScale(Transform.scale)
                * Matrix4.CreateRotationY(Transform.rotation.Y) // yaw
                * Matrix4.CreateRotationX(Transform.rotation.X) // pitch
                * Matrix4.CreateRotationZ(Transform.rotation.Z) // roll
                * Matrix4.CreateTranslation(Transform.position);

            Shader.SetMatrix4("uModelMat", model);
            Shader.SetMatrix4("uViewMat", camera.GetViewMatrix());
            Shader.SetMatrix4("uProjectionMat", camera.GetProjectionMatrix());
        }
    }
}
