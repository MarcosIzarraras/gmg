namespace GMG.Application.Common.Interfaces
{
    public interface IPathProvider
    {
        string GetWebRootPath();
        string CombinePath(params string[] paths);
    }
}
