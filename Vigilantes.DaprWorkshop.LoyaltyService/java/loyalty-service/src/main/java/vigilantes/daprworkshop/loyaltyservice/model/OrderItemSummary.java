package vigilantes.daprworkshop.loyaltyservice.model;

import lombok.Data;

@Data
public class OrderItemSummary {
 
    public int menuItemId;
    
    public String menuItemName; 
    
    public int quantity;
}