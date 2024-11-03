namespace GK1_MeshEditor.CustomControls
{
    partial class CustomSilder
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SliderName = new Label();
            Slider = new NoFocusTrackBar();
            SliderValue = new Label();
            ((System.ComponentModel.ISupportInitialize)Slider).BeginInit();
            SuspendLayout();
            // 
            // SliderName
            // 
            SliderName.AutoSize = true;
            SliderName.Dock = DockStyle.Top;
            SliderName.Location = new Point(0, 0);
            SliderName.Name = "SliderName";
            SliderName.Size = new Size(38, 15);
            SliderName.TabIndex = 0;
            SliderName.Text = "label1";
            // 
            // Slider
            // 
            Slider.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            Slider.AutoSize = false;
            Slider.LargeChange = 1;
            Slider.Location = new Point(0, 18);
            Slider.Name = "Slider";
            Slider.Size = new Size(180, 40);
            Slider.TabIndex = 1;
            Slider.TickStyle = TickStyle.None;
            Slider.Scroll += Slider_Scroll;
            // 
            // SliderValue
            // 
            SliderValue.AutoSize = true;
            SliderValue.Dock = DockStyle.Right;
            SliderValue.Location = new Point(175, 15);
            SliderValue.Margin = new Padding(0);
            SliderValue.Name = "SliderValue";
            SliderValue.Size = new Size(25, 15);
            SliderValue.TabIndex = 2;
            SliderValue.Text = "100";
            // 
            // CustomSilder
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(SliderValue);
            Controls.Add(Slider);
            Controls.Add(SliderName);
            Name = "CustomSilder";
            Size = new Size(200, 45);
            ((System.ComponentModel.ISupportInitialize)Slider).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label SliderName;
        private NoFocusTrackBar Slider;
        private Label SliderValue;
    }
}
