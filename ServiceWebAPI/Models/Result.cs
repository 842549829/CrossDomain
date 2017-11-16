namespace ServiceWebAPI
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 请求结果
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string Message { get; set; }
    }

    /// <summary>
    /// 请求结果
    /// </summary>
    /// <typeparam name="T">结果类型</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 最终结果
        /// </summary>
        public T Data { get; set; }
    }
}