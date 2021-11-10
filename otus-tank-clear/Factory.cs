using System;
using System.Numerics;
using System.Collections.Generic;

namespace OtusTankClear
{
    public class IoC
    {
        public static T resolve<T>(string key, IUObject obj, params object[] args)
        {
            try
            {
                obj.SetProperty("tempArgs", args);
                var t = obj.GetProperty(key);
                return (T)t;
            }
            catch
            {
                throw new Exception("Product not regist");
            }
        }

        public static object DelegateToObject(Func<object, object> del)
        {
            return del;
        }
    }



    public interface IRegister
    {
        void SetProduct(string newProduct, Func<object, ICommand> func);
        object[] GetParams();
    }

    public class RegisterAdapter : IRegister
    {
        readonly IUObject Obj;
        public RegisterAdapter(IUObject Obj)
        {
            this.Obj = Obj;
        }
        public void SetProduct(string newProduct, Func<object,ICommand> func)
        {        
            Obj.SetProperty(newProduct, func);
        }
        public object[] GetParams()
        {
            if (Obj.GetProperty("tempArgs") == null) throw new Exception("Error: tempArgs is null");
            return (object[])Obj.GetProperty("tempArgs");
        }
    }

    public class Register : ICommand
    {
        readonly IRegister register;
        public Register(IRegister register)
        {
            this.register = register;
        }

        public void Execute()
        {
            var t = register.GetParams();
            register.SetProduct((string)t[0], (Func<object, ICommand>)t[1]);
        }
    }
}