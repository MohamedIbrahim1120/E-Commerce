using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifcations
{
    public class ProductWithCountSpecification : BaseSpecification<Product,int>
    {
        public ProductWithCountSpecification(ProductSpecificationParamter SpecParams) : base(

              p =>
            (string.IsNullOrEmpty(SpecParams.Search) || p.Name.ToLower().Contains(SpecParams.Search.ToLower()))
            &&
            (!SpecParams.BrandId.HasValue || p.BrandId == SpecParams.BrandId)
            &&
            (!SpecParams.TypeId.HasValue || p.TypeId == SpecParams.TypeId)
            )
        {
            
        }
    }
}
