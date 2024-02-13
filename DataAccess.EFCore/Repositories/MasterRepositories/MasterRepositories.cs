using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DataAccess.EFCore.Repositories
{
    public class CustomErrorLogRepository : GenericRepository<CustomErrorLog>, ICustomErrorLogRepository
    {
        public CustomErrorLogRepository(ApplicationContext context) : base(context)
        {
        }
    }
    public class ImageRepository : GenericRepository<Image>, IImageRepository
    {
        public ImageRepository(ApplicationContext context) : base(context)
        {
        }
    }

    public class FileUploadRepository : GenericRepository<FileUploadDetail>, IFileUploadRepository
    {
        public FileUploadRepository(ApplicationContext context) : base(context)
        {
        }
        public List<dynamic> GetSelectList(Expression<Func<FileUploadDetail, bool>> expression, int skip = 0, int nop = 0)
        {


            var k = _context.FileUploads.AsNoTracking().Where(expression).AsQueryable();

            if (skip != 0)
            {
                k = k.Skip(skip);
            }
            if (nop != 0)
            {
                k = k.Take(nop);
            }
            var q = k.Select(u => new { code = u.Id.ToString(), label = u.Filename.ToString() }).ToList<dynamic>();
            return q;


        }
    }

}
