using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using FAXCOMEXLib;
using AdoNetWindow;
using System.Windows.Forms;
using System.IO;

namespace Libs.Tools
{
    public class FaxSender
    {
        private static FaxServer faxServer;
        private static IFaxServer fs;
        private static IFaxOutgoingArchive oa;
        private static IFaxOutgoingQueue oq;
        private static FaxOutgoingJobs jobs;
        private static IFaxOutgoingJob job;
        //private static FaxDocument faxDoc;
        DataGridViewRow rowFax;
        DataGridView dgv;
        string pre_fax_number;
        string jobId;

        public FaxSender(DataGridViewRow rowFax, DataGridView dgv)
        {
            this.rowFax = rowFax;
            this.dgv = dgv;
            try
            {
                faxServer = new FaxServer();
                fs = (IFaxServer)faxServer;
                faxServer.Connect(Environment.MachineName);
                RegisterFaxServerEvents();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void RegisterFaxServerEvents()
        {
            faxServer.OnOutgoingJobAdded +=
                    new IFaxServerNotify2_OnOutgoingJobAddedEventHandler(faxServer_OnOutgoingJobAdded);
            faxServer.OnOutgoingJobChanged +=
                    new IFaxServerNotify2_OnOutgoingJobChangedEventHandler(faxServer_OnOutgoingJobChanged);
            faxServer.OnOutgoingJobRemoved +=
                    new IFaxServerNotify2_OnOutgoingJobRemovedEventHandler(faxServer_OnOutgoingJobRemoved);
            faxServer.OnIncomingJobRemoved += 
                    new IFaxServerNotify2_OnIncomingJobRemovedEventHandler(faxServer_OnIncomingJobRemoved);

            var eventsToListen =
                      FAX_SERVER_EVENTS_TYPE_ENUM.fsetFXSSVC_ENDED | FAX_SERVER_EVENTS_TYPE_ENUM.fsetOUT_QUEUE
                    | FAX_SERVER_EVENTS_TYPE_ENUM.fsetOUT_ARCHIVE | FAX_SERVER_EVENTS_TYPE_ENUM.fsetQUEUE_STATE
                    | FAX_SERVER_EVENTS_TYPE_ENUM.fsetACTIVITY | FAX_SERVER_EVENTS_TYPE_ENUM.fsetDEVICE_STATUS;

            faxServer.ListenToServerEvents(eventsToListen);
        }

        #region Event Handlers/Listeners
        private void faxServer_OnIncomingJobRemoved(FaxServer pfaxserver, string bstrjobid)
        {
            jobId = bstrjobid;
            Console.WriteLine("IN - Job ID: " + bstrjobid + " Job Removed from Fax Queue");
            var baseDir = pfaxserver.Configuration.ArchiveLocation;
            var inbox = baseDir + "\\Inbox";
            string[] files = Directory.GetFiles(inbox);

            //Method That search for current file inside the file list...
            Func<string[], string, string> getFilePath =
                (fileList, searchId) => (
                    from file in fileList
                    let fileName = Path.GetFileNameWithoutExtension(file)
                    let dollarIndex = fileName.IndexOf('$')
                    where dollarIndex != -1
                    where Path.GetFileNameWithoutExtension(file)?.Substring(dollarIndex + 1) == searchId
                    select file
                    ).FirstOrDefault();

            string filePath = getFilePath(files, bstrjobid);
            
        }
        private void faxServer_OnOutgoingJobAdded(FaxServer faxServer, string bstrJobId)
        {
            Console.WriteLine("OnOutgoingJobAdded event fired. A fax is added to the outgoing queue.");
            rowFax.Cells["job_id"].Value = bstrJobId;
            dgv.Update();
        }

        private void faxServer_OnOutgoingJobChanged(FaxServer faxServer, string bstrJobId, FaxJobStatus JobStatus)
        {
            jobId = bstrJobId;
            //string d = faxServer.;
            string fax_number = "";
            oq = fs.Folders.OutgoingQueue;
            oa = fs.Folders.OutgoingArchive;
            try
            {
                jobs = oq.GetJobs();
                job = oq.GetJob(bstrJobId);
                foreach (IFaxOutgoingJob j in jobs)  //jobs=outgoingqueue = to be sent
                {
                    if (j.Id == bstrJobId)
                    {
                        fax_number = j.Recipient.FaxNumber;
                        System.Console.WriteLine("Current fax number : " + j.Recipient.FaxNumber);

                        for (int i = 0; i < dgv.Rows.Count; i++)
                        {
                            if (fax_number == dgv.Rows[i].Cells["fax_number"].Value.ToString())
                            {
                                dgv.Rows[i].Cells["is_complete"].Value = true;
                                break;
                            }
                        }
                    }
                }

                Console.WriteLine("OnOutgoingJobChanged event fired. A fax is changed to the outgoing queue.");
                faxServer.Folders.OutgoingQueue.Refresh();
                PrintFaxStatus(JobStatus, fax_number);
                pre_fax_number = fax_number;
            }
            catch
            { }
        }

        private void faxServer_OnOutgoingJobRemoved(FaxServer faxServer, string bstrJobId)
        {
            jobId = bstrJobId;
            if (rowFax.Cells["status"].Value != null && rowFax.Cells["status"].Value.ToString() == "Sending Fax...")
            { 
                rowFax.Cells["status"].Value = "Success!";
                rowFax.Cells["is_complete"].Value = true;
            }
            Console.WriteLine("OnOutgoingJobRemoved event fired. Fax job is removed to outbound queue.");
        }
        #endregion

        private void PrintFaxStatus(FaxJobStatus faxJobStatus, string fax_number)
        {
            string msg = "";
            if (faxJobStatus.ExtendedStatusCode == FAX_JOB_EXTENDED_STATUS_ENUM.fjesDIALING)
            {
                msg = "Dialing...";
                Console.WriteLine("Dialing...");
            }
            else if (faxJobStatus.ExtendedStatusCode == FAX_JOB_EXTENDED_STATUS_ENUM.fjesTRANSMITTING)
            {
                msg = "Sending Fax...";
                Console.WriteLine("Sending Fax...");
            }
            else if (faxJobStatus.Status == FAX_JOB_STATUS_ENUM.fjsCOMPLETED
                && faxJobStatus.ExtendedStatusCode == FAX_JOB_EXTENDED_STATUS_ENUM.fjesCALL_COMPLETED)
            {
                msg = "successfully";
                Console.WriteLine("Fax is sent successfully.");
            }
            else if (faxJobStatus.Status == FAX_JOB_STATUS_ENUM.fjsINPROGRESS
                && faxJobStatus.ExtendedStatusCode == FAX_JOB_EXTENDED_STATUS_ENUM.fjesCALL_COMPLETED)
            {
                msg = "successfully";
                Console.WriteLine("Fax is sent successfully.");
            }
            else if (faxJobStatus.ExtendedStatusCode == FAX_JOB_EXTENDED_STATUS_ENUM.fjesFATAL_ERROR)
            {               
                msg = "Error...";
                Console.WriteLine("Fax is Error.");
            }
            else if (faxJobStatus.Status == FAX_JOB_STATUS_ENUM.fjsFAILED)
            {
                msg = "Failed";
                Console.WriteLine("Fax is Failed.");
            }
            else if (faxJobStatus.Status == FAX_JOB_STATUS_ENUM.fjsPAUSED)
            {
                msg = "Paused";
                Console.WriteLine("Fax is Paused.");
            }
            else if (faxJobStatus.Status == FAX_JOB_STATUS_ENUM.fjsCANCELED)
            {
                msg = "Canceled";
                Console.WriteLine("Fax is Canceled.");
            }
            else if (faxJobStatus.Status == FAX_JOB_STATUS_ENUM.fjsRETRYING)
            {
                msg = "Retrying...";
                Console.WriteLine("Fax is Retrying.");
            }
            else if (faxJobStatus.Status == FAX_JOB_STATUS_ENUM.fjsINPROGRESS
                && faxJobStatus.ExtendedStatusCode.ToString() == "1073743874")
            {
                msg = "Time over..";
                Console.WriteLine("Fax is time over.");
            }
            else
            {
                msg = "Failed(Unkown error)";
                Console.WriteLine("Fax is Failed(Unkown error).");
            }
            //Status update
            if (dgv.Rows.Count > 0)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(fax_number))
                        fax_number = pre_fax_number;

                    if (dgv.Rows[i].Cells["fax_number"].Value != null && dgv.Rows[i].Cells["fax_number"].Value.ToString() == fax_number)
                    {
                        dgv.Rows[i].Cells["status"].Value = msg;
                        break;
                    }
                }
            }
        }

        public void SendFax(string faxNumber, string[] files_path, bool isReservation = false, string send_time = null)
        {
            FaxDocument faxDoc = new FaxDocument();
            try
            {
                FaxDocumentSetup(faxNumber, files_path, out faxDoc, isReservation, send_time);
                
                object submitReturnValue = new object();
                //단일 파일발송
                if (files_path.Length == 1)
                    submitReturnValue = faxDoc.Submit(faxServer.ServerName);
                //다량 파일 발송
                else
                    submitReturnValue = faxDoc.Submit2(faxServer.ServerName, out submitReturnValue);

                //submitReturnValue = faxDoc.Submit2(faxServer.ServerName, out submitReturnValue);
            }
            catch (System.Runtime.InteropServices.COMException comException)
            {
                //Status update
                rowFax.Cells["status"].Value = "보낼수 없은 첨부파일 형식입니다.";
                dgv.Update();
                StringBuilder qry = new StringBuilder();
                qry.Append($"=========================================================================");
                qry.Append($"\nError code : " + comException.ErrorCode);
                qry.Append($"\nError connecting to fax server. Error Message: " + comException.Message);
                qry.Append($"\nStackTrace: " + comException.StackTrace);
                qry.Append($"\n=========================================================================");
                Console.WriteLine(qry.ToString());
            }
        }
        string[] strJobIds;
        private void FaxDocumentSetup(string faxNumber, string[] files_path, out FaxDocument faxDoc, bool isReservation = false, string send_time = null)
        {
            faxDoc = new FaxDocument();
            faxDoc.Priority = FAX_PRIORITY_TYPE_ENUM.fptHIGH;
            faxDoc.ReceiptType = FAX_RECEIPT_TYPE_ENUM.frtNONE;
            faxDoc.AttachFaxToReceipt = true;

            faxDoc.Sender.Name = "Ato trading";
            faxDoc.Sender.Company = "Ato Trading";
            faxDoc.Subject = "Ato Handling Product List";
            faxDoc.DocumentName = "";
            faxDoc.Recipients.Add(faxNumber, "Customer");

            //예약발송
            if (isReservation)
            {
                faxDoc.ScheduleType = FAX_SCHEDULE_TYPE_ENUM.fstSPECIFIC_TIME;
                faxDoc.ScheduleTime = Convert.ToDateTime(send_time);
            }


            //단일발송
            if (files_path.Length == 1)
            {
                faxDoc.Body = files_path[0];
            }
            //다량발송
            else
            {
                string[] files = files_path;

                strJobIds = new string[files_path.Length];
                for (int i = 0; i < files_path.Length - 1; i++)
                {
                    strJobIds[i] = "fax" + i;
                }
                faxDoc.Bodies = files;
            }
        }
    }
}