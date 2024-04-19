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
    public partial class BuildingForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataTable dtBuilding, dataTableMaterial, dataTableBase, dtGoods;
        DataSet dsBuilding;
        SqlDataAdapter dataAdapterBuilding;
        BindingSource bsBuilding, bsMaterial, bsBase;
        List <Abonement> ab = new List<Abonement>();
        bool add_items;
        int hours = 0;
        Buy build = null;
        public int ID_kadastr { get; set; }
        private void BuildingForm_Load(object sender, EventArgs e)
        {
            loadComboBase();
            loadComboMaterial();
            LoadDataFromTable();
        }
        public BuildingForm()
        {
            InitializeComponent();
            add_items = false;
        }
        public BuildingForm(int ID)
        {
            InitializeComponent();
            ID_kadastr = ID;
            add_items = true;
            cmbCustomer.Enabled = false;
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if ((cmbAbonement.SelectedIndex == -1)
              || (cmbCustomer.SelectedIndex == -1))
            {
                MessageBox.Show("Ключевые поля пустые");
                return;
            }
            if (add_items == true)
            {
                UpdateData();
            }
            else
            {
                SaveData();
            }
        }
        void LoadDataFromTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {

                    SqlCommand SelectCommand = new SqlCommand(" SELECT ID_buy, tBuy.dateofbuy, "+
                        "tBuy.ID_customer, tCustomer.sFIO, " +
                        " tBuy.ID_abonement, tAbonement.abonement_name," +
                        " tAbonement.count_hour, tAbonement.price FROM tAbonement" +
                        " join(tbuy join tCustomer on tBuy.ID_customer = tCustomer.ID_customer)" +
                        " on tAbonement.ID_abonement = tBuy.ID_abonement WHERE ID_buy ='" + ID_kadastr + "'", connection);
                    SqlDataReader reader = SelectCommand.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        build = new Buy();
                        build.ID_buy = Convert.ToInt32(reader.GetValue(0));
                        build.dateofbuy = Convert.ToDateTime(reader.GetValue(1));
                        build.ID_customer = Convert.ToInt32(reader.GetValue(2));
                        build.customer_name = Convert.ToString(reader.GetValue(3));
                        build.ID_abonement= Convert.ToInt32(reader.GetValue(4));
                        build.abonement_name = Convert.ToString(reader.GetValue(5));
                        build.count_hour = Convert.ToInt32(reader.GetValue(6));
                        build.price = Convert.ToDouble(reader.GetValue(7));
                        tbIDbuy.Text = build.ID_buy.ToString();
                        tbCount.Text = build.count_hour.ToString();
                        tbPrice.Text = build.price.ToString();
                        hours = build.count_hour;
                        cmbCustomer.SelectedValue = build.ID_customer;
                        cmbAbonement.SelectedValue = build.ID_abonement;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
      

        private void button1_Click(object sender, EventArgs e)
        {
            if ((cmbAbonement.SelectedIndex == -1)
             || (cmbCustomer.SelectedIndex == -1))
            {
                MessageBox.Show("Ключевые поля пустые");
                return;
            }
            if (add_items == true)
            {
                int x =  Convert.ToInt32(tbCount.Text)- hours;
                MessageBox.Show(x.ToString());
                UpdateHours(x);
                UpdateData();
               
            }
            else
            {
                SaveData();
                UpdateHours(Convert.ToInt32(tbCount.Text));
            }
        }

        private void cmbAbonement_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Abonement x in ab)
            {
                if (x.ID_abonement == Convert.ToInt32(cmbAbonement.SelectedValue))
                {
                    tbCount.Text = x.count_hour.ToString();
                    tbPrice.Text = x.price.ToString();
                }
            }
            //if (bsBase.Count > 0)
            //{
            //    int i = bsBase.Find("ID_abonement", Convert.ToString(cmbAbonement.SelectedValue));
            //    tbCount.Text = Convert.ToString(((DataRowView)this.bsBase[i]).Row["count_hour"]);
            //}
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }


        void UpdateHours(int x)
        {
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand commandUpdate = new SqlCommand("UPDATE tCustomer SET" +
                                 " count_hour=count_hour+@count_hour " +
                                 "  WHERE ID_customer= @IDSS", connection);
                    commandUpdate.Parameters.AddWithValue("@count_hour", x);
                    commandUpdate.Parameters.AddWithValue("@IDSS", Convert.ToInt32(cmbCustomer.SelectedValue));
                    commandUpdate.ExecuteNonQuery();
               //     MessageBox.Show("Запись обновлена");
                }
            }
            catch (SqlException exception)
            {
                MessageBox.Show(exception.ToString());
            }
           
        }
        void loadComboMaterial()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter dataAdapterMaterial = new SqlDataAdapter("SELECT ID_customer, passport +' '+ sfio as sfio from tCustomer Order by sfio", connection);
                dataTableMaterial = new DataTable();
                dataAdapterMaterial.Fill(dataTableMaterial);
                bsMaterial = new BindingSource();
                bsMaterial.DataSource = dataTableMaterial;
                cmbCustomer.DataSource = dataTableMaterial;
                cmbCustomer.ValueMember = "ID_customer";
                cmbCustomer.DisplayMember = "sfio";
            }
        }
        void loadComboBase()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter dataAdapterBase = new SqlDataAdapter("SELECT ID_abonement, abonement_name, price, count_hour FROM tAbonement order by abonement_name", connection);
                dataTableBase = new DataTable();
                dataAdapterBase.Fill(dataTableBase);
                bsBase = new BindingSource();
                bsBase.DataSource = dataTableBase;
                cmbAbonement.DataSource = dataTableBase;
                cmbAbonement.ValueMember = "ID_abonement";
                cmbAbonement.DisplayMember = "abonement_name";
                ab = new List<Abonement>();
                for (int i = 0; i < bsBase.Count; i++)
                {
                    Abonement x = new Abonement();
                    x.abonement_name = Convert.ToString(((DataRowView)this.bsBase[i]).Row["abonement_name"]);
                    x.ID_abonement = Convert.ToInt32(((DataRowView)this.bsBase[i]).Row["ID_abonement"]);
                    x.price = Convert.ToDouble(((DataRowView)this.bsBase[i]).Row["price"]);
                    x.count_hour = Convert.ToInt32(((DataRowView)this.bsBase[i]).Row["count_hour"]);
                    ab.Add(x);
                }
               
            }
        }

        void SaveData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand commandInsert = new SqlCommand("INSERT INTO [tBuy] VALUES(" +
                                "@dateofbuy," +
                                "@ID_abonement," +
                                "@ID_customer" +
                                ") ; SELECT SCOPE_IDENTITY()", connection);
                    commandInsert.Parameters.AddWithValue("@dateofbuy", dtpDateofbuy.Value);
                    commandInsert.Parameters.AddWithValue("@ID_abonement", Convert.ToInt32(cmbAbonement.SelectedValue));
                    commandInsert.Parameters.AddWithValue("@ID_customer", Convert.ToInt32(cmbCustomer.SelectedValue));
                    ID_kadastr = Convert.ToInt32(commandInsert.ExecuteScalar());

                    MessageBox.Show("Запись добавлена");
                    add_items = true;
                    cmbCustomer.Enabled = false;
                }
            }
            catch (SqlException exception)
            {
                MessageBox.Show(exception.ToString());
            }
            finally
            {
                LoadDataFromTable();
            }
        }
        void UpdateData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand commandUpdate = new SqlCommand("UPDATE tBuy SET" +
                                 " dateofbuy=@dateofbuy," +
                                "ID_abonement=@ID_abonement" +
         //                       "ID_customer=@ID_customer " +
                                "  WHERE ID_buy= @IDSS", connection);
                    commandUpdate.Parameters.AddWithValue("@dateofbuy", dtpDateofbuy.Value);
                    commandUpdate.Parameters.AddWithValue("@ID_abonement", Convert.ToInt32(cmbAbonement.SelectedValue));
                //    commandUpdate.Parameters.AddWithValue("@ID_customer", Convert.ToInt32(cmbCustomer.SelectedValue));
                    commandUpdate.Parameters.AddWithValue("@IDSS", build.ID_buy);
                    commandUpdate.ExecuteNonQuery();
                    MessageBox.Show("Запись обновлена");
                }
            }
            catch (SqlException exception)
            {
                MessageBox.Show(exception.ToString());
            }
            finally
            {
               LoadDataFromTable();
            }
        }
    }
}