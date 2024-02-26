using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
	public class CategoryManager : ICategoryService
	{
		ICategoryDal _Categories;

		public CategoryManager(ICategoryDal categories)
		{
			_Categories = categories;
		}

		public List<Category> GetAll()
		{
			return _Categories.GetAll();
		}

		public Category GetById(int Id)
		{
			return _Categories.Get(c => c.CategoryId == Id);
		}
	}
}
