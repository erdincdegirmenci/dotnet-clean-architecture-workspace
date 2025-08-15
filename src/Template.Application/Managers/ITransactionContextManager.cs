using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Application.Managers
{
    public interface ITransactionContextManager
    {
        void SetTransaction(bool forcedCreateNewIdentifier = false);
        string GetTransaction();
        void DeleteTransaction();
        void SetJwtToken(string token);
        string GetJwtToken();
        void DeleteJwtToken();
        void SetLanguage(string language);
        string GetLanguage();
        void DeleteLanguage();
    }
}
