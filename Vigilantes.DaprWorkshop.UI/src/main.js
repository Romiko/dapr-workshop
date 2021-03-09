import Vue from "vue";
import ElementUI from "element-ui";
import locale from "element-ui/lib/locale/lang/en";
import "element-ui/lib/theme-chalk/index.css";
import App from "./App.vue";
import router from "./router";
import store from "./store/store";
import axios from 'axios'
import VueAxios from 'vue-axios'
import OrderHub from './order-hub'

//could put the imports here 

Vue.use(VueAxios, axios);
Vue.use(ElementUI, { locale });
Vue.use(OrderHub);

Vue.config.productionTip = false;

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount("#app");
