import Vue from "vue";

const state = {
  orders: [],
  completedOrders: 0,
  ordersRetrieved: false
};


const mutations = {
  SET_ORDERS: (state, ordersPayload) => {
    //sets the state's menuItem property to the menuItems array recieved as payload
    state.orders = ordersPayload;
  },
  COMPLETE_ORDER: (state) => {
    state.completedOrders++;
  }
};

const actions = {
  async fetchOutstandingOrders({ commit }) {
    try {
      var response = await Vue.axios.get(process.env.VUE_APP_MAKELINE_ORDERS_URL);

      if (response.data) {
        const resultArray = [];
        for (let key in response.data) {
          resultArray.push(response.data[key]);

          //passing the products recieved from response as a payload to the mutation
          commit("SET_ORDERS", resultArray);
        }
      }
    }
    catch (error) {
      /* The request was made and the server responded with a status code that falls out of the range of 2xx
       */
      if (error.response) {
        console.log(error.response.data);
        console.log(error.response.status);
        console.log(error.response.headers);
      }
      else if (error.request) {
        //The request was made but no response was received 
        console.log(error.request)
      }
      else {
        console.log('Error', error.message)
      }
    }
  },
  orderItemCompleted: ({ commit }) => {
    commit("COMPLETE_ORDER");
  }
}

const getters = {
  outstandingOrders: state => {
    return state.orders;
  },
  ordersRetrieved: state => {
    return state.ordersRetrieved;
  },
  orderCompletionPercentage: state => {
    if (state.orders.length > 0 || state.completedOrders > 0) {
      return Math.round(state.completedOrders / (state.orders.length + state.completedOrders) * 100);
    }
    else {
      return 0;
    }
  }
};

export default {
  state,
  mutations,
  actions,
  getters
};