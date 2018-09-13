using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Одиноко_проживающие
{
    public partial class ReplaceForm : Form
    {
        private Thread _poolThread;

        public ReplaceForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            _poolThread.Abort();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _poolThread.Abort();
            _poolThread.Join();
            var processInfo = new ProcessStartInfo
            {
                Verb = "runas",
                FileName = Application.ExecutablePath
            };

            try
            {
                Process.Start(processInfo);
            }
            catch (Win32Exception) { }
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _poolThread.Abort();
            Environment.Exit(0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            _poolThread.Abort();
            ReplaceIpAddressCorp();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _poolThread.Abort();
            ReplaceIpAddressIthernet();
        }

        private void ReplaceIpAddressCorp()
        {
            var commandServer = new CommandServer();

            var ipConfig = new CommandClient().ReaderFile("ipconfig.cfg");
            var adapterOptions = new AdapterOptions
            {
                Gateways = new[] { ipConfig[1].Split(' ')[4] },
                IpAdress = new[] { ipConfig[1].Split(' ')[2] },
                SubnetMask = new[] { ipConfig[1].Split(' ')[3] },
                DnsAddress = new[] { ipConfig[1].Split(' ')[4] },
                MacAddress = ipConfig[0].Split(' ')[2]
            };
            new IpConfiguration().SetAdapterOptions(adapterOptions);
            var status = commandServer.ConnectDataBase();

            if (status)
            {
                Close();
            }

            var ipa = new IPAddress.IpAddress();

            while (true)
            {
                var ipList = ipa.ListIp();

                if (ipList == "255.255.252.0" && ipList == "255.255.0.0") continue;
                status = commandServer.ConnectDataBase();

                if (!status) continue;
                Close();
                return;
            }
        }

        public void ReplaceIpAddressUpdate()
        {
            var ipConfig = new CommandClient().ReaderFile("ipconfig.cfg");
            var adapterOptions = new AdapterOptions
            {
                Gateways = new[] { ipConfig[1].Split(' ')[4] },
                IpAdress = new[] { ipConfig[1].Split(' ')[2] },
                SubnetMask = new[] { ipConfig[1].Split(' ')[3] },
                DnsAddress = new[] { ipConfig[1].Split(' ')[4] },
                MacAddress = ipConfig[0].Split(' ')[2]
            };
            new IpConfiguration().SetAdapterOptions(adapterOptions);
            var ipa = new IPAddress.IpAddress();

            while (true)
            {
                var ipList = ipa.ListIp();

                if (ipList == "255.255.252.0" && ipList == "255.255.0.0") continue;
                Close();
                return;
            }
        }

        private void ReplaceIpAddressIthernet()
        {
            var commandServer = new CommandServer();
            var ipConfig = new CommandClient().ReaderFile("ipconfig.cfg");
            var adapterOptions = new AdapterOptions
            {
                Gateways = new[] { ipConfig[2].Split(' ')[4] },
                IpAdress = new[] { ipConfig[2].Split(' ')[2] },
                SubnetMask = new[] { ipConfig[2].Split(' ')[3] },
                DnsAddress = new[] { ipConfig[2].Split(' ')[4] },
                MacAddress = ipConfig[0].Split(' ')[2]
            };
            new IpConfiguration().SetAdapterOptions(adapterOptions);
            var status = commandServer.ConnectDataBase();

            if (status)
            {
                Close();
                return;
            }

            var ipa = new IPAddress.IpAddress();

            while (true)
            {
                var ipList = ipa.ListIp();

                if (ipList != "255.255.252.0" && ipList != "255.255.0.0") continue;
                status = commandServer.ConnectDataBase();

                if (!status) continue;
                Close();
                return;
            }
        }

        private void ReplaceIpAddress()
        {
            var main = Owner as Form1;
            var commandServer = new CommandServer();
            var ipa = new IPAddress.IpAddress();
            var ipList = ipa.ListIp();

            if (ipList == "255.255.252.0" || ipList == "255.255.0.0")
            {
                var ipConfig = new CommandClient().ReaderFile("ipconfig.cfg");
                var adapterOptions = new AdapterOptions
                {
                    Gateways = new[] { ipConfig[2].Split(' ')[4] },
                    IpAdress = new[] { ipConfig[2].Split(' ')[2] },
                    SubnetMask = new[] { ipConfig[2].Split(' ')[3] },
                    DnsAddress = new[] { ipConfig[2].Split(' ')[4] },
                    MacAddress = ipConfig[0].Split(' ')[2]
                };
                new IpConfiguration().SetAdapterOptions(adapterOptions);
                var status = commandServer.ConnectDataBase();

                if (status)
                {
                    Invoke(new MethodInvoker(Close), null);
                    return;
                }

                while (true)
                {
                    if (main != null)
                        Invoke(new MethodInvoker(delegate { main.label1.Text = @"Проверяется подключение"; }));
                    ipList = ipa.ListIp();
                    if (ipList == "255.255.252.0" && ipList == "255.255.0.0") continue;
                    status = commandServer.ConnectDataBase();

                    if (!status) continue;
                    Invoke(new MethodInvoker(Close), null);
                    return;
                }
            }
            else
            {
                var ipConfig = new CommandClient().ReaderFile("ipconfig.cfg");
                var adapterOptions = new AdapterOptions
                {
                    Gateways = new[] { ipConfig[1].Split(' ')[4] },
                    IpAdress = new[] { ipConfig[1].Split(' ')[2] },
                    SubnetMask = new[] { ipConfig[1].Split(' ')[3] },
                    DnsAddress = new[] { ipConfig[1].Split(' ')[4] },
                    MacAddress = ipConfig[0].Split(' ')[2]
                };
                new IpConfiguration().SetAdapterOptions(adapterOptions);
                var status = commandServer.ConnectDataBase();

                if (status)
                {
                    Invoke(new MethodInvoker(Close), null);
                    return;
                }

                while (true)
                {
                    if (main != null)
                        Invoke(new MethodInvoker(delegate { main.label1.Text = @"Проверяется подключение"; }));
                    ipList = ipa.ListIp();

                    if (ipList != "255.255.252.0" && ipList != "255.255.0.0") continue;
                    status = commandServer.ConnectDataBase();

                    if (!status) continue;
                    Invoke(new MethodInvoker(Close), null);
                    return;
                }
            }
        }

        private void ReplaceForm_Shown(object sender, EventArgs e)
        {
            try
            {
                _poolThread = new Thread(ReplaceIpAddress);
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                button5.Enabled = false;
                _poolThread.Start();
            }
            catch (ThreadAbortException abortException)
            {
                if ((int)abortException.ExceptionState == 0)
                {
                    Thread.ResetAbort();
                }
            }
        }
    }
}
