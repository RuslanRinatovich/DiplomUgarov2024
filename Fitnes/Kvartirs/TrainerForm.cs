using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Kvartirs
{
    public partial class TrainerForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        BindingSource bsPeople, bsEduc;
        BindingSource bs2, bsPosts;
        DataTable dt, dtChilds, dtPosts, dtEduc, dtBrigada;
        private MaskedTextBox mtbData;
        DataTable dtJob;
        DataSet dsJob;
        private BindingSource bsJob, bsBrigada;
        SqlDataAdapter dataAdapter, dataAdapterPost;
        private bool Jobless = false;
        string IDSERIA = "";
        string IDNOMER = "";
        bool add_items, b1, b2;
        public TrainerForm()
        {
            InitializeComponent();
            LoadDataFromTable();
        }
    
        void LoadDataFromTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                ClearItems();
                connection.Open();
                SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM tWorker Order by sFIO, passport ", connection);
                dt = new DataTable();
                sda.Fill(dt);
                bsPeople = new BindingSource();
                //= dataTable;
                bsPeople.DataSource = dt;
                bindingNavigator1.BindingSource = bsPeople;
                // Очистка
                // textBox1
                if (bsPeople.Count > 0) toolStripButton2.Enabled = true;
                else toolStripButton2.Enabled = false;
                tbSurName.DataBindings.Clear();
                tbSurName.DataBindings.Add(new Binding("Text", bsPeople, "sFIO"));
                dtpBithday.DataBindings.Clear();
                dtpBithday.DataBindings.Add(new Binding("Value", bsPeople, "Bithday"));
                mtbPassport.DataBindings.Clear();
                mtbPassport.DataBindings.Add(new Binding("Text", bsPeople, "Passport"));
                tbPassportinfo.DataBindings.Clear();
                tbPassportinfo.DataBindings.Add(new Binding("Text", bsPeople, "Passportinfo"));
                tbAdress.DataBindings.Clear();
                tbAdress.DataBindings.Add(new Binding("Text", bsPeople, "Address"));
                tbPhone.DataBindings.Clear();
                tbPhone.DataBindings.Add(new Binding("Text", bsPeople, "Phone"));
                tbBase_worker.DataBindings.Clear();
                tbBase_worker.DataBindings.Add(new Binding("Text", bsPeople, "base_worker"));
                nudExperience.DataBindings.Clear();
                nudExperience.DataBindings.Add(new Binding("Text", bsPeople, "experience"));
                tbSpecial.DataBindings.Clear();
                tbSpecial.DataBindings.Add(new Binding("Text", bsPeople, "special"));
                tbPrice.DataBindings.Clear();
                tbPrice.DataBindings.Add(new Binding("Text", bsPeople, "price"));
                dgvTypeTO.DataSource = bsPeople;
                dgvTypeTO.Columns[0].Visible = false;
                dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvTypeTO.Columns[1].HeaderText = "ФИО";
                dgvTypeTO.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvTypeTO.Columns[2].HeaderText = "Дата рождения";
                dgvTypeTO.Columns[3].HeaderText = "Серия и номер паспорта";
                dgvTypeTO.Columns[4].HeaderText = "Кем и когда выдан";
                dgvTypeTO.Columns[5].HeaderText = "Адрес";
                dgvTypeTO.Columns[6].HeaderText = "Телефон";
                dgvTypeTO.Columns[7].HeaderText = "Образование";
                dgvTypeTO.Columns[8].HeaderText = "Стаж";
                dgvTypeTO.Columns[9].HeaderText = "Специализация";
                dgvTypeTO.Columns[10].HeaderText = "Стоимость часа";
                add_items = false;
            }
        }
       
        private void tbPassport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9')
            {
                return;
            }
            if (Char.IsControl(e.KeyChar)) return;
            e.Handled = true;
        }
        private void tbSurName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) return;
            if (Char.IsSeparator(e.KeyChar)) return;
            if (Char.IsControl(e.KeyChar)) return;
            e.Handled = true;
        }
        private void tbPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9') return;

            if (Char.IsControl(e.KeyChar)) return;
            e.Handled = true;
        }

        private void tbPrice_KeyPress(object sender, KeyPressEventArgs e)
        {

            System.Windows.Forms.TextBox textBox = (System.Windows.Forms.TextBox)sender;
            if ((e.KeyChar >= '0') && (e.KeyChar <= '9'))
                return;
            if (e.KeyChar == '.')
                e.KeyChar = ',';
            if (e.KeyChar == ',')
            {
                // в поле редактирования не может
                // быть больше одной запятой и запятая
                // не может быть первым символом
                if ((textBox.Text.IndexOf(',') != -1) ||
                      (textBox.Text.Length == 0))
                {
                    e.Handled = true;
                }
                return;
            }
            if (Char.IsControl(e.KeyChar))
            {
                // <Enter>, <Backspace>, <Esc>
                return;
            }
            // остальные символы запрещены
            e.Handled = true;
        }

        private void ClearItems()
        {
            tbSurName.Text = "";
            dtpBithday.Value = DateTime.Now;
            mtbPassport.Text = "";
            tbAdress.Text = "";
            tbPhone.Text = "";
            tbPassportinfo.Text = "";
            tbSpecial.Text = "";
            tbBase_worker.Text = "";
            tbPrice.Text = "0";
        }
        private void AddStripButton_Click(object sender, EventArgs e)
        {
            ClearItems();
            add_items = true;
            saveToolStripButton.Enabled = false;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (add_items)
            {
                add_items = false;
                saveToolStripButton.Enabled = true;
                LoadDataFromTable();
            }
            else if (bsPeople.Count > 0)
            {
                int i = bsPeople.Position;
                string ID_SS = ((DataRowView)this.bsPeople.Current).Row["ID_worker"].ToString();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись", "Внимание",
                            MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                        {
                            LoadDataFromTable();
                            return;
                        }
                        if (result == DialogResult.Yes)
                        {

                            SqlCommand commandDelete = new SqlCommand("Delete From tWorker where ID_worker = @ID_worker", connection);
                            commandDelete.Parameters.AddWithValue("@ID_worker", ID_SS);
                            commandDelete.ExecuteNonQuery();
                        }
                    }
                    catch (SqlException exception)
                    {
                        if ((uint)exception.ErrorCode == 0x80004005)
                            MessageBox.Show("Удаление прервано, есть связанные записи", "Error", MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        else
                            MessageBox.Show(exception.ToString());
                    }
                    finally
                    {
                        LoadDataFromTable();
                    }
                }
            }

        }

        void InsertData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tWorker] VALUES(" +
                                                    "@sFIO," +
                                                    "@bithday," +
                                                    "@passport," +
                                                    "@passportinfo," +
                                                    "@address," +
                                                    "@phone," +
                                                    "@base_worker," +
                                                    "@experience," +
                                                    "@special," +
                                                    "@price" +
                                                    ") ", connection);
                    commandInsert.Parameters.AddWithValue("@sFIO", tbSurName.Text);
                    commandInsert.Parameters.AddWithValue("@bithday", dtpBithday.Value.Date);
                    commandInsert.Parameters.AddWithValue("@passport", mtbPassport.Text);
                    commandInsert.Parameters.AddWithValue("@passportinfo", tbPassportinfo.Text);
                    commandInsert.Parameters.AddWithValue("@address", tbAdress.Text);
                    commandInsert.Parameters.AddWithValue("@phone", tbPhone.Text);
                    commandInsert.Parameters.AddWithValue("@base_worker", tbBase_worker.Text);
                    commandInsert.Parameters.AddWithValue("@experience", Convert.ToInt32(nudExperience.Value));
                    commandInsert.Parameters.AddWithValue("@special", tbSpecial.Text);
                    commandInsert.Parameters.AddWithValue("@price", tbPrice.Text);
                    commandInsert.ExecuteNonQuery();
                    MessageBox.Show("Запись добавлена");
                    add_items = false;
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    connection.Close();
                    LoadDataFromTable();
                }
            }
        }

        void UpdateData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string x = "";
                connection.Open();
                try
                {
                    string ID_SS = ((DataRowView)this.bsPeople.Current).Row["ID_worker"].ToString();
                    x = ID_SS;
                    SqlCommand commandUpdate = new SqlCommand("UPDATE tWorker SET" +
                                                    " sFIO=@sFIO," +
                                                    "bithday=@bithday," +
                                                    "passport=@passport," +
                                                    "passportinfo=@passportinfo," +
                                                    "address=@address," +
                                                    "phone=@phone," +
                                                    "base_worker=@base_worker," +
                                                    "experience=@experience," +
                                                    "special=@special," +
                                                    "price=@price " +
                                    "  WHERE ID_Worker= @IDSS", connection);
                    commandUpdate.Parameters.AddWithValue("@sFIO", tbSurName.Text);
                    commandUpdate.Parameters.AddWithValue("@bithday", dtpBithday.Value.Date);
                    commandUpdate.Parameters.AddWithValue("@passport", mtbPassport.Text);
                    commandUpdate.Parameters.AddWithValue("@passportinfo", tbPassportinfo.Text);
                    commandUpdate.Parameters.AddWithValue("@address", tbAdress.Text);
                    commandUpdate.Parameters.AddWithValue("@phone", tbPhone.Text);
                    commandUpdate.Parameters.AddWithValue("@base_worker", tbBase_worker.Text);
                    commandUpdate.Parameters.AddWithValue("@experience", Convert.ToInt32(nudExperience.Value));
                    commandUpdate.Parameters.AddWithValue("@special", tbSpecial.Text);
                    commandUpdate.Parameters.AddWithValue("@price", tbPrice.Text);
                    commandUpdate.Parameters.AddWithValue("@IDSS", ID_SS);
                    commandUpdate.ExecuteNonQuery();
                    MessageBox.Show("Запись обновлена");
                }
                catch (SqlException exception)
                {
                    MessageBox.Show(exception.ToString());
                }
                finally
                {
                    connection.Close();
                    LoadDataFromTable();
                    bsPeople.Position = bsPeople.Find("ID_worker", x);
                }
            }
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            if ((mtbPassport.Text == "") || (tbSurName.Text == "") || (tbPrice.Text == ""))

            {
                MessageBox.Show("Ключевые поля пустые", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (add_items)
            {
                InsertData();
                // UpdateData();
            }
            else
            {
                UpdateData();

            }
        }

        private void tbPassport_TextChanged(object sender, EventArgs e)
        {
            if (mtbPassport.Text == "" || tbSurName.Text == "") saveToolStripButton.Enabled = false;
            else saveToolStripButton.Enabled = true;

        }
        private void dgvTypeTO_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }
        private void dgGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex + 1).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);
        }

    }
}


