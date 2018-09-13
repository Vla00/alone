using System;
using System.Linq;
using System.Management;

namespace Одиноко_проживающие
{
    public class AdapterOptions
    {
        public string AdapterName { get; set; }

        public string MacAddress { get; set; }

        public string[] IpAdress { get; set; }

        public string ServiceName { get; set; }

        public string[] SubnetMask { get; set; }

        public string[] Gateways { get; set; }

        public string[] DnsAddress { get; set; }
    }

    public class IpConfiguration
    {
        public AdapterOptions GetAdapterOptions(string mac)
        {
            AdapterOptions adapterOptions = new AdapterOptions();
            ManagementClass network = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection managementObjectCollection = network.GetInstances();
            foreach (ManagementBaseObject managementBaseObject in from ManagementBaseObject managementBaseObject in managementObjectCollection where (bool)managementBaseObject["ipEnabled"] where (string)managementBaseObject["MacAddress"] == mac select managementBaseObject)
            {
                //adapterOptions.AdapterName = adapterName;
                adapterOptions.MacAddress = (string) managementBaseObject["MacAddress"];
                adapterOptions.IpAdress = (string[]) managementBaseObject["IPAddress"];
                adapterOptions.SubnetMask = (string[]) managementBaseObject["IPSubnet"];
                adapterOptions.Gateways = (string[]) managementBaseObject["DefaultIPGateway"];
                adapterOptions.DnsAddress = (string[]) managementBaseObject["DNSServerSearchOrder"];
                return adapterOptions;
            }
            return null;
        }

        public void SetAdapterOptions(AdapterOptions adapterOptions)
        {
            ManagementClass netManagementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection netManagementObjectCollection = netManagementClass.GetInstances();
            foreach (var o in netManagementObjectCollection)
            {
                try
                {
                    var managementObject = (ManagementObject)o;
                    if (!(bool)managementObject["ipEnabled"])
                        continue;
                    if ((string)managementObject["MacAddress"] != adapterOptions.MacAddress) continue;
                    ManagementBaseObject ipAddr = managementObject.GetMethodParameters("EnableStatic");
                    ManagementBaseObject ipGate = managementObject.GetMethodParameters("SetGateways");
                    ManagementBaseObject ipDns = managementObject.GetMethodParameters("SetDNSServerSearchOrder");
                    ipAddr["IPAddress"] = adapterOptions.IpAdress;
                    ipAddr["SubnetMask"] = adapterOptions.SubnetMask;
                    ipDns["DNSServerSearchOrder"] = adapterOptions.DnsAddress;
                    ipGate["DefaultIPGateway"] = adapterOptions.Gateways;

                    managementObject.InvokeMethod("SetDNSServerSearchOrder", ipDns, null);
                    managementObject.InvokeMethod("SetGateways", ipGate, null);
                    managementObject.InvokeMethod("EnableStatic", ipAddr, null);
                    return;
                }
                    // ReSharper disable once RedundantCatchClause
                catch (Exception)
                {
                    throw;
                }
                
            }
        }
    }
}
