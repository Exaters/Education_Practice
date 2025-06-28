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

            // Подключение обработчиков событий
            Сalculate.Click += Сalculate_Click;
            pictureBox.Paint += PictureBox_Paint;
            Statistics.Click += Statistics_Click;
            this.Load += MainWindow_Load;
            Help.Click += Help_Click;
            Exit.Click += Exit_Click;
            Information_about_programm.Click += Information_about_programm_Click;

        }

        private void Help_Click(object sender, EventArgs e)
        {
            string helpFile = Path.Combine(Application.StartupPath, "Help.chm");

            if (File.Exists(helpFile))
            {
                try
                {
                    System.Windows.Forms.Help.ShowHelp(this, helpFile);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show($"Не удалось открыть файл справки: {ex.Message}",
                                  "Ошибка",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
            else
            {
                _ = MessageBox.Show("Файл справки Help.chm не найден.\nПожалуйста, убедитесь, что он находится в папке с программой.",
                              "Файл справки отсутствует",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            DatabaseHelper.InitializeDatabase();
        }

        private void Сalculate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
            {
                return;
            }

            var lineType = Line.Text == "Горизонтальная" ? LineType.Horizontal : LineType.Vertical;
            int points = GetPointCount();

            double formulaArea = CalculateSegmentArea(_r, _x0, _y0, _distance, lineType);
            double monteCarloArea = CalculateMonteCarloArea(_r, _x0, _y0, _distance, lineType, points);

            label6.Text = $"Рассчёт по формуле: {formulaArea:F4}";
            label7.Text = $"Рассчёт по методу Монте-Карло: {monteCarloArea:F4}";
            SaveResults(lineType, points, formulaArea, monteCarloArea);
            pictureBox.Invalidate();
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
                _ = MessageBox.Show("Ошибка ввода данных", "Некорректный формат",
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

            if (Math.Abs(d) >= R)
            {
                return d <= -R ? circleArea : 0;
            }

            double h = Math.Sqrt((R * R) - (d * d));
            double segmentArea = (R * R * Math.Acos(d / R)) - (d * h);
            return Math.Max(segmentArea, circleArea - segmentArea);
        }

        private double CalculateMonteCarloArea(double R, double x0, double y0, double distance, LineType lineType, int totalPoints)
        {
            int hits = 0;
            var rand = new Random();
            double area = 4 * R * R;

            for (int i = 0; i < totalPoints; i++)
            {
                double x = _x0 - R + (rand.NextDouble() * 2 * R);
                double y = _y0 - R + (rand.NextDouble() * 2 * R);

                if (((x - x0) * (x - x0)) + ((y - y0) * (y - y0)) > R * R)
                {
                    continue;
                }

                bool inBigSegment = lineType == LineType.Vertical ? x >= distance : y >= distance;
                if (inBigSegment)
                {
                    hits++;
                }
            }

            return area * hits / totalPoints;
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
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
                {
                    g.DrawLine(gridPen, x, 0, x, pictureBox.Height);
                }

                for (int y = 0; y < pictureBox.Height; y += 20)
                {
                    g.DrawLine(gridPen, 0, y, pictureBox.Width, y);
                }
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
            float centerX = (pictureBox.Width / 2) + (float)(_x0 * CoordinateScale) - (diameter / 2);
            float centerY = (pictureBox.Height / 2) - (float)(_y0 * CoordinateScale) - (diameter / 2);

            using (var circlePen = new Pen(CircleColor, 2f))
            {
                g.DrawEllipse(circlePen, centerX, centerY, diameter, diameter);
            }

            // Линия
            using (var linePen = new Pen(LineColor, 1.5f))
            {
                if (Line.Text == "Горизонтальная")
                {
                    float lineY = (pictureBox.Height / 2) - (float)(_distance * CoordinateScale);
                    g.DrawLine(linePen, 0, lineY, pictureBox.Width, lineY);
                }
                else
                {
                    float lineX = (pictureBox.Width / 2) + (float)(_distance * CoordinateScale);
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
                double x = _x0 - _r + (rand.NextDouble() * 2 * _r);
                double y = _y0 - _r + (rand.NextDouble() * 2 * _r);
                bool inCircle = ((x - _x0) * (x - _x0)) + ((y - _y0) * (y - _y0)) <= _r * _r;
                bool inBigSegment = isHorizontal ? y >= _distance : x >= _distance;

                float px = (pictureBox.Width / 2) + (float)(x * CoordinateScale);
                float py = (pictureBox.Height / 2) - (float)(y * CoordinateScale);

                using (var brush = new SolidBrush(inCircle
                    ? (inBigSegment ? BigSegmentColor : SmallSegmentColor)
                    : OutsideColor))
                {
                    g.FillEllipse(brush, px - 1.5f, py - 1.5f, 3, 3);
                }
            }
        }

        private void Statistics_Click(object sender, EventArgs e)
        {
            new StatisticsScreen().Show();
        }

        private void Information_about_programm_Click(object sender, EventArgs e)
        {
            _ = MessageBox.Show(@"О программе 'Monte Carlo'

                            ОБЩИЕ СВЕДЕНИЯ
                            Наименование программы: Monte Carlo
                            Версия: BETA 0.1
                            Разработчик: EXATE

                            НАЗНАЧЕНИЕ ПРОГРАММЫ
                            Программа предназначена для вычисления площади большего сегмента окружности двумя методами:
                                1. По точной математической формуле
                                2. Методом Монте-Карло (статистическое моделирование)

                            Программа предоставляет инструменты для:
                                - Расчёта значений функции по заданным параметрам
                                - Визуализации результатов в графическом виде
                                - Сравнения точности разных методов вычислений

                            ФУНКЦИОНАЛЬНЫЕ ВОЗМОЖНОСТИ                                  Дополнительные возможности:
                            Основные функции:                                           - Графическая визуализация результатов
                            - Ввод параметров окружности:                               - Система сохранения и загрузки расчётов
                              * Координаты центра (X₀, Y₀)                              - Таблица результатов 'Analysis' для сравнения методов
                              * Радиус (R)                                              - Информационная форма 'About' с данными о программе
                              * Параметр секущей линии (C)
                            - Настройка метода Монте-Карло:
                              * Задание количества генерируемых точек
                              * Выбор направления расчёта (горизонтальное/вертикальное)

                            СИСТЕМНЫЕ ТРЕБОВАНИЯ
                            Минимальные требования:                                     Рекомендуемые требования:
                            - Операционная система: Windows 7 и выше                    - Операционная система: Windows 10/11
                            - Платформа: .NET Framework 4.7.2+                          - Платформа: .NET 7+
                            - Процессор: 1 ГГц                                          - Процессор: 2 ГГц и выше
                            - Оперативная память: 2 ГБ                                  - Оперативная память: 4 ГБ и более
                            - Свободное место на диске: 500 МБ

                            ТЕХНИЧЕСКАЯ ПОДДЕРЖКА                                                                                                                 КОНТАКТS
                            Для получения технической поддержки обращайтесь по указанным контактным данным.      Электронная почта: exate@exateinc.ru
                            В сообщении укажите:
                                1. Версию программы
                                2. Описание проблемы
                                3. Скриншоты ошибки (если имеются)

                            БЛАГОДАРНОСТИ
                            Автор выражает благодарность всем, кто участвовал в тестировании программы и предоставил ценные замечания по её улучшению.");
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}