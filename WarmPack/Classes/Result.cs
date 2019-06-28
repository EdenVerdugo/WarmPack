using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarmPack.Classes
{
    public class Result
    {
        public bool Value { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public Result()
        {

        }

        public Result(bool value, string msg)
        {
            this.Value = value;
            this.Message = msg;
        }

        public Result(bool value, string msg, object data)
        {
            this.Value = value;
            this.Message = msg;
            this.Data = data;
        }
    }

    public class Result<T>
    {
        public bool Value { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public Result()
        {

        }

        public Result(bool value, string msg)
        {
            this.Value = value;
            this.Message = msg;
        }

        public Result(bool value, string msg, T data)
        {
            this.Value = value;
            this.Message = msg;
            this.Data = data;
        }
    }
}
