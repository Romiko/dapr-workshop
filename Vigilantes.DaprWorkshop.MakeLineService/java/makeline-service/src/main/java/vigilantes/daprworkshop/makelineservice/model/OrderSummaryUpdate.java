package vigilantes.daprworkshop.makelineservice.model;

import lombok.Data;

@Data
public class OrderSummaryUpdate {
 
    public OrderSummaryUpdateData data;
    
    public Object metadata;
}