namespace ImageService.Infrastructure.Enums
{
    /// <summary>
    /// Associating command with numeric values
    /// </summary>
    public enum CommandEnum : int
    {
        NewFileCommand = 1,
        GetConfigCommand,
        LogCommand,
        CloseCommand,
        AddLogItem,
        CloseHandler,
        DisconnectClient
    }
}