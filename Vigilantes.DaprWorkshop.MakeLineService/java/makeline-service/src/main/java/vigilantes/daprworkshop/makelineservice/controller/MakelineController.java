package vigilantes.daprworkshop.makelineservice.controller;

import java.io.IOException;
import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Calendar;
import java.util.Collections;
import java.util.Date;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;

import javax.crypto.SecretKey;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.core.type.TypeReference;
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
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.client.RestTemplate;

import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.security.Keys;
import lombok.extern.slf4j.Slf4j;
import vigilantes.daprworkshop.makelineservice.model.OrderSummary;
import vigilantes.daprworkshop.makelineservice.model.OrderSummaryUpdateData;
import vigilantes.daprworkshop.makelineservice.model.StoreData;

import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;


@Slf4j
@RestController
public class MakelineController {

    private final ObjectMapper mapper;
    private final RestTemplate restTemplate;
    private final String signalRHubName = "orders";
    
    public MakelineController(RestTemplate restTemplate){
        this.restTemplate = restTemplate;
        this.mapper = new ObjectMapper();
        this.mapper.disable(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS);
        this.mapper.setDateFormat(new StdDateFormat().withColonInTimeZone(true));
    }

    @PostMapping("/orders")
    public ResponseEntity<String> makeOrder(@RequestBody byte[] cloudEvent) throws JsonProcessingException{
        
        // Deserialize incoming order summary
        String cloudEventJson = new String(cloudEvent);
        JsonNode jsonNode = mapper.readTree(cloudEventJson);
        OrderSummary orderSummary = mapper.treeToValue(jsonNode.get("data"), OrderSummary.class);
        log.info("Received Order Summary: {}", orderSummary);

        // TODO: Challenge 4 - Load current list of orders to be made from state store
        //                   - Add incoming order to the list
        //                   - Save modified list to state store 
        
        // TODO: Challenge 6 - Call the SignalR output binding with the incoming order summary

        return new ResponseEntity<>(HttpStatus.OK);
    }

    // TODO: Challenge 4 - Implement a method to get all orders
    //                   - Implement a method to delete an order
    @PostMapping(value="/negotiate")
    @CrossOrigin(origins = "http://127.0.0.1:8080", methods= {RequestMethod.POST}, allowCredentials="true")
    private ResponseEntity<String> negotiate() throws IOException {

        // this method will be called by the signalr client when connecting to
        // the azure signalr service. It will return an access token and the
        // endpoint details for the client to use when sending and receiving events.

        // get signalr connection string
        String connectionString = getSignalRConnectionString();

        // get map from connection string
        HashMap<String, String> map = getMapFromConnectionString(connectionString);

        // get signalr hub url
        String url = map.get("Endpoint") + "/client/?hub=" + signalRHubName;

        // generate token with claims
        String token = generateJWT("vigilantes.ui", url, map.get("AccessKey"));
        log.info("token: {}", token);

        // create returned object
        HashMap<String, String> response_map = new HashMap<String, String>();
        response_map.put("url", url);
        response_map.put("accessToken", token);

        // serialize data
        String json = mapper.writeValueAsString(response_map);
        log.info("json to return: {}", json);
        
        // set response
        return ResponseEntity.ok(json);
    }

    private String generateJWT(String nameID, String aud, String signature) {

        SecretKey secret = Keys.hmacShaKeyFor(signature.getBytes(StandardCharsets.UTF_8));

        Calendar cal = Calendar.getInstance();
        cal.setTime(new Date());
        cal.add(Calendar.HOUR_OF_DAY, 168); // 7 days
        Date expirationTime = cal.getTime();

        String jwt = Jwts.builder()
        .claim("nameid", nameID)
        .setIssuedAt(expirationTime)
        .setExpiration(expirationTime)
        .setAudience(aud)
        .setHeaderParam("typ", "JWT")
        .setHeaderParam("kid", signature)
        .signWith(secret)
        .compact();

        return jwt;
    }

    private HashMap<String, String> getMapFromConnectionString(String connectionString) {

        HashMap<String, String> result = new HashMap<String, String>();

        // create array
        String[] arr = connectionString.split(";", -1);

        // endpoint
        String[] endpoint_value = arr[0].split("=", 2);
        result.put("Endpoint", endpoint_value[1]);
        log.info("Endpoint: {}", endpoint_value[1]);

        // access key
        String[] accesskey_value = arr[1].split("=", 2);
        result.put("AccessKey", accesskey_value[1]);
        log.info("AccessKey: {}", accesskey_value[1]);

        // version
        String[] version_value = arr[2].split("=", 2);
        result.put("Version", version_value[1]);
        log.info("Version: {}", version_value[1]);

        return result;
    }

    private void sendOrderUpdate(String eventName, OrderSummary orderSummary) throws JsonProcessingException {

        // TODO: Challenge 6 - Send event to SignalR output binding
        // References:
        //    https://github.com/dapr/docs/tree/master/howto/send-events-with-output-bindings
        //    https://github.com/dapr/docs/blob/master/reference/specs/bindings/signalr.md
        //    Option: use the OrderSummaryUpdateData object
    }

    private String getSignalRConnectionString() throws IOException {

        String result = "";
        
        return result;
    }
}