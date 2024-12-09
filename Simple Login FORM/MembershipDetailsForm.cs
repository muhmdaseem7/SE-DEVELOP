using System;
using System.Windows.Forms;

namespace Login_FORM
{
    public partial class MembershipDetailsForm : Form
    {
        private Form parentForm;

        public MembershipDetailsForm(Form parent)
        {
            InitializeComponent();
            parentForm = parent;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            // Show the parent form (RegisterForm) and close this form
            if (parentForm != null)
            {
                parentForm.Show();
            }
            this.Close();
        }

    }
}

