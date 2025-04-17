using Domain.Models;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifcations
{
    public class ProductWithBrandsAndTypeSrpecification : BaseSpecification<Product,int>
    {
        public ProductWithBrandsAndTypeSrpecification(int id) : base(p => p.Id == id)
        {
            ApplyIncludes();
        }

        public ProductWithBrandsAndTypeSrpecification(ProductSpecificationParamter SpecParams) : base(
            p =>
            (string.IsNullOrEmpty(SpecParams.Search) || p.Name.ToLower().Contains(SpecParams.Search.ToLower()))
            &&
            (!SpecParams.BrandId.HasValue || p.BrandId == SpecParams.BrandId)
            &&
            (!SpecParams.TypeId.HasValue || p.TypeId == SpecParams.TypeId)
            )
        {
            ApplyIncludes();

            ApplySorting(SpecParams.Sort);

            ApplyPagination(SpecParams.PageIndex, SpecParams.PageSize);
        }  

        


        private void ApplyIncludes()
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }

        private void ApplySorting(string? sort)
        {

            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {

                    case "namedesc":
                        AddOrderByDescending(p => p.Name);
                        break;
                    case "priceasc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "pricedesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }
        }

    }
}
