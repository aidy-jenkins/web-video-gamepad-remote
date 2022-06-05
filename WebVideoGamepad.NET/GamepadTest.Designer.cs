namespace WebAppHost.NetFramework
{
    partial class GamepadTest
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.directionLabel = new System.Windows.Forms.Label();
            this.buttonLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // directionLabel
            // 
            this.directionLabel.AutoSize = true;
            this.directionLabel.Location = new System.Drawing.Point(113, 201);
            this.directionLabel.Name = "directionLabel";
            this.directionLabel.Size = new System.Drawing.Size(73, 13);
            this.directionLabel.TabIndex = 0;
            this.directionLabel.Text = "directionLabel";
            // 
            // buttonLabel
            // 
            this.buttonLabel.AutoSize = true;
            this.buttonLabel.Location = new System.Drawing.Point(565, 201);
            this.buttonLabel.Name = "buttonLabel";
            this.buttonLabel.Size = new System.Drawing.Size(35, 13);
            this.buttonLabel.TabIndex = 1;
            this.buttonLabel.Text = "label2";
            // 
            // GamepadTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.buttonLabel);
            this.Controls.Add(this.directionLabel);
            this.Name = "GamepadTest";
            this.Text = "GamepadTest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label directionLabel;
        private System.Windows.Forms.Label buttonLabel;
    }
}