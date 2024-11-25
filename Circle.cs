using System.Windows;
using System.Windows.Shapes;

namespace Kinderspiel
{
    public class Circle
    {
        private Ellipse _ellipse;
        private double _x;
        private double _y;
        private double _radius;
        private double _speed;
        private double _angle;

        private MainWindow window;

        private int HeaderOffset = 55;

        public Circle(MainWindow window, Ellipse _ellipse)
        {
            this.window = window;
            this._ellipse = _ellipse;

            Random random = new Random();
            this._angle = random.Next(0, 360);
            this._speed = 8;
            this._radius = 25;
            this._x = window.Width / 2;
            this._y = window.Height / 2;
        }

        public void Tick(List<Circle> circles)
        {
            double angleRadians = _angle * (Math.PI / 180d);

            double dx = _speed * Math.Cos(angleRadians) + _x;
            double dy = _speed * Math.Sin(angleRadians) + _y;

            Boolean v = dy < _radius;

            if (dx < _radius || dx > GetScreenWidth() - _radius)
            {
                this._angle = (-(_angle + 180)) % 360;
            }
            if (dy < _radius || dy > GetScreenHeight() - _radius)
            {
                this._angle = (-_angle) % 360;
            }

            setLocation(dx, dy);
            Update();
        }

        public void Update()
        {
            Thickness thickness = new Thickness(_x - _radius, _y - _radius + HeaderOffset, 0, 0);
            _ellipse.Margin = thickness;
            _ellipse.Width = _radius * 2;
            _ellipse.Height = _radius * 2;
        }

        public void setLocation(double x, double y)
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
            return Math.Sqrt(Math.Pow(this._x - circle._x, 2) + Math.Pow(this._y - circle._y, 2)) < (this._radius + circle._radius);
        }

        public Ellipse GetEllipse()
        {
            return _ellipse;
        }
    }
}