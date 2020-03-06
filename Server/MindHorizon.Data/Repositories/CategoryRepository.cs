using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MindHorizon.Data.Contracts;
using MindHorizon.Entities;
using MindHorizon.ViewModels.Category;
using MindHorizon.Common;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Linq.Dynamic.Core;

namespace MindHorizon.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MindHorizonDbContext _context;
        private readonly IMapper _mapper;
        public CategoryRepository(MindHorizonDbContext context, IMapper mapper)
        {
            _context = context;
            _context.CheckArgumentIsNull(nameof(_context));

            _mapper = mapper;
            _mapper.CheckArgumentIsNull(nameof(_mapper));
        }

        public async Task<List<CategoryViewModel>> GetPaginateCategoriesAsync(int offset, int limit, string orderBy, string searchText)
        {
            List<CategoryViewModel> categories = await _context.Categories.GroupJoin(_context.Categories,
                (cl => cl.ParentCategoryId),
                (or => or.CategoryId),
                ((cl, or) => new { CategoryInfo = cl, ParentInfo = or }))
                .SelectMany(p => p.ParentInfo.DefaultIfEmpty(), (x, y) => new { x.CategoryInfo, ParentInfo = y })
                .OrderBy(orderBy)
                .Skip(offset).Take(limit)
                .Select(c => new CategoryViewModel { CategoryId = c.CategoryInfo.CategoryId, CategoryName = c.CategoryInfo.CategoryName, ParentCategoryId = c.ParentInfo.CategoryId, ParentCategoryName = c.ParentInfo.CategoryName }).AsNoTracking().ToListAsync();

            foreach (var item in categories)
                item.Row = ++offset;

            return categories;
        }


        public async Task<List<TreeViewCategory>> GetAllCategoriesAsync()
        {
            var Categories =await (from c in _context.Categories
                              where (c.ParentCategoryId == null)
                              select new TreeViewCategory { id = c.CategoryId, title = c.CategoryName ,url=c.Url}).ToListAsync();
            foreach (var item in Categories)
            {
                BindSubCategories(item);
            }

            return Categories;
        }

        public void BindSubCategories(TreeViewCategory category)
        {
            var SubCategories = (from c in _context.Categories
                                 where (c.ParentCategoryId == category.id)
                                 select new TreeViewCategory { id = c.CategoryId, title = c.CategoryName , url=c.Url }).ToList();
            foreach (var item in SubCategories)
            {
                BindSubCategories(item);
                category.subs.Add(item);
            }
        }

        public Category FindByCategoryName(string categoryName)
        {
           return  _context.Categories.Where(c => c.CategoryName == categoryName.TrimStart().TrimEnd()).FirstOrDefault();
        }


        public bool IsExistCategory(string categoryName, string recentCategoryId = null)
        {
            if (!recentCategoryId.HasValue())
                return _context.Categories.Any(c => c.CategoryName.Trim().Replace(" ", "") == categoryName.Trim().Replace(" ", ""));
            else
            {
                var category = _context.Categories.Where(c => c.CategoryName.Trim().Replace(" ", "") == categoryName.Trim().Replace(" ", "")).FirstOrDefault();
                if (category == null)
                    return false;
                else
                {
                    if (category.CategoryId != recentCategoryId)
                        return true;
                    else
                        return false;
                }
            }
        }


    }
}
