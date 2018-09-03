using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using DataModel.DataViewModel;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using DataModel.DataEntity;
using DataModel.DataStore;
using DataModel.Extension;

namespace CMSPROJECT.Hubs
{
    public class ChatHub : Hub
    {
        private Ctrl cms_db = new Ctrl();
        static List<UserInfo> UsersList = new List<UserInfo>();
        static List<MessageInfo> MessageList = new List<MessageInfo>();
        static List<ChatMessInfo> ChatList = new List<ChatMessInfo>();

        public void Send(string name, string message)
        {
            DateTime date = DateTime.Now;

            ChatMessInfo tmpobj = new ChatMessInfo { UserName = name, Message = message, MsgTime = date, MsgDate = date.ToString("MM/dd/yyyy HH:mm")};
            ChatList.Add(tmpobj);

            Clients.All.addNewMessageToPage(name, message, date.ToString("MM/dd/yyyy HH:mm"));
        }

        public void Getlistchat()
        {
            Clients.All.clearScreen();
            foreach (ChatMessInfo item in ChatList)
            {
                Clients.All.addNewMessageToPage(item.UserName, item.Message, item.MsgTime.ToString("MM/dd/yyyy HH:mm"));
            }
        }

        public void ClearListChat()
        {
            ChatList.Clear();
            Clients.All.clearScreen();
        }

            



        /// <summary>
        /// hàm này dc gọi khi trang view chat dc render ra
        /// hàm này xác định user đang chat(thong tin user dc lấy từ user nhập vào từ trang ColectInforChat===>xem thêm từ đó nha)
        /// gán user vào 1 nhóm nếu đã có nhóm và tạo nhóm mới nếu chưa có nhóm
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="UserEmail"></param>
        public void Connect(string UserName, string UserEmail)
        {
            string id = Context.ConnectionId;
            string userGroup = "RANH";
            UserInfo userInfo = cms_db.CreateUserInfo(UserName, UserEmail, id);
            try
            {
                if (userInfo.tpflag == (int)EnumCore.FlagForChatHub.user_guest)
                {
                    //Đây là user bình thường đang tìm 1 admin rảnh,Tìm trong danh sách user hiện tại có
                    //userd=admin nào rảnh ko
                    //Nếu là phiẹn đăng nhập đầu tiên thì UsersList ko có user nào
                    UserInfo strg = this.FindUserInfor(UsersList, null, 
                        (int)EnumCore.FlagForChatHub.user_admin, (int)EnumCore.FlagForChatHub.admin_free, null, null);


                    //Nếu không có user rảnh lúc này sẽ lỗi và nhảy vào catch
                    UsersList.Where(s => s.ConnectionId == userInfo.ConnectionId).First().UserGroup = strg.UserGroup;




                    //nếu có useradmin rảnh thì set useradmin đó bận
                    strg.freeflag = (int)EnumCore.FlagForChatHub.admin_busy;
                    //add user khách vào listuser
                    UsersList.Add(userInfo);
                    //ADD user khách vào trong group
                    Groups.Add(Context.ConnectionId, userGroup);
                    Clients.Caller.onConnected(id, UserName, userInfo.UserID, userGroup);
                }
                else
                {//nếu user vừa gọi là user admin thi gán vào userlist và gán thong báo rảnh

                    UsersList.Add(userInfo);
                    // UserInfo waitguestuser = (from s in UsersList where (s.waitflag == 1) && (s.tpflag == 0) select s).First();
                    UserInfo waitguestuser = this.FindUserInfor(UsersList, (int)EnumCore.FlagForChatHub.waitting,
                                                            (int)EnumCore.FlagForChatHub.user_guest, null, null, null);
                    if (waitguestuser != null)
                    {
                        Groups.Add(waitguestuser.ConnectionId, userInfo.UserID.ToString());
                        waitguestuser.UserGroup = userInfo.UserID.ToString();

                        Clients.Client(waitguestuser.ConnectionId).onConnected(id, waitguestuser.UserName, waitguestuser.UserID, waitguestuser.UserGroup.ToString());
                        UsersList.Where(s => s.ConnectionId == userInfo.ConnectionId).First().freeflag = (int)EnumCore.FlagForChatHub.admin_busy;
                        UsersList.Where(s => s.ConnectionId == waitguestuser.ConnectionId).First().waitflag = (int)EnumCore.FlagForChatHub.notwaitting;
                    }

                    Groups.Add(Context.ConnectionId, userInfo.UserID.ToString());
                    UsersList.Where(s => s.ConnectionId == userInfo.ConnectionId).First().UserGroup = userInfo.UserID.ToString();
                    Clients.Caller.onConnected(id, UserName, userInfo.UserID, userInfo.UserID.ToString());
                }
            }
            catch
            {

               //nếu user khách binh thường thì add vào trong nhóm khách rảnh
                Groups.Add(Context.ConnectionId, "RANH");

                // gán vào nhóm rnah luôn
                userInfo.UserGroup = "RANH";

                UsersList.Add(userInfo);
                string msg = "Hiện tại tất cả các nhân viên đang bận xin quý khách chờ trong giây lát";
                Clients.Caller.getMessages(msg);
            }
        }
   
        public void SendMessageToGroup(string userName, string message)
        {
            if (UsersList.Count != 0)
            {
                UserInfo ObjUserInfo = (from s in UsersList where (s.UserName == userName) select s).First();

                MessageInfo tmpobj = new MessageInfo { UserName = userName, Message = message, UserGroup = ObjUserInfo.UserGroup };


                MessageList.Add(tmpobj);

                string strgroup = ObjUserInfo.UserGroup;

                Clients.Group(strgroup).getMessages(userName, message);
            }
        }
      
        public override System.Threading.Tasks.Task OnDisconnected(bool val)
        {
            string id = Context.ConnectionId;
            UserInfo item = UsersList.FirstOrDefault(x => x.ConnectionId == id);
            if (item != null)
            {
                UsersList.Remove(item);
                if (item.tpflag == (int)EnumCore.FlagForChatHub.user_guest)
                {
                    //nếu user ngat kết noi là user khách
                    //chuyển trạng thái user admin thành free user và tìm nếu có user khách chờ thì kết nối lại.
                    try
                    {
                        //tim user admin vua bi mat ket noi
                        UserInfo useradmin = this.FindUserInfor(UsersList, null,
                                 (int)EnumCore.FlagForChatHub.user_admin, null, null, item.UserGroup);
                        //tim user khách đang chờ 
                        UserInfo userwaiting = this.FindUserInfor(UsersList, (int)EnumCore.FlagForChatHub.waitting,
                                 (int)EnumCore.FlagForChatHub.user_guest, null, null, null);

                        if (useradmin != null)
                        {
                            //chuyen trang thai cho useradmin
                            UsersList.Where(s => s.tpflag == (int)EnumCore.FlagForChatHub.user_admin &&
                                s.UserGroup == item.UserGroup).First().freeflag = (int)EnumCore.FlagForChatHub.admin_free;
                            string message = "User khách đã rời khỏi cuộc trò chuyện đang chờ khách hàng mới";
                            Clients.Client(useradmin.ConnectionId).ClearScreen(useradmin.UserName, message);
                        }
                        if (userwaiting != null)
                        {
                            UsersList.Where(s => s.tpflag == (int)EnumCore.FlagForChatHub.user_guest &&
                                 s.waitflag == (int)EnumCore.FlagForChatHub.waitting).First().UserGroup = useradmin.UserGroup;
                            Groups.Add(userwaiting.ConnectionId, useradmin.UserGroup);
                            string message = "Bắt đầu cuộc trò chuyện";
                            Clients.Client(useradmin.ConnectionId).ClearScreen(useradmin.UserName, message);
                            Clients.Client(userwaiting.ConnectionId).ClearScreen(userwaiting.UserName, message);
                        }
                    }
                    catch
                    {
                        UserInfo useradmin = UsersList.Where(s => s.tpflag == (int)EnumCore.FlagForChatHub.user_admin 
                                                                                    && s.UserGroup == item.UserGroup).First();
                        string message = "User khách đã rời khỏi cuộc trò chuyện đang chờ khách hàng mới";
                        Clients.Client(useradmin.ConnectionId).ClearScreen(useradmin.UserName, message);

                    }
                }
                if (item.tpflag == 1)//nếu user ngat kết noi là user admin
                {
                    try
                    {
                        UserInfo Usergues = this.FindUserInfor(UsersList, null,
                            (int)EnumCore.FlagForChatHub.user_guest, null, null, item.UserGroup);

                        UserInfo newuseradmin = this.FindUserInfor(UsersList, null,
                         (int)EnumCore.FlagForChatHub.user_admin, (int)EnumCore.FlagForChatHub.admin_free, null, null);

                        if (Usergues != null)
                        {
                            UsersList.Where(s => s.tpflag == (int)EnumCore.FlagForChatHub.user_guest &&
                               s.UserGroup == item.UserGroup).First().waitflag = (int)EnumCore.FlagForChatHub.waitting;

                            string msg = "Có lỗi xảy ra với đường truyền xin quý khách chờ trong giây lát";
                            Clients.Client(Usergues.ConnectionId).NoExistAdmin(msg);
                        
                        }

                        if (newuseradmin != null)
                        {
                            Groups.Add(newuseradmin.ConnectionId, newuseradmin.UserGroup);
                            Groups.Add(Usergues.ConnectionId, newuseradmin.UserGroup);

                            UsersList.Where(s => s.tpflag == (int)EnumCore.FlagForChatHub.user_guest
                                && s.UserGroup == item.UserGroup).First().UserGroup = newuseradmin.UserGroup;

                            UsersList.Where(s => s.tpflag == (int)EnumCore.FlagForChatHub.user_guest
                                && s.UserGroup == newuseradmin.UserGroup).First().waitflag = (int)EnumCore.FlagForChatHub.notwaitting;

                            string message = "Bắt đầu cuộc trò chuyện";
                            Clients.Client(newuseradmin.ConnectionId).ClearScreen(newuseradmin.UserName, message);
                            Clients.Client(Usergues.ConnectionId).ClearScreen(Usergues.UserName, message);
                        }
                    }
                    catch
                    {
                        UserInfo Usergues = UsersList.Where(s => s.tpflag ==
                            (int)EnumCore.FlagForChatHub.user_guest && s.UserGroup == item.UserGroup).First();

                        string message = "User admin đã rời khỏi cuộc trò chuyện đang chờ user admin mới";
                        Clients.Client(Usergues.ConnectionId).ClearScreen(Usergues.UserName, message);
                    }
                }
                //save conversation to dat abase
            }
            return base.OnDisconnected(val);
        }

        private UserInfo FindUserInfor(List<UserInfo> UsersList, int? Waitflag, int? Tpflag, int? Freeflag, string ConnectId, string UserGroup)
        {
            try
            {

                if (Waitflag.HasValue)
                {
                    UsersList = UsersList.Where(s => s.waitflag == Waitflag).ToList();
                }
                if (Tpflag.HasValue)
                {
                    UsersList = UsersList.Where(s => s.tpflag == Tpflag).ToList();
                }
                if (Freeflag.HasValue)
                {
                    UsersList = UsersList.Where(s => s.freeflag == Freeflag).ToList();
                }
                if (!String.IsNullOrEmpty(ConnectId))
                {
                    UsersList = UsersList.Where(s => s.ConnectionId == ConnectId.ToString()).ToList();
                }
                if (!String.IsNullOrEmpty(UserGroup))
                {
                    UsersList = UsersList.Where(s => s.UserGroup == UserGroup.ToString()).ToList();
                }
                return UsersList.First();
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }


    public class NotifiHub : Hub
    {



    }

    }





