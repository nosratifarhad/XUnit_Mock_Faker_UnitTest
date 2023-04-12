using ECommerce.Domain.Products;
using ECommerce.Service.Services;
using Moq;
using FluentAssertions;
using ECommerce.Domain.Products.Dtos.ProductDtos;
using ECommerce.UnitTest.MockDatas;
using ECommerce.Domain.Products.Entitys;
using Bogus.DataSets;
using ECommerce.Service.InputModels.ProductInputModels;

namespace ECommerce.UnitTest.TestServices
{
    public class ProductServiceUnitTest
    {

        #region [ GetProductAsync ]

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async void When_ValidProductIdInGetProductAsync_Then_ReturnedProductViewModel(int productId)
        {
            var MoqProductReadRepository = new Mock<IProductReadRepository>();
            var MoqProductWriteRepository = new Mock<IProductWriteRepository>();

            MoqProductReadRepository.Setup(p => p.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProductDto()
                {
                    ProductId = productId,
                    ProductName = "test",
                    ProductTitle = "tst"
                });

            var ProductService = new ProductService(MoqProductReadRepository.Object, MoqProductWriteRepository.Object);

            var productViewModel = await ProductService.GetProductAsync(productId).ConfigureAwait(false);

            productViewModel.ProductId.Should().Be(productId);
            productViewModel.ProductName.Should().NotBeNull();
            productViewModel.ProductTitle.Should().NotBeNull();
            productViewModel.Should().NotBeNull();
        }

        [Theory]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        public async void When_InValidProductIdInGetProductAsync_Then_ReturnedProductViewModel(int productId)
        {
            var MoqProductReadRepository = new Mock<IProductReadRepository>();
            var MoqProductWriteRepository = new Mock<IProductWriteRepository>();

            MoqProductReadRepository.Setup(p => p.GetProductAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProductDto());

            var ProductService = new ProductService(MoqProductReadRepository.Object, MoqProductWriteRepository.Object);

            var productViewModel = await ProductService.GetProductAsync(productId).ConfigureAwait(false);

            productViewModel.ProductId.Should().Be(0);
            productViewModel.ProductName.Should().BeNull();
            productViewModel.ProductTitle.Should().BeNull();
            productViewModel.Should().NotBeNull();
        }

        [Theory]
        [InlineData(0)]
        public async void When_ZeroProductIdInGetProductAsync_Then_ThrowArgumentExceptionBeMessageProductIdIsInvalid(int productId)
        {
            var MoqProductReadRepository = new Mock<IProductReadRepository>();
            var MoqProductWriteRepository = new Mock<IProductWriteRepository>();

            var ProductService = new ProductService(MoqProductReadRepository.Object, MoqProductWriteRepository.Object);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
            async () =>
            {
                await ProductService.GetProductAsync(productId).ConfigureAwait(false);
            });

            exception.Message.Should().Be("Product Id Is Invalid");
        }

        #endregion [ GetProductAsync ]

        #region [ GetProductsAsync ]

        [Fact]
        public async void When_CallGetProductsAsync_Then_ReturnedCountZeroProductViewModelList()
        {
            var MoqProductReadRepository = new Mock<IProductReadRepository>();
            var MoqProductWriteRepository = new Mock<IProductWriteRepository>();

            MoqProductReadRepository.Setup(p => p.GetProductsAsync())
                .ReturnsAsync(new List<ProductDto>());

            var ProductService = new ProductService(MoqProductReadRepository.Object, MoqProductWriteRepository.Object);

            var productViewModel = await ProductService.GetProductsAsync().ConfigureAwait(false);

            productViewModel.Should().NotBeNull();
            productViewModel.Count().Should().Be(0);
        }

        [Fact]
        public async void When_CallGetProductsAsync_Then_ReturnedProductViewModelList()
        {
            var MoqProductReadRepository = new Mock<IProductReadRepository>();
            var MoqProductWriteRepository = new Mock<IProductWriteRepository>();

            MoqProductReadRepository.Setup(p => p.GetProductsAsync())
                .ReturnsAsync(new List<ProductDto>() { new ProductDto(), new ProductDto(), new ProductDto(), new ProductDto(), new ProductDto() });

            var ProductService = new ProductService(MoqProductReadRepository.Object, MoqProductWriteRepository.Object);

            var productViewModel = await ProductService.GetProductsAsync().ConfigureAwait(false);

            productViewModel.Should().NotBeNull();
            productViewModel.Count().Should().Be(5);
        }


        #endregion [ GetProductsAsync ]

        #region [ CreateProductAsync ]

        [Fact]
        public async void When_ProductNameIsNullInC1reateProductInputModelInCreateProductAsync_Then_ProductNameMustNotBeEmptyThrowException()
        {
            var MoqProductReadRepository = new Mock<IProductReadRepository>();
            var MoqProductWriteRepository = new Mock<IProductWriteRepository>();

            var ProductService = new ProductService(MoqProductReadRepository.Object, MoqProductWriteRepository.Object);

            var invalidC1reateProductInputModel = ProductMockData.ProductNameIsNullInC1reateProductInputModel();

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            async () =>
            {
                await ProductService.CreateProductAsync(invalidC1reateProductInputModel).ConfigureAwait(false);
            });

            exception.Message.Should().Be("Product Name must not be empty (Parameter 'productName')");
        }

        [Fact]
        public async void When_ProductTitleIsNullInCreateProductInputModelInCreateProductAsync_Then_ProductTitleMustNotBeEmptyThrowException()
        {
            var MoqProductReadRepository = new Mock<IProductReadRepository>();
            var MoqProductWriteRepository = new Mock<IProductWriteRepository>();

            var ProductService = new ProductService(MoqProductReadRepository.Object, MoqProductWriteRepository.Object);

            var invalidC1reateProductInputModel = ProductMockData.ProductTitleIsNullInCreateProductInputModel();

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            async () =>
            {
                await ProductService.CreateProductAsync(invalidC1reateProductInputModel).ConfigureAwait(false);
            });

            exception.Message.Should().Be("Product Title must not be empty (Parameter 'productTitle')");
        }

        [Fact]
        public async void When_ValidCreateProductInputModelInCreateProductAsync_Then_ReturnCreatedProductId()
        {
            var MoqProductReadRepository = new Mock<IProductReadRepository>();
            var MoqProductWriteRepository = new Mock<IProductWriteRepository>();

            MoqProductWriteRepository.Setup(p => p.CreateProductAsync(It.IsAny<Product>())).ReturnsAsync(1);

            var ProductService = new ProductService(MoqProductReadRepository.Object, MoqProductWriteRepository.Object);

            var invalidC1reateProductInputModel = ProductMockData.ValidCreateProductInputModel();

            var response = await ProductService.CreateProductAsync(invalidC1reateProductInputModel).ConfigureAwait(false);

            response.Should().Be(1);
            response.Should().NotBe(0);
        }

        #endregion [ CreateProductAsync ]

    }
}