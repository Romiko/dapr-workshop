import Vue from "vue";

const state = {
  cartItems: [],
  customerInfo: {
    customerName: "",
    loyaltyId: ""
  },
  checkoutStatus: null
};

const mutations = {
  SET_CUSTOMER(state, { customerName, loyaltyId }) {
    state.customerInfo.customerName = customerName;
    state.customerInfo.loyaltyId = loyaltyId;
  },

  ADD_CARTITEM(state, menuItem) {
    let cartItem = state.cartItems.find(
      item => item.menuItem.menuItemId === menuItem.menuItemId
    );
    if (cartItem) {
      //product already present in the cart. so increase the quantity
      cartItem.quantity++;
    } else {
      state.cartItems.push({
        menuItem: menuItem,
        quantity: 1
      });
    }
  },

  DELETE_CARTITEM(state, index) {
    //const index = state.cartItems.findIndex(element => element.menuItem.menuItemId == id);
    state.cartItems.splice(index, 1);
  },

  INCREASE_CARTITEM_QUANTITY: (state, menuItemId) => {
    let cartItem = state.cartItems.find(
      item => item.menuItem.menuItemId === menuItemId
    );
    if (cartItem) {
      //product already present in the cart. so increase the quantity
      cartItem.quantity++;
    }
  },

  DECREASE_CARTITEM_QUANTITY: (state, menuItemId) => {
    //find the product in the cart list
    let cartItem = state.cartItems.find(
      item => item.menuItem.menuItemId === menuItemId
    );
    cartItem.quantity--;
  },

  SET_CARTITEMS(state, { cartItems }) {
    state.cartItems = cartItems
  },

  SET_CHECKOUT_STATUS(state, status) {
    state.checkoutStatus = status
  },

  SET_CUSTOMERINFO(state, { customerInfo }) {
    state.customerInfo = customerInfo
  }
};

const actions = {
  increaseQuantity: ({ commit }, menuItemId) => {
    commit("INCREASE_CARTITEM_QUANTITY", menuItemId);
  },
  decreaseQuantity: ({ commit }, menuItemId) => {
    commit("DECREASE_CARTITEM_QUANTITY", menuItemId);
  },
  setCustomerInfo: ({ commit }, customerInfo) => {
    commit("SET_CUSTOMER", customerInfo);
  },
  deleteCartItem: ({ commit }, index) => {
    commit("DELETE_CARTITEM", index);
  },
  async checkout({ commit }) {
    var currentOrdersState = state.cartItems;
    var currentCustomerState = state.customerInfo;
    var customerOrderItems = [];

    commit('SET_CHECKOUT_STATUS', null);

    //empty cart
    commit('SET_CARTITEMS', { cartItems: [] })
    commit('SET_CUSTOMERINFO', {
      customerInfo: {
        customerName: "",
        loyaltyId: ""
      }
    })

    currentOrdersState.forEach(element => {
      customerOrderItems.push({
        menuItemId: element.menuItem.menuItemId,
        quantity: element.quantity
      });
    })

    const order = {
      customerName: currentCustomerState.customerName,
      loyaltyId: currentCustomerState.loyaltyId,
      storeId: "Redmond",
      orderItems: customerOrderItems
    };

    let headers = {
      "Content-Type": "application/json;charset=utf-8"
    };

    try {
      const response = await Vue.axios.post(process.env.VUE_APP_SUBMIT_ORDER_URL, order,
        {
          headers: headers
        })
      if (response.status == 200) {
        commit('SET_CHECKOUT_STATUS', 'successful')
        console.log("Checkout " + state.checkoutStatus);
      }
      console.log(response);
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

      commit('SET_CHECKOUT_STATUS', 'failed')
      //rollback to the cart saved before sending the request
      commit('SET_CARTITEMS', { cartItems: currentOrdersState }),
      commit('SET_CUSTOMERINFO', { customerInfo: currentCustomerState })
      console.log("Checkout " + state.checkoutStatus);
      //console.log(error.config);
    }
  }
};

const getters = {
  cartItems: state => {
    return state.cartItems;
  },
  checkoutStatus: state => {
    return state.checkoutStatus;
  },
  cartItemCount(state) {
    return state.cartItems.length;
  },
  cartTotalAmount: state => {
    return state.cartItems.reduce((total, item) => {
      return total + item.menuItem.price * item.quantity;
    }, 0);
  },
  customerInfo: state => {
    return state.customerInfo;
  },
  menuItemInCart: (state, id) => {
    let cartItem = state.cartItems.find(
      item => item.menuItem.menuItemId === id
    );
    if (cartItem) {
      return true;
    } else return false;
  }
};

export default {
  actions,
  state,
  mutations,
  getters
};
