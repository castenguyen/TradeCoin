using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Extension
{
    public static partial class EnumCore
    {

        #region BACKEND

        public enum BackendConst : int
        {
            page_size = 50,
            start_count = 0,
        }
        public enum ObjTypeId : int
        {
            tin_tuc = 6011,
            san_pham = 6010,
            Microsite = 6012,
            binh_luan = 6013,
            nguoi_dung = 6014,
            hinh_anh = 6026,
            video = 6028,
            album = 6029,
            banner = 6030,
            danh_muc = 6027,
            img_in_document = 1,
            page_infor=8187,
            page_infor_microsite = 8198,
            package= 11914,
            ticker= 11915,
            emailsupport= 11923,

        }
        public enum IsEvent : int
        {
            True = 1,
            False = 0
        }

        public enum ClassificationScheme : int
        {
          
            tin_tuc_bai_viet = 1,
            san_pham = 2,
            hinh_anh = 3,
            video_clip = 4,
            User_type = 5,
            role_type = 6,
            state_type = 7,
            comment = 8,
            Related_Content_Type = 3002,
            ActionType = 4002,
            display_type = 4003,
            Product_brand = 4005,
            xuat_xu = 4006,
            don_vi = 4007,
            page_infor = 4008,
            GenderType = 4011,
            tran_type = 4012,
            statusproduct_type = 4014,
            color_list = 4013,
            support_time = 4015,
            display_block_micro = 4018,
            loai_khuyen_mai=5018,
            trang_thai_don_hang = 4010,
            kich_thuoc = 5021,
            don_vi_hanh_chinh = 4009,
            status_ticker=5022,
        
        }

        public enum mediatype : int
        {
            hinh_anh = 2003,
            video_clip = 2004,
            hinh_anh_dai_dien = 8188,
        }

        public enum Result : int
        {
            action_true = 1,
            action_false = 0,
        }



        public enum StateType : int
        {
            cho_phep = 6015,
            khong_cho_phep = 6016,
            cho_duyet = 6017,
            da_xoa = 6148,

            enable = 1,
            disable = 0,
        }

        public enum StatusOrder : int
        {
            don_hang_duoc_ghi_nhan = 7184,
            nguoi_ban_xac_nhan_don_hang = 7185,
            hang_da_duoc_chuyen_di = 7186,
            don_hang_hoan_tat = 7187
        }

        public enum ContentRelatedType : int
        {
            contentitem_contenitem = 6018,
            contentitem_product = 6019,

        }


        public enum Classification : int
        {
            tin_tuc = 11884,
            gioi_thieu = 10815,
            huong_dan = 10814,
            pageinfor_tintuc = 6183,
            pageinfor_gioithieu = 6178,
            pageinfor_chinhsach = 6181,
            pageinfor_lienhe = 6182,

            gioi_tinh_nam = 6158,
            gioi_tinh_nu = 6159,
            gioi_tinh_khac = 6160,

            area_tinh = 6150,
            area_quan_huyen = 6151,
            area_phuong_xa = 6152,
            area_dia_diem_du_lich = 11829,

            dem_luot_thich_gh = 10804,
            dem_luot_mua_sp = 10803,
            dem_luot_thich_sp = 10802,
            moi_user_xem_gh = 10805



        }

        public enum ActionType : int
        {
            Create = 6024,
            Delete = 6025,

            Update = 6031,
            Login = 6032,
            Logout = 6033

        }


        public enum SystemConfig_MenuLeft : int
        {
            dashboard=1,
            classification=1,
            comment = 0,
            news = 1,
            pageinfo = 1,
            product =0,
            media = 1,
            display = 0,
            tag=0,
            user = 1,
            role=1,
            config = 1,
            history=1,
            order=0,
            promotion=0,
            package = 1,
            ticker=1,
            mail=1,
            chatgroup = 1,
            changepass =0,
        }
        public enum ProjectConfig_System : int
        {
        
             /// <summary>
             /// có 2 kieu đăng nhập
             /// 1 : Đăng nhập bằng usernam và password
             /// 2: Đăng nhập bằng mã token được gửi về email đăng ký
             /// ==>khi hệ thống đăng nhập bằng mã token thì LoginWithCode bật lên = 1 ngược lai là 0
             /// </summary>
             LoginWithCode = 1,
            
        }


        public enum EmailStatus : int
        {
            chua_xem = 11921,
            cho_ho_tro = 11924,
            da_ho_tro = 11925,
            da_xoa = 11922,

            da_xem = 11920,
        }

        public enum EmailClearType : int
        {
            xoa_mail_mod = 0,
            xoa_mail_member = 1,

        }


        public enum UpgradeStatus : int
        {
            cho_duyet = 1,
            duyet = 2,
            het_han = 3,

        }

        public enum AlertPageType : int
        {
            lockscreen = 0,
            FullScrenn = 1,
           

        }

        public enum Package : int
        {
           free=1,
           dem=2,
           dong=3,
           vang=4,
           kimcuong=5,


        }

        public enum PackageTimeType : int
        {
            thang = 1,
            quy = 2,
            vinhvien = 3,
        }


        public enum TickerStatusType : int
        {
            da_xoa = 11919,
            dang_chay = 11918,
            loi = 11917,
            lo = 11916,
        }

        public enum FlagForChatHub : int
        {
            admin_busy = 0,
            admin_free = 1,
            user_guest = 0,
            user_admin = 1,
            waitting = 1,
            notwaitting = 0,
        }


        #endregion END BACKEND

    }

    public static partial class ConstFrontEnd
    {

        #region FRONTEND
        public enum FontEndConstDisplayType : int
        {
            HomeSlider = 6020,
        }

        public enum Classifi_news_index : int
        {
            ykien = 11883,
            baiviet = 10815,
            doitac = 11909,
            visao = 11910

        }


        public enum FontEndConstProductCata : int
        {
            cay_giong_chai_mo = 11912,
            lan_rung = 11895,
            dendro = 11894,
            ho_diep = 11893,
            van_da = 11892,
            phan_thuoc = 11891,
            lan_quy_hiem = 11885

        }



        public enum FontEndConstNumberRecord : int
        {
            Nbr_News_In_Home = 10,
            Nbr_Ticker_In_Home = 10,

        }

    


        public enum FontEndConstPageinforMicrosite : int
        {

            gioi_thieu = 6178,
            chinh_sach = 6181,
            lien_he = 6182,
            tin_tuc = 6183,
        }

        public enum FrontendPageinfor : int
        {
            trade = 6141,
            crypto = 6145,
            intro = 6143,
            indexvideo = 6139,

        }

        #endregion END FRONTEND

    }


}
