#pragma warning disable IDE0130
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace NullPointersEtc.DrawHouseBadger3D
{
    public partial class Program : Form
    {
        public Program()
        {
            statusStrip1 = new();
            draw1stBadgerButton = new();
            draw2ndBadgerButton = new();
            draw3rdBadgerButton = new();
            draw4thBadgerButton = new();
            draw5thBadgerButton = new();
            draw6thBadgerButton = new();
            draw7thBadgerButton = new();
            draw8thBadgerButton = new();
            draw9thBadgerButton = new();
            draw10thBadgerButton = new();
            draw11thBadgerButton = new();
            draw12thBadgerButton = new();
            draw13thBadgerButton = new();
            draw14thBadgerButton = new();
            draw15thBadgerButton = new();
            draw16thBadgerButton = new();
            InitializeComponent();
        }

        [AllowNull, Description("Required designer variable.")]
        private System.ComponentModel.IContainer components = null;

        [Description("Clean up any resources being used.")]
        protected override void Dispose(
            [Description("true if managed resources should be disposed; otherwise, false.")]
            bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            drawButtons = new StatusStrip();
            drawHouseButton = new ToolStripStatusLabel();
            draw3dButton = new ToolStripStatusLabel();
            draw1stBadgerButton = new ToolStripStatusLabel();
            statusStrip1 = new StatusStrip();
            draw8thBadgerButton = new ToolStripStatusLabel();
            draw7thBadgerButton = new ToolStripStatusLabel();
            draw6thBadgerButton = new ToolStripStatusLabel();
            draw5thBadgerButton = new ToolStripStatusLabel();
            draw4thBadgerButton = new ToolStripStatusLabel();
            draw3rdBadgerButton = new ToolStripStatusLabel();
            draw2ndBadgerButton = new ToolStripStatusLabel();
            draw9thBadgerButton = new ToolStripStatusLabel();
            draw10thBadgerButton = new ToolStripStatusLabel();
            draw11thBadgerButton = new ToolStripStatusLabel();
            draw12thBadgerButton = new ToolStripStatusLabel();
            draw14thBadgerButton = new ToolStripStatusLabel();
            draw15thBadgerButton = new ToolStripStatusLabel();
            draw16thBadgerButton = new ToolStripStatusLabel();
            draw13thBadgerButton = new ToolStripStatusLabel();
            drawButtons.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // drawButtons
            // 
            drawButtons.Dock = DockStyle.Top;
            drawButtons.ImageScalingSize = new Size(20, 20);
            drawButtons.Items.AddRange(new ToolStripItem[] { drawHouseButton, draw3dButton, draw1stBadgerButton, draw2ndBadgerButton, draw3rdBadgerButton, draw4thBadgerButton, draw5thBadgerButton, draw6thBadgerButton, draw7thBadgerButton, draw8thBadgerButton, draw9thBadgerButton, draw10thBadgerButton, draw11thBadgerButton, draw12thBadgerButton });
            drawButtons.Location = new Point(0, 0);
            drawButtons.Name = "drawButtons";
            drawButtons.Size = new Size(800, 34);
            drawButtons.TabIndex = 0;
            drawButtons.Text = "statusStrip1";
            // 
            // drawHouseButton
            // 
            drawHouseButton.BackColor = SystemColors.ControlLightLight;
            drawHouseButton.BorderStyle = Border3DStyle.Raised;
            drawHouseButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            drawHouseButton.ForeColor = SystemColors.ControlDark;
            drawHouseButton.Margin = new Padding(4, 2, 4, 2);
            drawHouseButton.Name = "drawHouseButton";
            drawHouseButton.Padding = new Padding(5);
            drawHouseButton.Size = new Size(100, 30);
            drawHouseButton.Text = "Draw House";
            drawHouseButton.Click += OnDrawHouseButtonClicked;
            drawHouseButton.DoubleClick += OnDrawHouseButtonClicked;
            // 
            // draw3dButton
            // 
            draw3dButton.BackColor = SystemColors.ControlDark;
            draw3dButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw3dButton.ForeColor = SystemColors.ControlLightLight;
            draw3dButton.Margin = new Padding(4, 2, 4, 2);
            draw3dButton.Name = "draw3dButton";
            draw3dButton.Padding = new Padding(5);
            draw3dButton.Size = new Size(77, 30);
            draw3dButton.Text = "Draw 3D";
            draw3dButton.Click += OnDraw3dButtonClicked;
            draw3dButton.DoubleClick += OnDraw3dButtonClicked;
            // 
            // drawBadgerButton
            // 
            draw1stBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw1stBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw1stBadgerButton.ForeColor = SystemColors.ControlLight;
            draw1stBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw1stBadgerButton.Name = "drawBadgerButton";
            draw1stBadgerButton.Padding = new Padding(5);
            draw1stBadgerButton.Size = new Size(106, 30);
            draw1stBadgerButton.Text = "Draw Badger 1";
            draw1stBadgerButton.Click += OnDraw1stBadgerButtonClicked;
            draw1stBadgerButton.DoubleClick += OnDraw1stBadgerButtonClicked;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { draw13thBadgerButton, draw14thBadgerButton, draw15thBadgerButton, draw16thBadgerButton });
            statusStrip1.Location = new Point(0, 416);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(800, 34);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            draw8thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw8thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw8thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw8thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw8thBadgerButton.Name = "toolStripStatusLabel1";
            draw8thBadgerButton.Padding = new Padding(5);
            draw8thBadgerButton.Size = new Size(27, 30);
            draw8thBadgerButton.Text = "8";
            // 
            // toolStripStatusLabel2
            // 
            draw7thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw7thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw7thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw7thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw7thBadgerButton.Name = "toolStripStatusLabel2";
            draw7thBadgerButton.Padding = new Padding(5);
            draw7thBadgerButton.Size = new Size(27, 30);
            draw7thBadgerButton.Text = "7";
            // 
            // toolStripStatusLabel3
            // 
            draw6thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw6thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw6thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw6thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw6thBadgerButton.Name = "toolStripStatusLabel3";
            draw6thBadgerButton.Padding = new Padding(5);
            draw6thBadgerButton.Size = new Size(27, 30);
            draw6thBadgerButton.Text = "6";
            // 
            // toolStripStatusLabel4
            // 
            draw5thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw5thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw5thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw5thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw5thBadgerButton.Name = "toolStripStatusLabel4";
            draw5thBadgerButton.Padding = new Padding(5);
            draw5thBadgerButton.Size = new Size(27, 30);
            draw5thBadgerButton.Text = "5";
            // 
            // toolStripStatusLabel5
            // 
            draw4thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw4thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw4thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw4thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw4thBadgerButton.Name = "toolStripStatusLabel5";
            draw4thBadgerButton.Padding = new Padding(5);
            draw4thBadgerButton.Size = new Size(27, 30);
            draw4thBadgerButton.Text = "4";
            // 
            // toolStripStatusLabel6
            // 
            draw3rdBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw3rdBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw3rdBadgerButton.ForeColor = SystemColors.ControlLight;
            draw3rdBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw3rdBadgerButton.Name = "toolStripStatusLabel6";
            draw3rdBadgerButton.Padding = new Padding(5);
            draw3rdBadgerButton.Size = new Size(27, 30);
            draw3rdBadgerButton.Text = "3";
            // 
            // toolStripStatusLabel7
            // 
            draw2ndBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw2ndBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw2ndBadgerButton.ForeColor = SystemColors.ControlLight;
            draw2ndBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw2ndBadgerButton.Name = "toolStripStatusLabel7";
            draw2ndBadgerButton.Padding = new Padding(5);
            draw2ndBadgerButton.Size = new Size(27, 30);
            draw2ndBadgerButton.Text = "2";
            // 
            // toolStripStatusLabel8
            // 
            draw9thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw9thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw9thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw9thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw9thBadgerButton.Name = "toolStripStatusLabel8";
            draw9thBadgerButton.Padding = new Padding(5);
            draw9thBadgerButton.Size = new Size(27, 30);
            draw9thBadgerButton.Text = "9";
            // 
            // toolStripStatusLabel9
            // 
            draw10thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw10thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw10thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw10thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw10thBadgerButton.Name = "toolStripStatusLabel9";
            draw10thBadgerButton.Padding = new Padding(5);
            draw10thBadgerButton.Size = new Size(35, 30);
            draw10thBadgerButton.Text = "10";
            // 
            // toolStripStatusLabel10
            // 
            draw11thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw11thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw11thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw11thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw11thBadgerButton.Name = "toolStripStatusLabel10";
            draw11thBadgerButton.Padding = new Padding(5);
            draw11thBadgerButton.Size = new Size(35, 30);
            draw11thBadgerButton.Text = "11";
            // 
            // toolStripStatusLabel11
            // 
            draw12thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw12thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw12thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw12thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw12thBadgerButton.Name = "toolStripStatusLabel11";
            draw12thBadgerButton.Padding = new Padding(5);
            draw12thBadgerButton.Size = new Size(35, 30);
            draw12thBadgerButton.Text = "12";
            // 
            // toolStripStatusLabel13
            // 
            draw14thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw14thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw14thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw14thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw14thBadgerButton.Name = "toolStripStatusLabel13";
            draw14thBadgerButton.Padding = new Padding(5);
            draw14thBadgerButton.Size = new Size(35, 30);
            draw14thBadgerButton.Text = "14";
            // 
            // toolStripStatusLabel14
            // 
            draw15thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw15thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw15thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw15thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw15thBadgerButton.Name = "toolStripStatusLabel14";
            draw15thBadgerButton.Padding = new Padding(5);
            draw15thBadgerButton.Size = new Size(35, 30);
            draw15thBadgerButton.Text = "15";
            // 
            // toolStripStatusLabel15
            // 
            draw16thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw16thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw16thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw16thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw16thBadgerButton.Name = "toolStripStatusLabel15";
            draw16thBadgerButton.Padding = new Padding(5);
            draw16thBadgerButton.Size = new Size(35, 30);
            draw16thBadgerButton.Text = "16";
            // 
            // toolStripStatusLabel12
            // 
            draw13thBadgerButton.BackColor = SystemColors.ControlDarkDark;
            draw13thBadgerButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            draw13thBadgerButton.ForeColor = SystemColors.ControlLight;
            draw13thBadgerButton.Margin = new Padding(4, 2, 4, 2);
            draw13thBadgerButton.Name = "toolStripStatusLabel12";
            draw13thBadgerButton.Padding = new Padding(5);
            draw13thBadgerButton.Size = new Size(126, 30);
            draw13thBadgerButton.Text = "Draw Badger 13";


            draw2ndBadgerButton.Click += OnDraw2ndBadgerButtonClicked;
            draw2ndBadgerButton.DoubleClick += OnDraw2ndBadgerButtonClicked;
            draw3rdBadgerButton.Click += OnDraw3rdBadgerButtonClicked;
            draw3rdBadgerButton.DoubleClick += OnDraw3rdBadgerButtonClicked;
            draw4thBadgerButton.Click += OnDraw4thBadgerButtonClicked;
            draw4thBadgerButton.DoubleClick += OnDraw4thBadgerButtonClicked;
            draw5thBadgerButton.Click += OnDraw5thBadgerButtonClicked;
            draw5thBadgerButton.DoubleClick += OnDraw5thBadgerButtonClicked;
            draw6thBadgerButton.Click += OnDraw6thBadgerButtonClicked;
            draw6thBadgerButton.DoubleClick += OnDraw6thBadgerButtonClicked;
            draw7thBadgerButton.Click += OnDraw7thBadgerButtonClicked;
            draw7thBadgerButton.DoubleClick += OnDraw7thBadgerButtonClicked;
            draw8thBadgerButton.Click += OnDraw8thBadgerButtonClicked;
            draw8thBadgerButton.DoubleClick += OnDraw8thBadgerButtonClicked;
            draw9thBadgerButton.Click += OnDraw9thBadgerButtonClicked;
            draw9thBadgerButton.DoubleClick += OnDraw9thBadgerButtonClicked;
            draw10thBadgerButton.Click += OnDraw10thBadgerButtonClicked;
            draw10thBadgerButton.DoubleClick += OnDraw10thBadgerButtonClicked;
            draw11thBadgerButton.Click += OnDraw11thBadgerButtonClicked;
            draw11thBadgerButton.DoubleClick += OnDraw11thBadgerButtonClicked;
            draw12thBadgerButton.Click += OnDraw12thBadgerButtonClicked;
            draw12thBadgerButton.DoubleClick += OnDraw12thBadgerButtonClicked;
            draw13thBadgerButton.Click += OnDraw13thBadgerButtonClicked;
            draw13thBadgerButton.DoubleClick += OnDraw13thBadgerButtonClicked;
            draw14thBadgerButton.Click += OnDraw14thBadgerButtonClicked;
            draw14thBadgerButton.DoubleClick += OnDraw14thBadgerButtonClicked;
            draw15thBadgerButton.Click += OnDraw15thBadgerButtonClicked;
            draw15thBadgerButton.DoubleClick += OnDraw15thBadgerButtonClicked;
            draw16thBadgerButton.Click += OnDraw16thBadgerButtonClicked;
            draw16thBadgerButton.DoubleClick += OnDraw16thBadgerButtonClicked;
            // 
            // Program
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(statusStrip1);
            Controls.Add(drawButtons);
            Name = "Program";
            Text = "Draw House, 3D, or Badger";
            drawButtons.ResumeLayout(false);
            drawButtons.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        [STAThread]
        public static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Program());
        }

        private void OnDrawHouseButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawHouse() { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }

        private void OnDraw3dButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            TelevisionScreen.TelevisionScreen screen = new() { Dock = DockStyle.Fill };

            screen.Objects.Add(new NullPointersEtc.TelevisionScreen.Cube3DF(100));

            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
            }

            drawCanvas = screen;
            Controls.Add(drawCanvas);
        }

        private void OnDraw1stBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Standing, ViewDirection.Front) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }

        private void OnDraw2ndBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Standing, ViewDirection.Left) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw3rdBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Standing, ViewDirection.Back) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw4thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Standing, ViewDirection.Right) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw5thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Waving, ViewDirection.Front) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw6thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Waving, ViewDirection.Left) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw7thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Waving, ViewDirection.Back) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw8thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Waving, ViewDirection.Right) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw9thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.MidStride, ViewDirection.Front) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw10thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.MidStride, ViewDirection.Left) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw11thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.MidStride, ViewDirection.Back) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw12thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.MidStride, ViewDirection.Right) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw13thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Sitting, ViewDirection.Front) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw14thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Sitting, ViewDirection.Left) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw15thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Sitting, ViewDirection.Back) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }
        private void OnDraw16thBadgerButtonClicked(
            [AllowNull] object sender,
            EventArgs e)
        {
            if (drawCanvas is not null)
            {
                Controls.Remove(drawCanvas);
                drawCanvas.Dispose();
                drawCanvas = null;
            }

            drawCanvas = new DrawBadger(BadgerPose.Sitting, ViewDirection.Right) { Dock = DockStyle.Fill };
            Controls.Add(drawCanvas);
        }

        [AllowNull]
        private StatusStrip drawButtons = null;

        [AllowNull]
        private ToolStripStatusLabel drawHouseButton = null;

        [AllowNull]
        private ToolStripStatusLabel draw3dButton = null;

        [AllowNull]
        private ToolStripStatusLabel draw1stBadgerButton = null;

        [AllowNull]
        private UserControl drawCanvas;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel draw2ndBadgerButton;
        private ToolStripStatusLabel draw3rdBadgerButton;
        private ToolStripStatusLabel draw4thBadgerButton;
        private ToolStripStatusLabel draw5thBadgerButton;
        private ToolStripStatusLabel draw6thBadgerButton;
        private ToolStripStatusLabel draw7thBadgerButton;
        private ToolStripStatusLabel draw8thBadgerButton;
        private ToolStripStatusLabel draw9thBadgerButton;
        private ToolStripStatusLabel draw10thBadgerButton;
        private ToolStripStatusLabel draw11thBadgerButton;
        private ToolStripStatusLabel draw12thBadgerButton;
        private ToolStripStatusLabel draw13thBadgerButton;
        private ToolStripStatusLabel draw14thBadgerButton;
        private ToolStripStatusLabel draw15thBadgerButton;
        private ToolStripStatusLabel draw16thBadgerButton;
    }
}
