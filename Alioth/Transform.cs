namespace Alioth {
    public struct Transform(Vector3 position, Vector3 rotation, Vector3 scale) {
        
        public Vector3 position = position;
        public Vector3 rotation = rotation;
        public Vector3 scale = scale;
        
        public Transform() : this(Vector3.Zero, Vector3.Zero, Vector3.One) {
        }
        public Transform(Vector3 position) : this(position, Vector3.Zero, Vector3.One) {
        }
    }
}
