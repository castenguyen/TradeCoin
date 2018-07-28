using DataModel.DataEntity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Collections;

namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {
        /// <summary>
        /// lấy lên thông tin cấu hình của web. Vì là cấu hình nên bảng này chỉ có 1 dòng
        /// </summary>
        /// <returns></returns>
        public Config GetConfig()
        {
            Config Config = db.Configs.FirstOrDefault();
            return Config;
        }
        /// <summary>
        /// sửa table Config
        /// </summary>
        /// <param name="_config"></param>
        /// <returns></returns>
        public async Task<int> EditConfig(Config _config)
        {
            try
            {
                db.Entry(_config).State = EntityState.Modified;
                return await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}