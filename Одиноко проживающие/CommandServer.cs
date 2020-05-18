using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Одиноко_проживающие
{
    internal class CommandServer
    {
        private BackgroundWorker backgroundWorker1;
        private Wait _Wait;
        private SqlCommand _command;
        private SqlConnection _connect;
        private SqlConnection _connectSql;
        private readonly string _connectString = Load.ProgramLoad.Connect.ConnectionString;


        #region Подключение
        public bool IsServerConnected(string conn)
        {
            using (_connectSql = new SqlConnection(conn))
            {
                try
                {
                    _connectSql.Open();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public bool SearchServer()
        {
            SqlConnectionStringBuilder ConnectBuilder = new SqlConnectionStringBuilder();
            ConnectBuilder = Home.programConn.connectionStringBuilder;

            backgroundWorker1 = new BackgroundWorker();
            if(backgroundWorker1.IsBusy != true)
            {
                backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
                _Wait = new Wait(true);
                _Wait.Show();
                backgroundWorker1.RunWorkerAsync();
            }

            ConnectBuilder.DataSource = @"10.76.92.220\TCSON";
            if (IsServerConnected(ConnectBuilder.ConnectionString))
            {
                Home.programConn.sqlConnection.ConnectionString = ConnectBuilder.ConnectionString;
                _connectSql.ConnectionString = ConnectBuilder.ConnectionString;
                _Wait.Close();
                return true;
            }

            ConnectBuilder.DataSource = @"192.168.1.10\TCSON";
            if (IsServerConnected(ConnectBuilder.ConnectionString))
            {
                Home.programConn.sqlConnection.ConnectionString = ConnectBuilder.ConnectionString;
                _connectSql.ConnectionString = ConnectBuilder.ConnectionString;
                _Wait.Close();
                return true;
            }

            ConnectBuilder.DataSource = @"86.57.207.146,1434\TCSON";
            if (IsServerConnected(ConnectBuilder.ConnectionString))
            {
                Home.programConn.sqlConnection.ConnectionString = ConnectBuilder.ConnectionString;
                _connectSql.ConnectionString = ConnectBuilder.ConnectionString;
                _Wait.Close();
                return true;
            }

            MessageBox.Show("Автопоиск не смог найти сервер. Укажите данные входа вручную.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            new Configuration("base", null, true).ShowDialog();
            _Wait.Close();
            return false;
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _Wait.Message = "Поиск сервера";
        }

        //private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    _Wait.Message = "Поиск сервера";
        //}
        #endregion

        #region Методы
        public string[] ExecNoReturnServer(string nameFunction, string parameters)
        {
            using (_connect = new SqlConnection(this._connectString))
            {
                try
                {
                    _connect.Open();
                }
                catch (SqlException ex)
                {
                    if (!IsServerConnected(_connectString))
                    {
                        if(Convert.ToBoolean(Home.programConn.configurationProgram.AutoSearch))
                            SearchServer();
                        else
                            new Configuration("base", null, true).ShowDialog();
                        _connect.Open();
                    }                        
                    else
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        new CommandClient().WriteFileError(ex, "exec " + nameFunction + " " + parameters + ";");
                    }
                }

                _command = _connect.CreateCommand();
                _command.CommandType = CommandType.Text;
                string command;
                if (parameters != null)
                {
                    command = "exec " + nameFunction + " " + parameters + ";";
                }
                else
                {
                    command = "exec " + nameFunction;
                }
                _command.CommandText = command;
                _command.ExecuteNonQuery();
                var returnServer = new string[2];
                returnServer[1] = "Успешно";

                return returnServer;
            }
        }

        public string[] ExecReturnServer(string nameFunction, string parameters)
        {
            using (_connect = new SqlConnection(this._connectString))
            {
                try
                {
                    _connect.Open();
                }
                catch (SqlException ex)
                {
                    if (!IsServerConnected(_connectString))
                    {
                        if (Convert.ToBoolean(Home.programConn.configurationProgram.AutoSearch))
                            SearchServer();
                        else
                            new Configuration("base", null, true).ShowDialog();
                        _connect = new SqlConnection(this._connectString);
                        _connect.Open();
                    }
                    else
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        new CommandClient().WriteFileError(ex, "exec " + nameFunction + " " + parameters + ";");
                    }
                }
                _command = _connect.CreateCommand();
                _command.CommandType = CommandType.Text;
                string command;

                if (parameters != null)
                {
                    command = "exec " + nameFunction + " " + parameters + ";";
                }
                else
                {
                    command = "exec " + nameFunction;
                }
                _command.CommandText = command;
                var returnServer = new string[2];
                returnServer[0] = _command.ExecuteScalar().ToString();

                if (returnServer[0].Contains("успешно"))
                {
                    returnServer[1] = "1";
                }
                else
                {
                    returnServer[1] = "0";
                }

                return returnServer;
            }

        }

        internal List<string> ComboBoxList(string command, bool emptyLine)
        {
            using (_connect = new SqlConnection(this._connectString))
            {
                _command = _connect.CreateCommand();
                _command.CommandType = CommandType.Text;
                _command.CommandText = command;

                try
                {
                    _connect.Open();
                }
                catch (SqlException ex)
                {
                    if (!IsServerConnected(_connectString))
                    {
                        if (Convert.ToBoolean(Home.programConn.configurationProgram.AutoSearch))
                            SearchServer();
                        else
                            new Configuration("base", null, true).ShowDialog();
                        _connect.Open();
                    }
                    else
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        new CommandClient().WriteFileError(ex, command);
                    }
                }

                var dataReader = _command.ExecuteReader();
                var list = new List<string>();

                if (emptyLine)
                    list.Add("");

                while (dataReader.Read())
                {
                    list.Add(dataReader.GetString(0));
                }
                dataReader.Close();
                _connect.Close();
                return list;
            }
        }

        internal DataSet DataGridSet(string command)
        {
            using (_connect = new SqlConnection(this._connectString))
            {
                try
                {
                    _connect.Open();
                }
                catch (SqlException ex)
                {
                    if (!IsServerConnected(_connectString))
                    {
                        if (Convert.ToBoolean(Home.programConn.configurationProgram.AutoSearch))
                            SearchServer();
                        else
                            new Configuration("base", null, true).ShowDialog();
                        _connect.Open();
                    }
                    else
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        new CommandClient().WriteFileError(ex, command);
                    }
                }
                _command = new SqlCommand(command, _connect)
                {
                    CommandTimeout = 0
                };
                var dataAdapter = new SqlDataAdapter(_command);

                var dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                return dataSet;
            }
        }
        #endregion
    }
}