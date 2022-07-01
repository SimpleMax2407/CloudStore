using System;
using System.Runtime.Serialization;

namespace CS_BLL.Validation
{
    [Serializable]
    public class CloudStoreException : Exception
    {
        public CloudStoreException() : base() { }
        public CloudStoreException(string message) : base(message) { }
        protected CloudStoreException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class FileIsEmptyException : CloudStoreException
    {
        public FileIsEmptyException() : base("Downloaded file doesn't exist or empty") { }

        protected FileIsEmptyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class FileAlreadyExistException : CloudStoreException
    {
        public FileAlreadyExistException() : base("File already exist") { }

        protected FileAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class FileDoesntExistException : CloudStoreException
    {
        public FileDoesntExistException() : base("File doesn't exist") { }

        protected FileDoesntExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class FileDoesntExistInFSException : CloudStoreException
    {
        public FileDoesntExistInFSException() : base("File doesn't exist in file system") { }

        protected FileDoesntExistInFSException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class FileAlreadyExistInFSException : CloudStoreException
    {
        public FileAlreadyExistInFSException() : base("File already exist in file system") { }

        protected FileAlreadyExistInFSException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class UnknownException : CloudStoreException
    {
        public UnknownException() : base("Unknown error") { }

        protected UnknownException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
