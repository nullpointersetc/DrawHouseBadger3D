#pragma warning disable IDE0130
namespace NullPointersEtc.DrawHouseBadger3D
{
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    class DrawBadger : System.Windows.Forms.UserControl
    {
        public DrawBadger(BadgerPose p, ViewDirection v) {
            pose = p;
            view = v;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // --- Pose-dependent limb geometry (shared by Front and Back views) ---
            (LegPose leftLeg, LegPose rightLeg, ArmPose leftArm, ArmPose rightArm) limbs = pose switch
            {
                BadgerPose.Standing => (
                    leftLeg: new LegPose(210, 420, 65, 0, 65, 0, 40),
                    rightLeg: new LegPose(300, 420, 65, 0, 65, 0, 40),
                    leftArm: new ArmPose(122, 290, 65, -20, 65, 0, 42),
                    rightArm: new ArmPose(387, 290, 65, 20, 65, 0, 42)
                ),
                BadgerPose.Waving => (
                    leftLeg: new LegPose(210, 420, 65, 0, 65, 0, 40),
                    rightLeg: new LegPose(300, 420, 65, 0, 65, 0, 40),
                    leftArm: new ArmPose(122, 290, 65, -15, 65, 0, 42),
                    rightArm: new ArmPose(387, 290, 65, 150, 65, -40, 42)
                ),
                BadgerPose.MidStride => (
                    leftLeg: new LegPose(190, 420, 65, -25, 65, 0, 40),
                    rightLeg: new LegPose(320, 420, 65, 25, 65, -20, 40),
                    leftArm: new ArmPose(112, 290, 65, 25, 65, 15, 42),
                    rightArm: new ArmPose(397, 290, 65, -25, 65, -15, 42)
                ),
                BadgerPose.Sitting => (
                    leftLeg: new LegPose(210, 420, 65, -75, 65, 65, 40),
                    rightLeg: new LegPose(300, 420, 65, 75, 65, -65, 40),
                    leftArm: new ArmPose(122, 290, 65, -70, 65, 20, 42),
                    rightArm: new ArmPose(387, 290, 65, 70, 65, -20, 42)
                ),
                _ => throw new ArgumentOutOfRangeException()
            };

            RenderView(e.Graphics, view, limbs);
        }

        void RenderView(Graphics g,
            ViewDirection view,
            (LegPose leftLeg, LegPose rightLeg, ArmPose leftArm, ArmPose rightArm) limbs)
        {
            canvasWidth = ClientSize.Width;
            canvasHeight = ClientSize.Height;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillRectangle(bgBrush, 0, 0, canvasWidth, canvasHeight);

            switch (view)
            {
                case ViewDirection.Front:
                    DrawBody(g, limbs);
                    DrawVestFront(g);
                    DrawHeadFront(g);
                    break;

                case ViewDirection.Back:
                    {
                        // Viewed from behind, left/right are mirrored relative to the front view,
                        // so the whole skeleton is flipped horizontally.
                        var state = g.Save();
                        ApplyHorizontalFlip(g);
                        DrawBody(g, limbs);
                        DrawHeadBack(g);
                        g.Restore(state);
                        break;
                    }

                case ViewDirection.Left:
                    DrawProfile(g); // designed facing left
                    break;

                case ViewDirection.Right:
                    {
                        var state = g.Save();
                        ApplyHorizontalFlip(g);
                        DrawProfile(g); // same geometry, mirrored so it faces right
                        g.Restore(state);
                        break;
                    }
            }
        }
        void ApplyHorizontalFlip(Graphics g)
        {
            g.TranslateTransform(canvasWidth, 0);
            g.ScaleTransform(-1, 1);
        }

        // --- Shared front/back body: legs, torso, arms ---
        void DrawBody(Graphics g, (LegPose leftLeg, LegPose rightLeg, ArmPose leftArm, ArmPose rightArm) limbs)
        {
            DrawLeg(g, furBrush, furOutline, limbs.leftLeg);
            DrawLeg(g, furBrush, furOutline, limbs.rightLeg);
            FillRoundRectangle(g, furBrush, furOutline, new RectPose(160, 260, 190, 190, 0, 0, 0), radius: 40);
            DrawArm(g, furBrush, furOutline, limbs.leftArm);
            DrawArm(g, furBrush, furOutline, limbs.rightArm);
        }

        void DrawVestFront(Graphics g)
        {
            Point[] vest = {
        new Point(170, 270), new Point(340, 270),
        new Point(325, 400), new Point(185, 400)
    };
            g.FillPolygon(vestBrush, vest);
            g.DrawPolygon(furOutline, vest);
        }

        // --- Front-facing head: ears, black/white face stripes, eyes, nose, mouth ---
        void DrawHeadFront(Graphics g)
        {
            Rectangle headRect = new Rectangle(160, 100, 190, 170);
            g.FillEllipse(furBrush, headRect);
            g.DrawEllipse(furOutline, headRect);

            g.FillEllipse(furBrush, 160, 90, 45, 50);
            g.DrawEllipse(furOutline, 160, 90, 45, 50);
            g.FillEllipse(furBrush, 305, 90, 45, 50);
            g.DrawEllipse(furOutline, 305, 90, 45, 50);
            g.FillEllipse(lightBrush, 172, 105, 22, 28);
            g.FillEllipse(lightBrush, 317, 105, 22, 28);

            using (GraphicsPath leftStripe = RoundedStripe(190, 110, 45, 150))
                g.FillPath(lightBrush, leftStripe);
            using (GraphicsPath rightStripe = RoundedStripe(275, 110, 45, 150))
                g.FillPath(lightBrush, rightStripe);
            using (GraphicsPath centerStripe = RoundedStripe(232, 100, 46, 170))
                g.FillPath(blackBrush, centerStripe);

            g.DrawEllipse(furOutline, headRect);

            g.FillEllipse(blackBrush, 210, 165, 16, 20);
            g.FillEllipse(blackBrush, 284, 165, 16, 20);
            g.FillEllipse(Brushes.White, 214, 168, 5, 5);
            g.FillEllipse(Brushes.White, 288, 168, 5, 5);

            g.FillEllipse(blackBrush, 238, 210, 34, 24);

            using Pen mouthPen = new Pen(Color.Black, 3);
            g.DrawArc(mouthPen, 220, 220, 30, 25, 30, 100);
            g.DrawArc(mouthPen, 260, 220, 30, 25, 50, 100);
        }

        // --- Back-of-head: ears, a single dorsal stripe, no facial features ---
        void DrawHeadBack(Graphics g)
        {
            Rectangle headRect = new Rectangle(160, 100, 190, 170);
            g.FillEllipse(furBrush, headRect);
            g.DrawEllipse(furOutline, headRect);

            g.FillEllipse(furBrush, 160, 90, 45, 50);
            g.DrawEllipse(furOutline, 160, 90, 45, 50);
            g.FillEllipse(furBrush, 305, 90, 45, 50);
            g.DrawEllipse(furOutline, 305, 90, 45, 50);

            // Badgers have a pale dorsal stripe running down the back of the head/spine.
            using (GraphicsPath dorsalStripe = RoundedStripe(232, 100, 36, 170))
                g.FillPath(lightBrush, dorsalStripe);
            using (GraphicsPath dorsalStripeOnBody = RoundedStripe(238, 260, 24, 190))
                g.FillPath(lightBrush, dorsalStripeOnBody);

            g.DrawEllipse(furOutline, headRect);
        }
        // --- Side-profile view, designed facing LEFT (snout points toward x=0). ---
        // Draw order matters here: far limbs first (so the torso overlaps them),
        // then torso, then near limbs on top, then the head last (furthest forward).
        void DrawProfile(Graphics g)
        {
            var farLeg = new LegPose(255, 420, 62, 15, 62, -10, 34);
            var nearLeg = new LegPose(245, 420, 65, -15, 65, 10, 40);
            var farArm = new ArmPose(255, 290, 58, 10, 58, 0, 30);
            var nearArm = new ArmPose(245, 290, 62, -10, 62, 0, 38);

            DrawLeg(g, furBrush, furOutline, farLeg);
            DrawArm(g, furBrush, furOutline, farArm);

            // Torso: narrower than the front view, since we're seeing it edge-on-ish.
            var torso = new RectPose(185, 260, 130, 190, 0, 0, 0);
            FillRoundRectangle(g, furBrush, furOutline, torso, radius: 38);

            // Simple side vest patch.
            using (Brush vp = new SolidBrush(Color.FromArgb(160, 60, 40)))
            {
                var vestPatch = new RectPose(195, 275, 110, 120, 0, 0, 0);
                FillRoundRectangle(g, vp, furOutline, vestPatch, radius: 20);
            }

            DrawLeg(g, furBrush, furOutline, nearLeg);
            DrawArm(g, furBrush, furOutline, nearArm);

            // Far ear peeks out slightly behind the near ear for a hint of depth.
            g.FillEllipse(furBrush, 285, 100, 26, 30);
            g.DrawEllipse(furOutline, 285, 100, 26, 30);

            // Skull.
            Rectangle skull = new Rectangle(185, 85, 130, 130);
            g.FillEllipse(furBrush, skull);
            g.DrawEllipse(furOutline, skull);

            // Snout, extending toward the left (facing direction).
            using (GraphicsPath snout = RoundedRect(133, 143, 60, 46, 20))
            {
                g.FillPath(furBrush, snout);
                g.DrawPath(furOutline, snout);
            }

            // Near ear, on top of the skull.
            g.FillEllipse(furBrush, 262, 90, 40, 45);
            g.DrawEllipse(furOutline, 262, 90, 40, 45);
            g.FillEllipse(lightBrush, 272, 100, 20, 26);

            // Badger face stripe, running horizontally along the skull and snout.
            using (GraphicsPath whiteStripe = RoundedStripeHorizontal(138, 113, 165, 42))
                g.FillPath(lightBrush, whiteStripe);
            using (GraphicsPath blackStripe = RoundedStripeHorizontal(138, 126, 165, 16))
                g.FillPath(blackBrush, blackStripe);

            // Eye.
            g.FillEllipse(blackBrush, 222, 128, 16, 18);
            g.FillEllipse(Brushes.White, 226, 131, 5, 5);

            // Nose, at the tip of the snout.
            g.FillEllipse(blackBrush, 128, 155, 24, 22);

            // Mouth line.
            using Pen mouthPen = new Pen(Color.Black, 3);
            g.DrawLine(mouthPen, 138, 178, 175, 182);
        }

        private static void FillRoundRectangle(Graphics g,
            Brush brush, Pen pen, RectPose r, float radius = 15)
        {
            var state = g.Save();
            if (r.RotationDegrees != 0)
            {
                g.TranslateTransform(r.PivotX, r.PivotY);
                g.RotateTransform(r.RotationDegrees);
                g.TranslateTransform(-r.PivotX, -r.PivotY);
            }
            using var path = RoundedRect(r.X, r.Y, r.W, r.H, radius);
            g.FillPath(brush, path);
            g.DrawPath(pen, path);
            g.Restore(state);
        }

        public static GraphicsPath RoundedRect(float x, float y, float w, float h, float r)
        {
            var path = new GraphicsPath();
            path.AddArc(x, y, r, r, 180, 90);
            path.AddArc(x + w - r, y, r, r, 270, 90);
            path.AddArc(x + w - r, y + h - r, r, r, 0, 90);
            path.AddArc(x, y + h - r, r, r, 90, 90);
            path.CloseFigure();
            return path;
        }

        static GraphicsPath RoundedStripe(float x, float y, float w, float h)
        {
            // Vertical capsule (rounded top/bottom), used for the front-view face stripes.
            float r = w / 2f;
            var path = new GraphicsPath();
            path.AddArc(x, y, r * 2, r * 2, 180, 180);
            path.AddLine(x + w, y + r, x + w, y + h - r);
            path.AddArc(x, y + h - r * 2, r * 2, r * 2, 0, 180);
            path.CloseFigure();
            return path;
        }

        static GraphicsPath RoundedStripeHorizontal(float x, float y, float w, float h)
        {
            // Horizontal capsule (rounded left/right ends), used for the profile-view stripe.
            float r = h / 2f;
            var path = new GraphicsPath();
            path.AddArc(x, y, r * 2, r * 2, 90, 180);
            path.AddLine(x + r, y, x + w - r, y);
            path.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 180);
            path.CloseFigure();
            return path;
        }

        static void DrawLeg(Graphics g, Brush brush, Pen pen, LegPose leg)
        {
            var thighRect = new RectPose(leg.HipX - leg.Width / 2f, leg.HipY, leg.Width, leg.ThighLength, leg.ThighAngle, leg.HipX, leg.HipY);
            FillRoundRectangle(g, brush, pen, thighRect, radius: 15);

            PointF knee = RotatePoint(leg.HipX, leg.HipY + leg.ThighLength, leg.HipX, leg.HipY, leg.ThighAngle);

            float shinAbsoluteAngle = leg.ThighAngle + leg.KneeBend;
            var shinRect = new RectPose(knee.X - leg.Width / 2f, knee.Y, leg.Width, leg.ShinLength, shinAbsoluteAngle, knee.X, knee.Y);
            FillRoundRectangle(g, brush, pen, shinRect, radius: 15);

            float kneeCapSize = leg.Width * 0.7f;
            g.FillEllipse(brush, knee.X - kneeCapSize / 2, knee.Y - kneeCapSize / 2, kneeCapSize, kneeCapSize);
            g.DrawEllipse(pen, knee.X - kneeCapSize / 2, knee.Y - kneeCapSize / 2, kneeCapSize, kneeCapSize);

            PointF ankle = RotatePoint(knee.X, knee.Y + leg.ShinLength, knee.X, knee.Y, shinAbsoluteAngle);

            float footW = 60, footH = 30;
            var state = g.Save();
            g.TranslateTransform(ankle.X, ankle.Y);
            g.RotateTransform(shinAbsoluteAngle);
            using var footPath = RoundedRect(-footW / 2f, 0, footW, footH, 10);
            g.FillPath(brush, footPath);
            g.DrawPath(pen, footPath);
            g.Restore(state);
        }

        static void DrawArm(Graphics g, Brush brush, Pen pen, ArmPose arm)
        {
            var upperArmRect = new RectPose(arm.ShoulderX - arm.Width / 2f, arm.ShoulderY, arm.Width, arm.UpperArmLength, arm.UpperArmAngle, arm.ShoulderX, arm.ShoulderY);
            FillRoundRectangle(g, brush, pen, upperArmRect, radius: 18);

            PointF elbow = RotatePoint(arm.ShoulderX, arm.ShoulderY + arm.UpperArmLength, arm.ShoulderX, arm.ShoulderY, arm.UpperArmAngle);

            float forearmAbsoluteAngle = arm.UpperArmAngle + arm.ElbowBend;
            var forearmRect = new RectPose(elbow.X - arm.Width / 2f, elbow.Y, arm.Width, arm.ForearmLength, forearmAbsoluteAngle, elbow.X, elbow.Y);
            FillRoundRectangle(g, brush, pen, forearmRect, radius: 18);

            float elbowCapSize = arm.Width * 0.7f;
            g.FillEllipse(brush, elbow.X - elbowCapSize / 2, elbow.Y - elbowCapSize / 2, elbowCapSize, elbowCapSize);
            g.DrawEllipse(pen, elbow.X - elbowCapSize / 2, elbow.Y - elbowCapSize / 2, elbowCapSize, elbowCapSize);

            PointF wrist = RotatePoint(elbow.X, elbow.Y + arm.ForearmLength, elbow.X, elbow.Y, forearmAbsoluteAngle);

            float pawSize = 45;
            g.FillEllipse(brush, wrist.X - pawSize / 2, wrist.Y - pawSize / 2, pawSize, pawSize);
            g.DrawEllipse(pen, wrist.X - pawSize / 2, wrist.Y - pawSize / 2, pawSize, pawSize);
        }

        static PointF RotatePoint(float px, float py, float pivotX, float pivotY, float degrees)
        {
            double rad = degrees * Math.PI / 180.0;
            float dx = px - pivotX;
            float dy = py - pivotY;
            float rx = (float)(dx * Math.Cos(rad) - dy * Math.Sin(rad));
            float ry = (float)(dx * Math.Sin(rad) + dy * Math.Cos(rad));
            return new PointF(pivotX + rx, pivotY + ry);
        }

        private readonly BadgerPose pose ;
        private readonly ViewDirection view;

        int canvasWidth = 500;
        int canvasHeight = 600;

        static Color furGray { get => Color.FromArgb(90, 90, 95); }
        static Color furLight { get => Color.FromArgb(230, 230, 230); }

        Brush furBrush = new SolidBrush(furGray);
        Brush lightBrush = new SolidBrush(furLight);
        Brush blackBrush = new SolidBrush(Color.Black);
        Pen furOutline = new Pen(Color.Black, 3);
        Brush bgBrush = new SolidBrush(Color.FromArgb(235, 245, 235));
        Brush vestBrush = new SolidBrush(Color.FromArgb(160, 60, 40));
    }

    enum BadgerPose { Standing, Waving, MidStride, Sitting }
    enum ViewDirection { Front, Back, Left, Right }

    struct RectPose
    {
        public float X, Y, W, H, RotationDegrees, PivotX, PivotY;
        public RectPose(float x, float y, float w, float h, float rotationDegrees, float pivotX, float pivotY)
        {
            X = x; Y = y; W = w; H = h;
            RotationDegrees = rotationDegrees;
            PivotX = pivotX; PivotY = pivotY;
        }
    }

    struct LegPose
    {
        public float HipX, HipY, ThighLength, ThighAngle, ShinLength, KneeBend, Width;
        public LegPose(float hipX, float hipY, float thighLength, float thighAngle, float shinLength, float kneeBend, float width)
        {
            HipX = hipX; HipY = hipY;
            ThighLength = thighLength; ThighAngle = thighAngle;
            ShinLength = shinLength; KneeBend = kneeBend;
            Width = width;
        }
    }


    struct ArmPose
    {
        public float ShoulderX, ShoulderY, UpperArmLength, UpperArmAngle, ForearmLength, ElbowBend, Width;
        public ArmPose(float shoulderX, float shoulderY, float upperArmLength, float upperArmAngle, float forearmLength, float elbowBend, float width)
        {
            ShoulderX = shoulderX; ShoulderY = shoulderY;
            UpperArmLength = upperArmLength; UpperArmAngle = upperArmAngle;
            ForearmLength = forearmLength; ElbowBend = elbowBend;
            Width = width;
        }
    }
}
