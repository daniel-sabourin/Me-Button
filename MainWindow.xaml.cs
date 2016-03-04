using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace Project_0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point previousPosition;
        long previousTime;

        public MainWindow()
        {
            InitializeComponent();

            smileyControl.OnSmileClick += delegate(object sender, EventArgs e)
            {
                MoveTo(innerGrid, CalculateNewLocation(outerCanvas, innerGrid), 500);
            };

            smileyControl.OnFamilarityChange += delegate(object sender, EventArgs e)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    progressBar.Value = smileyControl.FamilarLevel;
                }));
            };
        }

        public double CalculateDistance(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X,2) + Math.Pow(p2.Y - p1.Y,2)); 
        }

        public void MoveTo(Grid target, Point point, double speed)
        {
                double d = CalculateDistance(point, new Point(Canvas.GetLeft(target), Canvas.GetTop(target)));
                //double speed = 500; // pixels / second
                double time = d / speed;

                DoubleAnimation animation1 = new DoubleAnimation(Canvas.GetLeft(target), point.X, TimeSpan.FromSeconds(time));
                DoubleAnimation animation2 = new DoubleAnimation(Canvas.GetTop(target), point.Y, TimeSpan.FromSeconds(time));

                CircleEase ease = new CircleEase();

                animation1.EasingFunction = ease;
                animation2.EasingFunction = ease;

                target.BeginAnimation(Canvas.LeftProperty, animation1);
                target.BeginAnimation(Canvas.TopProperty, animation2);
        }

        public Point CalculateNewLocation(Canvas canvas, Grid control)
        {
            Random rand = new Random();
            int x = rand.Next(0, (int)canvas.ActualWidth - (int)control.ActualWidth);
            int y = rand.Next(0, (int)canvas.ActualHeight - (int)control.ActualHeight);
            return new Point(x, y);
        }

        private void innerGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (previousTime != 0)
            {
                if (e.Timestamp - previousTime > 100)
                {
                    Point currentPosition = e.GetPosition(outerCanvas);
                    double d = CalculateDistance(currentPosition, previousPosition);

                    double speed = d / (e.Timestamp - previousTime) * 1000;

                    if (speed > smileyControl.MaxToleratedSpeed)
                    {
                        MoveTo(innerGrid, CalculateNewLocation(outerCanvas, innerGrid), 500);
                        smileyControl.FamilarLevel -= 1;
                    }

                    previousTime = e.Timestamp;
                    previousPosition = currentPosition;
                }
            }
            else
            {
                previousTime = e.Timestamp;
                previousPosition = e.GetPosition(outerCanvas);
            }
        }
    }
}
