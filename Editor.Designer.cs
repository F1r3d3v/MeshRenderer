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
            customSilder1 = new CustomControls.CustomSilder();
            customSilder2 = new CustomControls.CustomSilder();
            customSilder3 = new CustomControls.CustomSilder();
            checkBox1 = new CheckBox();
            groupBox2 = new GroupBox();
            flowLayoutPanel3 = new FlowLayoutPanel();
            customSilder4 = new CustomControls.CustomSilder();
            customSilder5 = new CustomControls.CustomSilder();
            customSilder6 = new CustomControls.CustomSilder();
            customSilder7 = new CustomControls.CustomSilder();
            button1 = new Button();
            groupBox3 = new GroupBox();
            flowLayoutPanel4 = new FlowLayoutPanel();
            button2 = new Button();
            checkBox2 = new CheckBox();
            texturePicker1 = new CustomControls.TexturePicker();
            texturePicker2 = new CustomControls.TexturePicker();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            groupBox3.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.AutoSize = true;
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.Controls.Add(renderCanvas);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1070, 602);
            panel1.TabIndex = 0;
            // 
            // renderCanvas
            // 
            renderCanvas.Location = new Point(0, 0);
            renderCanvas.Margin = new Padding(0);
            renderCanvas.Name = "renderCanvas";
            renderCanvas.Size = new Size(858, 599);
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
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(858, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(209, 599);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.AutoSize = true;
            groupBox1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox1.Controls.Add(flowLayoutPanel2);
            groupBox1.Location = new Point(0, 0);
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
            flowLayoutPanel2.Controls.Add(customSilder1);
            flowLayoutPanel2.Controls.Add(customSilder2);
            flowLayoutPanel2.Controls.Add(customSilder3);
            flowLayoutPanel2.Controls.Add(checkBox1);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel2.Location = new Point(3, 19);
            flowLayoutPanel2.Margin = new Padding(0);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(200, 154);
            flowLayoutPanel2.TabIndex = 0;
            // 
            // customSilder1
            // 
            customSilder1.BackColor = SystemColors.Control;
            customSilder1.Divider = 1;
            customSilder1.Location = new Point(0, 0);
            customSilder1.Margin = new Padding(0);
            customSilder1.MaxValue = 50;
            customSilder1.MinValue = 10;
            customSilder1.Name = "customSilder1";
            customSilder1.Size = new Size(200, 45);
            customSilder1.SliderText = "Triangulation density";
            customSilder1.TabIndex = 0;
            customSilder1.Value = 30;
            // 
            // customSilder2
            // 
            customSilder2.BackColor = SystemColors.Control;
            customSilder2.Divider = 1;
            customSilder2.Location = new Point(0, 45);
            customSilder2.Margin = new Padding(0);
            customSilder2.MaxValue = 45;
            customSilder2.MinValue = -45;
            customSilder2.Name = "customSilder2";
            customSilder2.Size = new Size(200, 45);
            customSilder2.SliderText = "Z axis rotation";
            customSilder2.TabIndex = 1;
            customSilder2.Value = 0;
            // 
            // customSilder3
            // 
            customSilder3.BackColor = SystemColors.Control;
            customSilder3.Divider = 1;
            customSilder3.Location = new Point(0, 90);
            customSilder3.Margin = new Padding(0);
            customSilder3.MaxValue = 10;
            customSilder3.MinValue = 0;
            customSilder3.Name = "customSilder3";
            customSilder3.Size = new Size(200, 45);
            customSilder3.SliderText = "X axis rotation";
            customSilder3.TabIndex = 2;
            customSilder3.Value = 0;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(5, 135);
            checkBox1.Margin = new Padding(5, 0, 0, 0);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(81, 19);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "Wireframe";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            groupBox2.AutoSize = true;
            groupBox2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox2.Controls.Add(flowLayoutPanel3);
            groupBox2.Location = new Point(0, 176);
            groupBox2.Margin = new Padding(0, 0, 3, 0);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(206, 231);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Lighting";
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.AutoSize = true;
            flowLayoutPanel3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel3.Controls.Add(customSilder4);
            flowLayoutPanel3.Controls.Add(customSilder5);
            flowLayoutPanel3.Controls.Add(customSilder6);
            flowLayoutPanel3.Controls.Add(customSilder7);
            flowLayoutPanel3.Controls.Add(button1);
            flowLayoutPanel3.Dock = DockStyle.Fill;
            flowLayoutPanel3.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel3.Location = new Point(3, 19);
            flowLayoutPanel3.Margin = new Padding(0);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(200, 209);
            flowLayoutPanel3.TabIndex = 0;
            // 
            // customSilder4
            // 
            customSilder4.BackColor = SystemColors.Control;
            customSilder4.Divider = 100;
            customSilder4.Location = new Point(0, 0);
            customSilder4.Margin = new Padding(0);
            customSilder4.MaxValue = 100;
            customSilder4.MinValue = 0;
            customSilder4.Name = "customSilder4";
            customSilder4.Size = new Size(200, 45);
            customSilder4.SliderText = "Coeficient kd";
            customSilder4.TabIndex = 0;
            customSilder4.Value = 0;
            // 
            // customSilder5
            // 
            customSilder5.BackColor = SystemColors.Control;
            customSilder5.Divider = 100;
            customSilder5.Location = new Point(0, 45);
            customSilder5.Margin = new Padding(0);
            customSilder5.MaxValue = 100;
            customSilder5.MinValue = 0;
            customSilder5.Name = "customSilder5";
            customSilder5.Size = new Size(200, 45);
            customSilder5.SliderText = "Coeficient ks";
            customSilder5.TabIndex = 1;
            customSilder5.Value = 0;
            // 
            // customSilder6
            // 
            customSilder6.BackColor = SystemColors.Control;
            customSilder6.Divider = 1;
            customSilder6.Location = new Point(0, 90);
            customSilder6.Margin = new Padding(0);
            customSilder6.MaxValue = 100;
            customSilder6.MinValue = 1;
            customSilder6.Name = "customSilder6";
            customSilder6.Size = new Size(200, 45);
            customSilder6.SliderText = "Coeficient m";
            customSilder6.TabIndex = 2;
            customSilder6.Value = 1;
            // 
            // customSilder7
            // 
            customSilder7.BackColor = SystemColors.Control;
            customSilder7.Divider = 1;
            customSilder7.Location = new Point(0, 135);
            customSilder7.Margin = new Padding(0);
            customSilder7.MaxValue = 10;
            customSilder7.MinValue = 0;
            customSilder7.Name = "customSilder7";
            customSilder7.Size = new Size(200, 45);
            customSilder7.SliderText = "Light source Z plane";
            customSilder7.TabIndex = 3;
            customSilder7.Value = 0;
            // 
            // button1
            // 
            button1.Location = new Point(3, 183);
            button1.Name = "button1";
            button1.Size = new Size(194, 23);
            button1.TabIndex = 4;
            button1.Text = "Light source color";
            button1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            groupBox3.AutoSize = true;
            groupBox3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox3.Controls.Add(flowLayoutPanel4);
            groupBox3.Location = new Point(0, 407);
            groupBox3.Margin = new Padding(0, 0, 3, 0);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(206, 192);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Material";
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.AutoSize = true;
            flowLayoutPanel4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            flowLayoutPanel4.Controls.Add(button2);
            flowLayoutPanel4.Controls.Add(checkBox2);
            flowLayoutPanel4.Controls.Add(texturePicker1);
            flowLayoutPanel4.Controls.Add(texturePicker2);
            flowLayoutPanel4.Dock = DockStyle.Fill;
            flowLayoutPanel4.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel4.Location = new Point(3, 19);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(200, 170);
            flowLayoutPanel4.TabIndex = 0;
            // 
            // button2
            // 
            button2.Location = new Point(3, 3);
            button2.Name = "button2";
            button2.Size = new Size(194, 23);
            button2.TabIndex = 5;
            button2.Text = "Surface color";
            button2.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Location = new Point(5, 29);
            checkBox2.Margin = new Padding(5, 0, 0, 0);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(144, 19);
            checkBox2.TabIndex = 6;
            checkBox2.Text = "Use texture as material";
            checkBox2.UseVisualStyleBackColor = true;
            // 
            // texturePicker1
            // 
            texturePicker1.ButtonText = "Choose texture";
            texturePicker1.DefaultTexture = "brickwall.jpg";
            texturePicker1.FilePath = null;
            texturePicker1.Location = new Point(0, 48);
            texturePicker1.Margin = new Padding(0);
            texturePicker1.Name = "texturePicker1";
            texturePicker1.Padding = new Padding(3);
            texturePicker1.Size = new Size(200, 61);
            texturePicker1.TabIndex = 7;
            // 
            // texturePicker2
            // 
            texturePicker2.ButtonText = "Choose normal map";
            texturePicker2.DefaultTexture = "brickwall_normal.jpg";
            texturePicker2.FilePath = null;
            texturePicker2.Location = new Point(0, 109);
            texturePicker2.Margin = new Padding(0);
            texturePicker2.Name = "texturePicker2";
            texturePicker2.Padding = new Padding(3);
            texturePicker2.Size = new Size(200, 61);
            texturePicker2.TabIndex = 8;
            // 
            // Editor
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(1417, 727);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Editor";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
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
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private GK1_PolygonEditor.Canvas renderCanvas;
        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private CustomControls.CustomSilder customSilder2;
        private CustomControls.CustomSilder customSilder1;
        private CustomControls.CustomSilder customSilder3;
        private CheckBox checkBox1;
        private FlowLayoutPanel flowLayoutPanel2;
        private FlowLayoutPanel flowLayoutPanel3;
        private CustomControls.CustomSilder customSilder4;
        private CustomControls.CustomSilder customSilder5;
        private CustomControls.CustomSilder customSilder6;
        private CustomControls.CustomSilder customSilder7;
        private GroupBox groupBox3;
        private Button button1;
        private FlowLayoutPanel flowLayoutPanel4;
        private Button button2;
        private CheckBox checkBox2;
        private CustomControls.TexturePicker texturePicker1;
        private CustomControls.TexturePicker texturePicker2;
    }
}
