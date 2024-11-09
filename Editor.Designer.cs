namespace GK1_MeshEditor
{
    partial class Editor
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            panel1 = new Panel();
            renderCanvas = new GK1_PolygonEditor.Canvas();
            flowLayoutPanel1 = new FlowLayoutPanel();
            groupBox1 = new GroupBox();
            flowLayoutPanel2 = new FlowLayoutPanel();
            csDensity = new CustomControls.CustomSilder();
            csZRotation = new CustomControls.CustomSilder();
            csXRotation = new CustomControls.CustomSilder();
            cbWireframe = new CheckBox();
            groupBox2 = new GroupBox();
            flowLayoutPanel3 = new FlowLayoutPanel();
            csCoefKd = new CustomControls.CustomSilder();
            csCoefKs = new CustomControls.CustomSilder();
            csCoefM = new CustomControls.CustomSilder();
            csLightZPlane = new CustomControls.CustomSilder();
            flowLayoutPanel5 = new FlowLayoutPanel();
            bLightColor = new Button();
            bAnimation = new Button();
            groupBox3 = new GroupBox();
            flowLayoutPanel4 = new FlowLayoutPanel();
            bSurfaceColor = new Button();
            cbTexture = new CheckBox();
            pTexture = new CustomControls.TexturePicker();
            cbNormalMap = new CheckBox();
            pNormalMap = new CustomControls.TexturePicker();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            flowLayoutPanel5.SuspendLayout();
            groupBox3.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(renderCanvas);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1157, 661);
            panel1.TabIndex = 0;
            // 
            // renderCanvas
            // 
            renderCanvas.Dock = DockStyle.Fill;
            renderCanvas.Location = new Point(0, 0);
            renderCanvas.Margin = new Padding(0);
            renderCanvas.Name = "renderCanvas";
            renderCanvas.Size = new Size(940, 661);
            renderCanvas.TabIndex = 1;
            renderCanvas.Text = "canvas1";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel1.Controls.Add(groupBox1);
            flowLayoutPanel1.Controls.Add(groupBox2);
            flowLayoutPanel1.Controls.Add(groupBox3);
            flowLayoutPanel1.Dock = DockStyle.Right;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(940, 0);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(5, 0, 3, 0);
            flowLayoutPanel1.Size = new Size(217, 661);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.AutoSize = true;
            groupBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox1.Controls.Add(flowLayoutPanel2);
            groupBox1.Location = new Point(5, 0);
            groupBox1.Margin = new Padding(0, 0, 3, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(206, 176);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Surface";
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel2.Controls.Add(csDensity);
            flowLayoutPanel2.Controls.Add(csZRotation);
            flowLayoutPanel2.Controls.Add(csXRotation);
            flowLayoutPanel2.Controls.Add(cbWireframe);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel2.Location = new Point(3, 19);
            flowLayoutPanel2.Margin = new Padding(0);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(200, 154);
            flowLayoutPanel2.TabIndex = 0;
            // 
            // csDensity
            // 
            csDensity.BackColor = SystemColors.Control;
            csDensity.Divider = 1;
            csDensity.Location = new Point(0, 0);
            csDensity.Margin = new Padding(0);
            csDensity.MaxValue = 50;
            csDensity.MinValue = 2;
            csDensity.Name = "csDensity";
            csDensity.Size = new Size(200, 45);
            csDensity.SliderText = "Triangulation density";
            csDensity.TabIndex = 0;
            csDensity.Value = 15;
            // 
            // csZRotation
            // 
            csZRotation.BackColor = SystemColors.Control;
            csZRotation.Divider = 1;
            csZRotation.Location = new Point(0, 45);
            csZRotation.Margin = new Padding(0);
            csZRotation.MaxValue = 90;
            csZRotation.MinValue = -90;
            csZRotation.Name = "csZRotation";
            csZRotation.Size = new Size(200, 45);
            csZRotation.SliderText = "Z axis rotation";
            csZRotation.TabIndex = 1;
            csZRotation.Value = 0;
            // 
            // csXRotation
            // 
            csXRotation.BackColor = SystemColors.Control;
            csXRotation.Divider = 1;
            csXRotation.Location = new Point(0, 90);
            csXRotation.Margin = new Padding(0);
            csXRotation.MaxValue = 180;
            csXRotation.MinValue = -180;
            csXRotation.Name = "csXRotation";
            csXRotation.Size = new Size(200, 45);
            csXRotation.SliderText = "X axis rotation";
            csXRotation.TabIndex = 2;
            csXRotation.Value = 45;
            // 
            // cbWireframe
            // 
            cbWireframe.AutoSize = true;
            cbWireframe.Checked = true;
            cbWireframe.CheckState = CheckState.Checked;
            cbWireframe.Location = new Point(5, 135);
            cbWireframe.Margin = new Padding(5, 0, 0, 0);
            cbWireframe.Name = "cbWireframe";
            cbWireframe.Size = new Size(81, 19);
            cbWireframe.TabIndex = 3;
            cbWireframe.Text = "Wireframe";
            cbWireframe.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.AutoSize = true;
            groupBox2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox2.Controls.Add(flowLayoutPanel3);
            groupBox2.Location = new Point(5, 176);
            groupBox2.Margin = new Padding(0, 0, 3, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(206, 260);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Lighting";
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.AutoSize = true;
            flowLayoutPanel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel3.Controls.Add(csCoefKd);
            flowLayoutPanel3.Controls.Add(csCoefKs);
            flowLayoutPanel3.Controls.Add(csCoefM);
            flowLayoutPanel3.Controls.Add(csLightZPlane);
            flowLayoutPanel3.Controls.Add(flowLayoutPanel5);
            flowLayoutPanel3.Dock = DockStyle.Fill;
            flowLayoutPanel3.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel3.Location = new Point(3, 19);
            flowLayoutPanel3.Margin = new Padding(0);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(200, 238);
            flowLayoutPanel3.TabIndex = 0;
            // 
            // csCoefKd
            // 
            csCoefKd.BackColor = SystemColors.Control;
            csCoefKd.Divider = 100;
            csCoefKd.Location = new Point(0, 0);
            csCoefKd.Margin = new Padding(0);
            csCoefKd.MaxValue = 100;
            csCoefKd.MinValue = 0;
            csCoefKd.Name = "csCoefKd";
            csCoefKd.Size = new Size(200, 45);
            csCoefKd.SliderText = "Coeficient kd";
            csCoefKd.TabIndex = 0;
            csCoefKd.Value = 50;
            // 
            // csCoefKs
            // 
            csCoefKs.BackColor = SystemColors.Control;
            csCoefKs.Divider = 100;
            csCoefKs.Location = new Point(0, 45);
            csCoefKs.Margin = new Padding(0);
            csCoefKs.MaxValue = 100;
            csCoefKs.MinValue = 1;
            csCoefKs.Name = "csCoefKs";
            csCoefKs.Size = new Size(200, 45);
            csCoefKs.SliderText = "Coeficient ks";
            csCoefKs.TabIndex = 1;
            csCoefKs.Value = 50;
            // 
            // csCoefM
            // 
            csCoefM.BackColor = SystemColors.Control;
            csCoefM.Divider = 1;
            csCoefM.Location = new Point(0, 90);
            csCoefM.Margin = new Padding(0);
            csCoefM.MaxValue = 100;
            csCoefM.MinValue = 1;
            csCoefM.Name = "csCoefM";
            csCoefM.Size = new Size(200, 45);
            csCoefM.SliderText = "Coeficient m";
            csCoefM.TabIndex = 2;
            csCoefM.Value = 10;
            // 
            // csLightZPlane
            // 
            csLightZPlane.BackColor = SystemColors.Control;
            csLightZPlane.Divider = 1;
            csLightZPlane.Location = new Point(0, 135);
            csLightZPlane.Margin = new Padding(0);
            csLightZPlane.MaxValue = 300;
            csLightZPlane.MinValue = -300;
            csLightZPlane.Name = "csLightZPlane";
            csLightZPlane.Size = new Size(200, 45);
            csLightZPlane.SliderText = "Light source Z plane";
            csLightZPlane.TabIndex = 3;
            csLightZPlane.Value = -300;
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.AutoSize = true;
            flowLayoutPanel5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel5.Controls.Add(bLightColor);
            flowLayoutPanel5.Controls.Add(bAnimation);
            flowLayoutPanel5.Dock = DockStyle.Fill;
            flowLayoutPanel5.Location = new Point(0, 180);
            flowLayoutPanel5.Margin = new Padding(0);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new Size(200, 58);
            flowLayoutPanel5.TabIndex = 6;
            // 
            // bLightColor
            // 
            bLightColor.Location = new Point(3, 3);
            bLightColor.Margin = new Padding(3, 3, 2, 3);
            bLightColor.Name = "bLightColor";
            bLightColor.Size = new Size(95, 52);
            bLightColor.TabIndex = 4;
            bLightColor.Text = "Light color";
            bLightColor.UseVisualStyleBackColor = true;
            bLightColor.Click += bLightColor_Click;
            // 
            // bAnimation
            // 
            bAnimation.Location = new Point(101, 3);
            bAnimation.Margin = new Padding(1, 3, 0, 3);
            bAnimation.Name = "bAnimation";
            bAnimation.Size = new Size(95, 52);
            bAnimation.TabIndex = 5;
            bAnimation.Text = "Play animation";
            bAnimation.UseVisualStyleBackColor = true;
            bAnimation.Click += bAnimation_Click;
            // 
            // groupBox3
            // 
            groupBox3.AutoSize = true;
            groupBox3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox3.Controls.Add(flowLayoutPanel4);
            groupBox3.Location = new Point(5, 436);
            groupBox3.Margin = new Padding(0, 0, 3, 0);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(206, 211);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Material";
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.AutoSize = true;
            flowLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel4.Controls.Add(bSurfaceColor);
            flowLayoutPanel4.Controls.Add(cbTexture);
            flowLayoutPanel4.Controls.Add(pTexture);
            flowLayoutPanel4.Controls.Add(cbNormalMap);
            flowLayoutPanel4.Controls.Add(pNormalMap);
            flowLayoutPanel4.Dock = DockStyle.Fill;
            flowLayoutPanel4.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel4.Location = new Point(3, 19);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(200, 189);
            flowLayoutPanel4.TabIndex = 0;
            // 
            // bSurfaceColor
            // 
            bSurfaceColor.Location = new Point(3, 3);
            bSurfaceColor.Name = "bSurfaceColor";
            bSurfaceColor.Size = new Size(194, 23);
            bSurfaceColor.TabIndex = 5;
            bSurfaceColor.Text = "Surface color";
            bSurfaceColor.UseVisualStyleBackColor = true;
            bSurfaceColor.Click += bSurfaceColor_Click;
            // 
            // cbTexture
            // 
            cbTexture.AutoSize = true;
            cbTexture.Location = new Point(5, 29);
            cbTexture.Margin = new Padding(5, 0, 0, 0);
            cbTexture.Name = "cbTexture";
            cbTexture.Size = new Size(84, 19);
            cbTexture.TabIndex = 6;
            cbTexture.Text = "Use texture";
            cbTexture.UseVisualStyleBackColor = true;
            cbTexture.CheckedChanged += cbTexture_CheckedChanged;
            // 
            // pTexture
            // 
            pTexture.ButtonText = "Choose texture";
            pTexture.DefaultTexture = "brickwall.jpg";
            pTexture.Location = new Point(0, 48);
            pTexture.Margin = new Padding(0);
            pTexture.Name = "pTexture";
            pTexture.Padding = new Padding(3);
            pTexture.Size = new Size(200, 61);
            pTexture.TabIndex = 7;
            // 
            // cbNormalMap
            // 
            cbNormalMap.AutoSize = true;
            cbNormalMap.Location = new Point(5, 109);
            cbNormalMap.Margin = new Padding(5, 0, 0, 0);
            cbNormalMap.Name = "cbNormalMap";
            cbNormalMap.Size = new Size(113, 19);
            cbNormalMap.TabIndex = 9;
            cbNormalMap.Text = "Use normal map";
            cbNormalMap.UseVisualStyleBackColor = true;
            cbNormalMap.CheckedChanged += cbNormalMap_CheckedChanged;
            // 
            // pNormalMap
            // 
            pNormalMap.ButtonText = "Choose normal map";
            pNormalMap.DefaultTexture = "brickwall_normal.jpg";
            pNormalMap.Location = new Point(0, 128);
            pNormalMap.Margin = new Padding(0);
            pNormalMap.Name = "pNormalMap";
            pNormalMap.Padding = new Padding(3);
            pNormalMap.Size = new Size(200, 61);
            pNormalMap.TabIndex = 8;
            // 
            // Editor
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1157, 661);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1173, 700);
            Name = "Editor";
            Text = "Mesh Editor";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            flowLayoutPanel5.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private GK1_PolygonEditor.Canvas renderCanvas;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private CustomControls.CustomSilder csZRotation;
        private CustomControls.CustomSilder csDensity;
        private CustomControls.CustomSilder csXRotation;
        private CheckBox cbWireframe;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel3;
        private CustomControls.CustomSilder csCoefKd;
        private CustomControls.CustomSilder csCoefKs;
        private CustomControls.CustomSilder csCoefM;
        private CustomControls.CustomSilder csLightZPlane;
        private GroupBox groupBox3;
        private Button bLightColor;
        private FlowLayoutPanel flowLayoutPanel4;
        private Button bSurfaceColor;
        private CheckBox cbTexture;
        private CustomControls.TexturePicker pTexture;
        private CustomControls.TexturePicker pNormalMap;
        private CheckBox cbNormalMap;
        private FlowLayoutPanel flowLayoutPanel5;
        private Button bAnimation;
    }
}
