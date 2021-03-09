import Vue from "vue";

const state = {
  menuItems: [],
};

const mutations = {
  SET_MENUITEMS: (state, menuItemsPayload) => {
    //sets the state's menuItem property to the menuItems array recieved as payload
    state.menuItems = menuItemsPayload;
  }
};

const actions = {
  async fetchMenuItems({ commit }) {
    try {
      var response = await Vue.axios.get(process.env.VUE_APP_MENUITEM_URL);
      console.log(response);
      if (response.status == 200) {
        const resultArray = [];
        for (let key in response.data) {
          resultArray.push(response.data[key]);
        }
        //passing the products recieved from response as a payload to the mutation
        commit("SET_MENUITEMS", resultArray);
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
  addToCart: ({ commit }, menuItem) => {
    commit("ADD_CARTITEM", menuItem);
  }
};

const getters = {
  menuItems: state => {
    return state.menuItems;
  },
  menuItemsRetrieved: state => {
    return state.menuItemsRetrieved;
  },
};

export default {
  state,
  mutations,
  actions,
  getters
};
