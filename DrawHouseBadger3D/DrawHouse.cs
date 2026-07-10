#pragma warning disable IDE0130

namespace NullPointersEtc.DrawHouseBadger3D
{
    using AllowNull = System.Diagnostics.CodeAnalysis.AllowNullAttribute;
    using SmoothingMode = System.Drawing.Drawing2D.SmoothingMode;
    using Timer = System.Windows.Forms.Timer;

    class DrawHouse : System.Windows.Forms.UserControl
    {
        public DrawHouse()
        {
            this.DoubleBuffered = true;

            this.animationTimer = new Timer { Interval = 30 };
            this.animationTimer.Tick += OnAnimationTimerTick;
            this.animationTimer.Start();
        }

        private void OnAnimationTimerTick(
            [AllowNull] object sender,
            EventArgs e)
        {
            progress += progressStep;
            if (progress > 1.0F) progress = 0.0F;
            Invalidate();
        }

        private static float EaseInOutCubic(float t)
        => t < 0.5f
            ? 4.0f * t * t * t
            : 1.0f - (-2.0f * t + 2.0f) * (-2.0f * t + 2.0f)
            * (-2.0f * t + 2.0f) / 2.0f;

        private void GetSunPosition(out float x, out float y)
        {
            float eased = EaseInOutCubic(progress);

            float margin = sunDiameter;
            x = -margin + eased * (ClientSize.Width + 2 * margin);

            float arcHeight = MathF.Sin(progress * MathF.PI);
            y = horizonY - arcHeight * (horizonY - peakY);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.LightSkyBlue);

            GetSunPosition(out float sunX, out float sunY);

            g.FillEllipse(Brushes.Gold, x: sunX, y: sunY,
                width: sunDiameter, height: sunDiameter);

            g.DrawEllipse(Pens.Orange, x: sunX, y: sunY,
                width: sunDiameter, height: sunDiameter);

            Rectangle body = new(x: 120, y: 220,
                width: 220, height: 160);

            g.FillRectangle(Brushes.SandyBrown, body);
            g.DrawRectangle(Pens.Black, body);

            Point roofLeft = new(x: 100, y: 220),
                roofRight = new(x: 360, y: 220),
                roofPeak = new(x: 230, y: 110);

            Point[] roofPoints = [roofLeft, roofRight, roofPeak];
            g.FillPolygon(Brushes.Firebrick, roofPoints);
            g.DrawPolygon(Pens.Black, roofPoints);

            Rectangle door = new(x: 200, y: 300,
                width: 60, height: 80);

            g.FillRectangle(Brushes.SaddleBrown, door);
            g.DrawRectangle(Pens.Black, door);

            g.FillEllipse(Brushes.Gold,
                x: 245, y: 335, width: 6, height: 6);

            DrawWindowPanes(g,
                new Rectangle(x: 140, y: 250, width: 40, height: 40));

            DrawWindowPanes(g,
                new Rectangle(x: 280, y: 250, width: 40, height: 40));

            g.FillRectangle(Brushes.ForestGreen,
                x: 0, y: (int)horizonY,
                ClientSize.Width,
                ClientSize.Height - (int)horizonY);

        }

        private void DrawWindowPanes(Graphics g, Rectangle window)
        {
            g.FillRectangle(Brushes.LightYellow, window);
            g.DrawRectangle(Pens.Black, window);

            g.DrawLine(Pens.Black, x1: window.Left,
                y1: window.Top + window.Height / 2,
                x2: window.Right,
                y2: window.Top + window.Height / 2);

            g.DrawLine(Pens.Black,
                x1: window.Left + window.Width / 2,
                y1: window.Top,
                x2: window.Left + window.Width / 2,
                y2: window.Bottom);
        }

        private readonly Timer animationTimer;
        private float progress = 0.0F;
        private const float progressStep = 1.0F / 400.0F;
        private const int sunDiameter = 60;
        private const float horizonY = 380.0F;
        private const float peakY = 40.0F;
    }
}

