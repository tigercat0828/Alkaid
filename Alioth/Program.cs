using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using (Game game = new(800, 600, "Alioth")) {
    game.Run();
}
public class Shader {
    int Handle;
    public Shader(string vertexShader, string fragmentShader)
    {
        string vertSource = File.ReadAllText(vertexShader);

    }
}
public class Game : GameWindow {
    public Game(int width, int height, string title) : base(
        GameWindowSettings.Default, 
        new NativeWindowSettings() { 
            Size = (width, height), 
            Title=title 
        } ){
    }
    protected override void OnUpdateFrame(FrameEventArgs args) {
        base.OnUpdateFrame(args);
        if(KeyboardState.IsKeyDown(Keys.Escape)) {
            Close();
        }
    }
    protected override void OnRenderFrame(FrameEventArgs args) {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        SwapBuffers();
    }
    protected override void OnLoad() {
        base.OnLoad();
        GL.ClearColor(0.2f, 0.3f, 0.3f, 0.5f);
    }
    protected override void OnResize(ResizeEventArgs e) {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}
