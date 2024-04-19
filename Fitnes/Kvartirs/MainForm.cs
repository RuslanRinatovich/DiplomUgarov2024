using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kvartirs
{
    public partial class MainForm : Form
    {
        AbonementForm material_form;
        AllBuildingForm all_building_form;
        AllFlats all_flats;
        private CustomerForm customerForm;
        TrainerForm trenerForm;
        public MainForm()
        {
            InitializeComponent();
        }
        private void mnuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Вы действительно хотите выйти из приложения", "Внимание",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
             private void mnuMaterial_Click(object sender, EventArgs e)
        {
            if (material_form == null || material_form.IsDisposed)
            {
                material_form = new AbonementForm();
                material_form.MdiParent = this;
                material_form.Show();
            }
            else
            {
                material_form.Activate();
            }
        }
        private void mnuBuilding_Click(object sender, EventArgs e)
        {
            if (all_building_form == null || all_building_form.IsDisposed)
            {
                all_building_form = new AllBuildingForm();
                all_building_form.MdiParent = this;
                all_building_form.Show();
            }
            else
            {
                all_building_form.Activate();
            }
        }
        private void mnuFlats_Click(object sender, EventArgs e)
        {
            if (all_flats == null || all_flats.IsDisposed)
            {
                all_flats = new AllFlats();
                all_flats.MdiParent = this;
                all_flats.Show();
            }
            else
            {
                all_flats.Activate();
            }
        }

         private void mnuCustomer_Click(object sender, EventArgs e)
        {
            if (customerForm == null || customerForm.IsDisposed)
            {
                customerForm = new CustomerForm();
                customerForm.MdiParent = this;
                customerForm.Show();
            }
            else
            {
                customerForm.Activate();
            }
        }
        private void mnuTrener_Click(object sender, EventArgs e)
        {
            if (trenerForm == null || trenerForm.IsDisposed)
            {
                trenerForm = new TrainerForm();
                trenerForm.MdiParent = this;
                trenerForm.Show();
            }
            else
            {
                trenerForm.Activate();
            }
        }
    }
}
