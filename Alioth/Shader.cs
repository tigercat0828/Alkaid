#define DEBUG
public class Shader : IDisposable {
    public readonly int Handle;
    public Shader(string vertexShader, string fragmentShader) {
        // Vertex Shader
        string vertSource = File.ReadAllText(vertexShader);
        int vShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vShader, vertSource);
        GL.CompileShader(vShader);
        GL.GetShader(vShader, ShaderParameter.CompileStatus, out int vsuccess);
        if (vsuccess == 0) {
            string infoLog = GL.GetShaderInfoLog(vShader);
            Console.WriteLine(infoLog);
        }

        // Fragment Shader
        string fragSource = File.ReadAllText(fragmentShader);
        int fShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fShader, fragSource);
        GL.CompileShader(fShader);
        GL.GetShader(fShader, ShaderParameter.CompileStatus, out int fsuccess);
        if (fsuccess == 0) {
            string infoLog = GL.GetShaderInfoLog(vShader);
            Console.WriteLine(infoLog);
        }

        Handle = GL.CreateProgram();

        GL.AttachShader(Handle, vShader);
        GL.AttachShader(Handle, fShader);

        GL.LinkProgram(Handle);

        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int success);
        if (success == 0) {
            string infoLog = GL.GetProgramInfoLog(Handle);
            Console.WriteLine(infoLog);
        }
        GL.DetachShader(Handle, vShader);
        GL.DetachShader(Handle, fShader);
        GL.DeleteShader(vShader);
        GL.DeleteShader(fShader);
    }
    public void Use() {
        GL.UseProgram(Handle);
    }
    private bool disposedValue = false;

    public void SetFloat(string pname, float value) {
        int location = GL.GetUniformLocation(Handle, pname);
#if DEBUG
        TipUniformError(location, pname);
#endif
        GL.Uniform1(location, value);
    }
    public void SetVector3(string pname, Vector3 value) {
        int location = GL.GetUniformLocation(Handle, pname);
#if DEBUG
        TipUniformError(location, pname);
#endif
        GL.Uniform3(location, value);
    }
    public void SetVector4(string pname, Color4 value) {
        int location = GL.GetUniformLocation(Handle, pname);
#if DEBUG
        TipUniformError(location, pname);
#endif
        GL.Uniform4(location, value);
    }
    public void SetVector4(string pname, Vector4 value) {
        int location = GL.GetUniformLocation(Handle, pname);
#if DEBUG
        TipUniformError(location, pname);
#endif
        GL.Uniform4(location, value);
    }
    public void SetInt(string pname, int value) {
        int location = GL.GetUniformLocation(Handle, pname);
#if DEBUG
        TipUniformError(location, pname);
#endif
        GL.Uniform1(location, value);
    }
    public void SetMatrix4(string pname, Matrix4 matrix) {
        int location = GL.GetUniformLocation(Handle, pname);
#if DEBUG
        TipUniformError(location, pname);
#endif
        GL.UniformMatrix4(location, false, ref matrix);
    }
    private void TipUniformError(int location, string pname) {
        if (location < 0) {
            Console.WriteLine($"Shader ({Handle}) have no uniform \"{pname}\".");
        }
    }
    public int GetAttribLocation(string attribName) {
        return GL.GetAttribLocation(Handle, attribName);
    }

    protected virtual void Dispose(bool disposing) {
        if (!disposedValue) {
            GL.DeleteProgram(Handle);
            disposedValue = true;
        }
    }

    ~Shader() {
        if (!disposedValue) {
            Console.WriteLine("GPU resource leak! Did you forget to call Dispose()?");
        }
    }
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
