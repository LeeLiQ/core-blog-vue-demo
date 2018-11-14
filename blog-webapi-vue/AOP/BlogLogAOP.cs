using System;
using System.IO;
using System.Linq;
using Castle.DynamicProxy;

namespace blog_webapi_vue.AOP
{
    public class BlogLogAOP : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var dataInterceptor = $"{DateTime.Now.ToString("yyyyMMddHHmmss")} " +
                $"Now executing：{ invocation.Method.Name} " +
                $"With parameters： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} \r\n";

            invocation.Proceed();

            var path = Directory.GetCurrentDirectory() + @"\Log";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var fileName = path + $@"\InterceptLog-{DateTime.Now.ToString("yyyyMMddHHmmss")}.log";

            var sw = File.AppendText(fileName);
            sw.WriteLine(dataInterceptor);
            sw.Close();
        }
    }
}