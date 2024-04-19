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
using Excel = Microsoft.Office.Interop.Excel;
namespace Kvartirs
{
    public partial class AllBuildingForm : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);

        DataTable dtBuilding, dataTableMaterial, dataTableBase;
        DataSet dsBuilding;
        SqlDataAdapter dataAdapterBuilding;
        BindingSource bsBuilding, bsMaterial, bsBase;
        bool add_items;
        public AllBuildingForm()
        {
            InitializeComponent();
            LoadDataFromTable();
            loadComboAbonement();
            loadComboCustomer();
        }
        void loadComboCustomer()
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
        void loadComboAbonement()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter dataAdapterBase = new SqlDataAdapter("SELECT ID_abonement, abonement_name FROM tAbonement order by abonement_name", connection);
                dataTableBase = new DataTable();
                dataAdapterBase.Fill(dataTableBase);
                bsBase = new BindingSource();
                bsBase.DataSource = dataTableBase;
                cmbAbonement.DataSource = dataTableBase;
                cmbAbonement.ValueMember = "ID_abonement";
                cmbAbonement.DisplayMember = "abonement_name";
            }
        }
        void LoadDataFromTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    dtBuilding = new DataTable();
                    dataAdapterBuilding = new SqlDataAdapter();
                    dataAdapterBuilding.SelectCommand = new SqlCommand(" SELECT rtrim(str(tBuy.ID_buy)) as ID_buy, tBuy.dateofbuy, "+
                        "tBuy.ID_customer, tCustomer.sFIO, tCustomer.bithday, "+
                        " tCustomer.passport, tBuy.ID_abonement, tAbonement.abonement_name,"+
                        " tAbonement.count_hour, tAbonement.price FROM tAbonement"+
                        " join(tbuy join tCustomer on tBuy.ID_customer = tCustomer.ID_customer)"+
                        " on tAbonement.ID_abonement = tBuy.ID_abonement", connection);
                    dataAdapterBuilding.Fill(dtBuilding);
                    bsBuilding = new BindingSource();
                    bsBuilding.DataSource = dtBuilding;
                    bindingNavigator1.BindingSource = bsBuilding;
                    dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvTypeTO.DataSource = bsBuilding;
                    dgvTypeTO.Columns[0].HeaderText = "Номер договора";
                    dgvTypeTO.Columns[1].HeaderText = "Дата покупки";
                    dgvTypeTO.Columns[2].Visible = false;
                    dgvTypeTO.Columns[3].HeaderText = "Клиент";
                    dgvTypeTO.Columns[4].HeaderText = "Дата рождения";
                    dgvTypeTO.Columns[5].HeaderText = "Серия и номер паспорта";
                    dgvTypeTO.Columns[6].Visible = false;
                    dgvTypeTO.Columns[7].HeaderText = "Абонемент";
                    dgvTypeTO.Columns[8].HeaderText = "кол-во часов";
                    dgvTypeTO.Columns[9].HeaderText = "Цена";
                    add_items = false;
                    if (bsBuilding.Count <= 0) tsbDelete.Enabled = false;
                    else tsbDelete.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            BuildingForm building_form = new BuildingForm();
            building_form.ShowDialog();
            LoadDataFromTable();
        }
        void ShowBuildingToChange()
        {
            if ((bsBuilding.Count > 0) && (dgvTypeTO.SelectedRows.Count > 0))
            {
                int ID_SS = Convert.ToInt32(((DataRowView)this.bsBuilding.Current).Row["ID_buy"]);
                BuildingForm building_form = new BuildingForm(ID_SS);
                building_form.ShowDialog();
                LoadDataFromTable();
                bsBuilding.Position = bsBuilding.Find("ID_buy", (ID_SS));
            }
        }
        private void tsbChange_Click(object sender, EventArgs e)
        {
            ShowBuildingToChange();
        }
        private void dgvTypeTO_DoubleClick(object sender, EventArgs e)
        {
            ShowBuildingToChange();
        }
        private void chbMaterial_CheckedChanged(object sender, EventArgs e)
        {
                FilterData();
        }
        private void chbBase_CheckedChanged(object sender, EventArgs e)
        {
                FilterData();
        }
        private void cmbMaterial_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }
        private void cmbBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            FilterData();
        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (bsBuilding.Count > 0)
            {
                int i = bsBuilding.Position;
                string ID_SS = Convert.ToString(((DataRowView)this.bsBuilding.Current).Row["ID_buy"]);
                int hours = Convert.ToInt32(((DataRowView)this.bsBuilding.Current).Row["count_hour"]);
                int id_c = Convert.ToInt32(((DataRowView)this.bsBuilding.Current).Row["ID_customer"]);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    try
                    {
                        DialogResult result = MessageBox.Show("Вы действительно хотите удалить запись", "Внимание", MessageBoxButtons.YesNo);
                        if (result == DialogResult.No)
                        {
                            LoadDataFromTable();
                            return;
                        }
                        if (result == DialogResult.Yes)
                        {
                            SqlCommand commandDelete = new SqlCommand("Delete From tBuy where ID_buy = @ID_buy", connection);
                            commandDelete.Parameters.AddWithValue("@ID_buy", ID_SS);
                            commandDelete.ExecuteNonQuery();
                            UpdateHours(hours, id_c);
                        }
                    }
                    catch (SqlException exception)
                    {
                        if (exception.HResult == -2146232060)
                            MessageBox.Show("Ошибка удаления, есть связанные записи");
                        else MessageBox.Show(exception.Message);
                    }
                    finally
                    {
                         LoadDataFromTable();
                    }
                }
            }
        }

        void UpdateHours(int x, int y)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand commandUpdate = new SqlCommand("UPDATE tCustomer SET" +
                                 " count_hour=count_hour-@count_hour " +
                                 "  WHERE ID_customer= @IDSS", connection);
                    commandUpdate.Parameters.AddWithValue("@count_hour", x);
                    commandUpdate.Parameters.AddWithValue("@IDSS", y);
                    commandUpdate.ExecuteNonQuery();
                  //  MessageBox.Show("Запись обновлена");
                }
            }
            catch (SqlException exception)
            {
                MessageBox.Show(exception.ToString());
            }

        }
        private void tsbExcel_Click(object sender, EventArgs e)
        {
            PrintExcel();
        }
        private void PrintExcel()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\" + "Prodagi" + ".xltx";
            Excel.Application xlApp = new Excel.Application();
            Excel.Worksheet xlSheet = new Excel.Worksheet();
            try
            {
                //добавляем книгу
                xlApp.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                          Type.Missing, Type.Missing);
                //делаем временно неактивным документ
                xlApp.Interactive = false;
                xlApp.EnableEvents = false;
                Excel.Range xlSheetRange;
                //выбираем лист на котором будем работать (Лист 1)
                xlSheet = (Excel.Worksheet)xlApp.Sheets[1];
                //Название листа
                xlSheet.Name = "Продажи";
                int row = 2;
                int i = 0;
               if (dgvTypeTO.RowCount > 0)
                {
                    for (i = 0; i < dgvTypeTO.RowCount; i++)
                    {
                        xlSheet.Cells[row, 1] = (i + 1).ToString();
                        xlSheet.Cells[row, 2] = dgvTypeTO.Rows[i].Cells[0].Value.ToString(); 
                        xlSheet.Cells[row, 3] = dgvTypeTO.Rows[i].Cells[1].Value.ToString(); 
                        xlSheet.Cells[row, 4] = dgvTypeTO.Rows[i].Cells[3].Value.ToString();
                        xlSheet.Cells[row, 5] = dgvTypeTO.Rows[i].Cells[4].Value.ToString();
                        xlSheet.Cells[row, 6] = dgvTypeTO.Rows[i].Cells[5].Value.ToString();
                        xlSheet.Cells[row, 7] = dgvTypeTO.Rows[i].Cells[7].Value.ToString();
                        xlSheet.Cells[row, 8] = dgvTypeTO.Rows[i].Cells[8].Value.ToString();
                        xlSheet.Cells[row, 9] = dgvTypeTO.Rows[i].Cells[9].Value.ToString();
                        row++;
                        Excel.Range r = xlSheet.get_Range("A" + row.ToString(), "I" + row.ToString());
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                }
                row--;
                xlSheetRange = xlSheet.get_Range("A2:I" + row.ToString(), Type.Missing);
                xlSheetRange.Borders.LineStyle = true;
                row++;
                //выбираем всю область данных*/
                xlSheetRange = xlSheet.UsedRange;
                //выравниваем строки и колонки по их содержимому
                xlSheetRange.Columns.AutoFit();
                xlSheetRange.Rows.AutoFit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                //Показываем ексель
                xlApp.Visible = true;
                xlApp.Interactive = true;
                xlApp.ScreenUpdating = true;
                xlApp.UserControl = true;
            }
        }
        private void tbKadastr_TextChanged(object sender, EventArgs e)
        {
            FilterData();
        }
        void FilterData()
        {
            bsBuilding.RemoveFilter();
            var queries = new List<string>();
            if (chbMaterial.Checked)
            {
                queries.Add(string.Format("[ID_customer]={0}", cmbCustomer.SelectedValue));
            }
            if (chbBase.Checked)
            {
                queries.Add(string.Format("[ID_abonement]={0}", cmbAbonement.SelectedValue));
            }
            if (tbKadastr.Text != "")
            {
                queries.Add(string.Format("[ID_buy] LIKE '%{0}%'", tbKadastr.Text));
            }
            if (queries.Count >= 1)
            {
                var queryFilter = String.Join(" AND ", queries);
                bsBuilding.Filter = queryFilter;
            }
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
    }
}
