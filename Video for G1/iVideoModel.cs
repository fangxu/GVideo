using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Video_for_G1
{
    interface iVideoModel
    {        
        void addVideo(String name);
        void addVideos(String[] names);
        void removeVideo(String name);
        void removeVideos(String[] names);
        void clearVideo();
        HashSet<VideoItem> getVideos();
        //String getTitle();
        //void setTitle(String title);
        void setAfterDone(AfterDone ad);
        AfterDone getAfterDone();

        void addObserver(iVideoView vv);
        void removeObserver(iVideoView vv);
        void notifyObservers();
    }
}
