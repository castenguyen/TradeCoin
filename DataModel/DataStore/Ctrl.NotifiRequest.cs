using DataModel.DataEntity;
using DataModel.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        public async Task<int> SaveNotifiRequest(long userId, string userName, string email)
        {
            NotifiRequest request = db.NotifiRequests.SingleOrDefault(x => x.Email == email);
            if (request != null)
            {
                return (int)EnumCore.Result.action_false; //Case email to request exist, we return zero
            }
            else
            {
                NotifiRequest newRequest = new NotifiRequest();
                newRequest.UserID = userId;
                newRequest.UserName = userName;
                newRequest.CrtdDT = DateTime.Now;
                newRequest.Email = email;
                try
                {
                    db.NotifiRequests.Add(newRequest);
                    return await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Core core = new Core();
                    core.AddToExceptionLog("SaveNotifiRequest", "NotifiRequestController", "Create notification request error: " + ex.Message, userId);
                    return (int)EnumCore.Result.action_false;
                }
            }
        }
    }
}
