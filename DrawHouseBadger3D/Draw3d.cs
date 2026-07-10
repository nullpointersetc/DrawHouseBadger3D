#pragma warning disable IDE0130

namespace NullPointersEtc.DrawHouseBadger3D
{
    using SmoothingMode = System.Drawing.Drawing2D.SmoothingMode;
    using Vector3 = System.Numerics.Vector3;

    class Draw3d : System.Windows.Forms.UserControl
    {
        private float focalLength = 300F;

        public Draw3d()
        {
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Vector3[] vertices = [
                new Vector3(-50, -50, 200),
                new Vector3(50,-50,200),
                new Vector3(50,50,200),
                new Vector3(-50,50,200),
                new Vector3(-50, -50, 300),
                new Vector3(50,-50,300),
                new Vector3(50,50,300),
                new Vector3(-50,50,300)
            ];

            PointF[] projected = new PointF[vertices.Length];

            for (int i = 0; i < vertices.Length; i++)
            {
                float x2d = (vertices[i].X * focalLength) / vertices[i].Z;
                float y2d = (vertices[i].Y * focalLength) / vertices[i].Z;

                projected[i] = new PointF(
                    x: x2d + this.Width / 2, y: -y2d + this.Height / 2);
            }

            using (Pen pen = new(Color.Blue, 2))
            {
                g.DrawLine(pen, projected[0], projected[1]);
                g.DrawLine(pen, projected[1], projected[2]);
                g.DrawLine(pen, projected[2], projected[3]);
                g.DrawLine(pen, projected[3], projected[0]);

                g.DrawLine(pen, projected[4+0], projected[4+1]);
                g.DrawLine(pen, projected[4+1], projected[4+2]);
                g.DrawLine(pen, projected[4+2], projected[4+3]);
                g.DrawLine(pen, projected[4+3], projected[4+0]);

                g.DrawLine(pen, projected[0], projected[4 + 0]);
                g.DrawLine(pen, projected[1], projected[4 + 1]);
                g.DrawLine(pen, projected[2], projected[4 + 2]);
                g.DrawLine(pen, projected[3], projected[4 + 3]);
            }
        }
    }
}
