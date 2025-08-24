using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Infra.Data.Dapper
{
    public sealed class SqliteGuidTextHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            // grava como string padrão "D" (36 chars com hífens)
            parameter.Value = value.ToString("D");
            parameter.DbType = DbType.String;
        }

        public override Guid Parse(object value)
        {
            return value switch
            {
                Guid g => g,                      
                string s => Guid.Parse(s),          
                _ => Guid.Parse(value.ToString()!)
            };
        }
    }
}
