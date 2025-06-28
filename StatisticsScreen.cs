using System.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace Education_practice
{
    public partial class StatisticsScreen : Form
    {
        private Chart chart;
        private ToolTip toolTip = new ToolTip();
        private Button deleteButton;
        private DataGridView dataGridView;

        public StatisticsScreen()
        {
            InitializeComponent();
            InitializeCustomComponents();
            LoadData();
        }

        private void InitializeCustomComponents()
        {
            this.BackColor = Color.LightCyan;
            this.ForeColor = Color.DarkSlateGray;
            this.Font = new Font("Times New Roman", 10.2F);
            this.Text = "Статистика расчетов";

            deleteButton = new Button
            {
                Text = "Очистить статистику",
                BackColor = Color.DarkSlateBlue,
                ForeColor = Color.Gold,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 30),
                Location = new Point(20, 20),
                Cursor = Cursors.Hand,
                Font = new Font("Times New Roman", 10.2F, FontStyle.Bold)
            };

            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);

            dataGridView = new DataGridView
            {
                BackColor = Color.LightCyan,
                ForeColor = Color.DarkSlateGray,
                BorderStyle = BorderStyle.None,
                Location = new Point(20, 70),
                Size = new Size(860, 300),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical
            };

            dataGridView.DefaultCellStyle.BackColor = Color.LightCyan;
            dataGridView.DefaultCellStyle.ForeColor = Color.DarkSlateGray;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.Gold;
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.DarkSlateBlue;
            dataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.DarkSlateBlue,
                ForeColor = Color.Gold,
                Font = new Font("Times New Roman", 10.2F, FontStyle.Bold)
            };

            this.Controls.Add(dataGridView);

            chart = new Chart
            {
                BackColor = Color.LightCyan,
                Location = new Point(10, 50),
                Size = new Size(860, 600),
                Visible = false,
                Padding = new Padding(10)
            };

            ChartArea chartArea = new ChartArea
            {
                BackColor = Color.LightCyan,
                AxisX = { LineColor = Color.DarkSlateGray, TitleForeColor = Color.DarkSlateGray },
                AxisY = { LineColor = Color.DarkSlateGray, TitleForeColor = Color.DarkSlateGray }
            };

            chart.ChartAreas.Add(chartArea);
            this.Controls.Add(chart);

            // Кнопка переключения между таблицей и графиком
            var toggleViewButton = new Button
            {
                Text = "Показать график",
                BackColor = Color.DarkSlateBlue,
                ForeColor = Color.Gold,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(180, 30),
                Location = new Point(220, 20),
                Cursor = Cursors.Hand,
                Font = new Font("Times New Roman", 10.2F, FontStyle.Bold)
            };

            toggleViewButton.FlatAppearance.BorderSize = 0;
            toggleViewButton.Click += ToggleViewButton_Click;
            this.Controls.Add(toggleViewButton);
        }

        private void LoadData()
        {
            DataTable results = DatabaseHelper.GetAllResults();

            dataGridView.DataSource = results;
            dataGridView.Columns["Id"].Visible = false;

            if (dataGridView.Columns.Contains("FormulaResult"))
            {
                dataGridView.Columns["FormulaResult"].HeaderText = "Формула";
            }

            if (dataGridView.Columns.Contains("MonteCarloResult"))
            {
                dataGridView.Columns["MonteCarloResult"].HeaderText = "Монте-Карло";
            }

            if (dataGridView.Columns.Contains("N"))
            {
                dataGridView.Columns["N"].HeaderText = "Итерации";
            }

            CreateChart(results);
        }

        private void CreateChart(DataTable results)
        {
            chart.Series.Clear();

            Series series = new Series
            {
                ChartType = SeriesChartType.Column,
                Name = "Разница методов",
                Color = Color.Gold,
                BorderColor = Color.DarkSlateGray,
                BorderWidth = 1,
                IsValueShownAsLabel = true,
                LabelFormat = "F4"
            };

            foreach (DataRow row in results.Rows)
            {
                double formula = Convert.ToDouble(row["FormulaResult"]);
                double monte = Convert.ToDouble(row["MonteCarloResult"]);
                int n = Convert.ToInt32(row["N"]);

                double difference = Math.Abs(formula - monte);
                _ = series.Points.AddXY(n, difference);
                series.Points[series.Points.Count - 1].Tag = n;
            }

            chart.Series.Add(series);
            chart.Titles.Clear();
            _ = chart.Titles.Add("Разница между формулой и методом Монте-Карло");
        }

        private void ToggleViewButton_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;

            if (dataGridView.Visible)
            {
                dataGridView.Visible = false;
                chart.Visible = true;
                button.Text = "Показать таблицу";
                this.Height = 720;
            }
            else
            {
                dataGridView.Visible = true;
                chart.Visible = false;
                button.Text = "Показать график";
                this.Height = 420;
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
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
                    dataGridView.DataSource = null;
                    chart.Series.Clear();
                    _ = MessageBox.Show("Данные успешно удалены!",
                                "Успех",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    _ = MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                                "Ошибка",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                }
            }
        }

        private void Chart_MouseMove(object sender, MouseEventArgs e)
        {
            var hitTest = chart.HitTest(e.X, e.Y);

            if (hitTest != null && hitTest.Object != null && hitTest.Object is DataPoint)
            {
                DataPoint point = (DataPoint)hitTest.Object;
                int nValue = (int)point.Tag;
                double diffValue = point.YValues[0];

                toolTip.Show($"N = {nValue}\nРазница = {diffValue:F4}",
                          chart,
                          e.X + 10,
                          e.Y + 10,
                          1000);
            }
            else
            {
                toolTip.Hide(chart);
            }
        }
    }
}