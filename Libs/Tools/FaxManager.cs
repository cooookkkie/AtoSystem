using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAXCOMEXLib;

namespace Libs.Tools
{
    public class FaxManager
    {

        public void FaxSender()
        {
            FaxServer faxServer = new FaxServer();
            faxServer.Connect("192.168.0.56");
            IFaxServer fs = (IFaxServer)faxServer;


            FaxDocument faxDoc = new FaxDocument();
            faxDoc.Priority = FAX_PRIORITY_TYPE_ENUM.fptHIGH;
            faxDoc.ReceiptType = FAX_RECEIPT_TYPE_ENUM.frtNONE;
            faxDoc.AttachFaxToReceipt = true;

            faxDoc.Sender.Name = "Ato trading";
            faxDoc.Sender.Company = "Ato Trading";
            faxDoc.Subject = "Ato Handling Product List";
            faxDoc.DocumentName = "";
            faxDoc.Recipients.Add("0512564100", "Customer");
            faxDoc.Body = @"C:\Cookies\TEMP\0512565560_0_0.tif";

            object submitReturnValue = faxDoc.Submit2(faxServer.ServerName, out submitReturnValue);


        }
    }
}
