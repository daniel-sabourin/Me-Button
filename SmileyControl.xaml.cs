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
using System.Timers;

namespace Project_0
{
    /// <summary>
    /// Interaction logic for SmileyControl.xaml
    /// </summary>
    public partial class SmileyControl : UserControl
    {
        public event EventHandler OnSmileClick;
        public event EventHandler OnFamilarityChange;

        /// <summary>
        /// Time (in seconds) that an Emotion will spend transitioning
        /// </summary>
        public double TransitionTime = 1;

        private double _familarLevel = 5;
        public double FamilarLevel
        {
            get { return _familarLevel; }
            set
            {
                // 0,1,2 = Grumpy
                // 3,4,5 = Idle
                // 6,7,8 = SmallSmile
                // 9,10,11 = Medium Smile
                // 12,13,14 = LargeSmile

                // UGLY - Please Change. It hurts to look at it
                if (value < 0)
                {
                    CurrentEmotion = Emotion.Grumpy;
                    _familarLevel = 0;
                }
                else if (value >= 0 && value < 3)
                {
                    CurrentEmotion = Emotion.Grumpy;
                    _familarLevel = value;
                }
                else if (value >= 3 && value < 6)
                {
                    CurrentEmotion = Emotion.Idle;
                    _familarLevel = value;
                }
                else if (value >= 6 && value < 9)
                {
                    CurrentEmotion = Emotion.SmallSmile;
                    _familarLevel = value;
                }
                else if (value >= 9 && value < 12)
                {
                    CurrentEmotion = Emotion.MediumSmile;
                    _familarLevel = value;
                }
                else if (value >= 12 && value < 15)
                {
                    CurrentEmotion = Emotion.LargeSmile;
                    _familarLevel = value;
                }
                else if (value >= 15)
                {
                    CurrentEmotion = Emotion.LargeSmile;
                    _familarLevel = 15;
                }

                //Console.WriteLine(_familarLevel);

                if (OnFamilarityChange != null)
                    OnFamilarityChange(null, null);
                  
            }
        }

        public enum Emotion { Grumpy, Idle, SmallSmile, MediumSmile, LargeSmile };

        private Dictionary<Emotion, Image> lookupDictionary;
        private Dictionary<Emotion, double> speedLookup;

        private double _MaxToleratedSpeed;
        public double MaxToleratedSpeed
        {
            get { return _MaxToleratedSpeed; }
        }

        private Emotion _currentEmotion = Emotion.Idle;
        public Emotion CurrentEmotion
        {
            get { return _currentEmotion; }
            set
            {
                AnimateTo(value);

                _MaxToleratedSpeed = speedLookup[value];

                _currentEmotion = value;
            }
        }

        public SmileyControl()
        {
            InitializeComponent();

            // Creates the Emotion/Image dictionary
            lookupDictionary = new Dictionary<Emotion, Image>() { { Emotion.SmallSmile, smile1 }, { Emotion.MediumSmile, smile2 }, { Emotion.LargeSmile, smile3 }, { Emotion.Idle, idle }, { Emotion.Grumpy, grumpy } };

            // Set values for MaxSpeed for each Emotion level
            speedLookup = new Dictionary<Emotion, double>() { { Emotion.Grumpy, 150 }, { Emotion.Idle, 150 }, { Emotion.SmallSmile, 250 }, { Emotion.MediumSmile, 500 }, { Emotion.LargeSmile, 1000 } };

            // refreshRate in seconds. Every XX seconds it will refresh
            double refreshRate = 1;

            // familarity lost per minute
            double degradeRate = 1;

            double amountPerCycle = (degradeRate / 60.0) * refreshRate;

            Timer degradeTimer = new Timer(refreshRate * 1000);
            degradeTimer.AutoReset = true;

            degradeTimer.Elapsed += delegate(object source, ElapsedEventArgs e)
            {
                FamilarLevel = FamilarLevel - amountPerCycle;
                //Console.WriteLine(FamilarLevel);
            };

            degradeTimer.Start();
        }

        private void AnimateTo(Emotion emotion)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (CurrentEmotion != emotion)
                {
                    Image currentEmotion = lookupDictionary[CurrentEmotion];
                    Image newEmotion = lookupDictionary[emotion];

                    DoubleAnimation fadeIn = new DoubleAnimation(newEmotion.Opacity, 1, TimeSpan.FromSeconds(TransitionTime));
                    DoubleAnimation fadeOut = new DoubleAnimation(currentEmotion.Opacity, 0, TimeSpan.FromSeconds(TransitionTime));

                    Storyboard sb = new Storyboard();
                    Storyboard.SetTarget(fadeOut, currentEmotion);
                    Storyboard.SetTargetProperty(fadeOut, new PropertyPath("(Image.Opacity)"));

                    Storyboard.SetTarget(fadeIn, newEmotion);
                    Storyboard.SetTargetProperty(fadeIn, new PropertyPath("(Image.Opacity)"));

                    sb.Children.Add(fadeOut);
                    sb.Children.Add(fadeIn);

                    sb.Begin();
                }

            }));
        }

        public void DisplayInterests()
        {

            Storyboard sb = new Storyboard();

            if (CurrentEmotion >= Emotion.Grumpy)
            {
                DoubleAnimation animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
                animation.AutoReverse = true;

                Storyboard.SetTarget(animation, controllerImage);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Image.Opacity)"));
                sb.Children.Add(animation);
            } if (CurrentEmotion >= Emotion.Idle)
            {
                DoubleAnimation animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
                animation.AutoReverse = true;

                Storyboard.SetTarget(animation, soccerImage);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Image.Opacity)"));
                sb.Children.Add(animation);
            } if (CurrentEmotion >= Emotion.SmallSmile)
            {
                DoubleAnimation animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
                animation.AutoReverse = true;

                Storyboard.SetTarget(animation, hardDriveImage);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Image.Opacity)"));
                sb.Children.Add(animation);
            } if (CurrentEmotion >= Emotion.MediumSmile)
            {
                DoubleAnimation animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
                animation.AutoReverse = true;

                Storyboard.SetTarget(animation, steamImage);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Image.Opacity)"));
                sb.Children.Add(animation);
            } if (CurrentEmotion >= Emotion.LargeSmile)
            {
                DoubleAnimation animation = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
                animation.AutoReverse = true;

                Storyboard.SetTarget(animation, minecraftImage);
                Storyboard.SetTargetProperty(animation, new PropertyPath("(Image.Opacity)"));
                sb.Children.Add(animation);

                DoubleAnimation animation2 = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(2));
                animation2.AutoReverse = true;

                Storyboard.SetTarget(animation2, leagueImage);
                Storyboard.SetTargetProperty(animation2, new PropertyPath("(Image.Opacity)"));
                sb.Children.Add(animation2);
            }





            sb.Begin();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FamilarLevel += 3;
            DisplayInterests();

            if (OnSmileClick != null)
                OnSmileClick(sender, e);
        }
    }
}
