using Business.Abstract;
using Core.Utilities.Results;
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

		public IDataResult<List<Category>> GetAll()
		{
			return new SuccessDataResult<List<Category>>(_Categories.GetAll());
		}

		public IDataResult<Category> GetById(int Id)
		{
			return new SuccessDataResult<Category>(_Categories.Get(c => c.CategoryId == Id));
		}
	}
}
