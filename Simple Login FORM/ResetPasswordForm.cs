using System;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Login_FORM
{
    public partial class ResetPasswordForm : Form
    {
        private Panel panel1;
        private TextBox EmailTextBox;
        private Button SendRequestButton;
        private Label label2;
        private Label label1;
        private Button ReturnButton;
        private Panel panel4;
        private PictureBox pictureBox1;
        private Label label15;
        private Label label14;
        private Label label13;
        private Label label12;
        private Label label11;

        // Adjust your connection string as needed
        private readonly string connectionString;

        public ResetPasswordForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["LoginPageDB"].ConnectionString;
        }

        private void SendRequestButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Please enter your email.");
                return;
            }

            try
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();

                    // Check if email exists
                    using (MySqlCommand checkCmd = new MySqlCommand("SELECT registration_id FROM LoginForm WHERE email=@email", con))
                    {
                        checkCmd.Parameters.AddWithValue("@email", email);
                        var userId = checkCmd.ExecuteScalar();

                        if (userId == null)
                        {
                            MessageBox.Show("No user found with that email.");
                            return;
                        }
                    }

                    // Set pending password reset
                    using (MySqlCommand updateCmd = new MySqlCommand("UPDATE LoginForm SET pending_password_reset=1 WHERE email=@email", con))
                    {
                        updateCmd.Parameters.AddWithValue("@email", email);
                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Your password reset request has been submitted. An administrator will review and if approved, you will receive a new password by email.");
                        }
                        else
                        {
                            MessageBox.Show("No rows were updated. Ensure that the email is correct and the 'pending_password_reset' column exists in the database.");
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Show detailed database error
                MessageBox.Show($"Database Error: {ex.Message}\n\nCheck your database connection, table and column names.");
            }
            catch (Exception ex)
            {
                // Show generic error
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (LoginForm loginForm = new LoginForm())
            {
                loginForm.ShowDialog();
            }
            this.Close();
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResetPasswordForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.ReturnButton = new System.Windows.Forms.Button();
            this.SendRequestButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkRed;
            this.panel1.Controls.Add(this.ReturnButton);
            this.panel1.Controls.Add(this.SendRequestButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.EmailTextBox);
            this.panel1.Location = new System.Drawing.Point(209, 191);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(814, 400);
            this.panel1.TabIndex = 0;
            // 
            // ReturnButton
            // 
            this.ReturnButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.ReturnButton.Location = new System.Drawing.Point(330, 320);
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.Size = new System.Drawing.Size(116, 23);
            this.ReturnButton.TabIndex = 4;
            this.ReturnButton.Text = "RETURN";
            this.ReturnButton.UseVisualStyleBackColor = true;
            this.ReturnButton.Click += new System.EventHandler(this.ReturnButton_Click);
            // 
            // SendRequestButton
            // 
            this.SendRequestButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.SendRequestButton.Location = new System.Drawing.Point(314, 238);
            this.SendRequestButton.Name = "SendRequestButton";
            this.SendRequestButton.Size = new System.Drawing.Size(148, 23);
            this.SendRequestButton.TabIndex = 3;
            this.SendRequestButton.Text = "SEND REQUEST";
            this.SendRequestButton.UseVisualStyleBackColor = true;
            this.SendRequestButton.Click += new System.EventHandler(this.SendRequestButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Red;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(231, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(336, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "TYPE YOUR EMAIL LINKED TO THE ACCOUNT";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Red;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(230, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(345, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "REQUEST FOR PASSWORD RESET HERE ";
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.EmailTextBox.Location = new System.Drawing.Point(171, 195);
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(448, 24);
            this.EmailTextBox.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.label11);
            this.panel4.Location = new System.Drawing.Point(0, -1);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1233, 100);
            this.panel4.TabIndex = 4;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(1133, 39);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 24);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Modern No. 20", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(752, -1);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(54, 83);
            this.label15.TabIndex = 4;
            this.label15.Text = ".";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Modern No. 20", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(391, -3);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(54, 83);
            this.label14.TabIndex = 3;
            this.label14.Text = ".";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Modern No. 20", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Red;
            this.label13.Location = new System.Drawing.Point(836, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(289, 45);
            this.label13.TabIndex = 2;
            this.label13.Text = "CAMBRIDGE";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Modern No. 20", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(482, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(226, 45);
            this.label12.TabIndex = 1;
            this.label12.Text = "CULTURE";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Modern No. 20", 25.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(80, 29);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(261, 45);
            this.label11.TabIndex = 0;
            this.label11.Text = "TOGETHER";
            // 
            // ResetPasswordForm
            // 
            this.BackColor = System.Drawing.Color.DarkRed;
            this.ClientSize = new System.Drawing.Size(1232, 736);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResetPasswordForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

    }
}

