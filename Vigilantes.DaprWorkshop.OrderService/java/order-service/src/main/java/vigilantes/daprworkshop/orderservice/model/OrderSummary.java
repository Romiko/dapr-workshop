package vigilantes.daprworkshop.orderservice.model;

import java.math.BigDecimal;
import java.util.Date;
import java.util.List;
import java.util.UUID;

import lombok.Data;

@Data
public class OrderSummary {

        private UUID orderId;

        private Date orderDate;

        private String storeId;

        private String customerName;

        private String loyaltyId;

        private List<OrderItemSummary> OrderItems;

        private BigDecimal orderTotal;

}