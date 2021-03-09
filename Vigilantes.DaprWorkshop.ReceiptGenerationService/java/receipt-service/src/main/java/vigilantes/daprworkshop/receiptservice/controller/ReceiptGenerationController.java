package vigilantes.daprworkshop.receiptservice.controller;

import java.util.Map;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;
import com.fasterxml.jackson.databind.util.StdDateFormat;

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
import vigilantes.daprworkshop.receiptservice.model.OrderSummary;

@Slf4j
@RestController
public class ReceiptGenerationController {

    private final RestTemplate restTemplate;
    private final ObjectMapper mapper;

    public ReceiptGenerationController(RestTemplate restTemplate){
        this.restTemplate = restTemplate;
        this.mapper = new ObjectMapper();
        this.mapper.disable(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS);
        this.mapper.setDateFormat(new StdDateFormat().withColonInTimeZone(true));
    }

    @PostMapping("/orders")
    public ResponseEntity<String> generateReceipt(@RequestBody byte[] cloudEvent) throws JsonProcessingException{
        String cloudEventJson = new String(cloudEvent);

        JsonNode jsonNode = mapper.readTree(cloudEventJson);
        OrderSummary orderSummary = mapper.treeToValue(jsonNode.get("data"), OrderSummary.class);
        log.info("Received Order Summary: {}", orderSummary);

        log.info("Writing Order Summary (receipt) to storage: {}", orderSummary);
        
        // TODO: Challenge 5 - Store receipt via a Dapr Output Binding that can be used as a data sink
        return new ResponseEntity<>(HttpStatus.OK);
    }
}