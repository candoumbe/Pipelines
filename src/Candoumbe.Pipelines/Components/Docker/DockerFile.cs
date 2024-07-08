using Nuke.Common.IO;

namespace Candoumbe.Pipelines.Components.Docker
{
    /// <summary>
    /// Wraps dockerfile information.
    /// </summary>
    /// <param name="Path">Path to the <c>Dockerfile</c></param>
    /// <param name="Name">Name of the image that will be build</param>
    /// <param name="Tag">Tag of the docker image</param>
    public record DockerFile(AbsolutePath Path, string Name, string Tag = "latest");
}
