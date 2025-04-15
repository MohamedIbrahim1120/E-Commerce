using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<ProductResultDto>> GetAllProductAsync()
        {
            // Get All Product From Throught ProductRepository
           var produt = await _unitOfWord.GetRepository<Product,int>().GetAllAsync();
            // Mapping IEnumerable<Product> To <IEnumerable<ProductResultDto>>
           var result = _mapper.Map<IEnumerable<ProductResultDto>>(produt);
            return result;
        }


        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
           var product = await _unitOfWord.GetRepository<Product,int>().GetAsync(id);
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
