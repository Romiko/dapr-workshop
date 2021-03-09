package vigilantes.daprworkshop.orderservice.controller;

import java.math.BigDecimal;
import java.time.Instant;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.UUID;

import org.springframework.http.HttpEntity;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

import lombok.extern.slf4j.Slf4j;
import vigilantes.daprworkshop.orderservice.model.CustomerOrder;
import vigilantes.daprworkshop.orderservice.model.CustomerOrderItem;
import vigilantes.daprworkshop.orderservice.model.MenuItem;
import vigilantes.daprworkshop.orderservice.model.OrderItemSummary;
import vigilantes.daprworkshop.orderservice.model.OrderSummary;

@Slf4j
@RestController
public class OrderController {

    private final RestTemplate restTemplate;

    public OrderController(RestTemplate restTemplate){
        this.restTemplate = restTemplate;
    }

    @PostMapping("/order")
    public ResponseEntity<String> newOrder(@RequestBody CustomerOrder customerOrder){
        log.info("Customer Order received: {}", customerOrder);
        
        OrderSummary orderSummary = createOrderSummary(customerOrder);
        log.info("Created Order Summary: {}", orderSummary);

        // TODO: Challenge 2 - Publish an OrderSummary message via Dapr
        return ResponseEntity.ok("Bummer. Business logic and pub/sub isn't implemented yet but, hey, at least your POST worked and you should see the order in the log! YOINK!");
    }

    private OrderSummary createOrderSummary(CustomerOrder order){
        // Retrieve all the menu items
        List<MenuItem> menuItems = MenuItem.getAll();

        // Iterate through the list of ordered items to calculate
        // the total and compile a list of item summaries.
        BigDecimal orderTotal = BigDecimal.ZERO;
        List<OrderItemSummary> itemSummaries = new ArrayList<>();
        for(CustomerOrderItem orderItem : order.getOrderItems()) {
                        
            MenuItem menuItem = menuItems.stream().filter(item -> item.getMenuItemId() == orderItem.getMenuItemId()).findFirst().orElse(null);
            
            BigDecimal menuItemTotalPrice = menuItem.getPrice().multiply(BigDecimal.valueOf(orderItem.getQuantity()));
            orderTotal = orderTotal.add(menuItemTotalPrice);

            OrderItemSummary orderItemSummary = new OrderItemSummary();
            orderItemSummary.setMenuItemId(orderItem.getMenuItemId());
            orderItemSummary.setQuantity(orderItem.getQuantity());
            orderItemSummary.setMenuItemName(menuItem.getName());
            itemSummaries.add(orderItemSummary);

        }

        // Initialize and return the order summary
        OrderSummary orderSummary = new OrderSummary();
        orderSummary.setOrderId(UUID.randomUUID());
        orderSummary.setStoreId(order.getStoreId());
        orderSummary.setCustomerName(order.getCustomerName());
        orderSummary.setLoyaltyId(order.getLoyaltyId());
        orderSummary.setOrderDate(Date.from(Instant.now()));
        orderSummary.setOrderItems(itemSummaries);
        orderSummary.setOrderTotal(orderTotal);

        return orderSummary;
    }

}