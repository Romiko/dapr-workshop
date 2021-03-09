package vigilantes.daprworkshop.virtualbarista.controller;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.fasterxml.jackson.databind.util.StdDateFormat;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

import lombok.extern.slf4j.Slf4j;
import vigilantes.daprworkshop.virtualbarista.model.OrderSummary;

@Slf4j
@RestController
public class VirtualBaristaController {

    private final ObjectMapper mapper;
    private final RestTemplate restTemplate;
    private final String storeId = "Redmond";

    public VirtualBaristaController(RestTemplate restTemplate){
        this.restTemplate = restTemplate;
        this.mapper = new ObjectMapper();
        this.mapper.disable(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS);
        this.mapper.setDateFormat(new StdDateFormat().withColonInTimeZone(true));
    }

    @PostMapping("/orders")
    public ResponseEntity<String> checkOrders() throws JsonProcessingException{
        
        log.info("schedule trigger invoked");

        List<OrderSummary> orderSummaries = new LinkedList<OrderSummary>(getOrders(storeId));

        if (orderSummaries.size() > 0){
            log.info("The Virtual Barista is making an order for " + orderSummaries.get(0).customerName);

            completeOrder(orderSummaries.get(0).orderId.toString());

            log.info(orderSummaries.get(0).customerName + ", your drinks are ready!");
        }

        // set response
        return ResponseEntity.ok("ok");
    }

    private void completeOrder(String orderId) {

    }

    private List<OrderSummary> getOrders(String storeId) throws JsonProcessingException {

        return new ArrayList<OrderSummary>();
    }
}