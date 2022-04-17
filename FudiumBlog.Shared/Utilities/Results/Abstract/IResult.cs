using FudiumBlog.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FudiumBlog.Shared.Utilities.Results.Abstract
{
    public interface IResult
    {
        public ResultStatus ResultStatus { get; }//ResultStatus.Success//ResultStatus.Error//enum
        public string Message { get; }//dönecek mesaj,değiştirilebilir olmayacak
        public Exception Exception { get; }
    }
}
