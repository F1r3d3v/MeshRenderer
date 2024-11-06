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
            SliderName.Padding = new Padding(5, 0, 0, 0);
            SliderName.Size = new Size(43, 15);
            SliderName.TabIndex = 0;
            SliderName.Text = "label1";
            // 
            // Slider
            // 
            Slider.AutoSize = false;
            Slider.Dock = DockStyle.Bottom;
            Slider.LargeChange = 1;
            Slider.Location = new Point(0, 18);
            Slider.Name = "Slider";
            Slider.Size = new Size(164, 27);
            Slider.TabIndex = 1;
            Slider.TickStyle = TickStyle.None;
            // 
            // SliderValue
            // 
            SliderValue.Dock = DockStyle.Right;
            SliderValue.Location = new Point(164, 0);
            SliderValue.Margin = new Padding(0);
            SliderValue.Name = "SliderValue";
            SliderValue.Size = new Size(36, 45);
            SliderValue.TabIndex = 2;
            SliderValue.Text = "0.00";
            SliderValue.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // CustomSilder
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(SliderName);
            Controls.Add(Slider);
            Controls.Add(SliderValue);
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
