using AutoMapper;
using Domain.Contracts;
using Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceManager(IUnitOfWord unitOfWord, IMapper mapper
                                ,IBasketRepository basketRepository) : IServiceManager
    {
        private readonly IUnitOfWord _unitOfWord = unitOfWord;
        private readonly IMapper _mapper = mapper;

        public IProductService ProductService {  get;} = new ProductService(unitOfWord,mapper);

        public IBasketService BasketService { get; } = new BasketService(basketRepository, mapper);
    }
}
