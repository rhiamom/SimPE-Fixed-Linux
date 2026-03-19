using SimPe.Interfaces.Files;
using System.Collections.Generic;

namespace SimPe
{
    /// <summary>
    /// One node in the Resource Tree — represents all resources of a given type.
    /// </summary>
    public class ResourceTypeNode
    {
        public string DisplayName { get; }
        public List<IPackedFileDescriptor> Descriptors { get; }

        public ResourceTypeNode(string typeName, List<IPackedFileDescriptor> descriptors)
        {
            Descriptors  = descriptors;
            DisplayName  = $"{typeName}  ({descriptors.Count})";
        }
    }
}
