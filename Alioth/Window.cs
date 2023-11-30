﻿using Alioth;
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
        Texture texContainer = new("./Assets/Textures/container.png");
        m_Axis = new(m_AxisShader);


        GL.ClearColor(0.0f, 0.0f, 0.0f, 0.5f);
        GL.Enable(EnableCap.DepthTest);

        m_Items =[
            new Cube(new Transform(), Color4.Yellow, m_PrimiShader, texContainer),
            new Sphere(0.5f, new Transform(new Vector3(3,3,3)), Color4.SpringGreen , m_PrimiShader),
        ];

        m_Camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);
        CursorState = CursorState.Grabbed;
    }
    protected override void OnRenderFrame(FrameEventArgs args) {
        base.OnRenderFrame(args);

        m_Time += 32.0 * args.Time;

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        m_Axis.Render(m_Camera);
        //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
        foreach (var item in m_Items) {
            item.Draw(m_Camera);
        }
        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);


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

        m_Items[0].Transform.rotation.Y += (float)e.Time * 4.0f;

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