using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Video_for_G1
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
