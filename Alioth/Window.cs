using Alioth;
using Alioth.Primitives;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Diagnostics;

public class Window : GameWindow {

    private Shader m_PrimiShader;
    private Shader m_AxisShader;
    private Stopwatch m_Stopwatch = new();
    private Camera m_Camera;
    private double m_Time;
    List<Primitive> m_Items;
    List<AABB> m_AABBs;
    AxisRenderer m_Axis;
    public Window(int width, int height, string title) : base(

        GameWindowSettings.Default,
        new NativeWindowSettings() {
            Size = (width, height),
            Title = title,
            APIVersion = new Version(4, 6)
        }) {
        Console.WriteLine("Vendor : " + GL.GetString(StringName.Vendor));
        Console.WriteLine("Renderer: " + GL.GetString(StringName.Renderer));
        Console.WriteLine("OpenGL Version : " + GL.GetString(StringName.Version));
        Console.WriteLine("GLSL Version : " + GL.GetString(StringName.ShadingLanguageVersion));
        m_Stopwatch.Start();
    }


    protected override void OnUnload() {
        base.OnUnload();
        m_PrimiShader.Dispose();
    }
    protected override void OnLoad() {
        base.OnLoad();
        m_PrimiShader = new Shader("./Assets/Shaders/Primitive.vert", "./Assets/Shaders/Primitive.frag");
        m_AxisShader = new Shader("./Assets/Shaders/Axis.vert", "./Assets/Shaders/Axis.frag");
        AABB.Shader = m_PrimiShader;
        Primitive.Shader = m_PrimiShader;
        AxisRenderer.Shader = m_AxisShader;
        Texture texContainer = new("./Assets/Textures/container.png");
        m_Axis = new();


        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.5f);
        GL.Enable(EnableCap.DepthTest);

        Random random = new();
        m_Items = [];

        for (int i = 0; i < 32; i++) {
            m_Items.Add(
                new Sphere(
                    random.NextSingle()*2,
                    new Transform(new Vector3(random.Next(-10, 10), random.Next(-10, 10), random.Next(-10, 10))),
                    new Color4(random.NextSingle(), random.NextSingle(), random.NextSingle(), 1.0f)
                )
            );
        }
        //m_Items =[
        //    //new Cube(new Transform(), Color4.Yellow, texContainer),
        //    new Sphere(0.5f, new Transform(new Vector3(3,3,3)), Color4.SpringGreen),
        //];
        m_AABBs = [];
        
        foreach (var item in m_Items) {
            item.GetAABBPoints(out Vector3 A, out Vector3 B);
            m_AABBs.Add(new AABB(A, B, Color4.GreenYellow));
        }
        m_AABBs.Sort(new AABBComparator());
    
        for (int i = 0; i < 32; i+=2) {
            m_AABBs.Add(new AABB(m_AABBs[i], m_AABBs[i + 1], Color4.Green));
        }
 
        //for (int i = 32; i < 48; i += 2) {
        //    m_AABBs.Add(new AABB(m_AABBs[i], m_AABBs[i + 1], Color4.Lime));
        //}
        m_Camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
        CursorState = CursorState.Grabbed;
    }
    protected override void OnRenderFrame(FrameEventArgs args) {
        base.OnRenderFrame(args);

        m_Time += 32.0 * args.Time;

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        //m_Axis.Draw(m_Camera);
        //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        foreach (var item in m_Items) {
            item.Draw(m_Camera);
        }
        foreach (var aabb in m_AABBs) {
            aabb.Draw(m_Camera);
        }
        
        //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);


        SwapBuffers();
    }
    protected override void OnResize(ResizeEventArgs e) {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
        GL.Viewport(0, 0, Size.X, Size.Y);
        // We need to update the aspect ratio once the window has been resized.
        m_Camera.AspectRatio = Size.X / (float)Size.Y;
    }
    protected override void OnUpdateFrame(FrameEventArgs e) {
        base.OnUpdateFrame(e);

        //m_Items[0].Transform.rotation.Y += (float)e.Time * 4.0f;

        if (!IsFocused) // Check to see if the window is focused
        {
            return;
        }

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.Escape)) {
            Close();
        }

        const float cameraSpeed = 1.5f;
        const float sensitivity = 0.2f;

        if (input.IsKeyDown(Keys.W)) {
            m_Camera.Position += m_Camera.Front * cameraSpeed * (float)e.Time; // Forward
        }

        if (input.IsKeyDown(Keys.S)) {
            m_Camera.Position -= m_Camera.Front * cameraSpeed * (float)e.Time; // Backwards
        }
        if (input.IsKeyDown(Keys.A)) {
            m_Camera.Position -= m_Camera.Right * cameraSpeed * (float)e.Time; // Left
        }
        if (input.IsKeyDown(Keys.D)) {
            m_Camera.Position += m_Camera.Right * cameraSpeed * (float)e.Time; // Right
        }
        if (input.IsKeyDown(Keys.Space)) {
            m_Camera.Position += m_Camera.Up * cameraSpeed * (float)e.Time; // Up
        }
        if (input.IsKeyDown(Keys.LeftShift)) {
            m_Camera.Position -= m_Camera.Up * cameraSpeed * (float)e.Time; // Down
        }

        // Get the mouse state
        var mouse = MouseState;

        if (m_FirstMove) // This bool variable is initially set to true.
        {
            m_LastPos = new Vector2(mouse.X, mouse.Y);
            m_FirstMove = false;
        }
        else {
            // Calculate the offset of the mouse position
            var deltaX = mouse.X - m_LastPos.X;
            var deltaY = mouse.Y - m_LastPos.Y;
            m_LastPos = new Vector2(mouse.X, mouse.Y);

            // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
            m_Camera.Yaw += deltaX * sensitivity;
            m_Camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
        }
    }
    private bool m_FirstMove = true;
    private Vector2 m_LastPos;
    protected override void OnMouseWheel(MouseWheelEventArgs e) {
        base.OnMouseWheel(e);

        m_Camera.Fov -= e.OffsetY;
    }

}
public class AABBComparator : IComparer<AABB> {
    Random random = new();
    public int Compare(AABB? x, AABB? y) {
        int axis = random.Next(0, 3);
        if (axis == 0) {
            if (x.A.X > y.A.X) return 1;
            else return -1;
        }
        else if(axis==1) {
            if (x.A.Y > y.A.Y) return 1;
            else return -1;
        }
        else {
            if (x.A.Z > y.A.Z) return 1;
            else return -1;
        }
    }
}
