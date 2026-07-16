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
        private NewPoint3D myCamera;
        private NewVector3D myWayForward;
        private NewVector3D myWayRight;
        private NewVector3D myWayUp;

        public TelevisionScreen()
        {
            myCamera = new(x: 0.0F, y: -400.0F, z: 400.0F);

            NewPoint3D origin = new(x: 0.0F, y: 0.0F, z: 0.0F);
            myWayForward = origin.Minus(myCamera).AsUnit();

            NewVector3D tempUp = new NewVector3D(x: myWayForward.X,
                y: myWayForward.Y, z: -myWayForward.Z);

            myWayRight = myWayForward.Cross(tempUp).AsUnit();

            myWayUp = myWayRight.Cross(myWayForward).AsUnit();

            _cameraPosition = 0;

            _cameraPositions =
                new List<Tuple<NewPoint3D, NewVector3D, NewVector3D, NewVector3D>>();

            _cameraPositions.Add(Tuple.Create(myCamera, myWayForward, myWayRight, myWayUp));

            for (int x1 = 40; x1 <= 400; x1 += 40)
            {
                NewPoint3D _cameraPosition1 = new(x: x1, y: -400.0F, z: 400.0F);
                NewVector3D _cameraForward1 = origin.Minus(_cameraPosition1).AsUnit();
                NewVector3D _cameraUp1 = new NewVector3D(x: _cameraForward1.X,
                    y: _cameraForward1.Y, z: -_cameraForward1.Z);
                NewVector3D _cameraRight1 = _cameraForward1.Cross(_cameraUp1).AsUnit();
                NewVector3D _cameraUp2 = _cameraRight1.Cross(_cameraForward1).AsUnit();

                _cameraPositions.Add(Tuple.Create(_cameraPosition1,
                    _cameraForward1, _cameraRight1, _cameraUp1));
            }

            for (int y1 = -360; y1 <= -280; y1 += 10)
            {
                NewPoint3D _cameraPosition1 = new(x: 400.0F, y: y1, z: 400.0F);
                NewVector3D _cameraForward1 = origin.Minus(_cameraPosition1).AsUnit();
                NewVector3D _cameraUp1 = new NewVector3D(x: _cameraForward1.X,
                    y: _cameraForward1.Y, z: -_cameraForward1.Z);
                NewVector3D _cameraRight1 = _cameraForward1.Cross(_cameraUp1).AsUnit();
                NewVector3D _cameraUp2 = _cameraRight1.Cross(_cameraForward1).AsUnit();

                _cameraPositions.Add(Tuple.Create(_cameraPosition1,
                    _cameraForward1, _cameraRight1, _cameraUp1));
            }

            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw, value: true);

            BackColor = Color.Black;

            MouseDown += TelevisionScreen_MouseDown;
            MouseMove += TelevisionScreen_MouseMove;
            MouseUp += TelevisionScreen_MouseUp;

            // Demo content so the control shows something useful out of the box.
            Objects.Add(Mesh3D.CreateCube(100, Color.Cyan));
        }

        public List<Mesh3D> Objects { get; } = new List<Mesh3D>();

        [Description("Where the camera sits in world space.")]
        public NewPoint3D CameraPosition { get => myCamera; }

        [Description("Where the camera is pointing. Always a unit vector.")]
        public NewVector3D CameraDirection { get => myWayForward; }

        [Description("Which way is to the right of the center of the image. Always a unit vector.")]
        public NewVector3D ThisWayRight { get => myWayRight; }

        [Description("Which way is up in the image. Always a unit vector.")]
        public NewVector3D ThisWayUp { get => myWayUp; }

        public void SetCamera(
            NewPoint3D position,
            NewVector3D direction,
            NewVector3D thisWayUp)
        {
            if (direction.IsZero)
                throw new ArgumentException(
                    nameof(direction) + " cannot be the zero vector.");

            if (thisWayUp.IsZero)
                throw new ArgumentException(
                    nameof(thisWayUp) + " cannot be the zero vector.");

            NewVector3D unitDirection = direction.AsUnit();

            NewVector3D unitThisWayUp = thisWayUp.AsUnit();

            NewVector3D orthogonalRight = unitDirection.Cross(unitThisWayUp);

            if (orthogonalRight.IsZero)
                throw new ArgumentException(
                    nameof(direction) +
                    " cannot be parallel to " +
                    nameof(thisWayUp));

            NewVector3D unitRight = orthogonalRight.AsUnit();

            NewVector3D orthogonalUp = unitRight.Cross(unitDirection);

            NewVector3D unitUp = orthogonalUp.AsUnit();

            myCamera = position;
            myWayForward = unitDirection;
            myWayRight = unitRight;
            myWayUp = unitUp;
            Invalidate();
        }

        public float DotsPerInch { get; set; } = 3.0F;

        public bool UsePerspective { get; set; } = true;

        //public float CameraDistance { get; set; } = 400.0F;

        private int _cameraPosition = 0;

        private readonly IList<Tuple<NewPoint3D, NewVector3D,
            NewVector3D, NewVector3D>> _cameraPositions;

        private void TelevisionScreen_MouseDown(
            object? sender, MouseEventArgs e)
        {
        }

        private void TelevisionScreen_MouseMove(
            object? sender, MouseEventArgs e)
        {
        }

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
        private PointF Project(NewPoint3D v)
        {
            // Transform into camera space: position relative to the eye,
            // expressed in terms of the camera's right/up/forward basis.
            NewVector3D relative = v.Minus(myCamera);

            float imageX = relative.Dot(myWayRight),
                depthY = relative.Dot(myWayForward), imageZ = relative.Dot(myWayUp);

            if (UsePerspective)
            {
                //imageX *= CameraDistance;
                //imageZ *= CameraDistance;

                //if (depthY > 1.0)
                //{
                //    imageX /= depthY;
                //    imageZ /= depthY;
                //}
            }

            return new PointF(
                x: Width / 2.0F + imageX /* * DotsPerInch */,
                y: Height / 2.0F - imageZ /* * DotsPerInch */);
        }

        [Description("Camera-space depth of a face's vertices (averaged), used for simple painter's-algorithm sorting.")]
        private float FaceDepth(Mesh3D mesh, int[] faceIndices)
        {
            float sum = 0;
            foreach (int idx in faceIndices)
            {
                NewVector3D relative = mesh.Vertices[idx].Minus(myCamera);
                sum += relative.Dot(myWayForward);
            }
            return sum / faceIndices.Length;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(BackColor);

            foreach (Mesh3D mesh in Objects)
            {
                foreach (Triangle3D tri in mesh.Triangles)
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

                if (mesh.DrawFaces && mesh.Faces.Count > 0)
                {
                    // Painter's algorithm: draw faces furthest from the camera first.
                    var orderedFaces = mesh.Faces
                        .OrderByDescending(f => FaceDepth(mesh, f));

                    using (var faceBrush = new SolidBrush(mesh.FaceColor))
                    using (var facePen = new Pen(mesh.EdgeColor))
                    {
                        foreach (int[] face in orderedFaces)
                        {
                            PointF[] pts = face.Select(idx => Project(mesh.Vertices[idx])).ToArray();
                            e.Graphics.FillPolygon(faceBrush, pts);
                            if (mesh.DrawEdges)
                                e.Graphics.DrawPolygon(facePen, pts);
                        }
                    }
                }
                else if (mesh.DrawEdges)
                {
                    using (var pen = new Pen(mesh.EdgeColor, 1.5f))
                    {
                        foreach (var edge in mesh.Edges)
                        {
                            PointF p1 = Project(mesh.Vertices[edge.Item1]);
                            PointF p2 = Project(mesh.Vertices[edge.Item2]);
                            e.Graphics.DrawLine(pen, p1, p2);
                        }
                    }
                }
            }
        }
    }

#pragma warning disable CS0659, IDE0290
    [Description("A simple vector in 3D space.")]
    public class NewVector3D
    {
        public NewVector3D(
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

        public bool Equals(NewVector3D that) =>
            this.xx == that.xx && this.yy == that.yy && this.zz == that.zz;

        public override bool Equals(object? obj) =>
            obj is NewVector3D that
            && this.xx == that.xx && this.yy == that.yy && this.zz == that.zz;

        public override string ToString() =>
            "[" + nameof(X) + ":" + xx.ToString() + ", " +
            nameof(Y) + ":" + yy.ToString() + ", " +
            nameof(Z) + ":" + zz.ToString() + "]";

        public NewVector3D Negate() => new(x: -xx, y: -yy, z: -zz);

        public NewVector3D Plus(NewVector3D that) =>
            new(x: xx + that.xx, y: yy + that.yy, z: zz + that.zz);

        public NewVector3D Minus(NewVector3D that) =>
            new(x: xx - that.xx, y: yy - that.yy, z: zz - that.zz);

        public NewVector3D Times(float that) =>
            new(x: xx * that, y: yy * that, z: zz * that);

        [Description("Rotate this point about the X axis by the given angle (radians).")]
        public NewVector3D RotatedX(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new NewVector3D(xx, yy * cos - zz * sin, yy * sin + zz * cos);
        }

        [Description("Rotate this point about the Y axis by the given angle (radians).")]
        public NewVector3D RotatedY(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new NewVector3D(xx * cos + zz * sin, yy, -xx * sin + zz * cos);
        }

        [Description("Rotate this point about the Z axis by the given angle (radians).")]
        public NewVector3D NewRotatedZ(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new NewVector3D(xx * cos - yy * sin, xx * sin + yy * cos, zz);
        }

        public float Dot(NewVector3D that) =>
            xx * that.xx + yy * that.yy + zz * that.zz;

        public NewVector3D Cross(NewVector3D that) =>
            new(x: yy * that.zz - zz * that.yy,
            y: zz * that.xx - xx * that.zz,
            z: xx * that.yy - yy * that.xx);

        [Description("A unit vector in the same direction as this vector")]
        public NewVector3D AsUnit() => IsZero ?
            throw new DivideByZeroException(
                    nameof(AsUnit) + " can be called only if IsZero is false")
            : this.Times(1.0F / Length);

        private readonly float xx, yy, zz;
    }

#pragma warning disable CS0659, IDE0290
    [Description("A simple point in 3D space.")]
    public class NewPoint3D
    {
        public NewPoint3D(
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

        public bool Equals(NewPoint3D that) =>
            this.xx == that.xx && this.yy == that.yy && this.zz == that.zz;

        public override bool Equals(object? obj) =>
            obj is NewPoint3D that
            && this.xx == that.xx && this.yy == that.yy && this.zz == that.zz;

        public override string ToString() =>
            "(" + nameof(X) + ":" + xx.ToString() + ", " +
            nameof(Y) + ":" + yy.ToString() + ", " +
            nameof(Z) + ":" + zz.ToString() + ")";

        public NewPoint3D Plus(NewVector3D that) =>
            new(x: xx + that.X, y: yy + that.Y, z: zz + that.Z);

        public NewPoint3D Minus(NewVector3D that) =>
            new(x: xx - that.X, y: yy - that.Y, z: zz - that.Z);

        public NewVector3D Minus(NewPoint3D that) =>
            new(x: xx - that.xx, y: yy - that.yy, z: zz - that.zz);

        [Description("Rotate this point about the X axis by the given angle (radians).")]
        public NewPoint3D RotatedX(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new(xx, yy * cos - zz * sin, yy * sin + zz * cos);
        }

        [Description("Rotate this point about the Y axis by the given angle (radians).")]
        public NewPoint3D RotatedY(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new(xx * cos + zz * sin, yy, -xx * sin + zz * cos);
        }

        [Description("Rotate this point about the Z axis by the given angle (radians).")]
        public NewPoint3D RotatedZ(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new(xx * cos - yy * sin, xx * sin + yy * cos, zz);
        }

        private readonly float xx, yy, zz;
    }

    public readonly struct Triangle3D
    {
        public Triangle3D(
            NewPoint3D first,
            NewPoint3D second,
            NewPoint3D third,
            System.Drawing.Color color)
        {
            NewVector3D front = third.Minus(second)
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

        public NewPoint3D First { get => myFirst; }
        public NewPoint3D Second { get => mySecond; }
        public NewPoint3D Third { get => myThird; }
        public NewVector3D Front { get => myFront; }

        public System.Drawing.Color Color { get => myColor; }

        private readonly NewPoint3D myFirst;
        private readonly NewPoint3D mySecond;
        private readonly NewPoint3D myThird;
        private readonly NewVector3D myFront;
        private readonly System.Drawing.Color myColor;
    }

    /// <summary>
    /// A wireframe (and optionally filled) 3D object made of vertices, edges and faces.
    /// Faces are defined as arrays of indices into Vertices, listed in order
    /// (used for optional filled-polygon rendering with simple depth sorting).
    /// </summary>
    public class Mesh3D
    {
        public IList<Triangle3D> Triangles { get; } = new List<Triangle3D>();
        public IList<NewPoint3D> Vertices { get; } = new List<NewPoint3D>();
        public List<Tuple<int, int>> Edges { get; } = new List<Tuple<int, int>>();
        public IList<int[]> Faces { get; } = new List<int[]>();
        public Color EdgeColor { get; set; } = Color.Lime;
        public Color FaceColor { get; set; } = Color.FromArgb(80, Color.Lime);
        public bool DrawFaces { get; set; } = false;
        public bool DrawEdges { get; set; } = true;

        /// <summary>Builds a simple cube centered on the origin with the given side length.</summary>
        public static Mesh3D CreateCube(double size, Color? color = null)
        {
            float h = (float)(size / 2.0);
            var mesh = new Mesh3D();

            /*
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

            NewPoint3D point1 = new(x: -h, y: -h, z: 0);
            NewPoint3D point2 = new(x: -h, y: h, z: 0);
            NewPoint3D point3 = new(x: h, y: h, z: 0);
            NewPoint3D point4 = new(x: h, y: -h, z: 0);

            NewPoint3D point5 = new(x: -h, y: -h, z: h + h);
            NewPoint3D point6 = new(x: -h, y: h, z: h + h);
            NewPoint3D point7 = new(x: h, y: h, z: h + h);
            NewPoint3D point8 = new(x: h, y: -h, z: h + h);

            mesh.Triangles.Add(new Triangle3D(
                first: point1, second: point4, third: point2,
                color: Color.Red));

            mesh.Triangles.Add(new Triangle3D(
                first: point3, second: point2, third: point4,
                color: Color.Chartreuse));

            mesh.Triangles.Add(new Triangle3D(
                first: point4, second: point8, third: point7,
                color: Color.Beige));

            mesh.Triangles.Add(new Triangle3D(
                first: point7, second: point3, third: point4,
                color: Color.DarkGoldenrod));

            Trace.WriteLine("Triangle 1: " + mesh.Triangles[0].First);
            Trace.WriteLine("Triangle 2: " + mesh.Triangles[0].Second);
            Trace.WriteLine("Triangle 3: " + mesh.Triangles[0].Third);
            Trace.WriteLine("Triangle Front: " + mesh.Triangles[0].Front);


            if (color.HasValue)
            {
                mesh.EdgeColor = color.Value;
                mesh.FaceColor = Color.FromArgb(80, color.Value);
            }

            // 8 corners of the cube.
            mesh.Vertices.Add(item: new NewPoint3D(x: -h, y: -h, z: -h)); // 0
            mesh.Vertices.Add(item: new NewPoint3D(x: h, y: -h, z: -h)); // 1
            mesh.Vertices.Add(item: new NewPoint3D(x: h, y: h, z: -h)); // 2
            mesh.Vertices.Add(item: new NewPoint3D(x: -h, y: h, z: -h)); // 3
            mesh.Vertices.Add(item: new NewPoint3D(x: -h, y: -h, z: h)); // 4
            mesh.Vertices.Add(item: new NewPoint3D(x: h, y: -h, z: h)); // 5
            mesh.Vertices.Add(item: new NewPoint3D(x: h, y: h, z: h)); // 6
            mesh.Vertices.Add(item: new NewPoint3D(x: -h, y: h, z: h)); // 7

            int[,] edgeIndices =
            {
                {0,1},{1,2},{2,3},{3,0}, // back face
                {4,5},{5,6},{6,7},{7,4}, // front face
                {0,4},{1,5},{2,6},{3,7}, // connectors
            };

            for (int i = 0; i < edgeIndices.GetLength(0); i++)
                mesh.Edges.Add(Tuple.Create(edgeIndices[i, 0], edgeIndices[i, 1]));

            mesh.Faces.Add(new[] { 0, 1, 2, 3 }); // back
            mesh.Faces.Add(new[] { 4, 5, 6, 7 }); // front
            mesh.Faces.Add(new[] { 0, 1, 5, 4 }); // bottom
            mesh.Faces.Add(new[] { 2, 3, 7, 6 }); // top
            mesh.Faces.Add(new[] { 0, 3, 7, 4 }); // left
            mesh.Faces.Add(new[] { 1, 2, 6, 5 }); // right

            return mesh;
        }
    }
}

