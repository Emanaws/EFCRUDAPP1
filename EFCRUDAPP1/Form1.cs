using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EFCRUDAPP1
{
    public partial class Form1 : Form
    {/*First Added Method by Redwan*/
        Student model = new Student();
       

        public Form1()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }
        /*addd another funtion to all clear butten */
        void Clear()
        {
           txtFirstName.Text = txtLastName.Text = txtCity.Text = txtAddress.Text = "";
            btnSave.Text = "Save";
            btnDelete.Enabled = false;
            model.StudentID = 0;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            GetDate(); /* to Display saved data*/
        }
        public void GetDate()
        {
            /*Connection*/
            EFDBEntities _db = new EFDBEntities();
            var result = _db.Students.ToList();
            dgvStudent.DataSource = result;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            /*dicelar mathod for all button*/
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();
            using (EFDBEntities db = new EFDBEntities())
            {
                /*------------------------*/
                if (model.StudentID == 0) //insert
                    db.Students.Add(model);
                else //update
                    db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            }
            /* call clear funtion here*/
            Clear();
            PopulateDataGridView();
            MessageBox.Show("Well Comme!! Submitted Successfully");
        }
        void PopulateDataGridView()
        {
            dgvStudent.AutoGenerateColumns = false;
            using(EFDBEntities db = new EFDBEntities())
            {
                /*dgvStudent>>>>>>>>>>> name of gridview*/
                dgvStudent.DataSource = db.Students.ToList<Student>();
            }
        }

        private void dgvStudent_DoubleClick(object sender, EventArgs e)
        {
            /*Method for update and Delete info*/
            if(dgvStudent.CurrentRow.Index != -1)
            {
                model.StudentID = Convert.ToInt32(dgvStudent.CurrentRow.Cells["StudentID"].Value);
                using(EFDBEntities db = new EFDBEntities())
                {
                    model = db.Students.Where(x => x.StudentID == model.StudentID).FirstOrDefault();
                    txtFirstName.Text = model.FirstName;
                    txtLastName.Text = model.LastName;
                    txtCity.Text = model.City;
                    txtAddress.Text = model.Address;
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to Delete record?", "EF CRUD Operation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            using(EFDBEntities db = new EFDBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == EntityState.Detached)
                        db.Students.Attach(model);
                    db.Students.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Deleted successfully by Redwan Omer");
                }
        }

        private void dgvStudent_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
