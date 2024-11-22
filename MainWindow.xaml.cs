using System.Printing;
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
using static System.Net.Mime.MediaTypeNames;

namespace Kinderspiel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int punkte = 0;
        private Random random = new Random();
        
        private List<Ellipse> images = new List<Ellipse>();
        private Ellipse selectedTarget;


        public MainWindow()
        {
            InitializeComponent();
            images.Add(red);
            images.Add(green);
            images.Add(blue);
            images.Add(purple);
            images.Add(orange);
            Update();
        }

        // Methode zum Bewegen des Bildes
        private Thickness MoveTarget(Ellipse ellipse)
        {
            int HeadLine = 40;
            double x = random.Next(0, (int)(this.Width - ellipse.Width - 35));
            double y = random.Next(HeadLine, (int)(this.Height - ellipse.Height - 35));
            return new Thickness(x,y,0,0);
        }

        private void MoveTargets()
        {
            List<Thickness> currentPositions = new List<Thickness>();
            foreach (Ellipse ellipse in images)
            {
                Thickness thickness = MoveTarget(ellipse);
                Boolean Overlaping = true;
                while (Overlaping)
                {
                    Overlaping = false;
                    foreach (Thickness t in currentPositions)
                    {
                        if (IsOverlaping(thickness, t, 40))
                        {
                            Overlaping = true;
                        }
                    }
                    if (Overlaping)
                    {
                        thickness.Top = (thickness.Top + 10) % (this.Height - 85);
                        thickness.Left = (thickness.Left + 10) % (this.Width - 85);
                    }
                }
                ellipse.Margin = thickness;
                currentPositions.Add(thickness);
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


        // Event, wenn das Bild angeklickt wird
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
                    punkte++;
                    Update();
                } else
                {
                    MessageBox.Show("verkackt, arschloch");
                }
            }
        }

        public void chooseNewTarget()
        {
            selectedTarget = images[random.Next(0, images.Count)];
            ColorInfo.Text = ColorName;
        }

        private void UpdatePunktestand()
        {
            Punktestand.Text = $"Punkte: {punkte}";
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