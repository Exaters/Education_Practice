namespace Education_Practice
{
    public partial class SplashScreen: Form
    {
        public SplashScreen()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None; // Убираем рамку
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.DarkSlateBlue; // фон
            this.Size = new Size(400, 200);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Рисуем текст
            using (Font font = new Font("Times New Roman", 24, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.Gold))
            {
                string text = "Monte-Carlo\nBy EXATER\nVer: ALPHA";
                SizeF textSize = e.Graphics.MeasureString(text, font);

                // Центрируем текст
                float x = (this.Width - textSize.Width) / 2;
                float y = (this.Height - textSize.Height) / 2;

                e.Graphics.DrawString(text, font, brush, x, y);
            }
        }
    }
}