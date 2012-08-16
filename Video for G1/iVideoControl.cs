using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Video_for_G1
{
    interface iVideoControl
    {
        void addVideo(String name);
        void addVideos(String[] names);
        void removeVideo(String name);        
        void removeVideos(String[] names);
        void clearVideo();
        void setModel(iVideoModel vm);
        void setView(iVideoView vv);

        void encode(String options, String output);
    }
}
