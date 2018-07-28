using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataEntity;
using System.Data.Entity;
using System.Collections;
using DataModel.Extension;
using DataModel.DataViewModel;
namespace DataModel.DataStore
{
    public partial class Ctrl : Core
    {

        public int CreateObjectRelate(ObjectRelate ObjObjectRelate)
        {
            try
            {
                db.ObjectRelates.Add(ObjObjectRelate);
                db.SaveChanges();
                return (int)EnumCore.Result.action_true;
            }
            catch (Exception e)
            {

                return (int)EnumCore.Result.action_false;
            }
        }

      

    }
}
