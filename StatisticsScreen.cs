using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Education_practice
{
    public partial class StatisticsScreen : Form
    {
        private Chart chart;
        private ToolTip toolTip = new ToolTip();

        public StatisticsScreen()
        {
            InitializeComponent();
            AddDeleteButton();
            CreateHistogram();
        }

        private void AddDeleteButton()
        {
            // Создаем маленькую кнопку удаления
            // В метод AddDeleteButton():
            Button deleteButton = new Button
            {
                Text = "", // Убираем текст
                BackgroundImage = SystemIcons.Error.ToBitmap(), // Иконка ошибки (красный крестик)
                BackgroundImageLayout = ImageLayout.Stretch,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(25, 25),
                Location = new Point(5, 5),
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Cursor = Cursors.Hand,
                TabStop = false // Чтобы не фокусировалась по Tab
            };

            deleteButton.MouseEnter += (s, e) =>
            {
                deleteButton.BackColor = Color.FromArgb(220, 80, 80);
                deleteButton.Size = new Size(27, 27);
                deleteButton.Location = new Point(4, 4);
            };

            deleteButton.MouseLeave += (s, e) =>
            {
                deleteButton.BackColor = Color.Red;
                deleteButton.Size = new Size(25, 25);
                deleteButton.Location = new Point(5, 5);
            };

            // Убираем стандартные границы у кнопки
            deleteButton.FlatAppearance.BorderSize = 0;

            // Обработчик клика
            deleteButton.Click += (sender, e) =>
            {
                var result = MessageBox.Show(
                        "Все сохраненные результаты расчетов будут удалены.\nПродолжить?",
                        "Подтверждение удаления",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        DatabaseHelper.DeleteAllResult();
                        RefreshChart();
                        MessageBox.Show("Данные успешно удалены!",
                                      "Успех",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                                      "Ошибка",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Error);
                    }
                }
            };

            // Добавляем кнопку на форму
            this.Controls.Add(deleteButton);
            deleteButton.BringToFront();
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            var hitTest = chart.HitTest(e.X, e.Y);

            if (hitTest != null && hitTest.Object != null)
            {
                if (hitTest.Object is DataPoint)
                {
                    DataPoint point = (DataPoint)hitTest.Object;
                    int nValue = (int)point.Tag; // Получаем N из Tag точки
                    double diffValue = point.YValues[0];

                    // Показываем подсказку
                    toolTip.Show($"N = {nValue}\nРазница = {diffValue:F4}",
                                chart,
                                e.X + 10,
                                e.Y + 10,
                                1000); // Показываем 1 секунду
                }
                else
                {
                    toolTip.Hide(chart);
                }
            }
            else
            {
                toolTip.Hide(chart);
            }
        }

        private void CreateHistogram()
        {
            // Удаляем старый график, если есть
            if (chart != null)
            {
                chart.MouseMove -= Chart_MouseMove;
                this.Controls.Remove(chart);
                chart.Dispose();
            }

            // Получаем данные из базы
            DataTable results = DatabaseHelper.GetAllResults();

            // Создаем новый элемент Chart
            chart = new Chart
            {
                Dock = DockStyle.Fill,
                Location = new Point(0, 0),
                Size = new Size(this.ClientSize.Width, this.ClientSize.Height)
            };

            // Настраиваем область графика
            ChartArea chartArea = new ChartArea();
            chartArea.AxisX.Title = "N (количество испытаний)";
            chartArea.AxisY.Title = "|Formula - MonteCarlo|";
            chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chart.ChartAreas.Add(chartArea);

            // Создаем серию для гистограммы
            Series series = new Series
            {
                ChartType = SeriesChartType.Column,
                Name = "Разница методов",
                Color = Color.SteelBlue,
                BorderColor = Color.DarkBlue,
                BorderWidth = 1,
                IsValueShownAsLabel = true,
                LabelFormat = "F4",
                Tag = "Series1" // Добавляем тег для идентификации
            };

            // Заполняем данными
            foreach (DataRow row in results.Rows)
            {
                double formula = Convert.ToDouble(row["FormulaResult"]);
                double monte = Convert.ToDouble(row["MonteCarloResult"]);
                int n = Convert.ToInt32(row["N"]);

                double difference = Math.Abs(formula - monte);
                DataPoint point = new DataPoint();
                point.SetValueXY(n, difference);
                point.Tag = n; // Сохраняем N в Tag точки
                series.Points.Add(point);
            }

            chart.Series.Add(series);

            // Добавляем обработчик движения мыши
            chart.MouseMove += Chart_MouseMove;

            // Добавляем легенду
            chart.Legends.Add(new Legend());

            // Добавляем заголовок
            chart.Titles.Add(new Title
            {
                Text = "Сравнение точности методов",
                Font = new Font("Arial", 12, FontStyle.Bold)
            });

            // Добавляем Chart на форму
            this.Controls.Add(chart);
            chart.SendToBack();
        }

        private void RefreshChart()
        {
            CreateHistogram();
        }
    }
}