package vigilantes.daprworkshop.receiptservice.model;

import java.math.BigDecimal;
import java.util.Date;
import java.util.List;
import java.util.UUID;

import lombok.Data;

@Data
public class OrderSummary {

    public UUID orderId;
    
    public Date orderDate;
    
    public String storeId;
    
    public String customerName;
    
    public String loyaltyId;
    
    public List<OrderItemSummary> orderItems;
    
    public BigDecimal orderTotal;
    
}