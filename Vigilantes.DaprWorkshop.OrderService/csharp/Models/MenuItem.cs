using System.Collections.Generic;
using Newtonsoft.Json;

namespace Vigilantes.DaprWorkshop.OrderService.Models
{
    public class MenuItem
    {
        [JsonProperty("menuItemid")]
        public int MenuItemId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        public static List<MenuItem> GetAll()
        {
            var items = new List<MenuItem>();
            items.Add(new MenuItem
            {
                MenuItemId = 1,
                Name = "Americano",
                Description = "Americano",
                Price = 2.99m,
                ImageUrl = "https://images.squarespace-cdn.com/content/v1/5a7cbe247131a5f17b3cc8fc/1519450022832-LOQH11HV28XLI0VI5YMA/ke17ZwdGBToddI8pDm48kNBhxsR5AixTPaSt36FQjZRZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZamWLI2zvYWH8K3-s_4yszcp2ryTI0HqTOaaUohrI8PIHEpb-MmdDNvFVgjmeoENIlexef176In2EgYPtI8R2-8KMshLAGzx4R3EDFOm1kBS/Americano-Coffee-Lounge-Americano-White-Cup-Coffee-Image.jpg?format=2500w"
            });
            items.Add(new MenuItem
            {
                MenuItemId = 2,
                Name = "Caramel Macchiato",
                Description = "Caramel Macchiato",
                Price = 4.99m,
                ImageUrl = "https://coffeeatthree.com/wp-content/uploads/macchiato-1a.jpg"
            });
            items.Add(new MenuItem
            {
                MenuItemId = 3,
                Name = "Latte",
                Description = "Latte",
                Price = 3.99m,
                ImageUrl = "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c6/Latte_art_3.jpg/440px-Latte_art_3.jpg"
            });

            return items;
        }
    }
}