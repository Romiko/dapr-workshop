package vigilantes.daprworkshop.orderservice.model;

import lombok.Data;

@Data
public class OrderItemSummary {
    
    private int menuItemId;

    private String menuItemName;

    private int quantity;

}