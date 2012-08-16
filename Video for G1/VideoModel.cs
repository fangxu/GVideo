using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;


namespace Video_for_G1
{
    class VideoModel : iVideoModel
    {
        HashSet<VideoItem> videos = new HashSet<VideoItem>(new SynonymComparer());
        ArrayList observers = new ArrayList();
        AfterDone afterDone;

        #region "Method"

        public void setAfterDone(AfterDone ad)
        {
            afterDone = ad;
        }
        public AfterDone getAfterDone()
        {
            return afterDone;
        }

        public void addVideo(String name)
        {
            videos.Add(new VideoItem(name));
            this.notifyObservers();
        }

        public void addVideos(String[] names)
        {
            foreach (String s in names)
            {
                videos.Add(new VideoItem(s));
            }
            this.notifyObservers();
        }

        public void removeVideo(String name)
        {
            videos.Remove(new VideoItem(name));
            this.notifyObservers();
        }

        public void removeVideos(String[] names)
        {
            foreach (String s in names)
            {
                videos.Remove(new VideoItem(s));
            }
            this.notifyObservers();
        }


        public void clearVideo()
        {
            videos.Clear();
            this.notifyObservers();
        }

        public HashSet<VideoItem> getVideos()
        {
            return videos;
        }

       /* public String getTitle()
        {
            return title;
        }

        public void setTitle(String title)
        {
            this.title = title;
        }*/

        #endregion

        #region "Observer"

        public void addObserver(iVideoView vv)
        {
            observers.Add(vv);
        }

        public void removeObserver(iVideoView vv)
        {
            observers.Remove(vv);
        }

        public void notifyObservers()
        {
            foreach (iVideoView vv in observers)
            {
                vv.updateView();
            }
        }

        
        #endregion

    }
}
