using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataEntity
{
    public class MyUserManager : UserManager<MyUser, long>
    {
        #region constructors and destructors

        public MyUserManager(IUserStore<MyUser, long> store)
            : base(store)
        {
        }

        #endregion

        #region methods
        public static MyUserManager Create(IdentityFactoryOptions<MyUserManager> options, IOwinContext context)
        {
            var manager = new MyUserManager(new UserStore<MyUser, MyRole, long, MyLogin, MyUserRole, MyClaim>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<MyUser, long>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength =1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            manager.RegisterTwoFactorProvider(
                "PhoneCode",
                new PhoneNumberTokenProvider<MyUser, long>
                {
                    MessageFormat = "Your security code is: {0}"
                });
            manager.RegisterTwoFactorProvider(
                "EmailCode",
                new EmailTokenProvider<MyUser, long>
                {
                    Subject = "Security Code",
                    BodyFormat = "Your security code is: {0}"
                });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<MyUser, long>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }

        #endregion
    }
}
