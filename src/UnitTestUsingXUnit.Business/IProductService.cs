﻿using UnitTestUsingXUnit.DataAccess.Dtos;

namespace UnitTestUsingXUnit.Business
{
    public interface IProductService
    {        
        Task<ProductDto> GetProductAsync(int productId);

        Task<IEnumerable<ProductDto>> GetProductsAsync();

        Task<int> CreateProductAsync(CreateProduct createProduct);

    }
}
