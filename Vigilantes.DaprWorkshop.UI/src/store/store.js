import Vue from "vue";
import Vuex from "vuex";

import cart from "./modules/cart";
import menu from "./modules/menu";
import orders from "./modules/orders";
import header from "./modules/header";

Vue.use(Vuex);

export default new Vuex.Store({
  modules: {
    menu,
    cart,
    orders, 
    header
  }
});
