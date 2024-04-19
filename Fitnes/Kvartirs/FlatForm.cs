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
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms.VisualStyles;
using static Kvartirs.models;

namespace Kvartirs
{
    public partial class FlatForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataTable dtFlat, dtRaschet, dtTarif, dtBuildings, dtCustomer;
        DataSet dsFlat;
        Visit visit = null;
        SqlDataAdapter dataAdapterFlat, dataAdapterRaschet;
        BindingSource bsVisitBindingSource, bsBuilding, bsTarif, bsCustomer, bsRaschet;
        bool add_items;
        bool caninto;
        public int ID_visit { get; set; }

        public int ID_customer { get; set; }
        private string fio;
        public FlatForm()
        {
            InitializeComponent();
            ID_visit = -1;
            ID_customer = -1;
            add_items = false;
            chbTrener.Checked = false;
            cmbWorker.Enabled = false;
            loadListBox();
            loadComboCustomer();
            cmbWorker.SelectedIndex = -1;
            
        }

        private void chbTrener_CheckedChanged(object sender, EventArgs e)
        {
            if (chbTrener.Checked)
            {
                cmbWorker.Enabled = true;
            }
            else
            {
                cmbWorker.Enabled = false;
                cmbWorker.SelectedIndex = -1;
            }
        }

        public FlatForm(int ID)
        {
            InitializeComponent();
            ID_visit = ID;
            ID_customer = -1;
            add_items = true;
            loadListBox();
            loadComboCustomer();

        }

        private void tbCustomer_TextChanged(object sender, EventArgs e)
        {
          bsBuilding.RemoveFilter();
            bsBuilding.Filter = string.Format("[customer] LIKE '%{0}%'", tbCustomer.Text);
        }

        private void lstCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }

        void loadListBox()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dtaBuildings = new SqlDataAdapter("SELECT ID_customer, passport + ' ' + LTRIM(RTRIM(sFIO)) as customer, sFiO  from tCustomer Order by passport, sFIO", connection);
                dtBuildings = new DataTable();
                dtaBuildings.Fill(dtBuildings);
                bsBuilding = new BindingSource();
                bsBuilding.DataSource = dtBuildings;
                lstCustomer.DataSource = bsBuilding;
                lstCustomer.ValueMember = "ID_customer";
                lstCustomer.DisplayMember = "customer";
            }
        }

        private void dtpTimeLeft_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTimeLeft.Value.TimeOfDay < dtpTimeIn.Value.TimeOfDay)
                dtpTimeLeft.Value = dtpTimeIn.Value;
        }

        private void dtpTimeIn_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTimeLeft.Value.TimeOfDay < dtpTimeIn.Value.TimeOfDay)
                dtpTimeLeft.Value = dtpTimeIn.Value;
        }

        void loadComboCustomer()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapterCustomer = new SqlDataAdapter("SELECT ID_worker, passport + ' ' + sFIO as worker from tWorker Order by passport, sFIO", connection);
                dtCustomer = new DataTable();
                dataAdapterCustomer.Fill(dtCustomer);
                bsCustomer = new BindingSource();
                bsCustomer.DataSource = dtCustomer;
                cmbWorker.DataSource = bsCustomer;
                cmbWorker.ValueMember = "ID_worker";
                cmbWorker.DisplayMember = "worker";
            }
        }

        void LoadDataFromTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    SqlCommand SelectCommand = new SqlCommand(" SELECT tVisit.ID_visit, tVisit.visitdate, "+
                        " tVisit.timein, tVisit.timeleft, tVisit.ID_customer, " +
                        " tCustomer.sFIO," +
                        " ISNULL(tVisit.ID_worker, 0) ID_worker, ISNULL(tWorker.sFIO, 'без тренера') Worker" +
                        " FROM tWorker RIGHT join(tVisit join tCustomer on tVisit.ID_customer = tCustomer.ID_customer) " +
                         " on tVisit.ID_worker = tWorker.ID_worker WHERE ID_visit = " + ID_visit+ "Order  by  tVisit.visitdate, tVisit.timein, tVisit.timeleft", connection);
                    SqlDataReader reader = SelectCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        visit = new Visit();
                        visit.ID_visit = Convert.ToInt32(reader.GetValue(0));
                        visit.visitdate = Convert.ToDateTime(reader.GetValue(1));

                        TimeSpan myTimeSpan = ((SqlDataReader)reader).GetTimeSpan(reader.GetOrdinal("timein"));
                        visit.timein = new DateTime(myTimeSpan.Ticks);
                        // visit.timein = Convert.ToDateTime(reader.GetValue(2));
                        //visit.timeleft = Convert.ToDateTime(reader.GetValue(3));
                        myTimeSpan = ((SqlDataReader)reader).GetTimeSpan(reader.GetOrdinal("timeleft"));
                        visit.timeleft = new DateTime(myTimeSpan.Ticks);
                        visit.ID_customer = Convert.ToInt32(reader.GetValue(4));
                        visit.customer_name = Convert.ToString(reader.GetValue(5));
                        visit.ID_worker = Convert.ToInt32(reader.GetValue(6));
                        visit.worker_name = Convert.ToString(reader.GetValue(7));
                        if (visit.ID_worker != 0)
                            chbTrener.Checked = true;
                        else
                        {
                            chbTrener.Checked = false;
                        }
                        tbCustomer.Text = visit.customer_name;
                        dtpDate.Value = visit.visitdate;
                        dtpTimeIn.Value = dtpDate.Value.Add(visit.timein.TimeOfDay);
                        dtpTimeLeft.Value = dtpDate.Value.Add(visit.timeleft.TimeOfDay);
                        cmbWorker.SelectedValue = visit.ID_worker;
                        ID_customer = visit.ID_customer;
                        tbTimeLeft.Text = String.Format("{0}ч. {1}мин.", GetMinutes() / 60, GetMinutes() % 60);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        int GetMinutes()
        {
            if (lstCustomer.SelectedIndex == -1)
                return 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    ID_customer = Convert.ToInt32(lstCustomer.SelectedValue);
                    SqlCommand commandSelect = new SqlCommand("SELECT (count_hour*60+ count_minutes) as minutes " +
                                 "FROM tCustomer " +
                                 "  WHERE ID_customer= @IDSS", connection);
                    commandSelect.Parameters.AddWithValue("@IDSS", Convert.ToInt32(ID_customer));
                    int x = Convert.ToInt32(commandSelect.ExecuteScalar());
                   // MessageBox.Show(x.ToString());
                    return x;
                }
            }
            catch (SqlException exception)
            {
                MessageBox.Show(exception.ToString());
            }
            return 0;
        }

        void UpdateHours(int x)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    ID_customer = Convert.ToInt32(lstCustomer.SelectedValue);
                    SqlCommand commandUpdate = new SqlCommand("UPDATE tCustomer SET" +
                                 " count_hour=@count_hour, count_minutes=@count_minutes " +
                                 "  WHERE ID_customer= @IDSS", connection);
                    commandUpdate.Parameters.AddWithValue("@count_hour", x / 60 );
                    commandUpdate.Parameters.AddWithValue("@count_minutes", x % 60);
                    commandUpdate.Parameters.AddWithValue("@IDSS", Convert.ToInt32(ID_customer));
                    commandUpdate.ExecuteNonQuery();
                    //     MessageBox.Show("Запись обновлена");
                }
            }
            catch (SqlException exception)
            {
                MessageBox.Show(exception.ToString());
            }

        }
        void SaveData()
        {
            int ID_SS = 0;
            try
            {
                connection.Close();
                connection.Open();
        SqlCommand commandInsert = new SqlCommand("INSERT INTO [tVisit] VALUES(" +
                                        "@visitdate," +
                                        "@timein," +
                                        "@timeleft," +
                                        "@ID_customer," +
                                        "@ID_worker" +
                                        ") ; SELECT SCOPE_IDENTITY()", connection);

                commandInsert.Parameters.AddWithValue("@visitdate", dtpDate.Value.Date);
                commandInsert.Parameters.AddWithValue("@timein", dtpTimeIn.Value.ToString("HH:mm"));
                commandInsert.Parameters.AddWithValue("@timeleft", dtpTimeLeft.Value.ToString("HH:mm"));
                ID_customer = Convert.ToInt32(lstCustomer.SelectedValue);
                commandInsert.Parameters.AddWithValue("@ID_customer",ID_customer);
                if (cmbWorker.SelectedIndex == -1)
                    commandInsert.Parameters.AddWithValue("@ID_worker", DBNull.Value);
                else
                    commandInsert.Parameters.AddWithValue("@ID_worker", cmbWorker.SelectedValue);

                ID_visit = Convert.ToInt32(commandInsert.ExecuteScalar());
                MessageBox.Show("Запись добавлена");
                add_items = true;
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

        void UpdateData()
        {
           
            try
            {
                connection.Close();
                connection.Open();
                SqlCommand commandUpdate = new SqlCommand("UPDATE tVisit SET" +
                                  " visitdate=@visitdate," +
                                  "timein=@timein," +
                                        "timeleft=@timeleft," +
                                        "ID_customer=@ID_customer," +
                                        "ID_worker=@ID_worker" +
                                "  WHERE ID_Visit= @IDSS", connection);
                commandUpdate.Parameters.AddWithValue("@visitdate", dtpDate.Value.Date);
                commandUpdate.Parameters.AddWithValue("@timein", dtpTimeIn.Value.ToString("HH:mm"));
                commandUpdate.Parameters.AddWithValue("@timeleft", dtpTimeLeft.Value.ToString("HH:mm"));
                ID_customer = Convert.ToInt32(lstCustomer.SelectedValue);
                commandUpdate.Parameters.AddWithValue("@ID_customer", ID_customer);
                if (cmbWorker.SelectedIndex ==-1)
                    commandUpdate.Parameters.AddWithValue("@ID_worker", DBNull.Value);
                else
                    commandUpdate.Parameters.AddWithValue("@ID_worker", cmbWorker.SelectedValue);
                commandUpdate.Parameters.AddWithValue("@IDSS", visit.ID_visit);
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
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (bsBuilding.Count <= 0)
                return;
            if (GetMinutes() == 0)
            {
                MessageBox.Show("Купите абонемент", "Вход запрещен", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (dtpTimeLeft.Value.TimeOfDay < dtpTimeIn.Value.TimeOfDay)
            {
                MessageBox.Show("Время входа и выхода не корректны", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if ((tbCustomer.Text == ""))
            {
                MessageBox.Show("Ключевые поля пустые", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
             int h = GetMinutes();
             int x = Convert.ToInt32(dtpTimeLeft.Value.Subtract(dtpTimeIn.Value).TotalMinutes);
            if (add_items == true)
            {
                int y = Convert.ToInt32(visit.timeleft.Subtract(visit.timein).TotalMinutes);
                x = x - y;
                if (x > h)
                {
                    MessageBox.Show("Не достаточно времени", "Вход запрещен", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                UpdateData();
                
                //MessageBox.Show(h.ToString() + " " + x.ToString()+" " + y.ToString());
                UpdateHours(h - x);
            }
            else
            {
                if (x > h)
                {
                    MessageBox.Show("Не достаточно времени", "Вход запрещен", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                SaveData();
                //MessageBox.Show(h.ToString() + " " + x.ToString());
                UpdateHours(h - x);
            }
            tbTimeLeft.Text = String.Format("{0}ч. {1}мин.", GetMinutes() / 60, GetMinutes() % 60);
        }

     

        private void FlatForm_Load(object sender, EventArgs e)
        {
            caninto = false;
            LoadDataFromTable();
            
        }

        private void lstBuilding_DoubleClick(object sender, EventArgs e)
        {
            if (lstCustomer.Items.Count <= 0)
                return;
            if (lstCustomer.SelectedIndex != -1)
            {
                tbCustomer.Text = lstCustomer.GetItemText(lstCustomer.SelectedItem);
                tbTimeLeft.Text = String.Format("{0}ч. {1}мин.", GetMinutes() / 60, GetMinutes()% 60); 
                               // MessageBox.Show(lstCustomer.SelectedValue.ToString());
            }
        }
    }
}
