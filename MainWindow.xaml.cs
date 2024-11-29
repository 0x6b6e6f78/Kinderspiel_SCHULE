using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

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

            for (int i = 0; i < 5; i++)
            {
                InitEllipse(i);
            }

            Update();
            StartMovement();
        }

        private Circle InitEllipse(int i)
        {
            List<string> colors = new List<string>(Circle.hexColors.Keys);

            Random random = new Random();
            Circle circle = new Circle(this, (Width / (colors.Count + 1)) * (i % colors.Count), random.Next(80, 200));
            circle.Name = colors[i % colors.Count];

            targets.Add(circle);
            MyGrid.Children.Add(circle.GetEllipse());
            foreach (Ellipse e in circle.GetEllipses())
            {
                MyGrid.Children.Add(e);
            }
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

        public void click(object sender, MouseButtonEventArgs e)
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