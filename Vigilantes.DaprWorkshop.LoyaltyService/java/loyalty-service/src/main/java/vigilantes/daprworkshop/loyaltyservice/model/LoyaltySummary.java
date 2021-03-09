package vigilantes.daprworkshop.loyaltyservice.model;

import lombok.Data;

@Data
public class LoyaltySummary {
    
    public String loyaltyId;
    
    public String customerName;
    
    public int pointsEarned;
    
    public int pointTotal;
}