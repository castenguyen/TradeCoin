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

        public async Task<int> AddComment(ContentComment ObjComment)
        {
            try
            {
                db.ContentComments.Add(ObjComment);
                return await db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                this.AddToExceptionLog("AddComment", "CoreCtrl_comment",e.ToString());
                return (int)EnumCore.Result.action_false;
            }

        }
        public IQueryable<ContentComment> GetlstComment()
        {
            var lstComment = db.ContentComments;
            return lstComment;
        }
        public ContentComment GetObjComment(long CommentID)
        {
            ContentComment ObjComment = db.ContentComments.Find(CommentID);
            return ObjComment;
        }
        public IQueryable<ContentComment> GetlstCommentHasNum(long? CommentId, long? ObjId, int? ObjTypeID, long? ParentId,int? state)
        {
            IQueryable<ContentComment> lstComment = db.ContentComments;
            if (CommentId.HasValue)
                lstComment = lstComment.Where(s => s.CommentId == CommentId.Value);
            if (ObjId.HasValue)
                lstComment = lstComment.Where(s => s.ContentObjId == ObjId.Value);
            if (ObjTypeID.HasValue)
                lstComment = lstComment.Where(s => s.ObjTypeId == ObjTypeID.Value);
            if (ParentId.HasValue)
                lstComment = lstComment.Where(s => s.ParentCommentId == ParentId.Value);
            if (!ParentId.HasValue)
                lstComment = lstComment.Where(s => s.ParentCommentId == null || s.ParentCommentId==0);
            if (state.HasValue)
                lstComment = lstComment.Where(s => s.StateId == state.Value);
            return lstComment;
        }

        public IQueryable<ContentComment> GetlstCommentByApprove()
        {
            var lstComment = db.ContentComments.Where(a => a.StateId == (long)EnumCore.StateType.cho_phep);
            return lstComment;
        }

        public List<CommentBlockViewModel> GetLstBlockComment(long ObjId,int ObjTypeID)
        {
            List<CommentBlockViewModel> MainObj = new List<CommentBlockViewModel>();
            List<ContentComment> LstParent = GetlstCommentHasNum(null, ObjId, ObjTypeID,null, (int)EnumCore.StateType.cho_phep).ToList();
            foreach (ContentComment item in LstParent)
            {
                CommentBlockViewModel itemmodel = new CommentBlockViewModel();
                List<ContentComment> lstChild = GetlstCommentHasNum(null, ObjId, ObjTypeID, item.CommentId, (int)EnumCore.StateType.cho_phep).ToList();
                itemmodel.MainObj = item;
                itemmodel.ListChild = lstChild;
                MainObj.Add(itemmodel);
            }
            return MainObj;
        }



        public IQueryable<ContentComment> GetlstCommentByUnApprove()
        {
            var lstComment = db.ContentComments.Where(a => a.StateId == (long)EnumCore.StateType.khong_cho_phep);
            return lstComment;
        }



        public IQueryable<ContentComment> GetlstContentComment()
        {
            try
            {
                return this.GetlstComment();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IEnumerable GetlstContentCommmentByApprove()
        {
            try
            {
                List<ContentComment> lst = this.GetlstCommentByApprove().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                return null;

            }
        }

        public IEnumerable GetlstContentCommmentByUnApprove()
        {
            try
            {
                List<ContentComment> lst = this.GetlstCommentByUnApprove().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                return null;

            }
        }


        public async Task<int> UpdateStateComment(int id, int state,DateTime AppDt,string AppName,long AppId)
        {
            ContentComment result = db.ContentComments.SingleOrDefault(a => a.CommentId == id);
            if (result != null)
            {
                result.StateId = state;
                result.AprvdDT = AppDt;
                result.AprvdUID = AppId;
                result.AprvdUerName = AppName;
                if (result.StateId == (int)EnumCore.StateType.cho_phep)
                    result.StateName = "Enable";
                if (result.StateId == (int)EnumCore.StateType.khong_cho_phep)
                    result.StateName = "Disable";
                db.Entry(result).State = EntityState.Modified;
            }
             return await db.SaveChangesAsync();
        }


        public int UpdateContentComment(ContentComment com)
        {
            db.Entry<ContentComment>(com).State = EntityState.Modified;
            return db.SaveChanges();
        }

        public void Delete(int? id)
        {
            var result = db.ContentComments.SingleOrDefault(a => a.CommentId == id);
            if (result != null)
            {
               // result.StateId = (long)EnumCore.StateType.trash;
                db.SaveChangesAsync();
            }
        }

        #region "TRI's Code"
        #region "Comments"
        /*
         * Method save comment of user
         * productId: product id
		 * comments: comment from logged user
		 * fullName: full name of logged user (default is empty string)
		 * email: email of logged user (default is empty string)
         * cms_db: Controll class from DataMode.DataStore
         * userId: Id of logged user
         */
        public async Task<int> SaveComments(long idObject, string comments, string fullName, string email, Ctrl cms_db, long userId, int objType,string objTypeName, long parentId)
        {
            if (fullName.Length > 250)
            {
                fullName = fullName.Substring(0, 249); //To make it fix to column data type length
            }

            if (email.Length > 250)
            {
                email = email.Substring(0, 249); //To make it fix to column data type length
            }
            if (objType == (int)EnumCore.ObjTypeId.san_pham)
            {
                Product product = cms_db.GetObjProductById(idObject);
                Classification classification = cms_db.GetObjClasscifiById((int)Extension.EnumCore.StateType.cho_duyet);
                if (product != null)
                {
                    ContentComment comment = new ContentComment();
                    comment.ParentCommentId = parentId;
                    comment.ContentObjId = idObject;
                    comment.ContentObjName = product.ProductName;
                    comment.Title = null;
                    comment.CommentText = StripHTMLAndScript(comments); //Keep comment clearn without html injection
                    comment.FullName = fullName;
                    comment.EmailAddress = email;
                    comment.CrtdDT = DateTime.Now;
                    comment.AprvdDT = null;
                    comment.AprvdUID = null;
                    comment.AprvdUerName = null;
                    comment.StateId = classification.ClassificationId;
                    comment.StateName = classification.ClassificationNM;
                    comment.CrtdGuestUserId = null;
                    comment.LikeCount = null;
                    comment.LastLikedDT = null;
                    comment.IPAddress = GetIPAddress();
                    comment.ObjTypeId = objType;
                    comment.ObjTypeName = objTypeName;
                    try
                    {
                        return await cms_db.AddComment(comment);
                    }
                    catch (Exception ex)
                    {
                        Core core = new Core();
                        core.AddToExceptionLog("SaveComments", "ProductController", "Create comment error: " + ex.Message, userId);
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                Microsite mic = cms_db.GetObjMicrositeByID(idObject);
                if (mic!=null)
                {
                    ContentComment comment = new ContentComment();
                    comment.ParentCommentId = parentId;
                    comment.ContentObjId = idObject;
                    comment.ContentObjName = mic.Name;
                    comment.Title = null;
                    comment.CommentText = StripHTMLAndScript(comments); //Keep comment clearn without html injection
                    comment.FullName = fullName;
                    comment.EmailAddress = email;
                    comment.CrtdDT = DateTime.Now;
                    comment.AprvdDT = null;
                    comment.AprvdUID = null;
                    comment.AprvdUerName = null;
                    comment.StateId = (int)EnumCore.StateType.cho_phep;
                    comment.StateName = "Cho phép";
                    comment.CrtdGuestUserId = null;
                    comment.LikeCount = null;
                    comment.LastLikedDT = null;
                    comment.IPAddress = GetIPAddress();
                    comment.ObjTypeId = objType;
                    comment.ObjTypeName = objTypeName;
                    try
                    {
                        return await cms_db.AddComment(comment);
                    }
                    catch (Exception ex)
                    {
                        Core core = new Core();
                        core.AddToExceptionLog("SaveComments", "ProductController", "Create comment error: " + ex.Message, userId);
                        return 0;
                    }
                }
                return 0;
            }
        }

        /*
         * Method getting all comments of a product
         * productId: ID of product (from product detail page)
         * objTypeId: Object type id (This can retrive comment for many kind like: product, media, news, videos etc...)
         * cms_db: Controll class from DataMode.DataStore
         */
        public List<ObjContentComment> GetCommentByProductId(int productId, int objTypeId, Ctrl cms_db)
        {
            Product product = cms_db.GetObjProductById(productId);
            if (product != null)
            {
                try
                {
                    List<ObjContentComment> comments = new List<ObjContentComment>();
                    string query = "SELECT cm.*, mc.ThumbURL AS UserAvatar, ur.Id AS UserId FROM ContentComment cm LEFT JOIN dbo.[User] ur "
                                        + "ON cm.EmailAddress = ur.EMail LEFT JOIN MediaContent mc ON ur.Id = mc.ContentObjId WHERE cm.ContentObjId = '" + productId + "' ORDER BY cm.CrtdDT DESC";
                    using (var context = new alluneedbEntities())
                    {
                        System.Data.Entity.Infrastructure.DbRawSqlQuery<ObjContentComment> data = db.Database.SqlQuery<ObjContentComment>(query);
                        comments = data.ToList();
                    }
                    if (comments.Count > 0)
                    {
                        return comments;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /*
         * Method getting latest comment of a product
         * productId: ID of product (from product detail page)
         * objTypeId: Object type id (This can retrive comment for many kind like: product, media, news, videos etc...)
         * cms_db: Controll class from DataMode.DataStore
         */
        public List<ObjContentComment> GetLatestCommentByProductId(int productId, int objTypeId, Ctrl cms_db)
        {
            Product product = cms_db.GetObjProductById(productId);
            if (product != null)
            {
                try
                {
                    List<ObjContentComment> comments = new List<ObjContentComment>();
                    string query = "SELECT top (1) cm.*, mc.ThumbURL AS UserAvatar, ur.Id AS UserId FROM ContentComment cm LEFT JOIN dbo.[User] ur "
                                        + "ON cm.EmailAddress = ur.EMail LEFT JOIN MediaContent mc ON ur.Id = mc.ContentObjId WHERE cm.ContentObjId = '" + productId + "' ORDER BY cm.CrtdDT DESC";
                    using (var context = new alluneedbEntities())
                    {
                        System.Data.Entity.Infrastructure.DbRawSqlQuery<ObjContentComment> data = db.Database.SqlQuery<ObjContentComment>(query);
                        comments = data.ToList();
                    }
                    if (comments.Count > 0)
                    {
                        return comments;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /*
         * Method getting comment by comment ID
         * commentId: ID of product (from product detail page)
         * cms_db: Controll class from DataMode.DataStore
         */
        public ObjContentComment GetCommentById(long commentId, Ctrl cms_db, long userId)
        {
            try
            {
                ObjContentComment comment = new ObjContentComment();
                string query = "SELECT cm.*, mc.ThumbURL AS UserAvatar, ur.Id AS UserId FROM ContentComment cm LEFT JOIN dbo.[User] ur "
                                    + "ON cm.EmailAddress = ur.EMail LEFT JOIN MediaContent mc ON ur.Id = mc.ContentObjId WHERE cm.CommentId = '" + commentId + "' ORDER BY cm.CrtdDT DESC";
                using (var context = new alluneedbEntities())
                {
                    System.Data.Entity.Infrastructure.DbRawSqlQuery<ObjContentComment> data = db.Database.SqlQuery<ObjContentComment>(query);
                    comment = data.FirstOrDefault();
                }
                return comment;
            }
            catch (Exception ex)
            {
                Core core = new Core();
                core.AddToExceptionLog("GetCommentById", "ProductController", "Get comment by ID Error: " + ex.Message, userId);
                return null;
            }
        }

        /*
         * TRI-28062016 Update like number from table dbo.ContentComment
		 * commentId: ID of comment
		 * fullName: Full name of logged user
		 * email: Email of logged user (default is empty string)
         * cms_db: DataModel Controller
         * userId: ID of logged user
         */
        public async Task<int> LikeComments(int commentId, string fullName, string email, Ctrl cms_db, long userId)
        {
            ContentComment comment = db.ContentComments.SingleOrDefault(a => a.CommentId == commentId);
            if (comment != null)
            {
                int newLikeCount = 1;
                if (comment.LikeCount != null)
                {
                    newLikeCount = (int)comment.LikeCount + 1;
                }
                comment.LikeCount = newLikeCount;
                try
                {
                    await db.SaveChangesAsync();
                    return newLikeCount;
                }
                catch (Exception ex)
                {
                    Core core = new Core();
                    core.AddToExceptionLog("LikeComments", "ProductController", "Like comment error: " + ex.Message, userId);
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /*
         * TRI-28062016 Add reply to comment
         * productId: ID of Product
         * replyContent: Reply contains from user
		 * commentId: ID of comment
		 * fullName: Full name of logged user
		 * email: Email of logged user (default is empty string)
         * cms_db: DataModel Controller
         * userId: ID of logged user
         */
        public async Task<ObjContentComment> ReplyComments(long productId, string replyContent, int commentId, string fullName, string email, Ctrl cms_db, long userId)
        {
            if (fullName.Length > 250)
            {
                fullName = fullName.Substring(0, 249); //To make it fix to column data type length
            }

            if (email.Length > 250)
            {
                email = email.Substring(0, 249); //To make it fix to column data type length
            }

            Product product = cms_db.GetObjProductById(productId);
            Classification classification = cms_db.GetObjClasscifiById((int)Extension.EnumCore.StateType.cho_duyet);
            if (product != null)
            {
                ContentComment comment = new ContentComment();
                comment.ParentCommentId = commentId;
                comment.ContentObjId = productId;
                comment.ContentObjName = product.ProductName;
                comment.Title = null;
                comment.CommentText = StripHTMLAndScript(replyContent); //Keep comment clearn without html injection
                comment.FullName = fullName;
                comment.EmailAddress = email;
                comment.CrtdDT = DateTime.Now;
                comment.AprvdDT = null;
                comment.AprvdUID = null;
                comment.AprvdUerName = null;
                comment.StateId = classification.ClassificationId;
                comment.StateName = classification.ClassificationNM;
                comment.CrtdGuestUserId = null;
                comment.LikeCount = null;
                comment.LastLikedDT = null;
                comment.IPAddress = GetIPAddress();
                comment.ObjTypeId = (int)Extension.EnumCore.ObjTypeId.san_pham;
                comment.ObjTypeName = "san_pham";
                try
                {
                    await cms_db.AddComment(comment);
                    ObjContentComment objContentComment = GetCommentById(comment.CommentId, cms_db, userId);
                    return objContentComment;
                }
                catch (Exception ex)
                {
                    Core core = new Core();
                    core.AddToExceptionLog("ReplyComments", "ProductController", "Save reply on comment error: " + ex.Message, userId);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region "Custom Model"
        //Custom class to contain comment objects
        public partial class ObjContentComment
        {
            public long CommentId { get; set; }
            public Nullable<long> ParentCommentId { get; set; }
            public Nullable<long> ContentObjId { get; set; }
            public string ContentObjName { get; set; }
            public string Title { get; set; }
            public string CommentText { get; set; }
            public string FullName { get; set; }
            public string EmailAddress { get; set; }
            public Nullable<System.DateTime> CrtdDT { get; set; }
            public Nullable<System.DateTime> AprvdDT { get; set; }
            public Nullable<long> AprvdUID { get; set; }
            public string AprvdUerName { get; set; }
            public Nullable<long> StateId { get; set; }
            public string StateName { get; set; }
            public Nullable<System.Guid> CrtdGuestUserId { get; set; }
            public Nullable<long> LikeCount { get; set; }
            public Nullable<System.DateTime> LastLikedDT { get; set; }
            public string IPAddress { get; set; }
            public Nullable<int> ObjTypeId { get; set; }
            public string ObjTypeName { get; set; }
            public string UserAvatar { get; set; }
            public Nullable<Int64> UserId { get; set; }
        }
        #endregion

        #region "Utils"
        /*
         * Getting current IP Address of client
         */
        public string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            return context.Request.UserHostAddress;
        }

        /*
         * Strip all html from string
         */
        public string StripHTMLAndScript(string input)
        {
            System.Text.RegularExpressions.Regex rRemScript = new System.Text.RegularExpressions.Regex(@"<script[^>]*>[\s\S]*?</script>");
            string output = rRemScript.Replace(input, "");
            return System.Text.RegularExpressions.Regex.Replace(output, "<.*?>", String.Empty);
        }
        #endregion
        #endregion
    }
}
