using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Kinderspiel
{
    public class Circle
    {
        public static Dictionary<string, string> hexColors = new Dictionary<string, string>()
        {
            { "Rot",    "#ff0000" },
            { "Blau",   "#2200dd" },
            { "Grün",   "#00aa22" },
            { "Gelb",   "#ffff00" },
            { "Lila",   "#81007f" }
        };

        private string _name;
        private Ellipse _ellipse;
        private double _x;
        private double _y;
        private double _radius;
        private double _speed;
        private double _angle;

        private MainWindow window;

        private int HeaderOffset = 55;

        private Boolean updated = false;

        public Circle(MainWindow window, Ellipse _ellipse, double x, double y)
        {
            this.window = window;
            this._ellipse = _ellipse;

            Random random = new Random();
            this._angle = random.Next(0, 360);
            this._speed = random.Next(1, 100) / 25d;
            this._radius = random.Next(30, 40);
            this._x = x;
            this._y = y;
        }

        public void Tick(List<Circle> circles)
        {
            double angleRadians = _angle * (Math.PI / 180d);
            double dx = _speed * Math.Cos(angleRadians) + _x;
            double dy = _speed * Math.Sin(angleRadians) + _y;

            if (dx < _radius || dx > GetScreenWidth() - _radius)
            {
                this.Angle = (-(_angle + 180)) % 360;
            }
            if (dy < _radius || dy > GetScreenHeight() - _radius)
            {
                this.Angle = (-_angle) % 360;
            }
            setLocation(dx, dy);
            CheckCollisions(circles);
        }

        private void CheckCollisions(List<Circle> circles)
        {
            if (updated)
            {
                return;
            }
            foreach (Circle circle in circles)
            {
                if (circle == this || circle.updated)
                {
                    continue;
                }
                if (this.Intersects(circle))
                {
                    double thetaGrad = Math.Atan2(this._y - circle._y, this._x - circle._x) * 180 / Math.PI;
                    this.Angle = (thetaGrad) % 360;
                    circle.Angle = (thetaGrad + 180) % 360;

                    string name = this.Name;
                    this.Name = circle.Name;
                    circle.Name = name;

                    double speed = this._speed;
                    this._speed = circle._speed;
                    circle._speed = speed;

                    updated = true;
                    break;
                }
            }
        }

        public void Update()
        {
            Thickness thickness = new Thickness(_x - _radius, _y - _radius + HeaderOffset, 0, 0);
            _ellipse.Margin = thickness;
            _ellipse.Width = _radius * 2;
            _ellipse.Height = _radius * 2;
            updated = false;
        }

        private void setLocation(double x, double y)
        {
            _x = Math.Max(Math.Min(GetScreenWidth() - _radius, x), _radius);
            _y = Math.Max(Math.Min(GetScreenHeight() - _radius, y), _radius);
        }

        private double GetScreenWidth()
        {
            if (this.window == null) return 0;
            return Math.Max(this.window.Width - 17, 0);
        }
        private double GetScreenHeight()
        {
            if (this.window == null) return 0;
            return Math.Max(this.window.Height - 40, 0) - HeaderOffset;
        }
        private Boolean Intersects(Circle circle)
        {
            return Distance(circle) < (this._radius + circle._radius);
        }

        private double Distance(Circle circle)
        {
            return Math.Sqrt(Math.Pow(this._x - circle._x, 2) + Math.Pow(this._y - circle._y, 2));
        }

        private double Distance(Circle circle, double x, double y)
        {
            return Math.Sqrt(Math.Pow(x - circle._x, 2) + Math.Pow(y - circle._y, 2));
        }

        public Ellipse GetEllipse()
        {
            return _ellipse;
        }

        public string Name {
            get
            {
                return _name;
            }
            set
            {
                this._name = value;
                this._ellipse.Fill = (SolidColorBrush)new BrushConverter().ConvertFrom(hexColors[value]);
            }
        }

        private double Angle
        {
            set
            {
                int cap = 20;
                Random random = new Random();
                double a = (value + (random.Next(0, cap) - (cap / 2))) % 360;
                this._angle = a;
            }
        }
    }
}