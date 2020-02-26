using Microsoft.EntityFrameworkCore;
using MindHorizon.Common;
using MindHorizon.Data.Contracts;
using MindHorizon.ViewModels.Video;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MindHorizon.Data.Repositories
{
    public class VideoRepository : IVideoRepository
    {
        private readonly MindHorizonDbContext _context;
        public VideoRepository(MindHorizonDbContext context)
        {
            _context = context;
        }


        public List<VideoViewModel> GetPaginateVideos(int offset, int limit, Func<VideoViewModel, Object> orderByAscFunc, Func<VideoViewModel, Object> orderByDescFunc, string searchText)
        {
            List<VideoViewModel> videos= _context.Videos.Where(c => c.Title.Contains(searchText))
                                    .Select(c => new VideoViewModel { VideoId = c.VideoId, Title = c.Title, Url = c.Url, Poster=c.Poster,PersianPublishDateTime=c.PublishDateTime.ConvertMiladiToShamsi("yyyy/MM/dd ساعت HH:mm:ss"),PublishDateTime=c.PublishDateTime})
                                    .OrderBy(orderByAscFunc).OrderByDescending(orderByDescFunc).Skip(offset).Take(limit).ToList();

          
            foreach (var item in videos)
                item.Row = ++offset;

            return videos;
        }

        public string CheckVideoFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);
            int fileNameCount = _context.Videos.Where(f => f.Poster == fileName).Count();
            int j = 1;
            while (fileNameCount != 0)
            {
                fileName = fileName.Replace(fileExtension, "") + j + fileExtension;
                fileNameCount = _context.Videos.Where(f => f.Poster == fileName).Count();
                j++;
            }

            return fileName;
        }
    }
}
