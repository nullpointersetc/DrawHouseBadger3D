#pragma warning disable IDE0130

namespace NullPointersEtc.DrawHouseBadger3D
{
    using SmoothingMode = System.Drawing.Drawing2D.SmoothingMode;
    using Vector3 = System.Numerics.Vector3;
    using Trace = System.Diagnostics.Trace;
    using Tuple = System.Tuple;
    using ThreeSingles = System.Tuple<System.Single, System.Single, System.Single>;
    using System.Drawing.Drawing2D;

    class Draw3d : System.Windows.Forms.UserControl
    {
        private float focalLength = 300F;

        #region Constructor with only the three vertices
        public Draw3d(Vector3 _topLeft,
            Vector3 _topRight,
            Vector3 _bottomLeft)
            : this(_topLeft, _topRight, _bottomLeft,
                  _tolerance: 1.0f / 1000000.0f)
        { }
        #endregion

        #region Constructor with three vertices and the tolerance
        public Draw3d(Vector3 _topLeft,
            Vector3 _topRight,
            Vector3 _bottomLeft,
            float _tolerance)
        {
            Vector3 edge1 = _topRight - _topLeft;
            Vector3 edge2 = _bottomLeft - _topLeft;

            float edge1Length = edge1.Length();
            float edge2Length = edge2.Length();

            if (edge1Length == 0)
            {
                throw new ArgumentException(paramName: nameof(_topRight),
                    message: "must be distinct from " + nameof(_topLeft));
            }

            if (edge2Length == 0)
            {
                throw new ArgumentException(paramName: nameof(_bottomLeft),
                    message: "must be distinct from " + nameof(_topLeft));
            }

            // Normalize the dot product by the edge lengths so "tolerance" means
            // something consistent (roughly |cos(angle between edges)|)
            // regardless of how large/small the plane is.
            float normalizedDot = Vector3.Dot(edge1, edge2) / (edge1Length * edge2Length);

            if (Math.Abs(normalizedDot) > _tolerance)
            {
                throw new ArgumentException(
                    paramName: nameof(_bottomLeft),
                    message: "(" + nameof(_bottomLeft) + " - " + nameof(_topLeft) +
                    ") must be orthogonal within " + _tolerance.ToString() +
                    " to (" + nameof(_topRight) + " - " + nameof(_topLeft) + ").");
            }

            a_topLeft = _topLeft;
            a_topRight = _topRight;
            a_bottomLeft = _bottomLeft;
            a_bottomRight = _topRight + _bottomLeft - _topLeft;

            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }
        #endregion

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            ThreeSingles p1 = Tuple.Create(100.0F, 100.0F, 100.0F);

            ThreeSingles p2 = Tuple.Create(100.0F, 200.0F, 100.0F);

            ThreeSingles p3 = Tuple.Create(200.0F, 100.0F, 100.0F);

            Trace.WriteLine(category: nameof(p1), value: p1);
            Trace.WriteLine(category: nameof(p2), value: p2);
            Trace.WriteLine(category: nameof(p3), value: p3);

            ThreeSingles p1_minus_p2 = Subtract(from: p1, subtract: p2);
            Trace.WriteLine(category: nameof(p1_minus_p2), value: p1_minus_p2);

            ThreeSingles p3_minus_p2 = Subtract(from: p3, subtract: p2);
            Trace.WriteLine(category: nameof(p3_minus_p2), value: p3_minus_p2);

            ThreeSingles p1_minus_p2_cross_p3_minus_p2 =
                Cross(from: p1_minus_p2, cross: p3_minus_p2);

            Trace.WriteLine(category: nameof(p1_minus_p2_cross_p3_minus_p2),
                value: p1_minus_p2_cross_p3_minus_p2);

            Trace.WriteLineIf(p1_minus_p2_cross_p3_minus_p2.Item3 > 0.0f,
                message: "Should draw");

            Trace.WriteLineIf(p1_minus_p2_cross_p3_minus_p2.Item3 < 0.0f,
                message: "Should not draw");

            Trace.WriteLineIf(p1_minus_p2_cross_p3_minus_p2.Item3 == 0.0f,
                message: "This is a single line");

            Vector3 pv1 = new Vector3(p1.Item1, p1.Item2, p1.Item3);
            Vector3 pv2 = new Vector3(p2.Item1, p2.Item2, p2.Item3);
            Vector3 pv3 = new Vector3(p3.Item1, p3.Item2, p3.Item3);

            Trace.WriteLine(category: nameof(pv1), value: pv1);
            Trace.WriteLine(category: nameof(pv2), value: pv2);
            Trace.WriteLine(category: nameof(pv3), value: pv3);

            Vector3 pv1_minus_pv2 = pv1 - pv2;
            Trace.WriteLine(category: nameof(pv1_minus_pv2), value: pv1_minus_pv2);

            Trace.WriteLineIf(pv1_minus_pv2.X == p1_minus_p2.Item1
                && pv1_minus_pv2.Y == p1_minus_p2.Item2
                && pv1_minus_pv2.Z == p1_minus_p2.Item3,
                message: "pv1_minus_pv2 == p1_minus_p2");

            Trace.WriteLineIf(!(pv1_minus_pv2.X == p1_minus_p2.Item1
                && pv1_minus_pv2.Y == p1_minus_p2.Item2
                && pv1_minus_pv2.Z == p1_minus_p2.Item3),
                message: "pv1_minus_pv2 == p1_minus_p2");

            Vector3 pv3_minus_pv2 = pv3 - pv2;

            Trace.WriteLine(category: nameof(pv3_minus_pv2), value: pv3_minus_pv2);

            Trace.WriteLineIf(pv3_minus_pv2.X == p3_minus_p2.Item1
                && pv3_minus_pv2.Y == p3_minus_p2.Item2
                && pv3_minus_pv2.Z == p3_minus_p2.Item3,
                message: "pv3_minus_pv2 == p3_minus_p2");

            Trace.WriteLineIf(!(pv3_minus_pv2.X == p3_minus_p2.Item1
                && pv3_minus_pv2.Y == p3_minus_p2.Item2
                && pv3_minus_pv2.Z == p3_minus_p2.Item3),
                message: "pv3_minus_pv2 != p3_minus_p2");

            Vector3 pv1_minus_pv2_cross_pv3_minus_pv2 = Vector3.Cross(pv1_minus_pv2, pv3_minus_pv2);

            Trace.WriteLine(category: nameof(pv1_minus_pv2_cross_pv3_minus_pv2),
                value: pv1_minus_pv2_cross_pv3_minus_pv2);

            Trace.WriteLineIf(pv1_minus_pv2_cross_pv3_minus_pv2.X == p1_minus_p2_cross_p3_minus_p2.Item1
                && pv1_minus_pv2_cross_pv3_minus_pv2.Y == p1_minus_p2_cross_p3_minus_p2.Item2
                && pv1_minus_pv2_cross_pv3_minus_pv2.Z == p1_minus_p2_cross_p3_minus_p2.Item3,
                message: "pv1_minus_pv2_cross_pv3_minus_pv2 == p1_minus_p2_cross_p3_minus_p2");

            Trace.WriteLineIf(!(pv1_minus_pv2_cross_pv3_minus_pv2.X == p1_minus_p2_cross_p3_minus_p2.Item1
                && pv1_minus_pv2_cross_pv3_minus_pv2.Y == p1_minus_p2_cross_p3_minus_p2.Item2
                && pv1_minus_pv2_cross_pv3_minus_pv2.Z == p1_minus_p2_cross_p3_minus_p2.Item3),
                message: "pv1_minus_pv2_cross_pv3_minus_pv2 != p1_minus_p2_cross_p3_minus_p2");

            if (p1_minus_p2_cross_p3_minus_p2.Item3 > 0.0f)
            {
                Brush pen2 = Brushes.Green                ;
                g.FillPolygon(pen2, [new PointF(x:p1.Item1, y: p1.Item2),
                    new PointF(x:p2.Item1,y:p2.Item2),
                    new PointF(x:p3.Item1,y:p3.Item2)]);
            }

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

                g.DrawLine(pen, projected[4 + 0], projected[4 + 1]);
                g.DrawLine(pen, projected[4 + 1], projected[4 + 2]);
                g.DrawLine(pen, projected[4 + 2], projected[4 + 3]);
                g.DrawLine(pen, projected[4 + 3], projected[4 + 0]);

                g.DrawLine(pen, projected[0], projected[4 + 0]);
                g.DrawLine(pen, projected[1], projected[4 + 1]);
                g.DrawLine(pen, projected[2], projected[4 + 2]);
                g.DrawLine(pen, projected[3], projected[4 + 3]);
            }
        }

        private static ThreeSingles Subtract(
            ThreeSingles from, ThreeSingles subtract)
            => Tuple.Create(item1: from.Item1 - subtract.Item1,
                item2: from.Item2 - subtract.Item2,
                item3: from.Item3 - subtract.Item3);
        private static ThreeSingles Cross(
            ThreeSingles from, ThreeSingles cross)
            => Tuple.Create(item1: (from.Item2 * cross.Item3) - (from.Item3 * cross.Item2),
              item2: (from.Item3 * cross.Item1) - (from.Item1 * cross.Item3),
              item3: (from.Item1 * cross.Item2) - (from.Item2 * cross.Item1)
            );

        private readonly Vector3 a_topLeft;
        private readonly Vector3 a_topRight;
        private readonly Vector3 a_bottomLeft;
        private readonly Vector3 a_bottomRight;

    }
}
