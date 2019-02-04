using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Одиноко_проживающие
{
    internal class CommandServer
    {
        private SqlCommand _command;
        private SqlConnection _connect = LoadProgram.Connect;

        public bool ConnectDb()
        {
            try
            {
                _connect.Open();
            }catch(SqlException ex)
            {
                if(ex.Number == 53)
                {
                    LoadProgram.InizializeConnectString();
                    _connect = LoadProgram.Connect;
                    try
                    {
                        _connect.Open();
                        _connect.Close();
                        return true;
                    }catch(SqlException)
                    {
                        return false;
                    }
                    
                }
                return false;
            }finally
            {
                _connect.Close();
            }
            return true;
        }

        #region Методы
        public string[] ExecNoReturnServer(string nameFunction, string parameters)
        {
            string[] returnServer;
            try
            {
                returnServer = GetServerCommandExecNoReturnRetry(nameFunction, parameters);
            }
            catch (Exception ex)
            {
                new CommandClient().WriteFileError(ex, nameFunction + " " + parameters);
                return new string[2] { "1", ""};
            }
            finally
            {
                _connect.Close();
            }
            return returnServer;
        }

        public string[] ExecReturnServer(string nameFunction, string parameters)
        {
            string[] returnServer = { };
            try
            {
                returnServer = GetServerCommandExecReturnServerRetry(nameFunction, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                new CommandClient().WriteFileError(ex, nameFunction + " " + parameters);
            }
            finally
            {
                _connect.Close();
            }
            return returnServer;
        }

        internal List<string> ComboBoxList(string command, bool emptyLine)
        {
            var list = new List<string>();

            try
            {
                list = GetComboBoxListRetry(command, emptyLine);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                new CommandClient().WriteFileError(exception, command);
            }
            finally
            {
                _connect.Close();
            }
            return list;
        }

        internal DataSet DataGridSet(string command)
        {
            var dataSet = new DataSet();
            try
            {
                dataSet = GetDataGridSetRetry(command);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                new CommandClient().WriteFileError(ex, command);
            }
            finally
            {
                _connect.Close();
            }
            return dataSet;
        }
        #endregion

        #region Методы основные
        private string[] GetServerCommandExecNoReturnRetry(string nameFunction, string parameters)
        {
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
            try
            {
                _connect.Open();
            }catch(SqlException)
            {
                ConnectDb();
                _connect.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _connect.Open();
            }

            _command.ExecuteNonQuery();
            var returnServer = new string[2];
            returnServer[1] = "Успешно";

            return returnServer;
        }

        private string[] GetServerCommandExecReturnServerRetry(string nameFunction, string parameters)
        {
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

            try
            {
                _connect.Open();
            }
            catch (SqlException)
            {
                ConnectDb();
                _connect.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _connect.Open();
            }

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

        private List<string> GetComboBoxListRetry(string command, bool emptyLine)
        {
            _command = _connect.CreateCommand();
            _command.CommandType = CommandType.Text;
            _command.CommandText = command;
            try
            {
                _connect.Open();
            }
            catch (SqlException)
            {
                ConnectDb();
                _connect.Open();
            }            
            var dataReader = _command.ExecuteReader();
            var list = new List<string>();

            if(emptyLine)
                list.Add("");

            while (dataReader.Read())
            {
                list.Add(dataReader.GetString(0));
            }
            dataReader.Close();
            _connect.Close();
            return list;
        }

        private DataSet GetDataGridSetRetry(string command)
        {
            try
            {
                _connect.Open();
            }catch (SqlException)
            {
                ConnectDb();
                _connect.Open();
            }

            _command = new SqlCommand(command, _connect);
            _command.CommandTimeout = 0;
            var dataAdapter = new SqlDataAdapter(_command);

            //DataTable dt = new DataTable();
            //dataAdapter.Fill(dt);
            //return dt;

            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            return dataSet;
        }
        #endregion
    }
}
