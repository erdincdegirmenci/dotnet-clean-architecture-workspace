using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Application.Managers;

namespace Template.Infrastructure.Security
{
    public class CustomJwtEventHandler : JwtBearerEvents, ICustomJwtEventHandler
    {
        readonly IUserContextManager<IUserContextModel> _userContextManager;
        readonly ITransactionContextManager _transactionContextManager;

        public CustomJwtEventHandler(IUserContextManager<IUserContextModel> userContextManager, ITransactionContextManager transactionContextManager)
        {
            _userContextManager = userContextManager;
            _transactionContextManager = transactionContextManager;
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            // jwt token varsa valid ise buraya girer, vadlidate işlemi otomatik

            if (_userContextManager.GetUser() == null || !_userContextManager.GetUser().Username.Equals(context.Principal.Identity.Name))
            {
                if (!_userContextManager.SetUser(context.Principal.Identity.Name))
                {
                    throw new Exception("User is not found. Name : " + context.Principal.Identity.Name);
                }
            }

            await Task.CompletedTask;
        }

        public override async Task MessageReceived(MessageReceivedContext context)
        {
            Microsoft.Extensions.Primitives.StringValues accessToken = context.Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                //Bearer boşluk kısmını trimliyoruz
                context.Token = accessToken.ToString().Substring(7);
                _transactionContextManager.SetJwtToken(context.Token);
            }
            else
            {
                _transactionContextManager.DeleteJwtToken();
            }

            _transactionContextManager.SetTransaction();

            await Task.CompletedTask;
        }

        public override async Task Forbidden(ForbiddenContext context)
        {
            // do something

            await Task.CompletedTask;
        }

        public override async Task AuthenticationFailed(AuthenticationFailedContext context)
        {
            _userContextManager.DeleteUser();

            await Task.CompletedTask;
        }
    }
}
