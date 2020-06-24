namespace DrawingModule.CommandLine
{
    public enum DocumentLockMode
    {
        None = 0,
        AutoWrite = 1,
        NotLocked = 2,
        Write = 4,
        ProtectedAutoWrite = 20,
        Read = 0x20,
        ExclusiveWrite = 0x40
    }
}