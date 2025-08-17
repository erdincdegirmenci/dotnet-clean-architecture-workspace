using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Infrastructure.Security
{
    public interface ICustomJwtEventHandler
    {
        Task TokenValidated(TokenValidatedContext context);
        Task MessageReceived(MessageReceivedContext context);
        Task Forbidden(ForbiddenContext context);
        Task AuthenticationFailed(AuthenticationFailedContext context);
    }
}
