package vigilantes.daprworkshop.makelineservice.model;

import java.util.List;

import lombok.Data;

@Data
public class StoreData {

    public String key;
    
    public List<OrderSummary> value;
    
}