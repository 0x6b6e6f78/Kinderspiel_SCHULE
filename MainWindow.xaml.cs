using System.Printing;
using System.Text;
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
        
        private List<Ellipse> targets = new List<Ellipse>();
        private Dictionary<Ellipse, Tuple<double, double>> directions = new Dictionary<Ellipse, Tuple<double, double>>();
        private Ellipse selectedTarget;

        public MainWindow()
        {
            InitializeComponent();
            init(red);
            init(green);
            init(blue);
            init(purple);
            init(orange);
            Update();
            StartBackgroundMovement();
        }

        private void init(Ellipse ellipse)
        {
            targets.Add(ellipse);
            directions.Add(ellipse, new Tuple<double, double>(1,1));
        }

        private Thickness MoveTarget(Ellipse ellipse)
        {
            int HeadLine = 40;
            double x = random.Next(0, (int)(this.Width - ellipse.Width - 35));
            double y = random.Next(HeadLine, (int)(this.Height - ellipse.Height - 35));
            return new Thickness(x,y,0,0);
        }

        private void MoveTargets_OLD()
        {
            List<Thickness> currentPositions = new List<Thickness>();
            foreach (Ellipse ellipse in targets)
            {
                Thickness thickness = MoveTarget(ellipse);
                Boolean Overlaping = true;
                while (Overlaping)
                {
                    Overlaping = false;
                    foreach (Thickness t in currentPositions)
                    {
                        if (IsOverlaping(thickness, t, 45))
                        {
                            Overlaping = true;
                        }
                    }
                    if (Overlaping)
                    {
                        thickness.Top = (thickness.Top + 10) % (this.Height - 80);
                        thickness.Left = (thickness.Left + 10) % (this.Width - 80);
                    }
                }
                ellipse.Margin = thickness;
                currentPositions.Add(thickness);
            }
        }
        private void MoveTargets()
        {
            foreach (Ellipse ellipse in targets)
            {
                ellipse.Margin = MoveTarget(ellipse);
                double n = (points + 1) * .3;
                directions[ellipse] = (new Tuple<double, double>(random.Next(3, (int)(100 * n)) / 100d, random.Next(3, (int)(100 * n)) / 100d));
            }
        }

        private async void StartBackgroundMovement()
        {
            while (true)
            {
                foreach (Ellipse ellipse in targets)
                {

                    Thickness t = ellipse.Margin;
                    Tuple<double, double> direction = directions[ellipse];
                    double dx = direction.Item1;
                    double dy = direction.Item2;
                    double xPosition = t.Left;
                    double yPosition = t.Top;

                    xPosition += dx * 2;
                    yPosition += dy * 2;

                    if (xPosition >= this.Width - 80 || xPosition <= 0)
                    {
                        dx *= -1;
                    }
                    if (yPosition >= this.Height - 80 || yPosition <= 45)
                    {
                        dy *= -1;
                    }

                    directions[ellipse] = new Tuple<double, double>(dx, dy);
                    t.Left = xPosition;
                    t.Top = yPosition;
                    ellipse.Margin = t;
                }

                await Task.Delay(16);
            }
        }

        private Boolean IsOverlaping(Thickness a, Thickness b, int size)
        {
            return a.Top - b.Top < size && a.Left - b.Left < size;
        }

        private void Update()
        {
            MoveTargets();
            UpdatePunktestand();
            chooseNewTarget();
        }


        private void click(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is UIElement))
            {
                return;
            }
            UIElement uiElement = (UIElement) sender;
            if (sender is Ellipse ellipse)
            {
                if (ellipse == selectedTarget)
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
        }

        private void UpdatePunktestand()
        {
            Punktestand.Text = $"Punkte: {points}";
        }

        public string ColorName
        {
            get {
                string text = "Wo ist die Farbe: ";
                if (selectedTarget == null)
                {
                    return "";
                }
                if (selectedTarget.Name.Equals("red"))
                {
                    text += "Rot";
                }
                else if(selectedTarget.Name.Equals("green"))
                {
                    text += "Grün";
                }
                else if(selectedTarget.Name.Equals("blue"))
                {
                    text += "Blau";
                }
                else if(selectedTarget.Name.Equals("purple"))
                {
                    text += "Lila";
                } else if (selectedTarget.Name.Equals("orange"))
                {
                    text += "Orange";
                }
                return text;
            }
        }
    }
}