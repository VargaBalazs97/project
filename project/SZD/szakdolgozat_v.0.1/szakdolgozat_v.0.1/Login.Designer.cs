namespace szakdolgozat_v._0._1
{
    partial class Login
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
            this.singin_Btn = new System.Windows.Forms.Button();
            this.user_name_TBox = new System.Windows.Forms.TextBox();
            this.password_TBox = new System.Windows.Forms.TextBox();
            this.user_name_Label = new System.Windows.Forms.Label();
            this.password_Label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // singin_Btn
            // 
            this.singin_Btn.Location = new System.Drawing.Point(203, 12);
            this.singin_Btn.Name = "singin_Btn";
            this.singin_Btn.Size = new System.Drawing.Size(81, 46);
            this.singin_Btn.TabIndex = 2;
            this.singin_Btn.Text = "Bejelentkezés";
            this.singin_Btn.UseVisualStyleBackColor = true;
            this.singin_Btn.Click += new System.EventHandler(this.L_SINGIN_Btn_Click);
            // 
            // user_name_TBox
            // 
            this.user_name_TBox.Location = new System.Drawing.Point(97, 12);
            this.user_name_TBox.Name = "user_name_TBox";
            this.user_name_TBox.Size = new System.Drawing.Size(100, 20);
            this.user_name_TBox.TabIndex = 0;
            // 
            // password_TBox
            // 
            this.password_TBox.Location = new System.Drawing.Point(97, 38);
            this.password_TBox.Name = "password_TBox";
            this.password_TBox.Size = new System.Drawing.Size(100, 20);
            this.password_TBox.TabIndex = 1;
            this.password_TBox.UseSystemPasswordChar = true;
            // 
            // user_name_Label
            // 
            this.user_name_Label.AutoSize = true;
            this.user_name_Label.Location = new System.Drawing.Point(10, 15);
            this.user_name_Label.Name = "user_name_Label";
            this.user_name_Label.Size = new System.Drawing.Size(81, 13);
            this.user_name_Label.TabIndex = 3;
            this.user_name_Label.Text = "Felhasználónév";
            // 
            // password_Label
            // 
            this.password_Label.AutoSize = true;
            this.password_Label.Location = new System.Drawing.Point(10, 41);
            this.password_Label.Name = "password_Label";
            this.password_Label.Size = new System.Drawing.Size(36, 13);
            this.password_Label.TabIndex = 4;
            this.password_Label.Text = "Jelszó";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 70);
            this.Controls.Add(this.password_Label);
            this.Controls.Add(this.user_name_Label);
            this.Controls.Add(this.password_TBox);
            this.Controls.Add(this.user_name_TBox);
            this.Controls.Add(this.singin_Btn);
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button singin_Btn;
        private System.Windows.Forms.TextBox user_name_TBox;
        private System.Windows.Forms.TextBox password_TBox;
        private System.Windows.Forms.Label user_name_Label;
        private System.Windows.Forms.Label password_Label;
    }
}

