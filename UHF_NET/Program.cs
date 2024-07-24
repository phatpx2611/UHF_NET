using PK_UHF_Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace UHF_NET
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                byte ComAddr = 0xFF;
                int PortHandle = 0;
                StaticClassReaderB.CloseNetPort(PortHandle);
                var fCmdRet = StaticClassReaderB.OpenNetPort(6000, "192.168.1.150", ref ComAddr, ref PortHandle);

                while (true)
                {
                    var list =  Reader.Inventory_G2(ref ComAddr, 0, 0, 0, PortHandle);
                    if (list.Count == 0)
                    {
                        StaticClassReaderB.CloseNetPort(PortHandle);
                        StaticClassReaderB.OpenNetPort(6000, "192.168.1.150", ref ComAddr, ref PortHandle);
                    }
                    foreach (var item in list)
                    {
                        Console.WriteLine(ByteArrayToString(item));
                    }

                }

            }
            catch (Exception ex)
            {

            }


        }
        private static string ByteArrayToString(byte[] b)
        {
            return BitConverter.ToString(b).Replace("-", "");
        }
    }
}
