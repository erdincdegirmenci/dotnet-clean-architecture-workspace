using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Domain.QueryTemplate
{
    public interface IQueryTemplate
    {
        string GetQuery(string key);
        string GetQuery(string key, string dynamicWhereClause);
    }
}
