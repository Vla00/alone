using System;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Data;
using Telerik.WinControls.UI;

namespace Одиноко_проживающие
{
    public partial class TypeHelp : RadForm
    {
        private BindingSource _bindingSource;

        public TypeHelp()
        {
            InitializeComponent();
            MyRussionRadGridLocalizationProvider.CurrentProvider = new MyRussionRadGridLocalizationProvider();
            MyRadMessageLocalizationProvider.CurrentProvider = new MyRadMessageLocalizationProvider();
        }

        private void TypeHelp_Shown(object sender, EventArgs e)
        {
            UpdateGridView();
        }

        private void UpdateGridView()
        {
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select key_typeHelp, typeHelp as [Тип помощи] from typeHelp order by typeHelp").Tables[0] };
            radGridView1.Invoke(new MethodInvoker(delegate ()
            {
                radGridView1.DataSource = _bindingSource;
                radGridView1.Columns[0].IsVisible = false;

                using (radGridView1.DeferRefresh())
                {
                    radGridView1.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;
                    foreach (GridViewDataColumn column in radGridView1.Columns)
                    {
                        column.BestFit();
                    }
                }

                radGridView1.Columns[1].WrapText = true;
            }));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*var commandServer = new CommandServer();

            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                if (_statusOperation)
                {
                    var returnOperation = commandServer.GetServerCommandExecNoReturnServer("addTypeHelp", "'" + textBox1.Text + "'");
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была добавленна.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была добавленна. Вы хотите добавить еще одну запись?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            UpdateGridView();
                            button5.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
                else
                {
                    dataGridView1.Visible = false;
                    if (dataGridView1.CurrentRow == null) return;
                    var returnOperation = commandServer.GetServerCommandExecNoReturnServer("editTypeHelp", "'" + dataGridView1.CurrentRow.Cells[0].Value + "','" + textBox1.Text + "'");
                    dataGridView1.Visible = true;
                    if (returnOperation[1] == "Ошибка")
                    {
                        MessageBox.Show(@"Запись не была изменена.", @"Ошибка", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (
                            MessageBox.Show(@"Запись была изменена. Вы хотите изменить еще одну запись?", @"Операция",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            UpdateGridView();
                            button5.PerformClick();
                        }
                        else
                        {
                            Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show(@"Заполните поле.", @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }*/
        }

        private void radGridView1_RowsChanging(object sender, GridViewCollectionChangingEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.ItemChanging)
            {
                var line = (GridViewRowInfo)e.NewItems[0];

                if(line.Cells[0].Value != null)
                {
                    if (e.NewValue != null)
                    {
                        if (string.IsNullOrEmpty(e.NewValue.ToString()))
                        {
                            RadMessageBox.Show("Не заполнено поле. Изменение отменено.", "Ошибка", MessageBoxButtons.OKCancel, RadMessageIcon.Info);
                        }
                        else
                        {
                            new CommandServer().GetServerCommandExecNoReturnServer("TypeHelpEdit", line.Cells[0].Value.ToString() + ",'" + e.NewValue.ToString() + "'");
                        }
                    }
                }                
            }
        }

        private void radGridView1_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            var commandServer = new CommandServer();
            _bindingSource = new BindingSource { DataSource = commandServer.GetDataGridSet(@"select key_typeHelp, typeHelp as [Тип помощи] from typeHelp order by typeHelp").Tables[0] };

            radGridView1.Invoke(new MethodInvoker(delegate ()
            {
                radGridView1.DataSource = _bindingSource.DataSource;
            }));
        }

        private void radGridView1_UserAddingRow(object sender, GridViewRowCancelEventArgs e)
        {
            if (e.Rows[0].Cells[1].Value == null || e.Rows[0].Cells[1].Value.ToString() == "")
            {
                RadMessageBox.Show("Не заполнено поле. Добавление отменено.", "Ошибка", MessageBoxButtons.OKCancel, RadMessageIcon.Info);
                e.Cancel = true;
                return;
            }
            var returnSqlServer = new CommandServer().GetServerCommandExecReturnServer("TypeHelpAdd", "'" + e.Rows[0].Cells[1].Value.ToString() + "'");
        }

        private void TypeHelp_FormClosing(object sender, FormClosingEventArgs e)
        {
            Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
