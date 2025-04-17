using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Services.Specifcations;
using Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWord _unitOfWord;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWord unitOfWord,IMapper mapper)
        {
            _unitOfWord = unitOfWord;
            _mapper = mapper;
        }

        public async Task<PaginationResponse<ProductResultDto>> GetAllProductAsync(ProductSpecificationParamter SpecParams)
        {
            var spec = new ProductWithBrandsAndTypeSrpecification(SpecParams);


            // Get All Product From Throught ProductRepository
           var produt = await _unitOfWord.GetRepository<Product,int>().GetAllAsync(spec);

            var specCount = new ProductWithCountSpecification(SpecParams);

            var count = await _unitOfWord.GetRepository<Product,int>().CountAsyn(specCount);
            // Mapping IEnumerable<Product> To <IEnumerable<ProductResultDto>>
           var result = _mapper.Map<IEnumerable<ProductResultDto>>(produt);


            return new PaginationResponse<ProductResultDto>(SpecParams.PageIndex,SpecParams.PageSize, count, result);
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandsAndTypeSrpecification(id);

           var product = await _unitOfWord.GetRepository<Product,int>().GetAsync(spec);
            if (product == null) return null;

          var result = _mapper.Map<ProductResultDto>(product);
            return result;

        }

        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
           var brand = await _unitOfWord.GetRepository<ProductBrand,int>().GetAllAsync();   
           var result = _mapper.Map<IEnumerable<BrandResultDto>>(brand); 
            return result;

        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types = await _unitOfWord.GetRepository<ProductType, int>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<TypeResultDto>>(types);
            return result;
        }
        
    }
}
