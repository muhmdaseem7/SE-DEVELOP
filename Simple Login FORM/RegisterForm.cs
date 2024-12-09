using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using BCrypt.Net;
using Microsoft.VisualBasic;
using System.Configuration;

namespace Login_FORM
{
    public partial class RegisterForm : Form
    {
        // The connection string specifies the server, database, user ID, password, port, and security settings needed
        private readonly string connectionString;
        private const string ADMIN_CODE = "ADMIN123"; // This constant stores the admin verification code used to validate that a user has permissions

        public RegisterForm()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["LoginPageDB"].ConnectionString;
            // Set DateTimePicker default values
            DobPicker.MaxDate = DateTime.Today;
            DobPicker.MinDate = DateTime.Today.AddYears(-120);
            DobPicker.Format = DateTimePickerFormat.Custom;
            DobPicker.CustomFormat = "dd-MM-yyyy";

        }

        private void MembershipDetailsLinkLabel_Click(object sender, EventArgs e)
        {
            // Open the MembershipDetailsForm
            var membershipDetailsForm = new MembershipDetailsForm(this); // Pass the current form as the parent
            this.Hide(); // Hide the current form (RegisterForm)
            membershipDetailsForm.Show(); // Show the MembershipDetailsForm
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate required fields
                if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
                    string.IsNullOrWhiteSpace(LastNameBox.Text) ||
                    string.IsNullOrWhiteSpace(AddressLine1Box.Text) ||
                    string.IsNullOrWhiteSpace(EmailBox.Text) ||
                    string.IsNullOrWhiteSpace(MobileNumberBox.Text) ||
                    string.IsNullOrWhiteSpace(PasswordBox.Text))
                {
                    MessageBox.Show("Please fill in all required fields.");
                    return;
                }

                if (!IsPasswordComplex(PasswordBox.Text))
                {
                    MessageBox.Show("Password must be at least 8 characters long, and include at least one uppercase letter, one lowercase letter, one digit, and one special character.");
                    return;
                }

                string selectedRole = UserTypeBox.SelectedItem.ToString();
                string selectedMembershipType = MembershipTypeBox.SelectedItem.ToString();
                string role = "member";

                if (selectedRole.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    // If Admin is selected, ask for the verification code
                    string inputCode = Microsoft.VisualBasic.Interaction.InputBox(
                        "Please enter the admin verification code:",
                        "Admin Verification",
                        ""
                    );

                    if (inputCode != ADMIN_CODE)
                    {
                        MessageBox.Show("Invalid admin verification code.");
                        return; // Stop registration here
                    }

                    role = "admin";
                }

                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = @"INSERT INTO LoginForm 
                        (role, membership_type, first_name, last_name, dob, address_line1, address_line2, 
                         post_code, city, country, email, mobile_number, password) 
                        VALUES 
                        (@role, @membershiptype, @firstName, @lastName, @dob, @addressLine1, @addressLine2, 
                         @postCode, @city, @country, @email, @mobile, @password)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@firstName", FirstNameBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@lastName", LastNameBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@dob", DobPicker.Value);
                        cmd.Parameters.AddWithValue("@addressLine1", AddressLine1Box.Text.Trim());
                        cmd.Parameters.AddWithValue("@addressLine2", AddressLine2Box.Text.Trim());
                        cmd.Parameters.AddWithValue("@postCode", PostCodeBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@city", CityBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@country", CountryBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", EmailBox.Text.Trim());
                        cmd.Parameters.AddWithValue("@mobile", MobileNumberBox.Text.Trim());
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(PasswordBox.Text);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);
                        cmd.Parameters.AddWithValue("@role", role);
                        cmd.Parameters.AddWithValue("@membershipType", selectedMembershipType);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Registration successful!");

                        // Clear all fields after successful registration
                        ClearFields();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show($"Database Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ClearFields()
        {
            FirstNameBox.Clear();
            LastNameBox.Clear();
            AddressLine1Box.Clear();
            AddressLine2Box.Clear();
            PostCodeBox.Clear();
            CityBox.Clear();
            CountryBox.Clear();
            EmailBox.Clear();
            MobileNumberBox.Clear();
            PasswordBox.Clear();
            DobPicker.Value = DateTime.Today;
            UserTypeBox.SelectedIndex = 0;
            MembershipTypeBox.SelectedIndex = -1;
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Close();  // This will close the RegisterForm
        }

        // This function checks whether the provided password meets the defined complexity requirements.
        private bool IsPasswordComplex(string password)
        {
            if (password.Length < 8) return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasDigit = false;
            bool hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (char.IsDigit(c)) hasDigit = true;
                else hasSpecial = true; // Consider any non-alphanumeric as special
            }

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }

    }

}