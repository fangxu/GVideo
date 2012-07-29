using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Video_for_G1
{
    public enum Status
    {
        waitting,
        processing,
        done,
        error,
    }

    public class VideoItem
    {
        private String name;
        private String path;
        private Status status;

        public String getName()
        {
            return name;
        }

        public String getPath()
        {
            return path;
        }

        public Status getStatus()
        {
            return status;
        }

        public void setStatus(Status s)
        {
            status = s;
        }

        public VideoItem(String path)
        {            
            if (path.Contains('\\'))
            {
                this.path = path;
                this.name = path.Substring(path.LastIndexOf('\\') + 1, path.Length - path.LastIndexOf('\\') - 1);
            } 
            else
            {
                this.name = path;
            }
            
        }
    }
}
