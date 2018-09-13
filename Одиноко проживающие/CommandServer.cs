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

        public bool ConnectDB()
        {
            try
            {
                _connect.Open();
            }catch(SqlException ex)
            {
                if(ex.Number == 53)
                {
                    LoadProgram.InizializeConnectString();
                    //LoadProgram.Connect.ConnectionString = LoadProgram.ConnectBuilder.ConnectionString;
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
        public string[] GetServerCommandExecNoReturnServer(string nameFunction, string parameters)
        {
            string[] returnServer = { };
            try
            {
                returnServer = GetServerCommandExecNoReturnRetry(nameFunction, parameters);
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

        public string[] GetServerCommandExecReturnServer(string nameFunction, string parameters)
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

        internal List<string> GetComboBoxList(string command, bool status)
        {
            var list = new List<string>();

            try
            {
                list = GetComboBoxListRetry(command, status);
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

        internal DataSet GetDataGridSet(string command)
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
                ConnectDB();
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
                ConnectDB();
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

        private List<string> GetComboBoxListRetry(string command, bool status)
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
                ConnectDB();
                _connect.Open();
            }            
            var dataReader = _command.ExecuteReader();
            var list = new List<string>();

            if(status)
                list.Add("");

            while (dataReader.Read())
            {
                list.Add(dataReader.GetString(0).ToString());
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
                ConnectDB();
                _connect.Open();
            }            
            var dataAdapter = new SqlDataAdapter(command, _connect);
            var dataSet = new DataSet();
            dataAdapter.Fill(dataSet);
            return dataSet;
        }
        #endregion
    }
}
