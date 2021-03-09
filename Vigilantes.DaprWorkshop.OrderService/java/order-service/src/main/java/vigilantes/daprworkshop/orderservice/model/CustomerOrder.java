package vigilantes.daprworkshop.orderservice.model;

import java.util.List;

import lombok.Data;

@Data
public class CustomerOrder {

    private String storeId;

    private String customerName;

    private String loyaltyId;

    private List<CustomerOrderItem> orderItems;
    
}