using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WarmPack.Threading
{
    //ayuda para generar esta clase con el tema de los dispatchers aqui https://stackoverflow.com/questions/1111369/how-do-i-create-and-show-wpf-windows-on-separate-threads
    //ayuda con las animaciones tomadas de aqui https://social.msdn.microsoft.com/Forums/vstudio/en-US/0875ebf8-bb77-45ea-a929-d40743a3bf03/spinning-progress-control-in-wpf?forum=wpf
    public class Splash
    {
        private static Window _splashWiew = null;
        private static bool _splasVisible = false;
        private static string _splashMessage = "Espere un momento por favor ...";

        private static Window View
        {
            get
            {
                if (_splashWiew == null)
                    SplasWindowBuilder();

                return _splashWiew;
            }
        }

        private static Canvas DrawCanvas()
        {
            Canvas canvas = new Canvas();
            canvas.Width = 100;
            canvas.Height = 100;
            canvas.RenderTransformOrigin = new Point(0.5, 0.5);

            for (int i = 0; i < 12; i++)
            {
                Line line = new Line()
                {
                    X1 = 50,
                    X2 = 50,
                    Y1 = 0,
                    Y2 = 20,
                    StrokeThickness = 5,
                    Stroke = Brushes.Gray,
                    Width = 100,
                    Height = 100
                };
                line.VerticalAlignment = VerticalAlignment.Center;
                line.HorizontalAlignment = HorizontalAlignment.Center;
                line.RenderTransformOrigin = new Point(.5, .5);
                line.RenderTransform = new RotateTransform(i * 30);
                line.Opacity = (double)i / 12;

                canvas.Children.Add(line);
            }

            return canvas;
        }

        private static void SplasWindowBuilder()
        {
            _splashWiew = new Window();
            _splashWiew.Width = 300;
            _splashWiew.Height = 250;
            _splashWiew.Topmost = true;
            _splashWiew.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            _splashWiew.ResizeMode = ResizeMode.NoResize;
            _splashWiew.WindowStyle = WindowStyle.None;
            _splashWiew.BorderBrush = Brushes.Black;
            _splashWiew.BorderThickness = new Thickness(2);


            Grid grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition());

            var canvas = DrawCanvas();

            var spin = new RotateTransform();
            spin.Angle = 0;

            canvas.RenderTransform = spin;

            DoubleAnimation a = new DoubleAnimation();
            a.From = 0;
            a.To = 360;
            a.RepeatBehavior = RepeatBehavior.Forever;
            a.SpeedRatio = 0.5;

            spin.BeginAnimation(RotateTransform.AngleProperty, a);

            //Grid.SetRow(canvas, 0);            

            var rowText = new RowDefinition();
            rowText.Height = new GridLength(100);
            grid.RowDefinitions.Add(rowText);


            TextBlock messageText = new TextBlock()
            {
                Text = _splashMessage,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 18,
                FontFamily = new System.Windows.Media.FontFamily("Arial")
            };


            Grid.SetRow(messageText, 1);

            grid.Children.Add(canvas);
            grid.Children.Add(messageText);

            _splashWiew.Content = grid;
        }

        public static void Show(string message = null)
        {
            _splasVisible = true;

            if(message == null)
            {
                _splashMessage = "Espere un momento por favor ...";
            }
            else
            {
                _splashMessage = message;
            }


            Thread newWindowThread = new Thread(new ThreadStart(ThreadStartingPoint));
            newWindowThread.SetApartmentState(ApartmentState.STA);
            newWindowThread.IsBackground = true;
            newWindowThread.Start();
        }

        private static void ThreadStartingPoint()
        {
            try
            {
                SplasWindowBuilder();

                View.Show();

                Dispatcher.Run();
            }
            catch (ThreadAbortException)
            {
                _splashWiew.Close();
                Dispatcher.CurrentDispatcher.InvokeShutdown();
            }

        }

        public static void Hide()
        {
            try
            {
                if (_splasVisible)
                {
                    _splasVisible = false;

                    if (_splashWiew.Dispatcher.CheckAccess())
                        _splashWiew.Close();
                    else
                        _splashWiew.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(_splashWiew.Close));


                    _splashWiew = null;
                }
            }
            catch
            {
                while(_splashWiew == null)
                {
                    Thread.Sleep(100);
                }

                _splashWiew.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(_splashWiew.Close));                
            }
        }

        public static Task.TaskDoMonitor RunTask(Action action, string message = null)
        {
            return Task.RunTask(action, true, message);
        }
    }
}