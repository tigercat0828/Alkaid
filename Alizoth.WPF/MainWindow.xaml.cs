using OpenTK.Mathematics;
using OpenTK.Wpf;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using OpenTK.Windowing.Common;
namespace Alizoth.WPF {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow {

        private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();
        public MainWindow() {                                                                       
            InitializeComponent();

            var settings = new GLWpfControlSettings {
                MajorVersion = 4,
                MinorVersion = 6,
                

            };
            OpenTkControl.Start(settings);
        }
        private void OpenTkControl_OnRender(TimeSpan delta) {
            var hue = (float)_stopwatch.Elapsed.TotalSeconds * 0.15f % 1;
            float alpha =1.0f;
            var c = Color4.FromHsv(new Vector4(alpha * hue, alpha * 0.75f, alpha * 0.75f, alpha));
            
            GL.ClearColor(c);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            GL.Finish(); //  add this line to work
        }
    }
}