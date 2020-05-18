using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Одиноко_проживающие
{
    internal class CommandClient
    {
        public string[] ReaderFile(string nameFile)
        {
            var sr = new StreamReader(nameFile, Encoding.UTF8);
            var countLineFile = 0;

            while (sr.ReadLine() != null)
            {
                countLineFile++;
            }

            sr = new StreamReader(nameFile);
            var strFile = new string[countLineFile];

            for (int i = 0; i < countLineFile; i++)
            {
                strFile[i] = sr.ReadLine();
            }

            return strFile;
        }

        public void ReadFileError()
        {
            try
            {
                string textAll = null;
                using (StreamReader sr = new StreamReader("messageError.txt", Encoding.Default))
                {
                    textAll = sr.ReadToEnd();
                }

                if (string.IsNullOrEmpty(textAll))
                    return;

                string[] textSplit = textAll.Split(new[] { "================================" }, StringSplitOptions.None);

                var command = new CommandServer();

                for (int i = 0; i < textSplit.Length - 1; i++)
                {
                    if(!string.IsNullOrEmpty(textSplit[i]))
                        command.ExecNoReturnServer("ErrorsAdd", "'" + textSplit[i].Replace("'", @"-") + "','user'");
                }

                File.Create("messageError.txt").Close();
            }
            catch(Exception ex)
            {
                WriteFileError(ex, "ReadFileError");
            }
        }

        public void WriteFileError(Exception message, string parametr)
        {
            using (var sw = new StreamWriter("messageError.txt", true, Encoding.Default))
            {
                sw.WriteLine("================================");
                sw.WriteLine("\n" + DateTime.Now);
                
                if (!string.IsNullOrEmpty(parametr))
                    sw.WriteLine("parameters: " + parametr);

                if (message == null) return;
                sw.WriteLine(message.Message);
                sw.WriteLine(message.StackTrace);
            }
        }

        public string CharTo(string s)
        {
            try
            {
                return s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length - 1).ToLower();
            }
            catch (Exception)
            {
                return s;
            }
        }

        #region Конфигурация
        public void CheackConfiguration()
        {
            if (!CheackUser())
                CreateUser();

            if (!CheackeAutoSearch())
                CreateAutoSearch();

            if (!CheacNameComp())
                CreateNameComp();
        }

        private bool CheackUser()
        {
            try
            {
                var document = new XmlDocument();
                document.Load("config.xml");

                XmlNode root = document.DocumentElement;
                foreach (XmlNode node in root.ChildNodes)
                {
                    foreach (XmlNode nod in node.ChildNodes)
                    {
                        switch (nod.Name)
                        {
                            case "Users":
                                return true;
                        }
                    }
                }
            }
            catch (Exception) { }
            return false;
        }

        private void CreateUser()
        {
            XDocument xdoc = XDocument.Load("config.xml");

            XElement root = new XElement("user");
            root.Add(new XElement("Users", ""));
            xdoc.Element("mconfig").Add(root);
            xdoc.Save("config.xml");
        }

        public bool CheackeAutoSearch()
        {
            try
            {
                var document = new XmlDocument();
                document.Load("config.xml");

                XmlNode root = document.DocumentElement;
                foreach (XmlNode node in root.ChildNodes)
                {
                    foreach (XmlNode nod in node.ChildNodes)
                    {
                        switch (nod.Name)
                        {
                            case "AutoSearch":
                                return true;
                        }
                    }
                }
            }
            catch (Exception) { }
            return false;
        }

        private void CreateAutoSearch()
        {
            XDocument xdoc = XDocument.Load("config.xml");
            XElement rootConnection = xdoc.Element("mconfig");

            //XElement root = new XElement("AutoSearch", "true");
            foreach (XElement xe in rootConnection.Elements("user").ToList())
            {
                xe.Add(new XElement("AutoSearch", "true"));
            }
            xdoc.Save("config.xml");
        }

        private bool CheacNameComp()
        {
            try
            {
                var document = new XmlDocument();
                document.Load("config.xml");

                XmlNode root = document.DocumentElement;
                foreach (XmlNode node in root.ChildNodes)
                {
                    foreach (XmlNode nod in node.ChildNodes)
                    {
                        switch (nod.Name)
                        {
                            case "NameComputer":
                                return true;
                        }
                    }
                }
            }
            catch (Exception) { }
            return false;
        }

        private void CreateNameComp()
        {
            XDocument xdoc = XDocument.Load("config.xml");
            XElement rootConnection = xdoc.Element("mconfig");

            foreach (XElement xe in rootConnection.Elements("user").ToList())
            {
                xe.Add(new XElement("NameComputer", Environment.MachineName));
            }

            xdoc.Save("config.xml");
        }

        #endregion
    }
}
