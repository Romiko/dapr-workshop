package vigilantes.daprworkshop.orderservice.controller;

import java.util.List;

import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RestController;

import vigilantes.daprworkshop.orderservice.model.MenuItem;

@RestController
public class MenuItemController {

    @GetMapping("/menuitem")
    public List<MenuItem> get(){    
        return MenuItem.getAll();
    }
    
}