using DataModel.DataEntity;
using DataModel.DataStore;
using DataModel.DataViewModel;
using DataModel.Extension;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Net.Http.Headers;


namespace Alluneecms.Controllers
{
    [RoutePrefix("api/NcoinService")]
    public class NcoinAPIController : ApiController
    {
        protected Ctrl cms_db = new Ctrl();
        protected alluneedbEntities db = new alluneedbEntities();
        private MyUserManager _AppUserManager = null;
        protected MyUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<MyUserManager>();
            }
        }

        private ApplicationRoleManager _AppRoleManager = null;

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        [Route("GetVideo")]
        public HttpResponseMessage GetVideo(long VideoId )
        {


            string fileName = "";
            MediaContent objVideo = new MediaContent();
            objVideo = db.MediaContents.Find(VideoId);
            if (objVideo != null)
            {
                fileName = objVideo.LinkHref;
            }
          

            var video = new VideoStream(fileName);
            Func<Stream, HttpContent, TransportContext, Task> func = video.WriteToStream;
            var response = Request.CreateResponse();
            response.Content = new PushStreamContent(func, new MediaTypeHeaderValue("video/mp4"));
            return response;
           
        }

    }


   


    public class VideoStream
    {
        private readonly string _filename;

        public VideoStream(string fileName)
        {
            string _ServerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            _filename = _ServerPath;
        }
        public async Task WriteToStream(Stream outputStream, HttpContent content, TransportContext context)
        {
            try
            {
                var buffer = new byte[20000000];
                using (FileStream video = File.Open(_filename, FileMode.Open, FileAccess.Read,FileShare.Read))
                {
                    var length = (int)video.Length;
                    var bytesRead = 1;

                    while (length > 0 && bytesRead > 0)
                    {
                        bytesRead = video.Read(buffer, 0, Math.Min(length, buffer.Length));
                        System.Diagnostics.Debug.WriteLine(string.Format("Length at Start: {0}; bytesread: {1}", length, bytesRead));

                        await
                            outputStream.WriteAsync(buffer, 0, bytesRead);

                        length -= bytesRead;
                    }
                }
            }
            catch (System.Web.HttpException httpEx)
            {
                Ctrl cms_db = new Ctrl();
                System.Diagnostics.Debug.WriteLine(httpEx.GetBaseException().Message);

                cms_db.AddToExceptionLog("WriteToStream--> System.Web.HttpException", "NcoinAPIController", httpEx.GetBaseException().Message.ToString());

                if (httpEx.ErrorCode == -2147023667) // The remote host closed the connection. 
                {
                   // cms_db.AddToExceptionLog("WriteToStream--> System.Web.HttpException -> The remote host closed the connection.", "NcoinAPIController", httpEx.GetBaseException().Message.ToString());
                    return;
                }
                    
            }
            catch (Exception ex)
            {
                Ctrl cms_db = new Ctrl();
                System.Diagnostics.Debug.WriteLine(ex.GetBaseException().Message);
                cms_db.AddToExceptionLog("WriteToStream", "NcoinAPIController", ex.ToString());
                return;
            }
            finally
            {
                outputStream.Close();
            }
        }
    }

}
