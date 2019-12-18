using System;

namespace WarmPack.Classes
{
    public class Result
    {
        public bool Value { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        private string _InfoMessage { get; set; }
        
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

        public Result(Exception ex)
        {
            this.Value = false;
            this.Message = ex.Message;            
        }

        public static Result Create(bool value, string msg)
        {
            return new Result(value, msg);
        }

        public static Result Create(bool value, string msg, object data)
        {
            return new Result(value, msg, data);
        }

        public static Result Create(Exception ex)
        {
            return new Result(ex);
        }

        public string InfoMessage()
        {
            return _InfoMessage;
        }

        public string InfoMessage(string infoMessageValue)
        {
            _InfoMessage = infoMessageValue;
            
            return InfoMessage();
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

        public Result(Exception ex)
        {
            this.Value = false;
            this.Message = ex.Message;
        }

        public static Result Create(bool value, string msg)
        {
            return new Result(value, msg);
        }

        public static Result Create(bool value, string msg, T data)
        {
            return new Result(value, msg, data);
        }

        public static Result Create(Exception ex)
        {
            return new Result(ex);
        }
    }
}
