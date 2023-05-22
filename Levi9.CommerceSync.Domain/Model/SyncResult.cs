namespace Levi9.CommerceSync.Domain.Model
{
    public class SyncResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
