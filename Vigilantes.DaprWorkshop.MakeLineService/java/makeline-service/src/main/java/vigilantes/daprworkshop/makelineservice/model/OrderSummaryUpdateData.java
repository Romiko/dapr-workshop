package vigilantes.daprworkshop.makelineservice.model;

import java.util.List;

import lombok.Data;

@Data
public class OrderSummaryUpdateData {
 
    public String target;

    public List<Object> arguments;
}