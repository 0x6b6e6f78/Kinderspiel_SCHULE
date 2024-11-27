using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace Kinderspiel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int points = 0;
        private Random random = new Random();
        
        private List<Circle> targets = new List<Circle>();
        private Circle selectedTarget;

        public MainWindow()
        {
            InitializeComponent();
            InitEllipse(red, 50, 50);
            InitEllipse(green, 120, 50);
            InitEllipse(blue, 200, 50);
            InitEllipse(purple, 100, 200);
            InitEllipse(yellow, 200, 200);
            Update();
            StartMovement();
        }

        private void InitEllipse(Ellipse ellipse, double x, double y)
        {
            targets.Add(new Circle(this, ellipse, x, y));
        }

        private async void StartMovement()
         {
             while (true)
            {
                foreach (Circle circle in targets)
                {
                    circle.Tick(targets);
                }
                foreach (Circle circle in targets)
                {
                    circle.Update();
                }

                await Task.Delay(16);
             }
         }

        private void Update()
        {
            UpdatePunktestand();
            chooseNewTarget();
        }

        private void click(object sender, MouseButtonEventArgs e)
        {
            if (selectedTarget == null) return;
            if (!(sender is UIElement))
            {
                return;
            }
            UIElement uiElement = (UIElement) sender;
            if (sender is Ellipse ellipse)
            {
                Circle circle = null;
                foreach (Circle circle1 in targets)
                {
                    if (circle1.GetEllipse() == ellipse)
                    {
                        circle = circle1; break;
                    }
                }
                if (circle == null)
                {
                    return;
                }

                if (circle.Name().Equals(selectedTarget.Name()))
                {
                    points++;
                    Update();
                } else
                {
                    MessageBox.Show("verkackt, arschloch");
                }
            }
        }

        public void chooseNewTarget()
        {
            selectedTarget = targets[random.Next(0, targets.Count)];
            ColorInfo.Text = ColorName;
            ColorInfo.Foreground = selectedTarget.GetEllipse().Fill;
        }

        private void UpdatePunktestand()
        {
            Punktestand.Text = $"Punkte: {points}";
        }

        public string ColorName
        {
            get {
                string text = "";
                if (selectedTarget != null)
                {
                    if (selectedTarget.Name().Equals("red"))
                    {
                        text += "Rot";
                    }
                    else if (selectedTarget.Name().Equals("green"))
                    {
                        text += "Grün";
                    }
                    else if (selectedTarget.Name().Equals("blue"))
                    {
                        text += "Blau";
                    }
                    else if (selectedTarget.Name().Equals("purple"))
                    {
                        text += "Lila";
                    }
                    else if (selectedTarget.Name().Equals("yellow"))
                    {
                        text += "Gelb";
                    }
                }
                return text;
            }
        }
    }
}