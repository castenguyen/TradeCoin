using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMSPROJECT.Areas.AdminCMS
{
    public class AdminCMSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AdminCMS";
            }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "AdminCMS_default",
                "AdminCMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 new { controller = "Branch|AdminHome|User" },
                 new[] { "CMSPROJECT.Areas.AdminCMS.Controllers" }
            );

            #region promotion

            context.MapRoute(
              "AdminCMS_Promotion_Index",
              "administrator/Promotion/Index",
              new { controller = "Promotion", action = "Index" }
              );

            context.MapRoute(
                "AdminCMS_Promotion_Create",
                "administrator/Promotion/Create",
                new { controller = "Promotion", action = "Create" }
                );

            context.MapRoute(
             "AdminCMS_Promotion_Delete",
             "administrator/Promotion/Delete",
             new { controller = "Promotion", action = "Delete" }
             );

            context.MapRoute(
            "AdminCMS_Promotion_GetSelectListProductByCate",
            "administrator/Promotion/GetSelectListProductByCate",
            new { controller = "Promotion", action = "GetSelectListProductByCate" }
            );


            #endregion

            #region Role
            context.MapRoute(
             "AdminCMS_IndexRole",
             "administrator/RoleManager/Index",
             new { controller = "RoleManager", action = "Index" }
          );


            context.MapRoute(
               "AdminCMS_AddNewRole",
               "administrator/RoleManager/AddRole",
               new { controller = "RoleManager", action = "AddNewRole" }
            );


            context.MapRoute(
             "AdminCMS_EditRole",
             "administrator/RoleManager/EditRole",
             new { controller = "RoleManager", action = "EditRole" }
          );

            context.MapRoute(
            "AdminCMS_CreateRoleGroup",
            "administrator/RoleManager/CreateRoleGroup",
            new { controller = "RoleManager", action = "CreateRoleGroup" }
         );



            #endregion

            #region AccountAdmin

            context.MapRoute(
              "AdminCMS_login1",
              "administrator",
              new { controller = "AccountAdmin", action = "Login" }
          );
            context.MapRoute(
               "AdminCMS_login",
               "administrator/Login",
               new { controller = "AccountAdmin", action = "Login" }
           );

            context.MapRoute(
              "AdminCMS_LoginWithToken",
              "administrator/LoginWithToken",
              new { controller = "AccountAdmin", action = "LoginWithToken" }
          );

            context.MapRoute(
            "AdminCMS_LoginWithTokenAlert",
            "administrator/LoginWithTokenAlert",
            new { controller = "AccountAdmin", action = "LoginWithTokenAlert" }
        );





            context.MapRoute(
               "AdminCMS_LogOff",
               "administrator/LogOff",
               new { controller = "AccountAdmin", action = "LogOff" }
           );

            context.MapRoute(
              "AdminCMS_Register",
              "administrator/Register",
              new { controller = "AccountAdmin", action = "Register" }
          );

            context.MapRoute(
             "AdminCMS_ResetPassword",
             "administrator/ResetPassword",
             new { controller = "AccountAdmin", action = "ResetPassword" }
         );



            context.MapRoute(
                "AdminCMS_ListUser",
                "administrator/Account/ListUser",
                new { controller = "AccountAdmin", action = "ListUser" }
                 );

            context.MapRoute(
              "AdminCMS_DetailUser",
              "administrator/Account/DetailUser",
              new { controller = "AccountAdmin", action = "DetailUser" }
               );

            context.MapRoute(
             "AdminCMS_ManagerUser",
             "administrator/Account/ManagerUser",
             new { controller = "AccountAdmin", action = "ManagerUser" }
              );

            context.MapRoute(
            "AdminCMS_UpgradePackage",
            "administrator/Account/UpgradePackage",
            new { controller = "AccountAdmin", action = "UpgradePackage" }
        );




            context.MapRoute(
             "AdminCMS_AddUserRole",
             "administrator/Account/AddUserRole",
             new { controller = "AccountAdmin", action = "AddUserRole" }
         );


            context.MapRoute(
        "AdminCMS_RemoveUserRole",
        "administrator/Account/RemoveUserRole",
        new { controller = "AccountAdmin", action = "RemoveUserRole" }
         );



            context.MapRoute(
      "AdminCMS_ChangePassword",
      "administrator/Account/ChangePassword",
      new { controller = "AccountAdmin", action = "ChangePassword" }
       );

            context.MapRoute(
     "AdminCMS_ConfirmEmail",
     "administrator/Account/ConfirmEmail",
     new { controller = "AccountAdmin", action = "ConfirmEmail" }
      );

            context.MapRoute(
      "AdminCMS_ChangeState",
      "administrator/Account/ChangeState",
      new { controller = "AccountAdmin", action = "ChangeState" }
       );
            context.MapRoute(
   "AdminCMS_ForgotPasswordConfirmation",
   "administrator/Account/ForgotPasswordConfirmation",
   new { controller = "AccountAdmin", action = "ForgotPasswordConfirmation" }
    );

            context.MapRoute(
      "AdminCMS_ForgotPassword",
      "administrator/Account/ForgotPassword",
      new { controller = "AccountAdmin", action = "ForgotPassword" }
       );
            context.MapRoute(
   "AdminCMS_ResetPasswordConfirmation",
   "administrator/Account/ResetPasswordConfirmation",
   new { controller = "AccountAdmin", action = "ResetPasswordConfirmation" }
    );
            context.MapRoute(
  "AdminCMS_Profile",
  "administrator/Account/Profile",
  new { controller = "AccountAdmin", action = "Profile" }
   );




            #endregion  END AccountAdmin

            #region CONTENITEM


            context.MapRoute(
          "AdminCMS_ContentItem_IndexPageInfor",
          "administrator/ManagePageInfor",
          new { controller = "ContentItem", action = "PageInforIndex" }
          );


            context.MapRoute(
             "AdminCMS_ContentItem_CreatePageInfo",
             "administrator/CreatePageInfo",
             new { controller = "ContentItem", action = "CreatePageInfor" }
             );
            context.MapRoute(
             "AdminCMS_ContentItem_EditPageInfo",
             "administrator/EditPageInfo/{id}",
             new { controller = "ContentItem", action = "EditPageInfor", id = UrlParameter.Optional }
               );



            context.MapRoute(
              "AdminCMS_ContentItem_Index",
              "administrator/ManageContentItem",
              new { controller = "ContentItem", action = "Index" }
              );


            context.MapRoute(
             "AdminCMS_ContentItem_Create",
             "administrator/CreateContentItem",
             new { controller = "ContentItem", action = "Create" }
             );
            context.MapRoute(
             "AdminCMS_ContentItem_Edit",
             "administrator/EditContentItem/{id}",
             new { controller = "ContentItem", action = "Edit", id = UrlParameter.Optional }
               );

            context.MapRoute(
           "AdminCMS_ContentItem_ChangeState",
           "administrator/ChangeStateContentItem/{id}",
           new { controller = "ContentItem", action = "ChangeState", id = UrlParameter.Optional }
            );

            context.MapRoute(
          "AdminCMS_ContentItem_Delete",
          "administrator/DeleteContentItem/{id}",
          new { controller = "ContentItem", action = "Delete", id = UrlParameter.Optional }
            );

            context.MapRoute(
            "AdminCMS_ContentItem_GetRelatedTag",
            "administrator/GetRelatedTag",
            new { controller = "ContentItem", action = "GetRelatedTag" }
               );

            context.MapRoute(
          "AdminCMS_ContentItem_GetRelatedContent",
          "administrator/RelatedContent",
          new { controller = "ContentItem", action = "RelatedContent" }
             );






            context.MapRoute(
          "AdminCMS_ContentItem_RelatedTag",
          "administrator/RelatedTagContentItem",
          new { controller = "ContentItem", action = "_RelatedTag" }
             );

            context.MapRoute(
      "AdminCMS_ContentItem_RelatedContent",
      "administrator/RelatedContentContentItem",
      new { controller = "ContentItem", action = "_RelatedContent" }
         );

            context.MapRoute(
      "AdminCMS_ContentItem_RelatedTagE",
      "administrator/RelatedTagEContentItem/{id}",
      new { controller = "ContentItem", action = "_RelatedTagE", id = UrlParameter.Optional }
         );


            context.MapRoute(
           "AdminCMS_ContentItem_RelatedContentE",
           "administrator/RelatedContentEContentItem/{id}",
           new { controller = "ContentItem", action = "_RelatedContentE", id = UrlParameter.Optional }
             );




            #endregion END CONTENITEM
            #region COMMENT

            context.MapRoute(
             "AdminCMS_CommentManager_Index",
             "administrator/CommentManager",
             new { controller = "CommentManager", action = "Index" }
             );


            context.MapRoute(
                "AdminCMS_CommentManager_ChangeState",
                "administrator/ChangeStateCommentManager/{id}",
                new { controller = "CommentManager", action = "ChangeState", id = UrlParameter.Optional }
                );

            #endregion ENDCOMMENT

            #region Product
            context.MapRoute(
             "AdminCMS_Product_Index",
             "administrator/ProductManager",
             new { controller = "ProductManager", action = "Index" }
             );
            context.MapRoute(
             "AdminCMS_Product_Create",
             "administrator/CreateProduct",
             new { controller = "ProductManager", action = "Create" }
             );
            context.MapRoute(
             "AdminCMS_Product_Edit",
             "administrator/EditProduct/{id}",
             new { controller = "ProductManager", action = "Edit", id = UrlParameter.Optional }
               );

            context.MapRoute(
                "AdminCMS_Product_ChangeState",
                "administrator/ChangeStateProduct/{id}",
                new { controller = "ProductManager", action = "ChangeState", id = UrlParameter.Optional }
                  );

            context.MapRoute(
               "AdminCMS_Product_Delete",
               "administrator/DeleteProduct/{id}",
               new { controller = "ProductManager", action = "Delete", id = UrlParameter.Optional }
                 );

            context.MapRoute(
                 "AdminCMS_Product_EditImage",
                 "administrator/EditImageProduct/{id}",
                 new { controller = "ProductManager", action = "EditImage", id = UrlParameter.Optional }
                   );

            context.MapRoute(
                 "AdminCMS_Product_AddMoreImage",
                 "administrator/AddMoreImageProduct/{id}",
                 new { controller = "ProductManager", action = "AddMoreImage", id = UrlParameter.Optional }
                   );

            context.MapRoute(
                  "AdminCMS_Product_DeleteImage",
                  "administrator/DeleteImageProduct/{id}",
                  new { controller = "ProductManager", action = "DeleteImage", id = UrlParameter.Optional }
                    );

            context.MapRoute(
                 "AdminCMS_Product_RelatedTag",
                 "administrator/RelatedTagProduct",
                 new { controller = "ProductManager", action = "_RelatedTag" }
                 );
            context.MapRoute(
                 "AdminCMS_Product_UploadImagePartial",
                 "administrator/UploadImagePartialProduct",
                 new { controller = "ProductManager", action = "_UploadImagePartial" }
                 );
            context.MapRoute(
                "AdminCMS_Product_UploadImagePartialE",
                "administrator/UploadImagePartialEProduct/{id}",
                new { controller = "ProductManager", action = "_UploadImagePartialE", id = UrlParameter.Optional }
                  );

            context.MapRoute(
               "AdminCMS_Product_RelatedTagE",
               "administrator/RelatedTagEProduct/{id}",
               new { controller = "ProductManager", action = "_RelatedTagE", id = UrlParameter.Optional }
                 );

            context.MapRoute(
              "AdminCMS_ProductManager_GetLstClassifiBySchemeId",
              "administrator/GetLstcatelogryProductByparent",
              new { controller = "ProductManager", action = "GetLstcatelogryProductByparent" }
          );

            context.MapRoute(
            "AdminCMS_ProductManager_GetSelectListBySchemeId",
            "administrator/GetSelectListBySchemeId",
            new { controller = "ProductManager", action = "GetSelectListBySchemeId" }
        );


            #endregion product

            #region classifischeme


            context.MapRoute(
             "AdminCMS_Classifischeme_index",
             "administrator/ManagerScheme",
             new { controller = "ClassificationScheme", action = "Index" }
         );


            context.MapRoute(
             "AdminCMS_Classifischeme_Create",
             "administrator/CreateScheme",
             new { controller = "ClassificationScheme", action = "Create" }
         );

            context.MapRoute(
               "AdminCMS_Classifischeme_Edit",
               "administrator/EditScheme/{id}",
               new { controller = "ClassificationScheme", action = "Edit", id = UrlParameter.Optional }
           );
            #endregion classifischeme

            #region Classicification


            context.MapRoute(
        "AdminCMS_Classicification_InputClass",
        "administrator/ManagerClassifi/InputClass",
        new { controller = "Classification", action = "InputClass" }
    );

            context.MapRoute(
              "AdminCMS_Classicification_index",
              "administrator/ManagerClassifi",
              new { controller = "Classification", action = "Index" }
          );
            context.MapRoute(
             "AdminCMS_Classicification_Create",
             "administrator/CreateClassifi",
             new { controller = "Classification", action = "Create" }
         );

            context.MapRoute(
               "AdminCMS_Classicification_Edit",
               "administrator/EditSClassifi/{id}",
               new { controller = "Classification", action = "Edit", id = UrlParameter.Optional }
           );

            context.MapRoute(
              "AdminCMS_Classicification_OrderbyAs",
              "administrator/OrderbyAsSClassifi/{id}",
              new { controller = "Classification", action = "OrderbyAs", id = UrlParameter.Optional }
          );

            context.MapRoute(
                "AdminCMS_Classicification_OrderbyDe",
                "administrator/OrderbyDeSClassifi/{id}",
                new { controller = "Classification", action = "OrderbyDe", id = UrlParameter.Optional }
            );

            context.MapRoute(
               "AdminCMS_Classicification_Delete",
               "administrator/DeleteSClassifi/{id}",
               new { controller = "Classification", action = "Delete", id = UrlParameter.Optional }
           );




            context.MapRoute(
                "AdminCMS_Classicification_UploadImagePartial",
                "administrator/UploadImagePartialSClassifi",
                new { controller = "Classification", action = "_UploadImagePartial" }
            );
            context.MapRoute(
               "AdminCMS_Classicification_UploadImagePartialE",
               "administrator/UploadImagePartialEClassifi/{id}",
               new { controller = "Classification", action = "_UploadImagePartialE", id = UrlParameter.Optional }
           );

            context.MapRoute(
               "AdminCMS_Classicification_GetLstClassifiBySchemeId",
               "administrator/GetLstClassifiBySchemeIdClassifi/{id}",
               new { controller = "Classification", action = "GetLstClassifiBySchemeId", id = UrlParameter.Optional }
           );

            #endregion Classicification

            #region config

            context.MapRoute(
            "AdminCMS_config_Index",
            "administrator/Config",
            new { controller = "Config", action = "Index" }
            );

            context.MapRoute(
            "AdminCMS_config_Edit",
            "administrator/EditConfig",
            new { controller = "Config", action = "Edit" }
            );

            #endregion Endconfig

            #region ContentTag

            context.MapRoute(
          "AdminCMS_ContentTag_Index",
          "administrator/ContentTag",
          new { controller = "ContentTag", action = "Index" }
          );

            context.MapRoute(
        "AdminCMS_ContentTag_CreateTag",
        "administrator/CreateTagContentTag",
        new { controller = "ContentTag", action = "CreateTag" }
        );

            #endregion EndcontenTag

            #region DisplayManager

            context.MapRoute(
              "AdminCMS_DisplayManager_Index",
              "administrator/DisplayManager",
              new { controller = "DisplayManager", action = "Create" }
              );

            context.MapRoute(
            "AdminCMS_DisplayManager_Delete",
            "administrator/DeleteDisplayManager/{id}",
            new { controller = "DisplayManager", action = "Delete", id = UrlParameter.Optional }
            );

            #endregion END DisplayManager

            #region MediaContent

            context.MapRoute(
             "AdminCMS_MediaManager_Index",
             "administrator/MediaManager",
             new { controller = "MediaManager", action = "Index" }
             );

            context.MapRoute(
            "AdminCMS_MediaManager_CreateAlbum",
            "administrator/CreateAlbumMedia",
            new { controller = "MediaManager", action = "CreateAlbum" }
            );

            context.MapRoute(
           "AdminCMS_MediaManager_DetailAllbum",
           "administrator/DetailAllbumMedia/{id}",
           new { controller = "MediaManager", action = "DetailAllbum", id = UrlParameter.Optional }
           );

            context.MapRoute(
         "AdminCMS_MediaManager_DeleteAlBum",
         "administrator/DeleteAlBumMedia/{id}",
         new { controller = "MediaManager", action = "DeleteAlBum", id = UrlParameter.Optional }
         );

            context.MapRoute(
        "AdminCMS_MediaManager_DeleteImage",
        "administrator/DeleteImageMedia/{id}",
        new { controller = "MediaManager", action = "DeleteImage", id = UrlParameter.Optional }
        );

            context.MapRoute(
          "AdminCMS_MediaManager_Edit",
          "administrator/EditMedia/{id}",
          new { controller = "MediaManager", action = "Edit", id = UrlParameter.Optional }
          );

            context.MapRoute(
          "AdminCMS_MediaManager_BannerManager",
          "administrator/BannerManagerMedia",
          new { controller = "MediaManager", action = "BannerManager" }
          );

            context.MapRoute(
             "AdminCMS_MediaManager_CreateBanner",
             "administrator/CreateBannerMedia",
             new { controller = "MediaManager", action = "CreateBanner" }
             );

            context.MapRoute(
              "AdminCMS_MediaManager_EditBanner",
              "administrator/EditBannerMedia/{id}",
              new { controller = "MediaManager", action = "EditBanner", id = UrlParameter.Optional }
              );

            context.MapRoute(
              "AdminCMS_MediaManager_DeleteBanner",
              "administrator/DeleteBannerMedia/{id}",
              new { controller = "MediaManager", action = "DeleteBanner", id = UrlParameter.Optional }
              );

            context.MapRoute(
               "AdminCMS_MediaManager_UploadImagePartial",
               "administrator/UploadImagePartialMedia",
               new { controller = "MediaManager", action = "_UploadImagePartial" }
               );

            context.MapRoute(
              "AdminCMS_MediaManager_UploadImagePartialE",
              "administrator/UploadImagePartialEMedia/{id}",
              new { controller = "MediaManager", action = "_UploadImagePartialE", id = UrlParameter.Optional }
              );



            context.MapRoute(
              "AdminCMS_MediaManager_VideoManager",
              "administrator/VideoManager",
              new { controller = "MediaManager", action = "VideoManager" }
              );

            context.MapRoute(
             "AdminCMS_MediaManager_CreateVideo",
             "administrator/CreateVideoMedia",
             new { controller = "MediaManager", action = "CreateVideo" }
             );

            context.MapRoute(
              "AdminCMS_MediaManager_EditVideo",
              "administrator/EditVideoMedia/{id}",
              new { controller = "MediaManager", action = "EditVideo", id = UrlParameter.Optional }
              );

            context.MapRoute(
              "AdminCMS_MediaManager_DeleteVideo",
              "administrator/DeleteVideoMedia/{id}",
              new { controller = "MediaManager", action = "DeleteVideo", id = UrlParameter.Optional }
              );

            #endregion End MediaContent

            #region Extention

            context.MapRoute(
             "AdminCMS_Dashboard_Index",
             "administrator/Dashboard",
             new { controller = "Dashboard", action = "Index" }
             );

            context.MapRoute(
           "AdminCMS_GetFriendlyurlFromTitle",
           "administrator/GetFriendlyurlFromTitle",
           new { controller = "Extension", action = "GetFriendlyurlFromTitle" }
              );

            context.MapRoute(
         "AdminCMS_CK_ImgUpload",
         "administrator/CK_ImgUpload",
         new { controller = "Extension", action = "CK_ImgUpload" }
        );


            context.MapRoute(
             "AdminCMS_UploadOneImgReturnUrl",
             "administrator/UploadOneImgReturnUrl",
             new { controller = "Extension", action = "UploadOneImgReturnUrl" }
               );

            context.MapRoute(
            "AdminCMS_Extension_ErrorExeption",
            "administrator/ErrorExeption",
            new { controller = "Extension", action = "ErrorExeption" }
              );

            context.MapRoute(
            "AdminCMS_Extension_AlertPage",
            "administrator/AlertPage",
            new { controller = "Extension", action = "AlertPage" }
              );

            context.MapRoute(
         "AdminCMS_DeleteDetailImage",
         "administrator/DeleteDetailImage",
         new { controller = "Extension", action = "DeleteDetailImage" }
            );


            #endregion

            #region HistoryManager
            context.MapRoute(
             "AdminCMS_HistoryManager_Index",
             "administrator/HistoryManager",
             new { controller = "HistoryManager", action = "Index" }
             );

            context.MapRoute(
             "AdminCMS_HistoryManager_Delete",
             "administrator/HistoryManager/Delete",
             new { controller = "HistoryManager", action = "Delete" }
             );

            #endregion

            #region ErrorManager
            context.MapRoute(
             "AdminCMS_ErrorManager_Index",
             "administrator/ErrorManager",
             new { controller = "ErrorManager", action = "Index" }
             );
            #endregion

            #region dashboad
            context.MapRoute(
           "AdminCMS_Dashboard_MainSliderPartial",
           "administrator/Dashboard/AdminCMS_Dashboard_MainSliderPartial",
           new { controller = "Dashboard", action = "MainSliderPartial" }
           );


            context.MapRoute(
         "AdminCMS_Dashboard_ModIndex",
         "administrator/Dashboard/ModIndex",
         new { controller = "Dashboard", action = "ModIndex" }
         );

            context.MapRoute(
        "AdminCMS_Dashboard_MainHeaderPartial",
        "administrator/Dashboard/MainHeaderPartial",
        new { controller = "Dashboard", action = "MainHeaderPartial" }
        );

            

            #endregion dashboad


            #region  DEMO


            context.MapRoute(
        "AdminCMS_Demo_DemoMainSliderPartial",
        "Demo/DemoMainSlider",
        new { controller = "Demo", action = "DemoMainSlider" }
        );

            context.MapRoute(
        "AdminCMS_Demo_DemoMainHeaderPartial",
        "Demo/DemoMainHeader",
        new { controller = "Demo", action = "DemoMainHeader" }
        );

            context.MapRoute(
               "AdminCMS_Demo_index",
               "Demo/index",
               new { controller = "Demo", action = "index" }
               );

            context.MapRoute(
                "AdminCMS_Demo_MailIndex",
                "Demo/Mail",
                new { controller = "Demo", action = "MailIndex" }
                );

            context.MapRoute(
                 "AdminCMS_Demo_ReplyMail",
                 "Demo/ReplyMail",
                 new { controller = "Demo", action = "ReplyMail" }
                 );

            context.MapRoute(
                "AdminCMS_Demo_Register",
                "Demo/Register",
                new { controller = "Demo", action = "Register" }
                );


            context.MapRoute(
                "AdminCMS_Demo_Login",
                "Demo/Login",
                new { controller = "Demo", action = "Login" }
                );


            context.MapRoute(
              "AdminCMS_Demo_IndexNews",
              "Demo/IndexNews",
              new { controller = "Demo", action = "IndexNews" }
              );

            context.MapRoute(
            "AdminCMS_Demo_CreateNews",
            "Demo/CreateNews",
            new { controller = "Demo", action = "CreateNews" }
            );

            context.MapRoute(
          "AdminCMS_Demo_UpdateNews",
          "Demo/UpdateNews",
          new { controller = "Demo", action = "UpdateNews" }
          );


            context.MapRoute(
            "AdminCMS_Demo_IndexTicker",
            "Demo/IndexTicker",
            new { controller = "Demo", action = "IndexTicker" }
            );

            context.MapRoute(
            "AdminCMS_Demo_CreateTicker",
            "Demo/CreateTicker",
            new { controller = "Demo", action = "CreateTicker" }
            );

            context.MapRoute(
          "AdminCMS_Demo_UpdateTicker",
          "Demo/UpdateTicker",
          new { controller = "Demo", action = "UpdateTicker" }
          );

            context.MapRoute(
     "AdminCMS_Demo_DetailUser",
     "Demo/DetailUser",
     new { controller = "Demo", action = "DetailUser" }
     );

            context.MapRoute(
   "AdminCMS_Demo_UserManager",
   "Demo/UserManager",
   new { controller = "Demo", action = "UserManager" }
   );


            context.MapRoute(
   "AdminCMS_Demo_CreatePackage",
   "Demo/CreatePackage",
   new { controller = "Demo", action = "CreatePackage" }
   );

            context.MapRoute(
                "AdminCMS_Demo_CreateCatalogry",
                "Demo/CreateCatalogry",
                new { controller = "Demo", action = "CreateCatalogry" }
                );


            context.MapRoute(
                "AdminCMS_Demo_IndexCatalogry",
                "Demo/IndexCatalogry",
                new { controller = "Demo", action = "IndexCatalogry" }
                );

            context.MapRoute(
            "AdminCMS_Demo_UpdateInfoAcc",
            "Demo/UpdateInfoAcc",
            new { controller = "Demo", action = "UpdateInfoAcc" }
            );


            context.MapRoute(
           "AdminCMS_Demo_Upgrade",
           "Demo/Upgrade",
           new { controller = "Demo", action = "Upgrade" }
           );


            context.MapRoute(
           "AdminCMS_Demo_ModIndex",
           "Demo/ModIndex",
           new { controller = "Demo", action = "ModIndex" }
           );

            context.MapRoute(
       "AdminCMS_Demo_IndexAdmin",
       "Demo/IndexAdmin",
       new { controller = "Demo", action = "IndexAdmin" }
       );

            context.MapRoute(
     "AdminCMS_Demo_ListTicker",
     "Demo/ListTicker",
     new { controller = "Demo", action = "ListTicker" }
     );

            context.MapRoute(
  "AdminCMS_Demo_ListNews",
  "Demo/ListNews",
  new { controller = "Demo", action = "ListNews" }
  );


            context.MapRoute(
  "AdminCMS_Demo_DetailNews",
  "Demo/DetailNews",
  new { controller = "Demo", action = "DetailNews" }
  );

            context.MapRoute(
            "AdminCMS_Demo_DetailTicker",
            "Demo/DetailTicker",
            new { controller = "Demo", action = "DetailTicker" }
            );

            context.MapRoute(
                "AdminCMS_Demo_ChatGroup",
                "Demo/ChatGroup",
                new { controller = "Demo", action = "ChatGroup" }
                );



            #endregion Demo

            #region Package



            context.MapRoute(
               "AdminCMS_PackageManager_Create",
               "administrator/packagemanager/create",
               new { controller = "PackageManager", action = "Create" }
               );

            context.MapRoute(
             "AdminCMS_PackageManager_FrontEndIndex",
             "administrator/packagemanager/FrontEndIndex",
             new { controller = "PackageManager", action = "FrontEndIndex" }
             );

            context.MapRoute(
           "AdminCMS_PackageManager_ConfirmUpdate",
           "administrator/packagemanager/ConfirmUpdate",
           new { controller = "PackageManager", action = "ConfirmUpdate" }
           );





            context.MapRoute(
              "AdminCMS_PackageManager_index",
              "administrator/packagemanager/index",
              new { controller = "PackageManager", action = "index" }
              );

            context.MapRoute(
            "AdminCMS_PackageManager_Update",
            "administrator/PackageManager/Update/{id}",
            new { controller = "PackageManager", action = "Update", id = UrlParameter.Optional }
              );


            #endregion

            #region Ticker

            context.MapRoute(
              "AdminCMS_tickermanager_Index",
              "administrator/tickermanager",
              new { controller = "TickerManager", action = "Index" }
              );
            context.MapRoute(
             "AdminCMS_tickermanager_Create",
             "administrator/createticker",
             new { controller = "TickerManager", action = "Create" }
             );
            context.MapRoute(
             "AdminCMS_tickermanager_Update",
             "administrator/UpdateTicker/{id}",
             new { controller = "TickerManager", action = "Update", id = UrlParameter.Optional }
               );

            context.MapRoute(
                "AdminCMS_Ticker_ChangeState",
                "administrator/ChangeStateTicker/{id}",
                new { controller = "TickerManager", action = "ChangeState", id = UrlParameter.Optional }
                  );

            context.MapRoute(
               "AdminCMS_Ticker_Delete",
               "administrator/DeleteTicker/{id}",
               new { controller = "TickerManager", action = "Delete", id = UrlParameter.Optional }
                 );

            #endregion

            #region EmailManager
            context.MapRoute(
              "AdminCMS_EmailIndex",
              "hop-thu",
              new { controller = "EmailManager", action = "Index" }
           );



            context.MapRoute(
              "AdminCMS_CreateNewEmail",
              "tao-moi-thu",
              new { controller = "EmailManager", action = "CreateNewEmail" }
           );

            context.MapRoute(
            "AdminCMS_DetailEmail",
            "chi-tiet-thu",
            new { controller = "EmailManager", action = "DetailEmail" }
         );

            context.MapRoute(
           "ChatGroup_Index",
           "chat-nhom",
           new { controller = "ChatGroup", action = "Index" }
           );

            context.MapRoute(
         "ChatGroup_Notification",
         "NewNotifi",
         new { controller = "Notification", action = "NewNotifi" }
         );


            context.MapRoute(
         "ChatGroup_SendNewNoifi",
         "SendNewNoifi",
         new { controller = "Notification", action = "SendNewNoifi" }
         );



            #endregion


            #region member

            context.MapRoute(
            "AdminCMS_member_ListTicker",
            "tat-ca-keo",
            new { controller = "Member", action = "ListTicker" }
            );

            context.MapRoute(
           "AdminCMS_member_DetailTicker",
           "chi-tiet-keo",
           new { controller = "Member", action = "DetailTicker" }
           );


            context.MapRoute(
           "AdminCMS_member_ListNews",
           "tat-ca-bai-viet",
           new { controller = "Member", action = "ListNews" }
           );

            context.MapRoute(
        "AdminCMS_member_DetailNews",
        "chi-tiet-bai-viet",
        new { controller = "Member", action = "DetailNews" }
        );

            context.MapRoute(
      "AdminCMS_member_MemberDashBoard",
      "trang-chinh",
      new { controller = "Member", action = "MemberDashBoard" }
      );

            

            #endregion


            #region ShopCartManager
            context.MapRoute(
               "ShopCartManager_Index",
               "administrator/ShopCartManager",
               new { controller = "ShopCartManager", action = "Index" }
               );

            context.MapRoute(
                "ShopCartManager_Detail",
                "administrator/ShopCartManager/OrderDetail",
                new { controller = "ShopCartManager", action = "OrderDetail" }
                );



            context.MapRoute(
              "ShopCartManager_delete2",
              "administrator/ShopCartManager/DeleteOrder",
              new { controller = "ShopCartManager", action = "DeleteOrder" }
              );

         
            #endregion
        }
    }
}