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
            init(red);
            init(green);
            init(blue);
            init(purple);
            init(yellow);
            Update();
            StartMovement();
        }

        private void init(Ellipse ellipse)
        {
            targets.Add(new Circle(this, ellipse));
        }

        private async void StartMovement()
         {
             while (true)
             {
                foreach (Circle circle in targets)
                {
                    circle.Tick(targets);
                }

                 await Task.Delay(16);
             }
         }

        private void Update()
        {
            //MoveTargets();
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
                if (ellipse == selectedTarget.GetEllipse())
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
                    if (selectedTarget.GetEllipse().Name.Equals("red"))
                    {
                        text += "Rot";
                    }
                    else if (selectedTarget.GetEllipse().Name.Equals("green"))
                    {
                        text += "Grün";
                    }
                    else if (selectedTarget.GetEllipse().Name.Equals("blue"))
                    {
                        text += "Blau";
                    }
                    else if (selectedTarget.GetEllipse().Name.Equals("purple"))
                    {
                        text += "Lila";
                    }
                    else if (selectedTarget.GetEllipse().Name.Equals("yellow"))
                    {
                        text += "Gelb";
                    }
                }
                return text;
            }
        }
    }
}