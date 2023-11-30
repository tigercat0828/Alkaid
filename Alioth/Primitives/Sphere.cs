namespace Alioth.Primitives {
    internal class Sphere : Primitive {
        float[] vertices;
        uint[] indices;
        public Sphere(float radius, Transform transform, Color4 color, Shader shader) : base(transform, color, shader) {
            GenerateSphere(radius, 10, 10, out List<float> vertexList, out List<uint> indexList);
            vertices = vertexList.ToArray();
            indices = indexList.ToArray();
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            EBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(float), indices, BufferUsageHint.StaticDraw);
          

            var vertexLocation = shader.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        }
        public override void Draw(Camera camera) {
            base.Draw(camera);
            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);
        }
        public static void GenerateSphere(float radius, int numLatitudeLines, int numLongitudeLines, out List<float> vertices, out List<uint> indices) {
            vertices = new List<float>();
            indices = new List<uint>();
            for (int lat = 0; lat <= numLatitudeLines; lat++) {
                float theta = lat * MathF.PI / numLatitudeLines;
                float sinTheta = (float)Math.Sin(theta);
                float cosTheta = (float)Math.Cos(theta);

                for (int lon = 0; lon <= numLongitudeLines; lon++) {
                    float phi = lon * 2 * MathF.PI / numLongitudeLines;
                    float sinPhi = (float)Math.Sin(phi);
                    float cosPhi = (float)Math.Cos(phi);

                    float x = cosPhi * sinTheta;
                    float y = cosTheta;
                    float z = sinPhi * sinTheta;

                    vertices.Add(radius * x);
                    vertices.Add(radius * y);
                    vertices.Add(radius * z);
                }
            }

            for (int lat = 0; lat < numLatitudeLines; lat++) {
                for (int lon = 0; lon < numLongitudeLines; lon++) {
                    uint first = (uint)((lat * (numLongitudeLines + 1)) + lon);
                    uint second = (uint)(first + numLongitudeLines + 1);

                    indices.Add(first);
                    indices.Add(second);
                    indices.Add(first + 1);

                    indices.Add(second);
                    indices.Add(second + 1);
                    indices.Add(first + 1);
                }
            }
        }


    }
}
