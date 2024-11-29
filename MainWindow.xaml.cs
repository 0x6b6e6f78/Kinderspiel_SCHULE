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
        private string selectedTarget;

        public MainWindow()
        {
            InitializeComponent();
            int i = 0;

            InitEllipse(circle1, i++);
            InitEllipse(circle2, i++);
            InitEllipse(circle3, i++);
            InitEllipse(circle4, i++);
            InitEllipse(circle5, i++);
            Update();
            StartMovement();
        }

        private Circle InitEllipse(Ellipse ellipse, int i)
        {
            List<string> colors = new List<string>(Circle.hexColors.Keys);

            Random random = new Random();
            Circle circle = new Circle(this, ellipse, (Width / (colors.Count + 1)) * i, random.Next(80, 200));
            circle.Name = colors[i % colors.Count];

            targets.Add(circle);
            return circle;
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

                if (circle.Name.Equals(selectedTarget))
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
            selectedTarget = targets[random.Next(0, targets.Count)].Name;
            ColorInfo.Text = selectedTarget;

            foreach (Circle circle in targets)
            {
                if (circle.Name == selectedTarget)
                {
                    ColorInfo.Foreground = circle.GetEllipse().Fill;
                    break;
                }
            }
        }

        private void UpdatePunktestand()
        {
            Punktestand.Text = $"Punkte: {points}";
        }
    }
}