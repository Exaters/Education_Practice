using System;
using System.Drawing;
using System.Windows.Forms;

namespace Education_practice
{
    public partial class MainWindow : Form
    {
        public enum LineType { Vertical, Horizontal }

        // Цветовая палитра
        private readonly Color BackgroundColor = Color.LightCyan;
        private readonly Color GridColor = Color.FromArgb(100, Color.DarkSlateGray);
        private readonly Color AxisColor = Color.DarkSlateGray;
        private readonly Color CircleColor = Color.Gold;
        private readonly Color LineColor = Color.FromArgb(220, 20, 60);
        private readonly Color BigSegmentColor = Color.FromArgb(255, 215, 0);
        private readonly Color SmallSegmentColor = Color.FromArgb(205, 133, 63);
        private readonly Color OutsideColor = Color.FromArgb(173, 216, 230);

        // Константы
        private const float CoordinateScale = 40;
        private const int MaxVisualPoints = 100000;
        private const int DefaultPoints = 20000;

        // Кэш параметров
        private double _r = 3.0, _x0, _y0, _distance;

        public MainWindow()
        {
            InitializeComponent();
            this.BackColor = BackgroundColor;
            Line.SelectedIndex = 0;

            // Подключение обработчиков событий с правильными именами
            Сalculate.Click += Сalculate_Click;
            pictureBox.Paint += PictureBox_Paint;
            Statistics.Click += Statistics_Click;
            this.Load += MainWindow_Load;
            label5.Click += Label5_Click;
        }

        private void MainWindow_Load(object? sender, EventArgs e)
        {
            DatabaseHelper.InitializeDatabase();
        }

        private void Сalculate_Click(object? sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            var lineType = Line.Text == "Горизонтальная" ? LineType.Horizontal : LineType.Vertical;
            int points = GetPointCount();

            double formulaArea = CalculateSegmentArea(_r, _x0, _y0, _distance, lineType);
            double monteCarloArea = CalculateMonteCarloArea(_r, _x0, _y0, _distance, lineType, points);

            label6.Text = $"По формуле: {formulaArea:F4}";
            label7.Text = $"По методу: {monteCarloArea:F4}";
            SaveResults(lineType, points, formulaArea, monteCarloArea);
            pictureBox.Invalidate();
        }

        private void Label5_Click(object? sender, EventArgs e)
        {
            // Обработчик клика по label5
        }

        private bool ValidateInputs()
        {
            try
            {
                _r = double.Parse(R.Text);
                _x0 = double.Parse(X0.Text);
                _y0 = double.Parse(Y0.Text);
                _distance = double.Parse(C.Text);
                return true;
            }
            catch
            {
                MessageBox.Show("Ошибка ввода данных", "Некорректный формат",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private int GetPointCount()
        {
            try
            {
                return Math.Min(int.Parse(Dots_Count.Text), 10_000_000);
            }
            catch
            {
                return DefaultPoints;
            }
        }

        private void SaveResults(LineType lineType, int points, double formulaArea, double monteCarloArea)
        {
            string typeStr = lineType == LineType.Horizontal ? "Горизонтальная" : "Вертикальная";
            DatabaseHelper.AddResult(_x0, _y0, _r, _distance, typeStr, points, formulaArea, monteCarloArea);
        }

        private static double CalculateSegmentArea(double R, double x0, double y0, double distance, LineType lineType)
        {
            double circleArea = Math.PI * R * R;
            double d = lineType == LineType.Vertical ? distance - x0 : distance - y0;

            if (Math.Abs(d) >= R) return d <= -R ? circleArea : 0;

            double h = Math.Sqrt(R * R - d * d);
            double segmentArea = R * R * Math.Acos(d / R) - d * h;
            return Math.Max(segmentArea, circleArea - segmentArea);
        }

        private double CalculateMonteCarloArea(double R, double x0, double y0, double distance, LineType lineType, int totalPoints)
        {
            int hits = 0;
            var rand = new Random();
            double area = 4 * R * R;

            for (int i = 0; i < totalPoints; i++)
            {
                double x = _x0 - R + rand.NextDouble() * 2 * R;
                double y = _y0 - R + rand.NextDouble() * 2 * R;

                if ((x - x0) * (x - x0) + (y - y0) * (y - y0) > R * R) continue;

                bool inBigSegment = lineType == LineType.Vertical ? x >= distance : y >= distance;
                if (inBigSegment) hits++;
            }

            return area * hits / totalPoints;
        }

        private void PictureBox_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            DrawCoordinateSystem(g);
            DrawCircleAndLine(g);
            DrawMonteCarloPoints(g);
        }

        private void DrawCoordinateSystem(Graphics g)
        {
            // Сетка
            using (var gridPen = new Pen(GridColor))
            {
                for (int x = 0; x < pictureBox.Width; x += 20)
                    g.DrawLine(gridPen, x, 0, x, pictureBox.Height);

                for (int y = 0; y < pictureBox.Height; y += 20)
                    g.DrawLine(gridPen, 0, y, pictureBox.Width, y);
            }

            // Оси
            using (var axisPen = new Pen(AxisColor, 1.5f))
            {
                g.DrawLine(axisPen, pictureBox.Width / 2, 0, pictureBox.Width / 2, pictureBox.Height);
                g.DrawLine(axisPen, 0, pictureBox.Height / 2, pictureBox.Width, pictureBox.Height / 2);
            }
        }

        private void DrawCircleAndLine(Graphics g)
        {
            // Окружность
            float diameter = (float)(2 * _r * CoordinateScale);
            float centerX = pictureBox.Width / 2 + (float)(_x0 * CoordinateScale) - diameter / 2;
            float centerY = pictureBox.Height / 2 - (float)(_y0 * CoordinateScale) - diameter / 2;

            using (var circlePen = new Pen(CircleColor, 2f))
            {
                g.DrawEllipse(circlePen, centerX, centerY, diameter, diameter);
            }

            // Линия
            using (var linePen = new Pen(LineColor, 1.5f))
            {
                if (Line.Text == "Горизонтальная")
                {
                    float lineY = pictureBox.Height / 2 - (float)(_distance * CoordinateScale);
                    g.DrawLine(linePen, 0, lineY, pictureBox.Width, lineY);
                }
                else
                {
                    float lineX = pictureBox.Width / 2 + (float)(_distance * CoordinateScale);
                    g.DrawLine(linePen, lineX, 0, lineX, pictureBox.Height);
                }
            }
        }

        private void DrawMonteCarloPoints(Graphics g)
        {
            int points = Math.Min(GetPointCount(), MaxVisualPoints);
            bool isHorizontal = Line.Text == "Горизонтальная";
            var rand = new Random();

            for (int i = 0; i < points; i++)
            {
                double x = _x0 - _r + rand.NextDouble() * 2 * _r;
                double y = _y0 - _r + rand.NextDouble() * 2 * _r;
                bool inCircle = (x - _x0) * (x - _x0) + (y - _y0) * (y - _y0) <= _r * _r;
                bool inBigSegment = isHorizontal ? y >= _distance : x >= _distance;

                float px = pictureBox.Width / 2 + (float)(x * CoordinateScale);
                float py = pictureBox.Height / 2 - (float)(y * CoordinateScale);

                using (var brush = new SolidBrush(inCircle
                    ? (inBigSegment ? BigSegmentColor : SmallSegmentColor)
                    : OutsideColor))
                {
                    g.FillEllipse(brush, px - 1.5f, py - 1.5f, 3, 3);
                }
            }
        }

        private void Statistics_Click(object? sender, EventArgs e)
        {
            new StatisticsScreen().Show();
        }
    }
}