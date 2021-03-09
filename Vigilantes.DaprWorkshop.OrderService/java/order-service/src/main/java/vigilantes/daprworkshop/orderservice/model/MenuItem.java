package vigilantes.daprworkshop.orderservice.model;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;

import lombok.Data;

@Data
public class MenuItem {

    private int menuItemId;

    private String name;

    private String description;

    private BigDecimal price;

    private String imageUrl;
    

    public static List<MenuItem> getAll(){
        List<MenuItem> items = new ArrayList<>();
        MenuItem menuItem = new MenuItem();
        menuItem.menuItemId = 1;
        menuItem.name = "Americano";
        menuItem.description = "Americano";
        menuItem.price = BigDecimal.valueOf(2.99);
        menuItem.imageUrl = "https://daprworkshop.blob.core.windows.net/images/americano.jpg";

        MenuItem menuItem2 = new MenuItem();
        menuItem2.menuItemId = 2;
        menuItem2.name = "Caramel Macchiato";
        menuItem2.description = "Caramel Macchiato";
        menuItem2.price = BigDecimal.valueOf(4.99);
        menuItem2.imageUrl = "https://daprworkshop.blob.core.windows.net/images/macchiato.jpg";

    
        MenuItem menuItem3 = new MenuItem();
        menuItem3.menuItemId = 3;
        menuItem3.name = "Latte";
        menuItem3.description = "Latte";
        menuItem3.price = BigDecimal.valueOf(3.99);
        menuItem3.imageUrl = "https://daprworkshop.blob.core.windows.net/images/latte.jpg";

        items.add(menuItem);
        items.add(menuItem2);
        items.add(menuItem3);
        
        return items;
    }

}