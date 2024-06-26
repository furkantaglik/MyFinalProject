﻿using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCC.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
	public class ProductManager : IProductService
	{
		IProductDal _productDal;
		ICategoryService _categoryService;

		public ProductManager(IProductDal productDal, ICategoryService categoryService)
		{
			_productDal = productDal;
			_categoryService = categoryService;
		}

		//Claim
		[SecuredOperation("product.add,admin")]
		[ValidationAspect(typeof(ProductValidator))]
		public IResult Add(Product product)
		{
			IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId),
				CheckIfProductNameExists(product.ProductName));

			if (result != null)
			{
				return result;
			}

			_productDal.Add(product);
			return new SuccessResult(Messages.ProductAdded);
		}

		public IResult Delete(Product product)
		{
			throw new NotImplementedException();
		}

		public IDataResult<List<Product>> GetAll()
		{
			if (DateTime.Now.Hour == 20)
			{
				return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
			}
			return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Messages.ProductListed);
		}

		public IDataResult<List<Product>> GetAllByCategoryId(int id)
		{
			return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
		}

		public IDataResult<Product> GetById(int productId)
		{
			return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
		}

		public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
		{
			return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
		}

		public IDataResult<List<ProductDetailDto>> GetProductDetails()
		{
			if (DateTime.Now.Hour == 20)
			{
				return new ErrorDataResult<List<ProductDetailDto>>(Messages.MaintenanceTime);
			}
			return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
		}

		[ValidationAspect(typeof(ProductValidator))]
		public IResult Update(Product product)
		{
			if (CheckIfProductCountOfCategoryCorrect(product.CategoryId).Success)
			{
				_productDal.Update(product);
				return new SuccessResult(Messages.ProductAdded);
			}
			return new ErrorResult();
		}

		private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
		{
			var result = _productDal.GetAll(p => p.CategoryId == categoryId).Count;
			if (result >= 10)
			{
				return new ErrorResult(Messages.ProductCountOfCategoryError);
			}

			return new SuccessResult();
		}

		private IResult CheckIfProductNameExists(string productName)
		{
			var result = _productDal.GetAll(p => p.ProductName == productName).Any();
			if (result)
			{
				return new ErrorResult(Messages.ProductNameAlreadyExists);
			}
			return new SuccessResult();
		}

		private IResult CheckIfCategoryLimitExcited()
		{
			var result = _categoryService.GetAll();
			if (result.Data.Count > 15)
			{
				return new ErrorResult(Messages.CategoryLimitExceded);
			}
			return new SuccessResult();
		}
	}

}
