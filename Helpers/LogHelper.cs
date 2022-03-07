
using PhonebookBL.Data;
using PhonebookBL.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PhonebookBL.Helpers
{
    public static class LogHelper
    {
        public static async Task<bool> CreateLog(string message, Severity serverity,PbLogDbContext dBContext, string functionName = "")
        {
            string logmessage = (functionName == "" ? message : functionName + " " + message);
            if (logmessage == "") return false;

            Log log = new Log() {Message = logmessage, Serverity = serverity};
            try
            {
                await dBContext.Logs.AddAsync(log);
                await dBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        
    }
}
