#pragma warning disable IDE0130
using System.ComponentModel;

namespace NullPointersEtc.TelevisionScreen
{
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms.VisualStyles;

    /// <summary>
    /// A UserControl that renders one or more 3D wireframe/solid objects.
    ///
    /// X+ is to the right of the origin, X- is to the left of the origin;
    /// Y+ is away from the viewer, Y- is toward the viewer; Z+ is above the
    /// XY plane, Z- is below it (see the Vector3D remarks for the full
    /// coordinate convention).
    ///
    /// Rendering uses a fixed camera (CameraPosition/CameraDirection/
    /// CameraOrientation) that views the scene, while RotationX/Y/Z spin the
    /// objects themselves about the origin before the camera transform is
    /// applied.
    ///
    /// Usage:
    ///   var screen = new TelevisionScreen();
    ///   screen.Objects.Add(Mesh3D.CreateCube(100));
    ///   someForm.Controls.Add(screen);
    ///
    /// Drag the mouse over the control to rotate the scene interactively.
    /// </summary>
    public class TelevisionScreen : UserControl
    {
        private Point3DF myCamera;
        private Vector3DF myWayForward;
        private Vector3DF myWayRight;
        private Vector3DF myWayUp;

        public TelevisionScreen()
        {
            int x1 = 0, y1 = -400, z1 = 400;
            myCamera = new(x: x1, y: y1, z: z1);

            Point3DF origin = new(x: 0.0F, y: 0.0F, z: 0.0F);
            myWayForward = origin.Minus(myCamera).AsUnit();
            myCameraDistance = origin.Minus(myCamera).Length;

            Vector3DF tempUp = new Vector3DF(x: myWayForward.X,
                y: myWayForward.Y, z: -myWayForward.Z);

            myWayRight = myWayForward.Cross(tempUp).AsUnit();

            myWayUp = myWayRight.Cross(myWayForward).AsUnit();

            _cameraPosition = 0;

            _cameraPositions =
                new List<Tuple<Point3DF, Vector3DF, Vector3DF, Vector3DF>>();

            _cameraPositions.Add(Tuple.Create(myCamera, myWayForward, myWayRight, myWayUp));

            while (y1 < 0)
            {
                x1 += 80;
                y1 += 80;
                Point3DF _cameraPosition1 = new(x: x1, y: y1, z: z1);
                Vector3DF _cameraForward1 = origin.Minus(_cameraPosition1).AsUnit();
                Vector3DF _cameraUp1 = new(x: _cameraForward1.X,
                    y: _cameraForward1.Y, z: -_cameraForward1.Z);
                Vector3DF _cameraRight1 = _cameraForward1.Cross(_cameraUp1).AsUnit();
                Vector3DF _cameraUp2 = _cameraRight1.Cross(_cameraForward1).AsUnit();

                _cameraPositions.Add(Tuple.Create(_cameraPosition1,
                    _cameraForward1, _cameraRight1, _cameraUp2));
            }

            while (x1 > 0)
            {
                x1 -= 80;
                y1 += 80;
                Point3DF _cameraPosition1 = new(x: x1, y: y1, z: z1);
                Vector3DF _cameraForward1 = origin.Minus(_cameraPosition1).AsUnit();
                Vector3DF _cameraUp1 = new(x: _cameraForward1.X,
                    y: _cameraForward1.Y, z: -_cameraForward1.Z);
                Vector3DF _cameraRight1 = _cameraForward1.Cross(_cameraUp1).AsUnit();
                Vector3DF _cameraUp2 = _cameraRight1.Cross(_cameraForward1).AsUnit();

                _cameraPositions.Add(Tuple.Create(_cameraPosition1,
                    _cameraForward1, _cameraRight1, _cameraUp2));
            }

            while (y1 > 0)
            {
                x1 -= 80;
                y1 -= 80;
                Point3DF _cameraPosition1 = new(x: x1, y: y1, z: z1);
                Vector3DF _cameraForward1 = origin.Minus(_cameraPosition1).AsUnit();
                Vector3DF _cameraUp1 = new(x: _cameraForward1.X,
                    y: _cameraForward1.Y, z: -_cameraForward1.Z);
                Vector3DF _cameraRight1 = _cameraForward1.Cross(_cameraUp1).AsUnit();
                Vector3DF _cameraUp2 = _cameraRight1.Cross(_cameraForward1).AsUnit();

                _cameraPositions.Add(Tuple.Create(_cameraPosition1,
                    _cameraForward1, _cameraRight1, _cameraUp2));
            }

            while (x1 < 0)
            {
                x1 += 80;
                y1 -= 80;
                Point3DF _cameraPosition1 = new(x: x1, y: y1, z: z1);
                Vector3DF _cameraForward1 = origin.Minus(_cameraPosition1).AsUnit();
                Vector3DF _cameraUp1 = new(x: _cameraForward1.X,
                    y: _cameraForward1.Y, z: -_cameraForward1.Z);
                Vector3DF _cameraRight1 = _cameraForward1.Cross(_cameraUp1).AsUnit();
                Vector3DF _cameraUp2 = _cameraRight1.Cross(_cameraForward1).AsUnit();

                _cameraPositions.Add(Tuple.Create(_cameraPosition1,
                    _cameraForward1, _cameraRight1, _cameraUp2));
            }

            int i2 = _cameraPositions.Count;
            for (int i1 = 0; i1 < i2; ++i1)
            {
                Point3DF _cameraPosition1 = _cameraPositions[i1].Item1;
                Point3DF _cameraPosition3 = new(x: _cameraPosition1.X,
                    y: _cameraPosition1.Y, z: -_cameraPosition1.Z);
                Vector3DF _cameraForward3 = origin.Minus(_cameraPosition3).AsUnit();
                Vector3DF _cameraUp3 = new(x: _cameraForward3.X,
                    y: _cameraForward3.Y, z: _cameraForward3.Z + 100);
                Vector3DF _cameraRight3 = _cameraForward3.Cross(_cameraUp3).AsUnit();
                Vector3DF _cameraUp4 = _cameraRight3.Cross(_cameraForward3).AsUnit();

                _cameraPositions.Add(Tuple.Create(_cameraPosition3,
                    _cameraForward3, _cameraRight3, _cameraUp4));
            }

            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw, value: true);

            BackColor = Color.Black;

            MouseUp += TelevisionScreen_MouseUp;
        }

        public List<Object3DF> Objects { get; } = new List<Object3DF>();

        [Description("Where the camera sits in world space.")]
        public Point3DF CameraPosition { get => myCamera; }

        [Description("Where the camera is pointing. Always a unit vector.")]
        public Vector3DF CameraDirection { get => myWayForward; }

        [Description("Which way is to the right of the center of the image. Always a unit vector.")]
        public Vector3DF ThisWayRight { get => myWayRight; }

        [Description("Which way is up in the image. Always a unit vector.")]
        public Vector3DF ThisWayUp { get => myWayUp; }

        public void SetCamera(
            Point3DF position,
            Vector3DF direction,
            Vector3DF thisWayUp)
        {
            if (direction.IsZero)
                throw new ArgumentException(
                    nameof(direction) + " cannot be the zero vector.");

            if (thisWayUp.IsZero)
                throw new ArgumentException(
                    nameof(thisWayUp) + " cannot be the zero vector.");

            float newCameraDistance = direction.Length;

            Vector3DF unitDirection = direction.AsUnit();

            Vector3DF unitThisWayUp = thisWayUp.AsUnit();

            Vector3DF orthogonalRight = unitDirection.Cross(unitThisWayUp);

            if (orthogonalRight.IsZero)
                throw new ArgumentException(
                    nameof(direction) +
                    " cannot be parallel to " +
                    nameof(thisWayUp));

            Vector3DF unitRight = orthogonalRight.AsUnit();

            Vector3DF orthogonalUp = unitRight.Cross(unitDirection);

            Vector3DF unitUp = orthogonalUp.AsUnit();

            myCamera = position;
            myWayForward = unitDirection;
            myWayRight = unitRight;
            myWayUp = unitUp;
            myCameraDistance = newCameraDistance;
            Invalidate();
        }

        public float DotsPerInch { get; set; } = 1.0F;

        public bool UsePerspective
        {
            get => myUsingPerspective;
            set => myUsingPerspective = value;
        }
        public bool UseOrthogonal
        {
            get => !myUsingPerspective;
            set => myUsingPerspective = !value;
        }

        private bool myUsingPerspective = true;

        public float CameraDistance { get => myCameraDistance; }

        private float myCameraDistance;

        private int _cameraPosition = 0;

        private readonly IList<Tuple<Point3DF, Vector3DF,
            Vector3DF, Vector3DF>> _cameraPositions;

        private void TelevisionScreen_MouseUp(
            object? sender, MouseEventArgs e)
        {
            ++_cameraPosition;
            if (_cameraPosition == _cameraPositions.Count) _cameraPosition = 0;

            myCamera = _cameraPositions[_cameraPosition].Item1;
            myWayForward = _cameraPositions[_cameraPosition].Item2;
            myWayRight = _cameraPositions[_cameraPosition].Item3;
            myWayUp = _cameraPositions[_cameraPosition].Item4;
            Invalidate();
        }

        /// <summary>
        /// Projects a world-space point to a 2D pixel location on this control,
        /// applying the current object rotation, the camera transform, and the
        /// current scale/projection mode.
        /// </summary>
        private PointF Project(Point3DF v)
        {
            // Transform into camera space: position relative to the eye,
            // expressed in terms of the camera's right/up/forward basis.
            Vector3DF relative = v.Minus(myCamera);

            float imageX = relative.Dot(myWayRight),
                depthY = relative.Dot(myWayForward), imageZ = relative.Dot(myWayUp);

            if (myUsingPerspective)
            {
                imageX *= myCameraDistance;
                imageZ *= myCameraDistance;

                if (depthY > 1.0)
                {
                    imageX /= depthY;
                    imageZ /= depthY;
                }
            }

            return new PointF(
                x: Width / 2.0F + imageX * DotsPerInch,
                y: Height / 2.0F - imageZ * DotsPerInch);
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.None;
            e.Graphics.Clear(BackColor);

            foreach (Object3DF mesh in Objects)
            {
                foreach (Triangle3DF tri in mesh.Triangles)
                {
                    if (tri.Front.Dot(myWayForward) > 0)
                    {
                        using (var brush = new SolidBrush(tri.Color))
                        {
                            e.Graphics.FillPolygon(brush,
                                [Project(tri.First), Project(tri.Second), Project(tri.Third)]);
                        }
                    }
                }
            }
        }
    }

#pragma warning disable CS0659, IDE0290
    [Description("A simple vector in 3D space.")]
    public class Vector3DF
    {
        public Vector3DF(
            [Description("Positive x is to the right of the origin. " +
                "Negative x is to the left of the origin.")]
            float x,
            [Description("Positive y is farther away from the viewer than the origin. "+
                "Negative y is nearer to the viewer than the origin.")]
            float y,
            [Description("Positive z is above the XY plane. " +
                "Negative z is below the XY plane.")]
            float z)
        {
            xx = x;
            yy = y;
            zz = z;
        }

        public float X { get => xx; }
        public float Y { get => yy; }
        public float Z { get => zz; }
        public bool IsZero => xx == 0.0F && yy == 0.0F && zz == 0.0F;
        public float Length =>
            MathF.Sqrt(xx * xx + yy * yy + zz * zz);

        public bool Equals(Vector3DF that) =>
            this.xx == that.xx && this.yy == that.yy && this.zz == that.zz;

        public override bool Equals(object? obj) =>
            obj is Vector3DF that
            && this.xx == that.xx && this.yy == that.yy && this.zz == that.zz;

        public override string ToString() =>
            "[" + nameof(X) + ":" + xx.ToString() + ", " +
            nameof(Y) + ":" + yy.ToString() + ", " +
            nameof(Z) + ":" + zz.ToString() + "]";

        public Vector3DF Negate() => new(x: -xx, y: -yy, z: -zz);

        public Vector3DF Plus(Vector3DF that) =>
            new(x: xx + that.xx, y: yy + that.yy, z: zz + that.zz);

        public Vector3DF Minus(Vector3DF that) =>
            new(x: xx - that.xx, y: yy - that.yy, z: zz - that.zz);

        public Vector3DF Times(float that) =>
            new(x: xx * that, y: yy * that, z: zz * that);

        public float Dot(Vector3DF that) =>
            xx * that.xx + yy * that.yy + zz * that.zz;

        public Vector3DF Cross(Vector3DF that) =>
            new(x: yy * that.zz - zz * that.yy,
            y: zz * that.xx - xx * that.zz,
            z: xx * that.yy - yy * that.xx);

        [Description("A unit vector in the same direction as this vector")]
        public Vector3DF AsUnit() => IsZero ?
            throw new DivideByZeroException(
                    nameof(AsUnit) + " can be called only if IsZero is false")
            : this.Times(1.0F / Length);

        private readonly float xx, yy, zz;
    }

#pragma warning disable CS0659, IDE0290
    [Description("A simple point in 3D space.")]
    public class Point3DF
    {
        public Point3DF(
            [Description("Positive x is to the right of the origin. " +
                "Negative x is to the left of the origin.")]
            float x,
            [Description("Positive y is farther away from the viewer than the origin. "+
                "Negative y is nearer to the viewer than the origin.")]
            float y,
            [Description("Positive z is above the XY plane. " +
                "Negative z is below the XY plane.")]
            float z)
        {
            xx = x;
            yy = y;
            zz = z;
        }

        public float X { get => xx; }
        public float Y { get => yy; }
        public float Z { get => zz; }

        public bool Equals(Point3DF that) =>
            this.xx == that.xx && this.yy == that.yy && this.zz == that.zz;

        public override bool Equals(object? obj) =>
            obj is Point3DF that
            && this.xx == that.xx && this.yy == that.yy && this.zz == that.zz;

        public override string ToString() =>
            "(" + nameof(X) + ":" + xx.ToString() + ", " +
            nameof(Y) + ":" + yy.ToString() + ", " +
            nameof(Z) + ":" + zz.ToString() + ")";

        public Point3DF Plus(Vector3DF that) =>
            new(x: xx + that.X, y: yy + that.Y, z: zz + that.Z);

        public Point3DF Minus(Vector3DF that) =>
            new(x: xx - that.X, y: yy - that.Y, z: zz - that.Z);

        public Vector3DF Minus(Point3DF that) =>
            new(x: xx - that.xx, y: yy - that.yy, z: zz - that.zz);

        [Description("Rotate this point about the X axis by the given angle (radians).")]
        public Point3DF RotatedX(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new(xx, yy * cos - zz * sin, yy * sin + zz * cos);
        }

        [Description("Rotate this point about the Y axis by the given angle (radians).")]
        public Point3DF RotatedY(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new(xx * cos + zz * sin, yy, -xx * sin + zz * cos);
        }

        [Description("Rotate this point about the Z axis by the given angle (radians).")]
        public Point3DF RotatedZ(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new(xx * cos - yy * sin, xx * sin + yy * cos, zz);
        }

        private readonly float xx, yy, zz;
    }

    public readonly struct Triangle3DF
    {
        public Triangle3DF(
            Point3DF first,
            Point3DF second,
            Point3DF third,
            System.Drawing.Color color)
        {
            Vector3DF front = third.Minus(second)
                .Cross(first.Minus(second));

            if (front.IsZero)
                throw new ArgumentException(
                    "The three points of a triangle must not be collinear.",
                    nameof(third));

            myFirst = first;
            mySecond = second;
            myThird = third;
            myColor = color;
            myFront = front.AsUnit();
        }

        public Point3DF First { get => myFirst; }
        public Point3DF Second { get => mySecond; }
        public Point3DF Third { get => myThird; }
        public Vector3DF Front { get => myFront; }

        public System.Drawing.Color Color { get => myColor; }

        private readonly Point3DF myFirst;
        private readonly Point3DF mySecond;
        private readonly Point3DF myThird;
        private readonly Vector3DF myFront;
        private readonly System.Drawing.Color myColor;
    }

    public class Object3DF
    {
        public IList<Triangle3DF> Triangles { get; } = new List<Triangle3DF>();
    }

    public class Cube3DF : Object3DF
    {
        public Cube3DF(double size)
        {
            float h = (float)(size / 2.0);

            /* Points are numbered like this:
             *
             *     6__________7
             *    /|         /|
             *   / |        / |
             *  /  |       /  |
             * 5__________8   |
             * |   |      |   |
             * |   |      |   |
             * |   2______|___3
             * |  /       |  /
             * | /        | /
             * |/         |/
             * 1__________4
             */

            point1 = new(x: -h, y: -h, z: 0);
            point2 = new(x: -h, y: h, z: 0);
            point3 = new(x: h, y: h, z: 0);
            point4 = new(x: h, y: -h, z: 0);

            point5 = new(x: -h, y: -h, z: h + h);
            point6 = new(x: -h, y: h, z: h + h);
            point7 = new(x: h, y: h, z: h + h);
            point8 = new(x: h, y: -h, z: h + h);

            this.Triangles.Add(new Triangle3DF(
                first: point1, second: point4, third: point2,
                color: Color.CornflowerBlue));

            this.Triangles.Add(new Triangle3DF(
                first: point3, second: point2, third: point4,
                color: Color.CornflowerBlue));

            this.Triangles.Add(new Triangle3DF(
                first: point4, second: point8, third: point7,
                color: Color.IndianRed));

            this.Triangles.Add(new Triangle3DF(
                first: point7, second: point3, third: point4,
                color: Color.IndianRed));

            this.Triangles.Add(new Triangle3DF(
                first: point3, second: point7, third: point6,
                color: Color.DarkGreen));

            this.Triangles.Add(new Triangle3DF(
                first: point3, second: point6, third: point2,
                color: Color.DarkGreen));

            this.Triangles.Add(new Triangle3DF(
                first: point2, second: point6, third: point5,
                color: Color.DarkOrange));

            this.Triangles.Add(new Triangle3DF(
                first: point2, second: point5, third: point1,
                color: Color.DarkOrange));

            this.Triangles.Add(new Triangle3DF(
                first: point1, second: point5, third: point8,
                color: Color.Navy));

            this.Triangles.Add(new Triangle3DF(
                first: point1, second: point8, third: point4,
                color: Color.Navy));

            this.Triangles.Add(new Triangle3DF(
                first: point5, second: point6, third: point7,
                color: Color.LightPink));

            this.Triangles.Add(new Triangle3DF(
                first: point5, second: point7, third: point8,
                color: Color.LightPink));
        }

        /* Points are numbered like this:
         *
         *    6_________7
         *   /|        /|
         *  / |       / |
         * 5_________8  |
         * |  |      |  |
         * |  |      |  |
         * |  2______|__3
         * | /       | /
         * |/        |/
         * 1_________4 */

        private readonly Point3DF point1, point2, point3, point4;
        private readonly Point3DF point5, point6, point7, point8;
    }
}

