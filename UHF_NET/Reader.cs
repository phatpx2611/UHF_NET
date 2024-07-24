﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PK_UHF_Test
{
    public static class Reader
    {
        public static List<byte[]> Inventory_G2(ref byte ConAddr,
                                              byte AdrTID,
                                              byte LenTID,
                                              byte TIDFlag,
                                              int PortHandle)
        {
            var epcBytes = new byte[5000];
            var epcBytesLen = 0;
            var epcCount = 0;
            StaticClassReaderB.Inventory_G2(ref ConAddr, AdrTID, LenTID, TIDFlag, epcBytes, ref epcBytesLen, ref epcCount, PortHandle);

            var epcList = new List<byte[]>(epcCount);
            using (var epcStream = new MemoryStream(epcBytes, 0, epcBytesLen))
            {
                for (int i = 0; i < epcCount; i++)
                {
                    var len = epcStream.ReadByte();
                    var epc = new byte[len];
                    epcStream.Read(epc, 0, len);
                    epcList.Add(epc);
                }

                if (epcStream.Position != epcStream.Length)
                {
                    throw new Exception("Tag count doesn't match.");
                }
            }

            return epcList;
        }


        public static void GetData(int portHandle)
        {
            byte[] ScanModeData = new byte[40960];
            int ValidDatalength, i;
            string temp, temps;
            ValidDatalength = 0;
            var fCmdRet = StaticClassReaderB.ReadActiveModeData(ScanModeData, ref ValidDatalength, portHandle);
            if (fCmdRet == 0)
            {
                temp = "";
                temps = ByteArrayToString(ScanModeData);
                for (i = 0; i < ValidDatalength; i++)
                {
                    temp = temp + temps.Substring(i * 2, 2) + " ";
                }
            }
        }

        private static string ByteArrayToHexString(byte[] data)
        {
            StringBuilder sb = new StringBuilder(data.Length * 3);
            foreach (byte b in data)
                sb.Append(Convert.ToString(b, 16).PadLeft(2, '0'));
            return sb.ToString().ToUpper();

        }

        public static string ByteArrayToString(byte[] b)
        {
            return BitConverter.ToString(b).Replace("-", "");
        }
    }

}