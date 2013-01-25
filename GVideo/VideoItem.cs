using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GVideo
{
    public enum Status
    {
        waitting,
        processing,
        done,
        error,
    }

    public enum AfterDone
    {
        nothing = 0,
        close = 1,
        shutdown = 2
    }

    public class VideoItem
    {
        private String name;
        private String path;
        private Status status;
        double bitRate;
        public double BitRate {
            get { return bitRate; }
        }

        public String getName() {
            return name;
        }

        public String getPath() {
            return path;
        }

        public Status getStatus() {
            return status;
        }

        public void setStatus(Status s) {
            status = s;
        }


        public VideoItem(String path) {
            if (path.Contains('\\')) {
                this.path = path;
                this.name = path.Substring(path.LastIndexOf('\\') + 1, path.Length - path.LastIndexOf('\\') - 1);
            } else {
                this.name = path;
            }
            VideoInfo vf = new VideoInfo(path);
            this.bitRate = vf.BitRate;
        }
    }
}
