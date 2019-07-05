using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Classes
{
    public class ResultList<T>
    {
        public bool Value { get; set; }
        public string Message { get; set; }
        public virtual List<T> Data { get; set; }

        public ResultList()
        {

        }

        public ResultList(Result<List<T>> result)
        {
            Value = result.Value;
            Message = result.Message;
            Data = result.Data;
        }

        public ResultList(bool value, string msg)
        {
            Value = value;
            Message = msg;
        }

        public ResultList(bool value, string msg, List<T> data)
        {
            Value = value;
            Message = msg;
            Data = data;
        }
    }
}
