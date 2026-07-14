#pragma warning disable IDE0130
using System.ComponentModel;

namespace NullPointersEtc.TelevisionScreen
{
    using System.Drawing.Drawing2D;
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
        private Vector3D myCamera;
        private Vector3D myWayForward;
        private Vector3D myWayRight;
        private Vector3D myWayUp;

        public TelevisionScreen()
        {
            myCamera = new(x: 0.0F, y: -1000.0F, z: 1000.0F);
            myWayForward = new(x: 0.0F, y: 724.0F / 1024.0F, z: -724.0F / 1024.0F);
            myWayRight = new(x: 1.0F, y: 0.0F, z: 0.0F);
            myWayUp = myWayRight.Cross(myWayForward);

            SetStyle(ControlStyles.AllPaintingInWmPaint
                     | ControlStyles.UserPaint
                     | ControlStyles.OptimizedDoubleBuffer
                     | ControlStyles.ResizeRedraw, true);

            BackColor = Color.Black;

            MouseDown += TelevisionScreen_MouseDown;
            MouseMove += TelevisionScreen_MouseMove;
            MouseUp += TelevisionScreen_MouseUp;

            // Demo content so the control shows something useful out of the box.
            Objects.Add(Mesh3D.CreateCube(100, Color.Cyan));
        }

        public List<Mesh3D> Objects { get; } = new List<Mesh3D>();

        [Description("Where the camera sits in world space.")]
        public Vector3D CameraPosition
        {
            get => myCamera;
        }

        [Description("Where the camera is pointing. " +
            "When it is set, it is resized to a unit vector.")]
        public Vector3D CameraDirection { get => myWayForward; }

        [Description("Which way is to the right of the center of the image. " +
            "When it is set, it is resized to a unit vector.")]
        public Vector3D ThisWayRight { get => myWayRight; }

        [Description("Which way is up in the image. " +
            "When it is set, it is resized to a unit vector.")]
        public Vector3D ThisWayUp { get => myWayUp; }

        public void SetCamera(
            Vector3D position,
            Vector3D direction,
            Vector3D thisWayUp)
        {
            if (direction.IsZero)
                throw new ArgumentException(
                    nameof(direction) + " cannot be the zero vector.");

            if (thisWayUp.IsZero)
                throw new ArgumentException(
                    nameof(thisWayUp) + " cannot be the zero vector.");

            Vector3D unitDirection = direction.AsUnit();

            Vector3D unitThisWayUp = thisWayUp.AsUnit();

            Vector3D orthogonalRight = unitDirection.Cross(unitThisWayUp);

            if (orthogonalRight.IsZero)
                throw new ArgumentException(
                    nameof(direction) +
                    " cannot be parallel to " +
                    nameof(thisWayUp));

            Vector3D unitRight = orthogonalRight.AsUnit();

            Vector3D orthogonalUp = unitRight.Cross(unitDirection);

            Vector3D unitUp = orthogonalUp.AsUnit();

            myWayForward = unitDirection;
            myWayRight = unitRight;
            myWayUp = unitUp;
            Invalidate();
        }

        /// <summary>Rotation about the X axis, in radians.</summary>
        public float RotationX { get; set; } = 0.4F;

        /// <summary>Rotation about the Y axis, in radians.</summary>
        public float RotationY { get; set; } = 0.6F;

        /// <summary>Rotation about the Z axis, in radians.</summary>
        public float RotationZ { get; set; } = 0.0F;

        /// <summary>Uniform scale applied after projection, in pixels per world unit.</summary>
        public float DotsPerInch { get; set; } = 3.0F;

        /// <summary>If true, uses simple perspective projection; otherwise orthographic.</summary>
        public bool UsePerspective { get; set; } = true;

        /// <summary>
        /// Focal length: the distance from the camera to the virtual projection
        /// plane, used to DotsPerInch perspective projection. This is independent of
        /// CameraPosition and mainly acts as a "zoom" control.
        /// </summary>
        public double CameraDistance { get; set; } = 400.0;

        /// <summary>If true, the mouse can be dragged over the control to rotate the scene.</summary>
        public bool AllowMouseRotate { get; set; } = true;

        private Point _lastMousePos;
        private bool _dragging;

        private void TelevisionScreen_MouseDown(
            object? sender, MouseEventArgs e)
        {
            if (!AllowMouseRotate) return;
            _dragging = true;
            _lastMousePos = e.Location;
        }

        private void TelevisionScreen_MouseMove(
            object? sender, MouseEventArgs e)
        {
            if (!AllowMouseRotate || !_dragging) return;

            int dx = e.Location.X - _lastMousePos.X;
            int dy = e.Location.Y - _lastMousePos.Y;

            // Dragging right yaws the object around the vertical (Z) axis;
            // dragging down pitches it forward around the horizontal (X) axis.
            RotationZ += dx * 0.01F;
            RotationX += dy * 0.01F;

            _lastMousePos = e.Location;
            Invalidate();
        }

        private void TelevisionScreen_MouseUp(
            object? sender, MouseEventArgs e)
        {
            _dragging = false;

            SetCamera(position: new(x: 160.0F, y: -1000.0F, z: 1000.0F),
                direction: new(x: 100.0F, y: 1000.0F, z: -1000.0F),
                thisWayUp: myWayUp);
        }

        /// <summary>
        /// Projects a world-space point to a 2D pixel location on this control,
        /// applying the current object rotation, the camera transform, and the
        /// current scale/projection mode.
        /// </summary>
        private PointF Project(Vector3D v)
        {
            // Apply object rotation (objects spin about the origin in world space).
            Vector3D world = v.RotatedX(RotationX).RotatedY(RotationY).RotatedZ(RotationZ);

            // Transform into camera space: position relative to the eye,
            // expressed in terms of the camera's right/up/forward basis.
            Vector3D relative = world.Minus(myCamera);
            double camX = relative.Dot(myWayRight);
            double camY = relative.Dot(myWayForward);
            double camZ = relative.Dot(myWayUp);

            double px, pz;

            if (UsePerspective)
            {
                double depth = camY;
                if (depth < 1.0) depth = 1.0; // avoid divide-by-zero / points behind the camera
                double factor = CameraDistance / depth;
                px = camX * factor;
                pz = camZ * factor;
            }
            else
            {
                px = camX;
                pz = camZ;
            }

            // Convert view-plane units to pixels. Camera-space +X is to the
            // right (no flip needed); the vertical screen axis is flipped
            // because pixel Y grows downward while camera-space +up does not.
            float screenX = (float)(Width / 2.0 + px * DotsPerInch);
            float screenY = (float)(Height / 2.0 - pz * DotsPerInch);

            return new PointF(screenX, screenY);
        }

        /// <summary>Camera-space depth of a face's vertices (averaged), used for simple painter's-algorithm sorting.</summary>
        private double FaceDepth(Mesh3D mesh, int[] faceIndices)
        {
            double sum = 0;
            foreach (int idx in faceIndices)
            {
                Vector3D world = mesh.Vertices[idx]
                    .RotatedX(RotationX).RotatedY(RotationY).RotatedZ(RotationZ);
                Vector3D relative = world.Minus(myCamera);
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


    [Description("A simple point in 3D space.")]
    public readonly struct Vector3D(
        [Description("Positive x is to the right of the origin. " +
        "Negative x is to the left of the origin.")]
        float x,
        [Description("Positive y is farther away from the viewer than the origin. "+
        "Negative y is nearer to the viewer than the origin.")]
        float y,
        [Description("Positive z is above the XY plane. " +
        "Negative z is below the XY plane.")]
        float z)
        : IEquatable<Vector3D>
    {
        public readonly float X => xx;
        public readonly double Y => yy;
        public readonly double Z => zz;

        public readonly bool Equals(Vector3D that) =>
            this.xx == that.xx && this.yy == that.yy && this.zz == that.zz;

        public override readonly bool Equals(object? obj) =>
            obj is Vector3D that
            && this.xx == that.xx && this.yy == that.yy && this.zz == that.zz;

        public override readonly int GetHashCode() =>
            HashCode.Combine(xx, yy, zz);

        public readonly Vector3D Negate() =>
            new Vector3D(x: -xx, y: -yy, z: -zz);

        public readonly Vector3D Plus(Vector3D that) =>
            new(x: xx + that.xx, y: yy + that.yy, z: zz + that.zz);

        public readonly Vector3D Minus(Vector3D that) =>
            new(x: xx - that.xx, y: yy - that.yy, z: zz - that.zz);

        public readonly Vector3D Times(float that) =>
            new(x: xx * that, y: yy * that, z: zz * that);

        [Description("Rotate this point about the X axis by the given angle (radians).")]
        public Vector3D RotatedX(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new Vector3D(xx, yy * cos - zz * sin, yy * sin + zz * cos);
        }

        [Description("Rotate this point about the Y axis by the given angle (radians).")]
        public Vector3D RotatedY(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new Vector3D(xx * cos + zz * sin, yy, -xx * sin + zz * cos);
        }

        [Description("Rotate this point about the Z axis by the given angle (radians).")]
        public Vector3D RotatedZ(float radians)
        {
            float cos = MathF.Cos(radians), sin = MathF.Sin(radians);
            return new Vector3D(xx * cos - yy * sin, xx * sin + yy * cos, zz);
        }

        public readonly double Dot(Vector3D that) =>
            xx * that.xx + yy * that.yy + zz * that.zz;

        public readonly Vector3D Cross(Vector3D that) =>
            new(x: yy * that.zz - zz * that.yy,
            y: zz * that.xx - xx * that.zz,
            z: xx * that.yy - yy * that.xx);

        public readonly bool IsZero =>
            xx == 0.0F && yy == 0.0F && zz == 0.0F;

        public float Length =>
            MathF.Sqrt(xx * xx + yy * yy + zz * zz);

        [Description("A unit vector in the same direction as this vector")]
        public Vector3D AsUnit() =>
            !IsZero ? Times(1.0F / Length)
                : throw new DivideByZeroException(
                    nameof(AsUnit) + " can be called only if IsZero is false");

        private readonly float xx = x, yy = y, zz = z;
    }

    /// <summary>
    /// A wireframe (and optionally filled) 3D object made of vertices, edges and faces.
    /// Faces are defined as arrays of indices into Vertices, listed in order
    /// (used for optional filled-polygon rendering with simple depth sorting).
    /// </summary>
    public class Mesh3D
    {
        public List<Vector3D> Vertices { get; } = new List<Vector3D>();
        public List<Tuple<int, int>> Edges { get; } = new List<Tuple<int, int>>();
        public List<int[]> Faces { get; } = new List<int[]>();
        public Color EdgeColor { get; set; } = Color.Lime;
        public Color FaceColor { get; set; } = Color.FromArgb(80, Color.Lime);
        public bool DrawFaces { get; set; } = false;
        public bool DrawEdges { get; set; } = true;

        /// <summary>Builds a simple cube centered on the origin with the given side length.</summary>
        public static Mesh3D CreateCube(double size, Color? color = null)
        {
            float h = (float)(size / 2.0);
            var mesh = new Mesh3D();

            if (color.HasValue)
            {
                mesh.EdgeColor = color.Value;
                mesh.FaceColor = Color.FromArgb(80, color.Value);
            }

            // 8 corners of the cube.
            mesh.Vertices.AddRange(new[]
            {
                new Vector3D(-h, -h, -h), // 0
                new Vector3D( h, -h, -h), // 1
                new Vector3D( h,  h, -h), // 2
                new Vector3D(-h,  h, -h), // 3
                new Vector3D(-h, -h,  h), // 4
                new Vector3D( h, -h,  h), // 5
                new Vector3D( h,  h,  h), // 6
                new Vector3D(-h,  h,  h), // 7
            });
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

