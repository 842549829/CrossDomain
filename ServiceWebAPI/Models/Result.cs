namespace ServiceWebAPI
{
    public class Result
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string Authorization { get; set; }
    }

    public class Result<T> : Result
    {
        public T Data { get; set; }
    }
}