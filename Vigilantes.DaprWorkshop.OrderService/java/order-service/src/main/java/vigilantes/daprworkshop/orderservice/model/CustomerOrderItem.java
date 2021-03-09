package vigilantes.daprworkshop.orderservice.model;

import lombok.Data;

@Data
public class CustomerOrderItem {

    private int menuItemId;

    private int quantity;
    
}