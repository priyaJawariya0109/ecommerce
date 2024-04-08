
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ecommerceModels.Model
{
    public class Serviceparameters
    {
        public Serviceparameters()
        {
            ParameterList = new List<Param>()
            {
            };
            Ldtparamname = new string[] { };
            LdtParams = new DataTable[] { };
        }
        public string ProcedureName { get; set; }
        public List<Param> ParameterList { get; set; }
        public string[] Ldtparamname { get; set; }
        public DataTable[] LdtParams { get; set; }
        public string OutputParamName { get; set; }
    }

    public class Param
    {
        public Param()
        {
        }

        public Param(string Name, dynamic Value)
        {
            this.Name = Name;
            this.Value = Value;
        }
        public string Name { get; set; }
        public dynamic Value { get; set; }
    }
}
