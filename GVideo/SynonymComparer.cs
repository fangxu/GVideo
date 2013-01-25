using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVideo
{
    public class SynonymComparer : IEqualityComparer<VideoItem>
    {
        public bool Equals(VideoItem one, VideoItem two)
        {
            // Adjust according to requirements
            return String.Equals(one.getName(), two.getName());

        }

        public int GetHashCode(VideoItem item)
        {
            return StringComparer.CurrentCulture.GetHashCode(item.getName());

        }
    }
}
