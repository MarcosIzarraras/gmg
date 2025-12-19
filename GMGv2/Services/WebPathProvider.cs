using GMG.Application.Common.Interfaces;

namespace GMGv2.Services
{
    public class WebPathProvider : IPathProvider
    {
        private readonly IWebHostEnvironment _environment;

        public WebPathProvider(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public string GetWebRootPath()
        {
            return _environment.WebRootPath;
        }

        public string CombinePath(params string[] paths)
        {
            return Path.Combine(paths);
        }
    }
}
