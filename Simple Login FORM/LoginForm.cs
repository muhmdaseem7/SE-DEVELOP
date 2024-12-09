using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using System.Configuration;

namespace Login_FORM
{
    public partial class LoginForm : Form
    {
        // The connection string specifies the server, database, user ID, password, port, and security settings needed
        private readonly string connectionString;

        public LoginForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["LoginPageDB"].ConnectionString;

        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.FormClosed += (s, args) => this.Show();  // Show login form when register form closes
            this.Hide();
            registerForm.Show();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(UsernameBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Text))
            {
                MessageBox.Show("Please enter both username and password.");
                return;
            }

            try
            {
                using (var con = new MySqlConnection(connectionString))
                using (var cmd = new MySqlCommand("SELECT password FROM LoginForm WHERE email=@email", con))
                {
                    cmd.Parameters.AddWithValue("@email", UsernameBox.Text.Trim());
                    cmd.Parameters.AddWithValue("@password", PasswordBox.Text);

                    con.Open();
                    var hashedPassword = cmd.ExecuteScalar()?.ToString();

                    if (hashedPassword != null && BCrypt.Net.BCrypt.Verify(PasswordBox.Text, hashedPassword))
                    {
                        MessageBox.Show("Login successful!");
                        this.Hide();
                        using (Main fm = new Main())
                        {
                            fm.ShowDialog();
                        }
                        this.Close(); // Close login form after main form closes
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                        PasswordBox.Clear(); // Clear password for security
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void ResetPasswordButton_Click(object sender, EventArgs e)
        {
            // Navigate the user to the ResetPasswordForm
            ResetPasswordForm resetForm = new ResetPasswordForm();
            resetForm.FormClosed += (s, args) => this.Show();  // Show login form when the reset form closes
            this.Hide();
            resetForm.Show();
        }
        // This method overrides the default behavior of the form when it is closing.
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
                Application.Exit();
        }

    }
}