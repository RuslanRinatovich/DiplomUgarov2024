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
    public partial class AllFlats : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString;
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ConnectionString);
        DataTable dtFlat, dataTableMaterial, dataTableBase;
        DataSet dsFlat;
        SqlDataAdapter dataAdapterFlat;
        BindingSource bsFlat, bsMaterial, bsBase;
        private void tsbChange_Click(object sender, EventArgs e)
        {
            ShowBuildingToChange();
        }
        void ShowBuildingToChange()
        {
            if ((bsFlat.Count > 0) && (dgvTypeTO.SelectedRows.Count > 0))
            {
                int ID_SS = Convert.ToInt32(((DataRowView)this.bsFlat.Current).Row["ID_visit"]);
                FlatForm flat_form = new FlatForm(ID_SS);
                flat_form.ShowDialog();
                LoadDataFromTable();
                bsFlat.Position = bsFlat.Find("ID_visit", (ID_SS));
            }
        }
        private void dgvTypeTO_DoubleClick(object sender, EventArgs e)
        {
            ShowBuildingToChange();
        }
        private void tbKadastr_TextChanged(object sender, EventArgs e)
        {
            bsFlat.RemoveFilter();
            bsFlat.Filter = string.Format("[sFIO] LIKE '%{0}%'", tbKadastr.Text);
        }

        int GetMinutes()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    int ID_SS = Convert.ToInt32(((DataRowView)this.bsFlat.Current).Row["ID_customer"]);
                    SqlCommand commandSelect = new SqlCommand("SELECT (count_hour*60+ count_minutes) as minutes " +
                                 "FROM tCustomer " +
                                 "  WHERE ID_customer= @IDSS", connection);
                    commandSelect.Parameters.AddWithValue("@IDSS", ID_SS);
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
                    int ID_SS = Convert.ToInt32(((DataRowView)this.bsFlat.Current).Row["ID_customer"]);
                    SqlCommand commandUpdate = new SqlCommand("UPDATE tCustomer SET" +
                                 " count_hour=@count_hour, count_minutes=@count_minutes " +
                                 "  WHERE ID_customer= @IDSS", connection);
                    commandUpdate.Parameters.AddWithValue("@count_hour", x / 60);
                    commandUpdate.Parameters.AddWithValue("@count_minutes", x % 60);
                    commandUpdate.Parameters.AddWithValue("@IDSS", Convert.ToInt32(ID_SS));
                    commandUpdate.ExecuteNonQuery();
                    //     MessageBox.Show("Запись обновлена");
                }
            }
            catch (SqlException exception)
            {
                MessageBox.Show(exception.ToString());
            }

        }
        private void tsbDelete_Click(object sender, EventArgs e)
        {
            if (bsFlat.Count > 0)
            {
                int i = bsFlat.Position;
                int ID_SS = Convert.ToInt32(((DataRowView)this.bsFlat.Current).Row["ID_visit"]);
                TimeSpan x = TimeSpan.Parse((((DataRowView)this.bsFlat.Current).Row["timein"]).ToString());
                TimeSpan y = TimeSpan.Parse((((DataRowView)this.bsFlat.Current).Row["timeleft"]).ToString());
               // DateTime y = Convert.ToDateTime(((DataRowView)this.bsFlat.Current).Row["timeleft"]);
                int z = Convert.ToInt32(y.Subtract(x).TotalMinutes);
               //MessageBox.Show(z.ToString());
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
                            SqlCommand commandDelete = new SqlCommand("Delete From tVisit where ID_visit = @ID_visit", connection);
                            commandDelete.Parameters.AddWithValue("@ID_visit", ID_SS);
                            commandDelete.ExecuteNonQuery();
                            UpdateHours(GetMinutes()+z);
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
        private void tsbAdd_Click(object sender, EventArgs e)
        {
            FlatForm flat_form = new FlatForm();
            flat_form.ShowDialog();
            LoadDataFromTable();
        }
        private void tsbExcel_Click(object sender, EventArgs e)
        {
            PrintExcel();
        }
        private void PrintExcel()
        {
            string fileName = System.Windows.Forms.Application.StartupPath + "\\" + "Visit" + ".xltx";
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
                xlSheet.Name = "Квартиры";
                int row = 2;
                int i = 0;
                if (dgvTypeTO.RowCount > 0)
                {
                    for (i = 0; i < dgvTypeTO.RowCount; i++)
                    {
                        string x = "";
                        xlSheet.Cells[row, 1] = (i+1).ToString();
                        DateTime y = Convert.ToDateTime(dgvTypeTO.Rows[i].Cells[1].Value);
                        xlSheet.Cells[row, 2] = y.Date.ToShortDateString(); 
                        xlSheet.Cells[row, 3] = dgvTypeTO.Rows[i].Cells[2].Value.ToString(); 
                        xlSheet.Cells[row, 4] = dgvTypeTO.Rows[i].Cells[3].Value.ToString();
                        xlSheet.Cells[row, 5] = dgvTypeTO.Rows[i].Cells[6].Value.ToString();
                        y = Convert.ToDateTime(dgvTypeTO.Rows[i].Cells[7].Value);
                        xlSheet.Cells[row, 6] = y.Date.ToShortDateString();
                        xlSheet.Cells[row, 7] = dgvTypeTO.Rows[i].Cells[8].Value.ToString();
                        xlSheet.Cells[row, 8] = dgvTypeTO.Rows[i].Cells[10].Value.ToString();
                        row++;
                        Excel.Range r = xlSheet.get_Range("A" + row.ToString(), "H" + row.ToString());
                        r.Insert(Excel.XlInsertShiftDirection.xlShiftDown);
                    }
                }
                row--;
                xlSheetRange = xlSheet.get_Range("A2:H" + row.ToString(), Type.Missing);
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
        bool add_items;
        public AllFlats()
        {
            InitializeComponent();
            LoadDataFromTable();
        }

        void LoadDataFromTable()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    dtFlat = new DataTable();
                    dataAdapterFlat = new SqlDataAdapter();
                    dataAdapterFlat.SelectCommand = new SqlCommand(" SELECT tVisit.ID_visit, tVisit.visitdate,"+
                        " tVisit.timein, tVisit.timeleft, tCustomer.Photo, tVisit.ID_customer, "+
                        " tCustomer.sFIO, tCustomer.bithday, tCustomer.passport,"+
                        " ISNULL(tVisit.ID_worker, 0) ID_worker, ISNULL(tWorker.sFIO, 'без тренера') Worker" +
                        " FROM tWorker RIGHT join(tVisit join tCustomer on tVisit.ID_customer = tCustomer.ID_customer) " +
                         " on tVisit.ID_worker = tWorker.ID_worker Order by  tVisit.visitdate, tVisit.timein, tVisit.timeleft", connection);
                    dataAdapterFlat.Fill(dtFlat);
                    bsFlat = new BindingSource();
                    bsFlat.DataSource = dtFlat;
                    bindingNavigator1.BindingSource = bsFlat;
                    dgvTypeTO.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dgvTypeTO.DataSource = bsFlat;
                    dgvTypeTO.Columns[0].Visible = false;
                    dgvTypeTO.Columns[1].HeaderText = "Дата посещения";
                    dgvTypeTO.Columns[2].HeaderText = "Время входа";
                    dgvTypeTO.Columns[3].HeaderText = "Время выхода";
                    dgvTypeTO.Columns[4].HeaderText = "Фото";
                    dgvTypeTO.Columns[5].Visible = false;
                    dgvTypeTO.Columns[6].HeaderText ="Клиент";
                    dgvTypeTO.Columns[7].HeaderText = "Дата рождения";
                    dgvTypeTO.Columns[8].HeaderText = "Серия и номер паспорта";
                    dgvTypeTO.Columns[10].HeaderText = "Тренер";
                    dgvTypeTO.Columns[9].Visible = false;
                    ((DataGridViewImageColumn)dgvTypeTO.Columns[4]).ImageLayout = DataGridViewImageCellLayout.Zoom;
                    add_items = false;
                    if (bsFlat.Count <= 0) tsbDelete.Enabled = false;
                    else tsbDelete.Enabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
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
