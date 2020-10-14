using System;
using System.IO;
using System.Linq;
using Castle.DynamicProxy;


namespace CQRS_Simple.Infrastructure
{
    /// <summary>
    /// 拦截器 需要实现 IInterceptor接口 Intercept方法
    /// </summary>
    public class CallLogger : IInterceptor
    {
        TextWriter _output;

        public CallLogger(TextWriter output)
        {
            _output = output;
        }

        /// <summary>
        /// 拦截方法 打印被拦截的方法执行前的名称、参数和方法执行后的 返回结果
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.IsPublic)
            {
                _output.WriteLine("你正在调用方法 \"{0}\"  参数是 {1}... ",
                    invocation.Method.Name,
                    string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

                //在被拦截的方法执行完毕后 继续执行
                invocation.Proceed();

                _output.WriteLine("方法执行完毕，返回结果：{0}", invocation.ReturnValue);
            }
        }
    }
}