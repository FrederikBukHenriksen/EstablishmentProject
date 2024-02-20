using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Utils;

namespace EstablishmentProject.test.Application_Test.FilterSales_Test
{
    public class FitlerSalesBySalesItems_Test
    {
        private Establishment establishment;

        private Item coffee;
        private Item tea;
        private Item water;
        private Table table;
        private Sale sale_empty;
        private Sale sale_coffee;
        private Sale sale_coffee_tea;
        private Sale sale_coffee_tea_water;

        public FitlerSalesBySalesItems_Test()
        {

            establishment = new Establishment("Cafe 1");

            coffee = establishment.CreateItem("coffee", 0);
            establishment.AddItem(coffee);

            tea = establishment.CreateItem("tea", 0);
            establishment.AddItem(tea);

            water = establishment.CreateItem("water", 0);
            establishment.AddItem(water);

            sale_empty = establishment.CreateSale(DateTime.Today);
            establishment.AddSale(sale_empty);
            sale_coffee = establishment.CreateSale(DateTime.Today, itemAndQuantity: new List<(Item, int)> { (coffee, 1) });
            establishment.AddSale(sale_coffee);
            sale_coffee_tea = establishment.CreateSale(DateTime.Today, itemAndQuantity: new List<(Item, int)> { (coffee, 1), (tea, 1) });
            establishment.AddSale(sale_coffee_tea);
            sale_coffee_tea_water = establishment.CreateSale(DateTime.Today, itemAndQuantity: new List<(Item, int)> { (coffee, 1), (tea, 1), (water, 1) });
            establishment.AddSale(sale_coffee_tea_water);

        }

        [Fact]
        public void Any()
        {
            //ARRANGE
            FilterSalesBySalesItems salesSorting = new FilterSalesBySalesItems(any: [coffee.Id]);

            //ACT
            var sales = SalesFilterHelper.FilterSalesOnSalesItems(establishment.GetSales(), salesSorting);

            //ASSERT
            Assert.Equal(3, sales.Count());

            Assert.Contains(sale_coffee, sales);
            Assert.Contains(sale_coffee_tea, sales);
            Assert.Contains(sale_coffee_tea_water, sales); ;
        }

        [Fact]
        public void All()
        {
            //ARRANGE
            var salesSorting = new FilterSalesBySalesItems(all: [coffee.Id, tea.Id]);

            //ACT
            var sales = SalesFilterHelper.FilterSalesOnSalesItems(establishment.GetSales(), salesSorting);

            //ASSERT
            Assert.Equal(2, sales.Count());

            Assert.Contains(sale_coffee_tea, sales);
            Assert.Contains(sale_coffee_tea_water, sales);
        }

        [Fact]
        public void Excatly()
        {
            //ARRANGE
            var salesSorting = new FilterSalesBySalesItems(exactly: [coffee.Id, tea.Id]);

            //ACT
            var sales = SalesFilterHelper.FilterSalesOnSalesItems(establishment.GetSales(), salesSorting);

            //ASSERT
            Assert.Equal(1, sales.Count());

            Assert.Contains(sale_coffee_tea, sales);
        }
    }
}
